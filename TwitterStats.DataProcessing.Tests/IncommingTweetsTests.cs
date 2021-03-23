using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using TwitterStats.DataProcessing;
using TwitterStats.ServiceLibrary.Services.Processing;
using TwitterStats.ServiceLibrary.Services.QueueServices;
using TwitterStats.Contracts;

namespace TwitterStats.DataProcessing.Tests
{
    [TestClass]
    public class IncommingTweetsTests
    {
        private MockRepository mockRepository;

        private Mock<IHttpClientFactory> mockHttpClientFactory;
        private Mock<IProcessedIncommingQueue> mockProcessedIncommingQueue;
        private Mock<IIncommingTweetQueue> mockIncommingTweetQueue;
        private Mock<ITweetScrapingService> mockTweetScrapingService;
        private Mock<IIncommingTweetQueuePoison> mockIncommingTweetQueuePoison;
        private Mock<ITweetAggregationService> mockTweetAggregationService;
        private ILogger logger;
        private string rawTweet;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).TrimEnd(@"\bin".ToArray());
            var dataFilePath = Path.GetFullPath(Path.Combine(binDirectory, Constants.DataFileRawTweetName));
            rawTweet = File.ReadAllText(dataFilePath); 
            

            this.mockHttpClientFactory = this.mockRepository.Create<IHttpClientFactory>();
            this.mockProcessedIncommingQueue = this.mockRepository.Create<IProcessedIncommingQueue>();
            this.mockIncommingTweetQueue = this.mockRepository.Create<IIncommingTweetQueue>();
            this.mockTweetScrapingService = this.mockRepository.Create<ITweetScrapingService>();
            this.mockIncommingTweetQueuePoison = this.mockRepository.Create<IIncommingTweetQueuePoison>();
            this.mockTweetAggregationService = this.mockRepository.Create<ITweetAggregationService>();

            ILoggerFactory loggerFactory = new LoggerFactory();
            logger = LoggerFactoryExtensions.CreateLogger<TweetScrapingService>(loggerFactory);


            this.mockTweetAggregationService.Setup(o => o.ProcessIncommingTweet(this.mockHttpClientFactory.Object, It.IsAny<string>(),
                                                        It.IsAny<string>(), It.IsAny<int>(), logger)).Returns(Task.CompletedTask);
        }

        private IncommingTweets CreateIncommingTweets()
        {
            return new IncommingTweets(
                this.mockHttpClientFactory.Object,
                this.mockProcessedIncommingQueue.Object,
                this.mockIncommingTweetQueue.Object,
                this.mockTweetScrapingService.Object,
                this.mockIncommingTweetQueuePoison.Object,
                this.mockTweetAggregationService.Object);
        }

        [TestMethod]
        public async Task Run_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var incommingTweets = this.CreateIncommingTweets();
            string myQueueItem = rawTweet;
            
            // Act
            await  incommingTweets.Run(myQueueItem, logger);
             
            this.mockRepository.VerifyAll();
        }
    }
}
