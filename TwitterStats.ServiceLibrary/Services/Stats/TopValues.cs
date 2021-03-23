using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TwitterStats.ServiceLibrary.Services.Stats
{
    public class TopValues : ITopValues
    {

        public List<KeyValuePair<string, int>> TopDomains(List<string> urls)
        {
            List<string> all = new List<string>();

            foreach (var list in urls)
            {
                var containedUrls = JsonConvert.DeserializeObject<List<string>>(list);
                if (containedUrls.Count() > 0)
                {
                    var domain = "";
                    foreach (var url in containedUrls)
                    {
                        if (url != null || url != "")
                        {
                            domain = new Uri(url).Host;
                            all.Add(domain);
                        }
                    }
                }
            }

            var result = all.GroupBy(o => o).ToDictionary(o => o.Key, p => p.Count()).OrderByDescending(o => o.Value).ToList();

            return result;
        }

        public List<KeyValuePair<string, int>> TopValue(List<string> hashtags)
        {
            List<string> all = new List<string>();

            foreach (var list in hashtags)
            {
                all = all.Concat(JsonConvert.DeserializeObject<List<string>>(list)).ToList();
            }

            var result = all.GroupBy(o => o).ToDictionary(o => o.Key, p => p.Count()).OrderByDescending(o => o.Value).ToList();

            return result;
        }

        public List<KeyValuePair<string, int>> TopEmoji(List<string> emojis)
        {
            List<string> all = new List<string>();

            foreach (var list in emojis)
            {
                if (list.Count() > 0)
                {
                    all = all.Concat(list.Split(",").ToList()).ToList();
                }
            }

            var result = all.GroupBy(o => o).ToDictionary(o => o.Key, p => p.Count()).OrderByDescending(o => o.Value).ToList();

            return result;
        }

    }
}
