using Microsoft.WindowsAzure.Storage.Queue;
using System.Threading.Tasks;

namespace TwitterStats.ServiceLibrary.Services.QueueServices
{

    public interface IQueueUpTweetsQueue
    {
        Task<bool> SendMessageOntoQueue(string message);
        Task<CloudQueueMessage> DequeueMessage();

    }
    public interface IQueueUpTweetsQueuePoison
    {
        Task<bool> SendMessageOntoQueue(string message);
        Task<CloudQueueMessage> DequeueMessage();
    }

    public interface IIncommingTweetQueue
    {
        Task<bool> SendMessageOntoQueue(string message);
        Task<CloudQueueMessage> DequeueMessage();
    }

    public interface IIncommingTweetQueuePoison
    {
        Task<bool> SendMessageOntoQueue(string message);
        Task<CloudQueueMessage> DequeueMessage();
    }

    public interface IProcessedIncommingQueue
    {
        Task<bool> SendMessageOntoQueue(string message);
        Task<CloudQueueMessage> DequeueMessage();
    }

    public interface IProcessedIncommingQueuePoison
    {
        Task<bool> SendMessageOntoQueue(string message);
        Task<CloudQueueMessage> DequeueMessage();
    }

}
