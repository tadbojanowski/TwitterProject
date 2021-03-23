using System.Threading;
using System.Threading.Tasks;

namespace TwitterStats.WebClient.Hubs
{
    public interface IQueueHub
    {
        Task<string> QueueUpTweets(CancellationToken cancellationToken); 
    }
}
