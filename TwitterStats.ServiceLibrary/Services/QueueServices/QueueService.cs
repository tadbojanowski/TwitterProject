using Microsoft.WindowsAzure.Storage.Queue;
using System.Threading.Tasks;

namespace TwitterStats.ServiceLibrary.Services.QueueServices
{
    public class QueueService : IQueueService
    {
        private readonly ICloudStorageAccountHandler _cloudStorageAccountHandler;
        public QueueService(ICloudStorageAccountHandler cloudStorageAccountHandler)
        {
            _cloudStorageAccountHandler = cloudStorageAccountHandler;
        }
        public virtual async Task<bool> SendMessageOntoQueue(string message)
        {
            CloudQueueMessage queueMessage = new CloudQueueMessage(message);
            var result = await _cloudStorageAccountHandler.AddMessageAsync(queueMessage);
            return result;
        }

        public async Task<CloudQueueMessage> DequeueMessage()
        {
            // Get the next message
            CloudQueueMessage retrievedMessage = await _cloudStorageAccountHandler.GetMessageAsync();
            // Delete the message
            if (retrievedMessage != null)
            {
                if(!await _cloudStorageAccountHandler.DeleteMessageAsync(retrievedMessage))
                {
                    throw new System.Exception("Failed to DeleteMessageAsync");
                }
            }
            return retrievedMessage;
        }
    }
}

 