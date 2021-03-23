using System;
using System.Collections.Generic;
using System.Linq;
using TwitterStats.Contracts;

namespace TwitterStats.ServiceLibrary.Services.Stats
{
    public class AverageValues : IAverageValues
    {
        public double GetAverage(List<ProcessedTweet> records, TimeDivision timeDivision)
        {
            try
            {
                if (records.Count > 0)
                {
                    switch (timeDivision)
                    {
                        case TimeDivision.PerSecond:
                            var groupSec = records.GroupBy(o => new { o.YearMonthDay, o.Hour, o.Minute, o.Second }).Select(o => new { key = o.Key, count = o.Count() });
                            var sumSec = groupSec.Select(o => o.count).Sum<int>(sel => sel);
                            var totalSec = groupSec.Count();
                            return sumSec / totalSec;

                        case TimeDivision.PerMinute:
                            var groupMin = records.GroupBy(o => new { o.YearMonthDay, o.Hour, o.Minute }).Select(o => new { key = o.Key, count = o.Count() });
                            var sumMin = groupMin.Select(o => o.count).Sum<int>(sel => sel);
                            var totalMin = groupMin.Count();
                            return sumMin / totalMin;

                        case TimeDivision.PerHour:
                            var groupHr = records.GroupBy(o => new { o.YearMonthDay, o.Hour }).Select(o => new { key = o.Key, count = o.Count() });
                            var sumHr = groupHr.Select(o => o.count).Sum<int>(sel => sel);
                            var totalHr = groupHr.Count();
                            return sumHr / totalHr;

                        default:
                            return 0.0;
                    }
                }
                return 0.0;
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}
