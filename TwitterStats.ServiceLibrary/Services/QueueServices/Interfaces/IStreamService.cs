using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TwitterStats.Contracts;

namespace TwitterStats.ServiceLibrary.Services.QueueServices
{
    public interface IStreamService
    {
        //Task<string> ReciveTweets(IHttpClientFactory httpClientFactory, TwitterConfiguration twitterConfiguration, IIncommingTweetQueue incommingTweetQueue, IStreamReaderHandler streamReaderHandler);
        Task<string> ReciveTweets(IHttpClientProvider clientProvider, TwitterConfiguration twitterConfiguration, IIncommingTweetQueue incommingTweetQueue, IStreamReaderHandler streamReaderHandler, CancellationToken cancellationToken);
    }
}