using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwitterStats.ServiceLibrary.Services;
using TwitterStats.ServiceLibrary.Services.Stats;

namespace TwitterStats.ServiceLibrary.Tests.Services
{
    [TestClass]
    public class RuntimeStatsServiceTests
    {
        private MockRepository mockRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
        }

        private RuntimeStatsService CreateService()
        {
            return new RuntimeStatsService();
        }

        [TestMethod]
        public void CreateServiceAndTestProperties()
        {
            // Arrange
            var service = this.CreateService();

            // Assert 
            Assert.IsTrue(service.ProcessedTweets.Count == 0);
            Assert.IsFalse(service.RunningCollection);

            // Act
            service.ProcessedTweets.Add(new Contracts.ProcessedTweet());
            service.RunningCollection = true;

            // Assert 
            Assert.IsTrue(service.ProcessedTweets.Count == 1);
            Assert.IsTrue(service.RunningCollection);
           
            this.mockRepository.VerifyAll();
        }
    }
}
