using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TwitterStats.ServiceLibrary.Services.Processing;
using TwitterStats.Contracts;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using Moq.Protected;
using System.Threading;
using System.Net;

namespace TwitterStats.ServiceLibrary.Tests.Services.Processing
{
    [TestClass]
    public class TweetScrapingServiceTests
    {
        private MockRepository mockRepository;
        private Mock<IHttpClientFactory> mockHttpClientFactory;
        private ILogger logger;
        private List<ProcessedTweet> testData;
        private List<List<string>> testDataMentions;
        private List<List<string>> testDataHashtags;
        private List<List<string>> testDataLinks;
        private List<string> testDataEmojis;
        private string emojiCodesUCompleteDash; 

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockHttpClientFactory = this.mockRepository.Create<IHttpClientFactory>();

            ILoggerFactory loggerFactory = new LoggerFactory();
            logger = LoggerFactoryExtensions.CreateLogger<TweetScrapingService>(loggerFactory);

            var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).TrimEnd(@"\bin".ToArray());
            var dataFilePath = Path.GetFullPath(Path.Combine(binDirectory, Constants.DataFileName));
            var testDataString = File.ReadAllText(dataFilePath);            
            testData = JsonConvert.DeserializeObject<List<ProcessedTweet>>(testDataString);

            var dataFileMentionsName = Path.GetFullPath(Path.Combine(binDirectory, Constants.DataFileMentionsName));
            var dataFileMentions = File.ReadAllText(dataFileMentionsName);
            testDataMentions = JsonConvert.DeserializeObject<List<List<string>>>(dataFileMentions);

            var dataFileHashtagsName = Path.GetFullPath(Path.Combine(binDirectory, Constants.DataFileHashtagsName));
            var dataFileHashtags = File.ReadAllText(dataFileHashtagsName);
            testDataHashtags = JsonConvert.DeserializeObject<List<List<string>>>(dataFileHashtags);

            var dataFileLinksName = Path.GetFullPath(Path.Combine(binDirectory, Constants.DataFileLinksName));
            var dataFileLinks = File.ReadAllText(dataFileLinksName);
            testDataLinks = JsonConvert.DeserializeObject<List<List<string>>>(dataFileLinks);

            var codesFileEmojiName = Path.GetFullPath(Path.Combine(binDirectory, Constants.EmojiCodesFileName));
            emojiCodesUCompleteDash = File.ReadAllText(codesFileEmojiName);

            var dataFileEmojiName = Path.GetFullPath(Path.Combine(binDirectory, Constants.DataFileEmojisName));
            var dataFileEmojis = File.ReadAllText(dataFileEmojiName);
            testDataEmojis = JsonConvert.DeserializeObject<List<string>>(dataFileEmojis);

            var response = new Mock<HttpResponseMessage>();
            response.SetupAllProperties();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    RequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://google.com")
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            this.mockHttpClientFactory.Setup(o => o.CreateClient(It.IsAny<string>())).Returns(client);
        }

        private TweetScrapingService CreateService()
        {
            return new TweetScrapingService();
        }

        [TestMethod]
        public async Task CollectDataWithRegex_RegexMentionsStateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            string pattern = Constants.RegexMentions;
            
            for (int i = 0; i < testData.Count; i++)
            {
                string tweet = testData[i]?.Text;
                int tweetCount = 0;
                
                // Act
                var result = await service.CollectDataWithRegex(
                    pattern,
                    tweet,
                    tweetCount,
                    logger);

                // Assert                
                CollectionAssert.AreEqual(testDataMentions[i], result);
            }
             
            this.mockRepository.Verify();
        }

        [TestMethod]
        public async Task CollectDataWithRegex_RegexHashtagsStateUnderTest_ExpectedBehavior()
        {
            
            // Arrange
            var service = this.CreateService();
            string pattern = Constants.RegexHashtags;
            for (int i = 0; i < testData.Count; i++)
            {
                string tweet = testData[i]?.Text;
                int tweetCount = 0;  

                // Act
                var result = await service.CollectDataWithRegex(
                    pattern,
                    tweet,
                    tweetCount,
                    logger);



                // Assert
                CollectionAssert.AreEqual(testDataHashtags[i], result);
            }

            
            this.mockRepository.Verify();
        }

        [TestMethod]
        public async Task CollectDataWithRegex_RegexLinksStateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            string pattern = Constants.RegexLinks;
            for (int i = 0; i < testData.Count; i++)
            {
                string tweet = testData[i]?.Text;
                int tweetCount = 0;

                // Act
                var result = await service.CollectDataWithRegex(
                    pattern,
                    tweet,
                    tweetCount,
                    logger);
                
                // Assert
                CollectionAssert.AreEqual(testDataLinks[i], result);
            }
             
            this.mockRepository.Verify();
        }

        [TestMethod]
        public void GetEmojisFromString_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();

            for (int i = 0; i < testData.Count; i++)
            {
                string tweet = testData[i]?.Text;
                int tweetCount = 0;

                // Act
                var result = service.GetEmojisFromString(
                    tweet,
                    emojiCodesUCompleteDash,
                    tweetCount,
                    logger);

                // Assert
                Assert.AreEqual(testDataEmojis[i], result);
            }
             
            this.mockRepository.Verify();
        }

        [TestMethod]
        //[Ignore]
        public async Task LengthenUrls_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();

            List<string> urls = testData.Select(o => o.Urls).ToList();
            int tweetCount = 0;
            int urlCount = 0;
            for (int i = 0; i < urls.Count; i++)
            {
                var url = JsonConvert.DeserializeObject<List<string>>(urls[i]);
                // Act
                var result = await service.LengthenUrls(
                    mockHttpClientFactory.Object,
                    url,
                    tweetCount,
                    logger);

                foreach (var item in result)
                {
                    urlCount++;
                }
                tweetCount++;
            }
            // Assert
            Assert.AreEqual(tweetCount, 278);
            Assert.AreEqual(urlCount, 101);
            this.mockRepository.Verify();
        }
    }
}
