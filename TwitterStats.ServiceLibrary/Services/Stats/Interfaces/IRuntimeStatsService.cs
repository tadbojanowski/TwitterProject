using System.Collections.Generic;
using TwitterStats.Contracts;

namespace TwitterStats.ServiceLibrary.Services.Stats
{
    public interface IRuntimeStatsService
    {
        List<ProcessedTweet> ProcessedTweets { get; set; }
        bool RunningCollection { get; set; }
    }
}
