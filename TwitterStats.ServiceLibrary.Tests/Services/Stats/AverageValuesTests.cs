using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TwitterStats.Contracts;
using TwitterStats.ServiceLibrary.Services.Stats;

namespace TwitterStats.ServiceLibrary.Tests.Services.Stats
{
    [TestClass]
    public class AverageValuesTests
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

        private AverageValues CreateAverageValues()
        {
            return new AverageValues();
        }

        [TestMethod]
        public void GetAverage_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var averageValues = this.CreateAverageValues();
             
            TimeDivision timeDivisionHour = TimeDivision.PerHour;
            TimeDivision timeDivisionMinute = TimeDivision.PerMinute;
            TimeDivision timeDivisionSecond = TimeDivision.PerSecond;

            // Act
            var resultHour = averageValues.GetAverage(testData, timeDivisionHour);
            var resultMinute = averageValues.GetAverage(testData, timeDivisionMinute);
            var resultSecond = averageValues.GetAverage(testData, timeDivisionSecond);

            // Assert 
            Assert.AreEqual(278, resultHour);
            Assert.AreEqual(278, resultMinute);
            Assert.AreEqual(55, resultSecond);

            
            this.mockRepository.VerifyAll();
        }
    }
}
