using System;
using System.Collections.Generic;
using System.Text;
using TwitterStats.Contracts;

namespace TwitterStats.ServiceLibrary.Services.Stats
{

    public class RuntimeStatsService : IRuntimeStatsService
    {
        private List<ProcessedTweet> _processedTweets;

        public RuntimeStatsService()
        {
            _processedTweets = new List<ProcessedTweet>();
        }

        public List<ProcessedTweet> ProcessedTweets
        {
            get
            {
                return _processedTweets;
            }
            set
            {
                _processedTweets = value;
            }
        }

        public bool RunningCollection { get; set; }
    }
}
