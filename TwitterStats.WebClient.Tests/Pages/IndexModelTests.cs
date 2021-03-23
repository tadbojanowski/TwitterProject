using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwitterStats.ServiceLibrary.Services.Stats;
using TwitterStats.WebClient.Pages;

namespace TwitterStats.WebClient.Tests.Pages
{
    [TestClass]
    public class IndexModelTests
    {
        private MockRepository mockRepository;

        private Mock<IRuntimeStatsService> mockRuntimeStatsService;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockRuntimeStatsService = this.mockRepository.Create<IRuntimeStatsService>();
            this.mockRuntimeStatsService.Setup(o => o.RunningCollection).Returns(false); 
        }

        private IndexModel CreateIndexModel()
        {
            return new IndexModel(
                this.mockRuntimeStatsService.Object);
        }

        [TestMethod]
        public void OnGet_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var indexModel = this.CreateIndexModel();

            // Act
            indexModel.OnGet();

            // Assert

            Assert.AreEqual(false, indexModel.RunningCollection);
             
            this.mockRepository.VerifyAll();
        }
    }
}
