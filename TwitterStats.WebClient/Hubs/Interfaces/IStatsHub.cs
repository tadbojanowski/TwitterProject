using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using TwitterStats.Contracts;

namespace TwitterStats.WebClient.Hubs
{
    public interface IStatsHub
    {
        IAsyncEnumerable<AllStats> Counter(CancellationToken cancellationToken);
    }
}