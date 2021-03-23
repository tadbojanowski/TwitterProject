using System.IO;

namespace TwitterStats.ServiceLibrary.Services.QueueServices
{
    public class StreamReaderHandler : IStreamReaderHandler
    {
        public StreamReader GetStreamReader(Stream stream)
        {
            return new StreamReader(stream);
        }
    }

}
