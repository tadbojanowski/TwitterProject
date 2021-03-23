using Microsoft.WindowsAzure.Storage.Queue;
using System.Threading.Tasks;

namespace TwitterStats.ServiceLibrary.Services.QueueServices
{
    public interface ICloudStorageAccountHandler
    {
        public int Id { get; set; }
        Task<bool> AddMessageAsync(CloudQueueMessage message);
        Task<bool> DeleteMessageAsync(CloudQueueMessage message);
        Task<CloudQueueMessage> GetMessageAsync();
    }
}

 