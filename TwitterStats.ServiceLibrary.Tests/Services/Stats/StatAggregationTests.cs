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
    public class StatAggregationTests
    {
        private MockRepository mockRepository;

        private Mock<IAverageValues> mockAverageValues;
        private Mock<IPercentageValues> mockPercentageValues;

        private List<ProcessedTweet> testData;
        private AllStats testDataAllStats;
       private TopValues topValues;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockAverageValues = this.mockRepository.Create<IAverageValues>();
            this.mockPercentageValues = this.mockRepository.Create<IPercentageValues>();

            var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).TrimEnd(@"\bin".ToArray());
            var dataFilePath = Path.GetFullPath(Path.Combine(binDirectory, Constants.DataFileName));
            var testDataString = File.ReadAllText(dataFilePath);
            testData = JsonConvert.DeserializeObject<List<ProcessedTweet>>(testDataString);

            var dataAllStatsFilePath = Path.GetFullPath(Path.Combine(binDirectory, Constants.DataFileAllStatsName));
            var testDataAllStatsString = File.ReadAllText(dataAllStatsFilePath);
            testDataAllStats = JsonConvert.DeserializeObject<AllStats>(testDataAllStatsString);
            

            this.mockAverageValues.Setup(o => o.GetAverage(testData, TimeDivision.PerSecond)).Returns(57.45645);
            this.mockAverageValues.Setup(o => o.GetAverage(testData, TimeDivision.PerMinute)).Returns(245.3563);
            this.mockAverageValues.Setup(o => o.GetAverage(testData, TimeDivision.PerHour)).Returns(3456.435324);

            topValues = new TopValues();
            var top5domains = topValues.TopDomains(testData.Select(o => o.Urls).ToList());
            var top5Hashtags = topValues.TopEmoji(testData.Select(o => o.Emojis).ToList());
            var top5Mentions =  topValues.TopValue(testData.Select(o => o.Hashtags).ToList());
            var top5Emojis = topValues.TopDomains(testData.Select(o => o.Urls).ToList());

            this.mockPercentageValues.Setup(o => o.GetPercentage(testData, SearchType.InstagramLink)).Returns(25.539568345323744);
            this.mockPercentageValues.Setup(o => o.GetPercentage(testData, SearchType.TwitterPicture)).Returns(25.539568345323744);
            this.mockPercentageValues.Setup(o => o.GetPercentage(testData, SearchType.Emojis)).Returns(25.539568345323744);
            this.mockPercentageValues.Setup(o => o.GetPercentage(testData, SearchType.Urls)).Returns(35.611510791366911);
        }

        private StatAggregation CreateStatAggregation()
        {
            return new StatAggregation(topValues, this.mockAverageValues.Object, this.mockPercentageValues.Object);
        }

        [TestMethod]
        public void CollectStats_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var statAggregation = this.CreateStatAggregation();
             

            // Act
            var result = statAggregation.CollectStats(testData);
            
            // Assert
            Assert.AreEqual(testDataAllStats.Averages.TotalRecived, result.Averages.TotalRecived);
            Assert.AreEqual(testDataAllStats.Averages.PerHourAverage, result.Averages.PerHourAverage);
            Assert.AreEqual(testDataAllStats.Averages.PerMinuteAverage, result.Averages.PerMinuteAverage);
            Assert.AreEqual(testDataAllStats.Averages.PerSecondAverage, result.Averages.PerSecondAverage);

            Assert.AreEqual(testDataAllStats.Stats.PercentEmojis, result.Stats.PercentEmojis);
            Assert.AreEqual(testDataAllStats.Stats.PercentPics, result.Stats.PercentPics);
            Assert.AreEqual(testDataAllStats.Stats.PercentUrls, result.Stats.PercentUrls); 

            Assert.AreEqual(testDataAllStats.Stats.TopDomains, result.Stats.TopDomains);
            Assert.AreEqual(testDataAllStats.Stats.TopEmojis, result.Stats.TopEmojis);
            Assert.AreEqual(testDataAllStats.Stats.TopHashtags, result.Stats.TopHashtags);
            Assert.AreEqual(testDataAllStats.Stats.TopMentions, result.Stats.TopMentions);
             
            this.mockRepository.VerifyAll();
        }
    }
}
