using System.Collections.Generic;
using System.Linq;
using TwitterStats.Contracts;

namespace TwitterStats.ServiceLibrary.Services.Stats
{
    public static class Extensions
    {
        public static void AddTweet(this List<ProcessedTweet> list, ProcessedTweet processedTweet)
        {
            if(!list.Any(o => o.RowKey == processedTweet.RowKey))
            {
                list.Add(processedTweet);
            }
        }
    }
}
