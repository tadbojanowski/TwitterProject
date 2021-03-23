using System.IO;

namespace TwitterStats.ServiceLibrary.Services.QueueServices
{
    public interface IStreamReaderHandler
    {
        StreamReader GetStreamReader(Stream stream);
    }

}
