using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TwitterStats.ServiceLibrary.Services.QueueServices;
using TwitterStats.ServiceLibrary.Services.Stats;
using TwitterStats.WebClient.Hubs;

namespace TwitterStats.WebClient.Tests.Hubs
{
    [TestClass]
    public class StatsHubTests
    {
        private MockRepository mockRepository;

        private Mock<IProcessedIncommingQueue> mockProcessedIncommingQueue;
        private Mock<IRuntimeStatsService> mockRuntimeStatsService;
        private Mock<IStatAggregation> mockStatAggregation;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockProcessedIncommingQueue = this.mockRepository.Create<IProcessedIncommingQueue>();
            this.mockRuntimeStatsService = this.mockRepository.Create<IRuntimeStatsService>();
            this.mockStatAggregation = this.mockRepository.Create<IStatAggregation>();
        }

        private StatsHub CreateStatsHub()
        {
            return new StatsHub(
                this.mockProcessedIncommingQueue.Object,
                this.mockRuntimeStatsService.Object,
                this.mockStatAggregation.Object);
        }

        [TestMethod]
        public void Counter_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var statsHub = this.CreateStatsHub();
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            // Act
            var result = statsHub.Counter(cancellationToken);

            // Assert 
            Assert.AreEqual(false, cancellationToken.IsCancellationRequested);
            
            this.mockRepository.VerifyAll();
        }
    }
}
