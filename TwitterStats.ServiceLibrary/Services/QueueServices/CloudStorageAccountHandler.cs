using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Threading.Tasks;

namespace TwitterStats.ServiceLibrary.Services.QueueServices
{
    public class CloudStorageAccountHandler : ICloudStorageAccountHandler
    {
        private readonly CloudStorageAccount _storageAccount;
        private readonly CloudQueueClient _queueClient;
        private readonly CloudQueue _queue;
        public CloudStorageAccountHandler()
        { 

        }
        public CloudStorageAccountHandler(string connectionString, string queueName)
        {
            _storageAccount = CloudStorageAccount.Parse(connectionString);
            _queueClient = _storageAccount.CreateCloudQueueClient();
            _queue = _queueClient.GetQueueReference(queueName);
            _queue.CreateIfNotExistsAsync().GetAwaiter().GetResult();
        }

        public int Id { get; set; }

        public async Task<CloudQueueMessage> GetMessageAsync()
        {
            return await _queue.GetMessageAsync();
        }

        public async Task<bool> DeleteMessageAsync(CloudQueueMessage message)
        {
            await _queue.DeleteMessageAsync(message);
            return true;
        }

        public async Task<bool> AddMessageAsync(CloudQueueMessage message)
        {
            await _queue.AddMessageAsync(message);
            return true;
        }
    }
}

 