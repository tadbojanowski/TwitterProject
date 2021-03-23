using Microsoft.WindowsAzure.Storage.Queue;
using System.Threading.Tasks;

namespace TwitterStats.ServiceLibrary.Services.QueueServices
{
    public interface IQueueService
    {
        Task<bool> SendMessageOntoQueue(string message);
        Task<CloudQueueMessage> DequeueMessage();
    }
}
