using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using TwitterStats.ServiceLibrary.Services.QueueServices;
using TwitterStats.ServiceLibrary.Services.Stats;
using TwitterStats.WebClient.Hubs;
using TwitterStats.Contracts;
using Moq.Protected;
using System.Net;
using System.Threading;

namespace TwitterStats.WebClient.Tests.Hubs
{
    [TestClass]
    public class QueueHubTests
    {
        private MockRepository mockRepository;

        //private Mock<IHttpClientFactory> mockHttpClientFactory;
        private Mock<IHttpClientProvider> mockHttpClientProvider;
        private string expectedResult;
        IIncommingTweetQueue incommingTweetQueue;
        private Mock<IRuntimeStatsService> mockRuntimeStatsService;
        private Mock<IStreamService> mockStreamService;
        private IConfiguration configuration;
        private Mock<IStreamReaderHandler> mockStreamReaderHandler;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Loose);
            expectedResult = "some result";
            //this.mockHttpClientFactory = this.mockRepository.Create<IHttpClientFactory>();
            this.mockHttpClientProvider = this.mockRepository.Create<IHttpClientProvider>();
            incommingTweetQueue = new IncommingTweetQueue(new CloudStorageAccountHandler());
            this.mockRuntimeStatsService = this.mockRepository.Create<IRuntimeStatsService>();             
            this.mockStreamService = this.mockRepository.Create<IStreamService>();

            var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).TrimEnd(@"\bin".ToArray());
            var dataFilePath = Path.GetFullPath(Path.Combine(binDirectory, Constants.AppSettingsJson));
            configuration = new ConfigurationBuilder()
                .AddJsonFile(dataFilePath, optional: true)
                .Build();

            this.mockStreamReaderHandler = this.mockRepository.Create<IStreamReaderHandler>();

            this.mockStreamService.Setup(o => o.ReciveTweets(this.mockHttpClientProvider.Object, It.IsAny<TwitterConfiguration>(), 
                                                It.IsAny<IIncommingTweetQueue>(), It.IsAny<IStreamReaderHandler>(), It.IsAny<CancellationToken>()))
                                            .Returns(Task.FromResult(expectedResult));
            this.mockStreamService.SetupAllProperties();
            this.mockRuntimeStatsService.SetupAllProperties();
            this.mockHttpClientProvider.SetupAllProperties(); 
        }

        private QueueHub CreateQueueHub()
        {
            return new QueueHub(
                this.mockHttpClientProvider.Object,
                incommingTweetQueue,
                configuration,
                this.mockRuntimeStatsService.Object,
                this.mockStreamService.Object,
                this.mockStreamReaderHandler.Object);
        }

        [TestMethod]
        public async Task QueueUpTweets_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var queueHub = this.CreateQueueHub();

            // Act
            var result = await queueHub.QueueUpTweets(new CancellationToken());

            // Assert
            Assert.AreEqual(expectedResult, result);
            this.mockRepository.VerifyAll();
        }
    }
}


