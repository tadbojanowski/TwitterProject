using System.Collections.Generic;
using System.Linq;
using TwitterStats.Contracts;

namespace TwitterStats.ServiceLibrary.Services.Stats
{
    public class PercentageValues : IPercentageValues
    {
        public double GetPercentage(List<ProcessedTweet> records, SearchType searchType)
        {
            double sum = 0;
            switch (searchType)
            {
                case SearchType.InstagramLink:
                    sum = records.Where(o => o.InstagramLink != "[]").Count();
                    break;
                case SearchType.TwitterPicture:
                    sum = records.Where(o => o.TwitterPicture != "[]").Count();
                    break;
                case SearchType.Emojis:
                    sum = records.Where(o => o.Emojis != "").Count();
                    break;
                case SearchType.Urls:
                    sum = records.Where(o => o.Urls != "[]").Count();
                    break;
                default:
                    break;
            }
            return (sum / records.Count) * 100.0;
        }
    }
}
