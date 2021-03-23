using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterStats.ServiceLibrary.Services.QueueServices
{
    public class QueueUpTweetsQueue : QueueService, IQueueUpTweetsQueue
    {
        public QueueUpTweetsQueue(ICloudStorageAccountHandler cloudStorageAccountHandler)
            : base(cloudStorageAccountHandler) { }

    }
    public class QueueUpTweetsQueuePoison : QueueService, IQueueUpTweetsQueuePoison
    {
        public QueueUpTweetsQueuePoison(ICloudStorageAccountHandler cloudStorageAccountHandler)
            : base(cloudStorageAccountHandler) { }
    }

    public class IncommingTweetQueue : QueueService, IIncommingTweetQueue
    {
        public IncommingTweetQueue(ICloudStorageAccountHandler cloudStorageAccountHandler)
            : base(cloudStorageAccountHandler) { }
    }

    public class IncommingTweetQueuePoison : QueueService, IIncommingTweetQueuePoison
    {
        public IncommingTweetQueuePoison(ICloudStorageAccountHandler cloudStorageAccountHandler)
            : base(cloudStorageAccountHandler) { }
    }

    public class ProcessedIncommingQueue : QueueService, IProcessedIncommingQueue
    {
        public ProcessedIncommingQueue(ICloudStorageAccountHandler cloudStorageAccountHandler)
            : base(cloudStorageAccountHandler) { }
    }

    public class ProcessedIncommingQueuePoison : QueueService, IProcessedIncommingQueuePoison
    {
        public ProcessedIncommingQueuePoison(ICloudStorageAccountHandler cloudStorageAccountHandler)
            : base(cloudStorageAccountHandler) { }
    }

}
