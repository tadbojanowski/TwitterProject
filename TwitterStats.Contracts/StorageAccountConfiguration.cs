namespace TwitterStats.Contracts
{
    public class StorageAccountConfiguration
    {
        public string TwitterStreamTriggerQueue { get; set; }
        public string TwitterStreamTriggerQueuePoison { get; set; }
        public string RawTweetProcessQueue { get; set; }
        public string RawTweetProcessQueuePoison { get; set; }
    }
}
