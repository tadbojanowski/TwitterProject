using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterStats.Contracts
{
    public static class Constants
    {
        public const string StorageConnectionName         = "AzureWebJobsStorage";
        public const string TwitterBaseUrlName            = "Twitter:BaseUrl";
        public const string TwitterOAuthTokenName         = "Twitter:OAuthToken";
        public const string TwitterSampleStreamUrlName    = "Twitter:SampleStreamUrl";
        public const string TwitterSampleStreamRetryCount = "Twitter:SampleStreamRetryCount";
        public const string UrlLengthenRetryCount         = "Twitter:UrlLengthenRetryCount";
        public const string PartitionKeyFormat            = "tweets";

        public const string AzureStorageTweetsTableName          = "ProcessTweets";
        public const string AzureStorageTotalsPerSecondTableName = "TotalsPerSecond";
        public const string AzureStorageTotalsPerMinuteTableName = "TotalsPerMinute";
        public const string AzureStorageTotalsPerHourTableName   = "TotalsPerHour";

        public const string QueueUpTweetsQueue                = "queue-up-tweets";
        public const string QueueUpTweetsQueuePoison          = "queue-up-tweets-poison";
        public const string IncommingTweetQueue               = "incomming-tweet";
        public const string IncommingTweetQueuePoison         = "incomming-tweet-poison";
        public const string ProcessedIncommingQueue           = "processed-incomming-tweet";
        public const string ProcessedIncommingQueuePoison     = "processed-incomming-tweet-poison";


        public const string StatsSecondQueue       = "stats-second-tweet";
        public const string StatsSecondQueuePoison = "stats-second-tweet-poison";
        public const string StatsMinuteQueue       = "stats-minute-tweet";
        public const string StatsMinuteQueuePoison = "stats-minute-tweet-poison";
        public const string StatsHourQueue         = "stats-hour-incomming-tweet";
        public const string StatsHourQueuePoison   = "stats-hour-incomming-tweet-poison";

        //public const string EmojiListFileName = "EmojiListFileName";
        //public const string RegexMentions     = "Regex:Mentions";
        //public const string RegexHashtags     = "Regex:Hashtags";
        //public const string RegexLinks        = "Regex:Links";
        //public const string RegexInstagram    = "Regex:Instagram";
        //public const string RegexTwitter      = "Regex:Twitter";

        public const string DataFileRawTweetName = "TestData\\test_data_tweet.json";
        public const string AppSettingsJson      = "TestData\\appsettings.Development.json";
        public const string DataFileName         = "TestData\\table_data.json";
        public const string DataFileMentionsName = "TestData\\test_data_mentions.json";
        public const string DataFileHashtagsName = "TestData\\test_data_hashtags.json";
        public const string DataFileLinksName    = "TestData\\test_data_links.json";
        public const string DataFileEmojisName   = "TestData\\test_data_emojis.json";
        public const string DataFileAllStatsName = "TestData\\test_data_all_stats.json";
        public const string EmojiCodesFileName   = "RuntimeData\\emojiCodesUCompleteDash.txt";

        public const string RegexMentions     = "@\\w+";
        public const string RegexHashtags     = "#\\w+";
        public const string RegexLinks        = "(http|https|ftp|)\\:\\/\\/[0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*(:(0-9)*)*(\\/?)([a-zA-Z0-9\\-\\.\\?\\,\\'\\/\\\\\\+&%\\$#_]*)?([a-zA-Z0-9\\-\\?\\,\\'\\/\\+&%\\$#_]+)";
        public const string RegexInstagram    = "(?=.*\\binstagram\\b)(?=.*\\b/p/\\b)";
        public const string RegexTwitter      = "(?=.*\\btwitter\\b)(?=.*\\b/photo/\\b)";

    }

    public enum SearchType
    {
        InstagramLink,
        TwitterPicture,
        Emojis,
        Urls
    }
    public enum TimeDivision
    {
        PerSecond,
        PerMinute,
        PerHour
    }
}
