using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TwitterStats.ServiceLibrary.Services.QueueServices
{
    public class HttpClientProvider : IHttpClientProvider
    {
        private readonly HttpClient _httpClient;

        public HttpClientProvider()
        {
            _httpClient = new HttpClient();
        }

        public Task<Stream> GetStreamAsync(string requestUri)
        {
            return _httpClient.GetStreamAsync(requestUri);
        }
        public Uri BaseAddress { get { return _httpClient.BaseAddress; } set { _httpClient.BaseAddress = value; } }
        public HttpRequestHeaders DefaultRequestHeaders 
        { 
            get 
            { 
                return _httpClient.DefaultRequestHeaders; 
            }            
        }
    }
}
