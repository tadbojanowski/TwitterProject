using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TwitterStats.ServiceLibrary.Services.QueueServices
{
    public interface IHttpClientProvider
    {
        Task<Stream> GetStreamAsync(string requestUri);
        Uri BaseAddress { get; set; }
        HttpRequestHeaders DefaultRequestHeaders { get; }
    }
}
