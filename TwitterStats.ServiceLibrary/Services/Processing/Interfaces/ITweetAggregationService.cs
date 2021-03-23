using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace TwitterStats.ServiceLibrary.Services.Processing
{
    public interface ITweetAggregationService
    {
        Task ProcessIncommingTweet(IHttpClientFactory httpClientFactory, string rawTweetData, string knownEmojis, int tweetCount, ILogger logger);
    }
}