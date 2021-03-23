using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using TwitterStats.Contracts;
using TwitterStats.ServiceLibrary;
using TwitterStats.ServiceLibrary.Services;
using TwitterStats.ServiceLibrary.Services.Processing;
using TwitterStats.ServiceLibrary.Services.QueueServices;

[assembly: FunctionsStartup(typeof(TwitterStats.DataProcessing.Startup))]
namespace TwitterStats.DataProcessing
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string storageConnectionString = Environment.GetEnvironmentVariable(Constants.StorageConnectionName).ToString();
            int sampleStreamRetryCount = Int32.Parse(Environment.GetEnvironmentVariable(Constants.TwitterSampleStreamRetryCount).ToString());
            int urlLengthenRetryCount = Int32.Parse(Environment.GetEnvironmentVariable(Constants.UrlLengthenRetryCount).ToString());

            builder.Services.AddHttpClient<IncommingTweets>().AddPolicyHandler(GetRetryPolicy(urlLengthenRetryCount));
            builder.Services.AddTransient<ITweetScrapingService, TweetScrapingService>();
            builder.Services.AddTransient<ITweetAggregationService, TweetAggregationService>();

            builder.Services.AddTransient<IQueueUpTweetsQueue>(_queueUpTweets => new QueueUpTweetsQueue(new CloudStorageAccountHandler( storageConnectionString, Constants.QueueUpTweetsQueue)));
            builder.Services.AddTransient<IQueueUpTweetsQueuePoison>(_queueUpTweetsPoison => new QueueUpTweetsQueuePoison(new CloudStorageAccountHandler(storageConnectionString, Constants.QueueUpTweetsQueuePoison)));

            builder.Services.AddTransient<IIncommingTweetQueue>(_incommingTweetQueue => new IncommingTweetQueue(new CloudStorageAccountHandler(storageConnectionString, Constants.IncommingTweetQueue)));
            builder.Services.AddTransient<IIncommingTweetQueuePoison>(_incommingTweetQueuePoison => new IncommingTweetQueuePoison(new CloudStorageAccountHandler(storageConnectionString, Constants.IncommingTweetQueuePoison)));

            builder.Services.AddTransient<IProcessedIncommingQueue>(_processedIncommingQueue => new ProcessedIncommingQueue(new CloudStorageAccountHandler(storageConnectionString, Constants.ProcessedIncommingQueue)));
            builder.Services.AddTransient<IProcessedIncommingQueuePoison>(_processedIncommingQueue => new ProcessedIncommingQueuePoison(new CloudStorageAccountHandler(storageConnectionString, Constants.ProcessedIncommingQueuePoison)));
              
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
     
}
