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
    public class PercentageValuesTests
    {
        private MockRepository mockRepository;

        private List<ProcessedTweet>         testData;
        private double PercentEmojis         = 25.539568345323744;
        private double PercentInstagramLink  = 0;
        private double PercentTwitterPicture = 20.863309352517987;
        private double PercentUrls           = 35.611510791366911;



        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).TrimEnd(@"\bin".ToArray());
            var dataFilePath = Path.GetFullPath(Path.Combine(binDirectory, Constants.DataFileName));
            var testDataString = File.ReadAllText(dataFilePath);
            testData = JsonConvert.DeserializeObject<List<ProcessedTweet>>(testDataString);
        }

        private PercentageValues CreatePercentageValues()
        {
            return new PercentageValues();
        }

        [TestMethod]
        public void GetPercentage_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var percentageValues = this.CreatePercentageValues();

            SearchType searchTypeEmojis = SearchType.Emojis;
            SearchType searchTypeInstagramLink = SearchType.InstagramLink;
            SearchType searchTypeTwitterPicture = SearchType.TwitterPicture;
            SearchType searchTypeUrls = SearchType.Urls;

            // Act
            var resultPercentEmojis         = percentageValues.GetPercentage(testData, searchTypeEmojis);
            var resultPercentInstagramLink  = percentageValues.GetPercentage(testData, searchTypeInstagramLink);
            var resultPercentTwitterPicture = percentageValues.GetPercentage(testData, searchTypeTwitterPicture);
            var resultPercentUrls           = percentageValues.GetPercentage(testData, searchTypeUrls);

            // Assert
            Assert.AreEqual(PercentEmojis, resultPercentEmojis);
            Assert.AreEqual(PercentInstagramLink, resultPercentInstagramLink);
            Assert.AreEqual(PercentTwitterPicture, resultPercentTwitterPicture);
            Assert.AreEqual(PercentUrls, resultPercentUrls);

            this.mockRepository.VerifyAll();
        }
    }
}
