using System.Collections.Generic;

namespace TwitterStats.ServiceLibrary.Services.Stats
{
    public interface ITopValues
    {
        List<KeyValuePair<string, int>> TopDomains(List<string> urls);
        List<KeyValuePair<string, int>> TopEmoji(List<string> emojis);
        List<KeyValuePair<string, int>> TopValue(List<string> hashtags);
    }
}