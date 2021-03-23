using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using TwitterStats.Contracts;
using TwitterStats.ServiceLibrary.Services;
using TwitterStats.ServiceLibrary.Services.QueueServices;
using TwitterStats.ServiceLibrary.Services.Stats;
using TwitterStats.WebClient.Hubs;

namespace TwitterStats.WebClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

         
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            string storageConnectionString = Configuration.GetValue<string>(Constants.StorageConnectionName).ToString();
             
            int sampleStreamRetryCount = Int32.Parse(Configuration.GetValue<string>(Constants.TwitterSampleStreamRetryCount).ToString());
            int urlLengthenRetryCount = Int32.Parse(Configuration.GetValue<string>(Constants.UrlLengthenRetryCount).ToString());

            services.AddSingleton<IRuntimeStatsService, RuntimeStatsService>();
            services.AddTransient<ITopValues, TopValues>();
            services.AddTransient<IAverageValues, AverageValues>();
            services.AddTransient<IPercentageValues, PercentageValues>();
            services.AddTransient<IStatAggregation, StatAggregation>();
            services.AddTransient<IStreamService, StreamService>();
            services.AddTransient<IStreamReaderHandler, StreamReaderHandler>();
            services.AddTransient<IHttpClientProvider, HttpClientProvider>();

            services.AddHttpClient<IQueueHub, QueueHub>().AddPolicyHandler(GetRetryPolicy(sampleStreamRetryCount));  

            services.AddTransient<IIncommingTweetQueue>(_incommingTweetQueue => new IncommingTweetQueue(new CloudStorageAccountHandler(storageConnectionString, Constants.IncommingTweetQueue)));
            services.AddTransient<IProcessedIncommingQueue>(_processedIncommingQueue => new ProcessedIncommingQueue(new CloudStorageAccountHandler(storageConnectionString, Constants.ProcessedIncommingQueue)));

            services.AddAntiforgery(option => option.HeaderName = "X-XSRF-TOKEN");

            services.AddRazorPages();
            services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;
                o.KeepAliveInterval = TimeSpan.FromSeconds(3);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages(); 
                endpoints.MapHub<StatsHub>("/statsHub");  
                endpoints.MapHub<QueueHub>("/queueHub");
            });
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
