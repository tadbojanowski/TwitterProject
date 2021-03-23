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
    public class TopValuesTests
    {
        private MockRepository mockRepository;
        private List<ProcessedTweet> testData;
         
        private string top5domains = @"[{'Key':'twitter.com','Value':90},{'Key':'t.co','Value':7},{'Key':'www.youtube.com','Value':2},{'Key':'onlyfans.com','Value':1},{'Key':'han.gl','Value':1}]";
        private string top5Hashtags = @"[{'Key':'#呪術廻戦','Value':4},{'Key':'#SB19KenWHATMenPa','Value':3},{'Key':'#CRAVITY','Value':2},{'Key':'#SB19','Value':2},{'Key':'#MVอ้อนไม่เก่ง','Value':2}]";
        private string top5Mentions = @"[{'Key':'@SB19Official','Value':5},{'Key':'@thebox_movie','Value':2},{'Key':'@uratasama_kari','Value':2},{'Key':'@mngwell_','Value':1},{'Key':'@VijayVst0502','Value':1}]";
        private string top5Emojis = @"[{'Key':'😭','Value':14},{'Key':'❤','Value':9},{'Key':'✨','Value':8},{'Key':'🤣','Value':7},{'Key':'😂','Value':6}]";

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).TrimEnd(@"\bin".ToArray());
            var dataFilePath = Path.GetFullPath(Path.Combine(binDirectory, Constants.DataFileName));            
            var testDataString = File.ReadAllText(dataFilePath);
            testData = JsonConvert.DeserializeObject<List<ProcessedTweet>>(testDataString);
            
        }

        private TopValues CreateTopValues()
        {
            return new TopValues();
        }

        [TestMethod]
        public void TopDomains_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var topValues = this.CreateTopValues();
            List<string> urls = testData.Select(o => o.Urls).ToList();
            var list = JsonConvert.DeserializeObject<List<KeyValuePair<string, int>>>(top5domains);

            // Act
            var result = topValues.TopDomains(urls).Take(5).ToList();            

            for (int i = 0; i < result.Count; i++)
            {
                // Assert
                Assert.AreEqual(list[i], result[i]);
            }

            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public void TopValueHashtags_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var topValues = this.CreateTopValues();
            List<string> hashtags = testData.Select(o => o.Hashtags).ToList();
            var list = JsonConvert.DeserializeObject<List<KeyValuePair<string, int>>>(top5Hashtags);

            // Act
            var result = topValues.TopValue(hashtags).Take(5).ToList();
            
            for (int i = 0; i < result.Count; i++)
            {
                // Assert
                Assert.AreEqual(list[i], result[i]);
            }
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public void TopValueMentions_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var topValues = this.CreateTopValues();
            List<string> mentions = testData.Select(o => o.Mentions).ToList();
            var list = JsonConvert.DeserializeObject<List<KeyValuePair<string, int>>>(top5Mentions);
            
            // Act
            var result = topValues.TopValue(mentions).Take(5).ToList();
             
            for (int i = 0; i < result.Count; i++)
            {
                // Assert
                Assert.AreEqual(list[i], result[i]);
            }
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public void TopEmoji_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var topValues = this.CreateTopValues();
            List<string> emojis = testData.Select(o => o.Emojis).ToList();
            var list = JsonConvert.DeserializeObject<List<KeyValuePair<string, int>>>(top5Emojis);

            // Act
            var result = topValues.TopEmoji(emojis).Take(5).ToList();
             
            for (int i = 0; i < result.Count; i++)
            {
                // Assert
                Assert.AreEqual(list[i], result[i]);
            }
            this.mockRepository.VerifyAll();
        }
    }
}
