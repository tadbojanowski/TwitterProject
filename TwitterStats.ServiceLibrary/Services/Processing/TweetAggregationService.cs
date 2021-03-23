using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TwitterStats.Contracts;
using TwitterStats.ServiceLibrary.Services.QueueServices;

namespace TwitterStats.ServiceLibrary.Services.Processing
{
    public class TweetAggregationService : ITweetAggregationService
    {
        private readonly IProcessedIncommingQueue _processedIncommingQueue;
        private readonly ITweetScrapingService _tweetScrapingService;

        public TweetAggregationService(IProcessedIncommingQueue processedIncommingQueue, ITweetScrapingService tweetScrapingService)
        {
            _processedIncommingQueue = processedIncommingQueue;
            _tweetScrapingService = tweetScrapingService;
        }

        public async Task ProcessIncommingTweet(IHttpClientFactory httpClientFactory, string rawTweetData, string knownEmojis, int tweetCount, ILogger logger)
        {
            var tasks = new List<Task<string>>();
            try
            {
                if (rawTweetData != null)
                {
                    Tweet tweet = JsonConvert.DeserializeObject<Tweet>(rawTweetData);
                    DateTime stamp;
                    DateTime.TryParse(tweet?.data?.created_at, out stamp);
                    string partitionKey = Constants.PartitionKeyFormat;

                    if (tweet != null && stamp != null && tweet?.data?.id != null)
                    {
                        var mentionsTask = Task.Run(() => _tweetScrapingService.CollectDataWithRegex(Constants.RegexMentions, tweet?.data?.text, tweetCount, logger));
                        var hashTagTask = Task.Run(() => _tweetScrapingService.CollectDataWithRegex(Constants.RegexHashtags, tweet?.data?.text, tweetCount, logger));
                        var urlTask = Task.Run(() => _tweetScrapingService.CollectDataWithRegex(Constants.RegexLinks, tweet?.data?.text, tweetCount, logger));
                        var emojiTask = Task.Run(() => _tweetScrapingService.GetEmojisFromString(tweet?.data?.text, knownEmojis, tweetCount, logger));

                        Task.WaitAll(urlTask, emojiTask, hashTagTask, mentionsTask);

                        var urlLengthenTask = await Task.Run(() => _tweetScrapingService.LengthenUrls(httpClientFactory, urlTask.Result, tweetCount, logger));



                        Regex regexInstagram = new Regex(Constants.RegexInstagram);
                        Regex regexTwitter = new Regex(Constants.RegexTwitter);

                        ProcessedTweet rawTweet = new ProcessedTweet()
                        {
                            TweetCount = tweetCount,
                            Hour = stamp.Hour,
                            Minute = stamp.Minute,
                            Second = stamp.Second,
                            Text = tweet?.data?.text,
                            Hashtags = JsonConvert.SerializeObject(hashTagTask.Result),
                            Urls = JsonConvert.SerializeObject(urlLengthenTask),
                            Emojis = emojiTask.Result,
                            Mentions = JsonConvert.SerializeObject(mentionsTask.Result.ToList()),
                            TwitterPicture = JsonConvert.SerializeObject(urlLengthenTask.Where(o => regexTwitter.IsMatch(o)).ToList()),
                            InstagramLink = JsonConvert.SerializeObject(urlLengthenTask.Where(o => regexInstagram.IsMatch(o)).ToList()),
                            PartitionKey = partitionKey,
                            RowKey = tweet?.data?.id,
                            TweetStamp = stamp,
                            YearMonthDay = stamp.Year.ToString() + stamp.Month.ToString() + stamp.Day.ToString()
                        };

                        await _processedIncommingQueue.SendMessageOntoQueue(JsonConvert.SerializeObject(rawTweet));

                        logger.LogInformation($"ProcessIncommingTweet saved tweet {tweetCount} ");
                    }
                }
            }
            catch (Exception exc)
            {
                logger.LogError($"Process Incomming Tweet process failed: {exc.Message} {exc.StackTrace}");
                //throw exc;
            }
            logger.LogInformation($"Process Incomming Tweet {tweetCount} ");
        }
    }
}
