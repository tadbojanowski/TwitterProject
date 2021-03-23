using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TwitterStats.Contracts;
using TwitterStats.Contracts.TweetModels;

namespace TwitterStats.ServiceLibrary.Services.QueueServices
{
    public class StreamService : IStreamService
    {
        public async Task<string> ReciveTweets(IHttpClientProvider client, TwitterConfiguration twitterConfiguration
                                , IIncommingTweetQueue incommingTweetQueue, IStreamReaderHandler streamReaderHandler, CancellationToken cancellationToken)
        {
            int tweetCount = 0;
            var tasks = new List<Task>();
            DateTime startRun = DateTime.UtcNow;

            try
            {

                client.BaseAddress = new Uri(twitterConfiguration.BaseUrl);

                if (client != null)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + twitterConfiguration.OAuthToken);
                    var stream = await client.GetStreamAsync(twitterConfiguration.SampleStreamUrl);

                    try
                    {
                        using (var reader = streamReaderHandler.GetStreamReader(stream))
                        {
                            do
                            {                                
                                var rawTweetData = await reader.ReadLineAsync();
                                RawTweetModel messageWithData = new RawTweetModel() { TweetCount = tweetCount, RawTweetData = rawTweetData };
                                // send new tweet onto queue for processing                                 
                                tasks.Add(Task.Run(() => incommingTweetQueue.SendMessageOntoQueue(JsonConvert.SerializeObject(messageWithData))));
                                tweetCount++;
                                cancellationToken.ThrowIfCancellationRequested();
                            } while (true);
                        }
                        //
                    }
                    catch (OperationCanceledException)
                    {
                        // canceled
                    }
                    catch (Exception exc)
                    {
                        throw exc;
                    }
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            Task.WaitAll(tasks.ToArray());

            return "Done Collecting Tweets... ";
        }
    }
}
