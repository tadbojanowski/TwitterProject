using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using TwitterStats.Contracts;
using TwitterStats.ServiceLibrary.Services.Processing;
using TwitterStats.ServiceLibrary.Services.QueueServices;

namespace TwitterStats.ServiceLibrary.Tests.Services.Processing
{
    [TestClass]
    public class TweetAggregationServiceTests
    {
        private MockRepository mockRepository;
        private ILogger logger;
        private Mock<IProcessedIncommingQueue> mockProcessedIncommingQueue;
        private Mock<ITweetScrapingService> mockTweetScrapingService;
        private Mock<IHttpClientFactory> mockHttpClientFactory;
        private string emojiCodesUCompleteDash;
        private List<ProcessedTweet> testData;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockProcessedIncommingQueue = this.mockRepository.Create<IProcessedIncommingQueue>();
            this.mockTweetScrapingService = this.mockRepository.Create<ITweetScrapingService>();

            ILoggerFactory loggerFactory = new LoggerFactory();
            logger = LoggerFactoryExtensions.CreateLogger<TweetScrapingService>(loggerFactory);

            var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).TrimEnd(@"\bin".ToArray());
            var dataFilePath = Path.GetFullPath(Path.Combine(binDirectory, Constants.DataFileName));
            var testDataString = File.ReadAllText(dataFilePath);
            testData = JsonConvert.DeserializeObject<List<ProcessedTweet>>(testDataString);

            var codesFileEmojiName = Path.GetFullPath(Path.Combine(binDirectory, Constants.EmojiCodesFileName));
            emojiCodesUCompleteDash = File.ReadAllText(codesFileEmojiName);

            this.mockHttpClientFactory = this.mockRepository.Create<IHttpClientFactory>();
        }

        private TweetAggregationService CreateService()
        {
            return new TweetAggregationService(
                this.mockProcessedIncommingQueue.Object,
                this.mockTweetScrapingService.Object);
        }

        [TestMethod]
        public async Task ProcessIncommingTweet_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();

            for (int i = 0; i < testData.Count; i++)
            {
                string tweet = testData[i]?.Text;
                int tweetCount = 0;

                // Act
               await service.ProcessIncommingTweet(
                    this.mockHttpClientFactory.Object,
                    tweet,
                    emojiCodesUCompleteDash,
                    tweetCount,
                    logger);                  
            }
             
            this.mockRepository.VerifyAll();
        }
    }
}
