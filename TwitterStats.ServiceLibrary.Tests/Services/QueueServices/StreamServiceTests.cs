using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwitterStats.Contracts;
using TwitterStats.ServiceLibrary.Services.QueueServices;
using System.Configuration;
using System.Net.Http.Headers;

namespace TwitterStats.ServiceLibrary.Tests.Services.QueueServices
{
    [TestClass]
    public class StreamServiceTests
    {
        private MockRepository mockRepository;
        //private Mock<IHttpClientFactory> mockHttpClientFactory;
        private Mock<IHttpClientProvider> mockHttpClientProvider;
        private Mock<IStreamReaderHandler> mockStreamReaderHandler;
        private Mock<IIncommingTweetQueue> mockIncommingTweetQueue;
        private IConfiguration configuration;
        private TwitterConfiguration twitterConfiguration;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Loose);
            //this.mockHttpClientFactory = this.mockRepository.Create<IHttpClientFactory>();
            this.mockHttpClientProvider = this.mockRepository.Create<IHttpClientProvider>();
            this.mockStreamReaderHandler = this.mockRepository.Create<IStreamReaderHandler>();
            this.mockIncommingTweetQueue = this.mockRepository.Create<IIncommingTweetQueue>();

            this.mockIncommingTweetQueue.Setup(o => o.SendMessageOntoQueue(It.IsAny<string>())).Returns(Task.FromResult(true));

            string fakeFileContents = "some content";
            byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);

            MemoryStream fakeMemoryStream = new MemoryStream(fakeFileBytes);

            this.mockStreamReaderHandler.Setup(o => o.GetStreamReader(It.IsAny<Stream>())).Returns(() => new StreamReader(fakeMemoryStream));

            //var response = new Mock<HttpResponseMessage>();
            //response.SetupAllProperties();
            //var mockHttpMessageHandler = new Mock<HttpMessageHandler>();


            //mockHttpMessageHandler.Protected()
            //    .Setup<Task<Stream>>("GetStreamAsync", ItExpr.IsAny<string>())
            //    .ReturnsAsync(fakeMemoryStream);


            //var client = new HttpClient(mockHttpMessageHandler.Object);



            //this.mockHttpClientFactory.Setup(o => o.CreateClient(It.IsAny<string>())).Returns(client.Object);


            //this.mockHttpClientProvider = new Mock<IHttpClientProvider>();
            mockHttpClientProvider.SetupAllProperties();
            mockHttpClientProvider.Setup(o => o.GetStreamAsync(It.IsAny<string>())).ReturnsAsync(fakeMemoryStream, new System.TimeSpan(0, 0, 1));

            mockHttpClientProvider.SetupProperty(o => o.BaseAddress);

            var headers = new HttpClient().DefaultRequestHeaders;
            mockHttpClientProvider.SetupGet(o => o.DefaultRequestHeaders).Returns(headers);

            var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).TrimEnd(@"\bin".ToArray());
            var dataFilePath = Path.GetFullPath(Path.Combine(binDirectory, Constants.AppSettingsJson));
            configuration = new ConfigurationBuilder()
                .AddJsonFile(dataFilePath, optional: true)
                .Build();

            twitterConfiguration = new TwitterConfiguration()
            {
                BaseUrl = configuration.GetValue<string>(Constants.TwitterBaseUrlName).ToString(),
                OAuthToken = configuration.GetValue<string>(Constants.TwitterOAuthTokenName).ToString(),
                SampleStreamUrl = configuration.GetValue<string>(Constants.TwitterSampleStreamUrlName).ToString()
            };

        }

        private StreamService CreateService()
        {
            return new StreamService();
        }

        [TestMethod]
        public async Task ReciveTweets_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            var cancelSource = new CancellationTokenSource();
            cancelSource.Cancel();

            // Act
            var result = await service.ReciveTweets(
                mockHttpClientProvider.Object,
                twitterConfiguration,
                mockIncommingTweetQueue.Object,
                mockStreamReaderHandler.Object,
                cancelSource.Token);

            // Assert
            Assert.AreEqual("Done Collecting Tweets... ", result);
            this.mockRepository.Verify();
        }
    }
}
