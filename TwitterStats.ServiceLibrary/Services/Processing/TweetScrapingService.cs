using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwitterStats.ServiceLibrary.Services.Processing
{
    public class TweetScrapingService : ITweetScrapingService
    {
         
        public Task<List<string>> CollectDataWithRegex(string pattern, string tweet, int tweetCount, ILogger logger)
        {
            List<string> list = new List<string>();

            try
            {
                Regex regex = new Regex(pattern);
                MatchCollection ms = regex.Matches(tweet);

                foreach (var split in ms.ToArray())
                {
                    string individual = split.ToString();                    
                    list.Add(individual);
                }
                logger.LogInformation($"regex tweet {tweetCount} ");
            }
            catch (Exception exc)
            {
                logger.LogDebug($"Collect with Regex process failed: {exc.Message} {exc.StackTrace}");
                throw exc;
            }
            logger.LogInformation($"Collect with Regex tweet {tweetCount} ");
            return Task.FromResult(list);
        }

        public string GetEmojisFromString(string tweet, string latestEmoji, int tweetCount, ILogger logger)
        {
            string result = "";
            try
            {
                string[] emojis = latestEmoji.Split(";");
                foreach (var emoji in emojis)
                {
                    string searchString = string.Join(string.Empty, emoji.Split('-').Select(hex => char.ConvertFromUtf32(Convert.ToInt32(hex, 16))));

                    if (tweet.Contains(searchString))
                    {
                        var count = Regex.Matches(tweet, searchString).Count;
                        for (int i = 0; i < count; i++)
                        {
                            result += searchString + ",";
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                logger.LogDebug($"Get Emojis process failed: {exc.Message} {exc.StackTrace}");
                throw exc;
            }
            logger.LogInformation($"Get Emojis tweet {tweetCount} ");
            return result.TrimEnd(',');
        }
         
        public async Task<List<string>> LengthenUrls(IHttpClientFactory httpClientFactory, List<string> urls, int tweetCount, ILogger logger)
        {
            List<string> hosts = new List<string>();
            try
            {
                HttpClient httpClient = httpClientFactory.CreateClient("LengthenUrls");
                foreach (var item in urls)
                {
                    try
                    {
                        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri(item));
                        var result = await httpClient.SendAsync(httpRequestMessage);
                        hosts.Add(result.RequestMessage.RequestUri.ToString());
                    }
                    catch (Exception exc)
                    {
                        logger.LogDebug($"Url Lengthen process failed: {exc.Message} ");                         
                    }                    
                }                
            }
            catch (Exception exc)
            {
                logger.LogDebug($"Url Lengthen process failed: {exc.Message} ");                 
            }
            logger.LogInformation($"Url Lengthen tweet {tweetCount} ");
            return hosts;
        }
    }
}
