using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TwitterStats.Contracts;
using TwitterStats.ServiceLibrary.Services;
using TwitterStats.ServiceLibrary.Services.QueueServices;
using TwitterStats.ServiceLibrary.Services.Stats;

namespace TwitterStats.WebClient.Hubs
{

    public class QueueHub : Hub, IQueueHub
    {
        //private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly IIncommingTweetQueue _incommingTweetQueue;
        private readonly TwitterConfiguration _twitterConfiguration;
        private readonly IStreamService _streamService;
        private IRuntimeStatsService _runtimeStatsService;
        private readonly IStreamReaderHandler _streamReaderHandler;

        public QueueHub(IHttpClientProvider httpClientProvider, IIncommingTweetQueue incommingTweetQueue, IConfiguration configuration, 
                                        IRuntimeStatsService runtimeStatsService, IStreamService streamService, IStreamReaderHandler streamReaderHandler)
        {
            _httpClientProvider = httpClientProvider;
            _incommingTweetQueue = incommingTweetQueue;
            _runtimeStatsService = runtimeStatsService;
            _streamService = streamService;
            _streamReaderHandler = streamReaderHandler;

            _twitterConfiguration = new TwitterConfiguration()
            {
                BaseUrl         = configuration.GetValue<string>(Constants.TwitterBaseUrlName).ToString(),
                OAuthToken      = configuration.GetValue<string>(Constants.TwitterOAuthTokenName).ToString(),
                SampleStreamUrl = configuration.GetValue<string>(Constants.TwitterSampleStreamUrlName).ToString()
            };
        }

        public async Task<string> QueueUpTweets(CancellationToken cancellationToken)
        {
            _runtimeStatsService.RunningCollection = true;
            string result = "Reciving Tweets...";             
            try
            {
                result = await _streamService.ReciveTweets(_httpClientProvider, _twitterConfiguration, _incommingTweetQueue, _streamReaderHandler, cancellationToken);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return result;
        }
    }
}
