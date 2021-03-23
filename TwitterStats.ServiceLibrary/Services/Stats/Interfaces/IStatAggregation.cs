using System.Collections.Generic;
using TwitterStats.Contracts;

namespace TwitterStats.ServiceLibrary.Services.Stats
{
    public interface IStatAggregation
    {
        AllStats CollectStats(List<ProcessedTweet> records);
    }
}