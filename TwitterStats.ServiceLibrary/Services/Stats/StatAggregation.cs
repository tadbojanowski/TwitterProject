using System;
using System.Collections.Generic;
using System.Linq;
using TwitterStats.Contracts; 

namespace TwitterStats.ServiceLibrary.Services.Stats
{
    public class StatAggregation : IStatAggregation
    {
        private readonly ITopValues _topValues;
        private readonly IAverageValues _averageValues;
        private readonly IPercentageValues _percentageValues;

        public StatAggregation(ITopValues topValues, IAverageValues averageValues, IPercentageValues percentageValues)
        {
            _topValues = topValues;
            _averageValues = averageValues;
            _percentageValues = percentageValues;
        }

        public AllStats CollectStats(List<ProcessedTweet> records)
        {
            AllStats allStats = new AllStats();
            allStats.Averages = new Averages();
            allStats.Stats = new Statistics();

            try
            {
                if (records.Count > 0)
                {
                    allStats.Averages.TotalRecived = records.Count();

                    allStats.Averages.PerSecondAverage = _averageValues.GetAverage(records, TimeDivision.PerSecond);
                    allStats.Averages.PerMinuteAverage = _averageValues.GetAverage(records, TimeDivision.PerMinute);
                    allStats.Averages.PerHourAverage = _averageValues.GetAverage(records, TimeDivision.PerHour);

                    allStats.Stats.TopEmojis = string.Join(",", _topValues.TopEmoji(records.Select(o => o.Emojis).ToList()).Take(5));
                    allStats.Stats.TopHashtags = string.Join(",", _topValues.TopValue(records.Select(o => o.Hashtags).ToList()).Take(5));
                    allStats.Stats.TopMentions = string.Join(",", _topValues.TopValue(records.Select(o => o.Mentions).ToList()).Take(5));
                    allStats.Stats.TopDomains = string.Join(",", _topValues.TopDomains(records.Select(o => o.Urls).ToList()).Take(5));

                    var ercentInstagramPics = _percentageValues.GetPercentage(records, SearchType.InstagramLink);
                    var ercentTwitterPics = _percentageValues.GetPercentage(records, SearchType.TwitterPicture);

                    allStats.Stats.PercentPics = ercentInstagramPics + ercentTwitterPics;
                    allStats.Stats.PercentEmojis = _percentageValues.GetPercentage(records, SearchType.Emojis);
                    allStats.Stats.PercentUrls = _percentageValues.GetPercentage(records, SearchType.Urls);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return allStats;
        }
    }
}
