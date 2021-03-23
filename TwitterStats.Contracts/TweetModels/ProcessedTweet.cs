using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.WindowsAzure.Storage.Table;

namespace TwitterStats.Contracts
{
    public class ProcessedTweet : TableEntity
    {
        public int TweetCount { get; set; }
        public DateTime TweetStamp { get; set; }
        public string YearMonthDay { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
        public string Data { get; set; }

        public string Text { get; set; }

        public string Emojis { get; set; }
        public string Mentions { get; set; }
        public string Urls { get; set; }
        public string Hashtags { get; set; }

        public string TwitterPicture { get; set; }

        public string InstagramLink { get; set; }
    }


}
