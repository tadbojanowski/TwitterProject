using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using TwitterStats.Contracts;
using TwitterStats.ServiceLibrary.Services;
using TwitterStats.ServiceLibrary.Services.QueueServices;
using TwitterStats.ServiceLibrary.Services.Stats;

namespace TwitterStats.WebClient.Hubs
{
    public class StatsHub : Hub, IStatsHub
    {
        private readonly IProcessedIncommingQueue _processedIncommingQueue;
        private readonly IStatAggregation _statAggregation;
        private IRuntimeStatsService _runtimeStatsService;

        public StatsHub(IProcessedIncommingQueue processedIncommingQueue, IRuntimeStatsService runtimeStatsService, IStatAggregation statAggregation)
        {
            _processedIncommingQueue = processedIncommingQueue;
            _runtimeStatsService = runtimeStatsService;
            _statAggregation = statAggregation;
        }

        public async IAsyncEnumerable<AllStats> Counter([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            do
            {
                cancellationToken.ThrowIfCancellationRequested();
                AllStats allStats = new AllStats();
                try
                {
                    var nextTweet = await _processedIncommingQueue.DequeueMessage();
                    if (nextTweet != null)
                    {
                        ProcessedTweet tweet = JsonConvert.DeserializeObject<ProcessedTweet>(nextTweet.AsString);
                        _runtimeStatsService.ProcessedTweets.AddTweet(tweet);
                        allStats = _statAggregation.CollectStats(_runtimeStatsService.ProcessedTweets);
                    }
                }
                catch (Exception exc)
                {
                    throw exc;
                }
                yield return allStats;

            } while (true);
        }
    }
}
