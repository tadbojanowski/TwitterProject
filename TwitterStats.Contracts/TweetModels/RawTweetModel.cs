using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterStats.Contracts.TweetModels
{
    public class RawTweetModel : TableEntity
    {
        public int TweetCount { get; set; }
        public string RawTweetData { get; set; }
    }



}
