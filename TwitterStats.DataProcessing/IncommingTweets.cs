using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TwitterStats.Contracts;
using TwitterStats.Contracts.TweetModels;
using TwitterStats.ServiceLibrary.Services;
using TwitterStats.ServiceLibrary.Services.Processing;
using TwitterStats.ServiceLibrary.Services.QueueServices;

namespace TwitterStats.DataProcessing
{
    public class IncommingTweets
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IIncommingTweetQueue _incommingTweetQueue;
        private readonly IIncommingTweetQueuePoison _incommingTweetQueuePoison;
        private readonly IProcessedIncommingQueue _processedIncommingQueue;
        private readonly ITweetScrapingService _tweetProcessingService;
        private readonly ITweetAggregationService _tweetAggregationService;

        private readonly string _knownEmojis;
        public IncommingTweets(IHttpClientFactory httpClientFactory, IProcessedIncommingQueue processedIncommingQueue, 
                                IIncommingTweetQueue incommingTweetQueue, ITweetScrapingService tweetProcessingService,
                                IIncommingTweetQueuePoison incommingTweetQueuePoison, ITweetAggregationService tweetAggregationService)
        {
           
            var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).TrimEnd(@"\bin".ToArray());
            var emojiFilePath = Path.GetFullPath(Path.Combine(binDirectory, Constants.EmojiCodesFileName));             
            _knownEmojis = File.ReadAllText(emojiFilePath);

            _httpClientFactory         = httpClientFactory;
            _processedIncommingQueue   = processedIncommingQueue;
            _incommingTweetQueue       = incommingTweetQueue;
            _incommingTweetQueuePoison = incommingTweetQueuePoison;
            _tweetProcessingService    = tweetProcessingService;
            _tweetAggregationService   = tweetAggregationService;
        }

        [FunctionName("IncommingTweets")]
        public async Task Run([QueueTrigger(Constants.IncommingTweetQueue, Connection = Constants.StorageConnectionName)] string myQueueItem, ILogger _logger)
        {
            var tasks = new List<Task>();
            RawTweetModel rawTweetDataMessage = JsonConvert.DeserializeObject<RawTweetModel>(myQueueItem);

            try
            {
                await Task.Run(() => _tweetAggregationService.ProcessIncommingTweet(_httpClientFactory,rawTweetDataMessage?.RawTweetData, _knownEmojis, rawTweetDataMessage.TweetCount, _logger));
            }
            catch (Exception exc)
            {
                _logger.LogError($"IncommingTweets, re-queue it: {exc.Message} {exc.StackTrace}");                
                await Task.Run(() => _incommingTweetQueuePoison.SendMessageOntoQueue(JsonConvert.SerializeObject($@"{exc.Message} - {exc.StackTrace} - {myQueueItem}")));
            }
            _logger.LogInformation($"IncommingTweets process finished: {rawTweetDataMessage.TweetCount}");
        }

    }
}
