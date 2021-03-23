using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TwitterStats.Contracts;
using TwitterStats.ServiceLibrary.Services.Stats;

namespace TwitterStats.ServiceLibrary.Tests.Services.Stats
{
    [TestClass]
    public class RuntimeStatsServiceTests
    {
        private MockRepository mockRepository;

        private List<ProcessedTweet> testData;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).TrimEnd(@"\bin".ToArray());
            var dataFilePath = Path.GetFullPath(Path.Combine(binDirectory, Constants.DataFileName));
            var testDataString = File.ReadAllText(dataFilePath);
            testData = JsonConvert.DeserializeObject<List<ProcessedTweet>>(testDataString);
        }

        private RuntimeStatsService CreateService()
        {
            return new RuntimeStatsService();
        }

        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var service = this.CreateService();

            // Assert
            Assert.IsFalse(service.RunningCollection);
            service.RunningCollection = true;
            Assert.IsTrue(service.RunningCollection);
            // Act
            service.ProcessedTweets = testData;
            // Assert
            Assert.IsTrue(service.ProcessedTweets.Count > 0);
                        
            this.mockRepository.VerifyAll();
        }
    }
}
