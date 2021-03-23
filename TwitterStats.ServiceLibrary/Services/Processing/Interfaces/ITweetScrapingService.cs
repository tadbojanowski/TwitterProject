using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TwitterStats.ServiceLibrary.Services.Processing
{
    public interface ITweetScrapingService
    { 
        Task<List<string>> CollectDataWithRegex(string pattern, string tweet, int tweetCount, ILogger logger);
        string GetEmojisFromString(string tweet, string latestEmoji, int tweetCount, ILogger logger);
        Task<List<string>> LengthenUrls(IHttpClientFactory httpClientFactory, List<string> urls, int tweetCount, ILogger logger);
    }
}