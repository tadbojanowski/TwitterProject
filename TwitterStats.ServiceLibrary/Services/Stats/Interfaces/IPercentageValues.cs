using System.Collections.Generic;
using TwitterStats.Contracts;

namespace TwitterStats.ServiceLibrary.Services.Stats
{
    public interface IPercentageValues
    {
        double GetPercentage(List<ProcessedTweet> records, SearchType searchType);
    }
}