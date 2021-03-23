using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterStats.Contracts
{
    public class TwitterConfiguration
    {
        public string BaseUrl { get; set; }
        public string OAuthToken { get; set; }
        public string SampleStreamUrl { get; set; }
    }
}
