using System.Collections.Generic;
using TwitterStats.Contracts;

namespace TwitterStats.ServiceLibrary.Services.Stats
{
    public interface IAverageValues
    {
        double GetAverage(List<ProcessedTweet> records, TimeDivision timeDivision);
    }
}