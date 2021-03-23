namespace TwitterStats.Contracts
{
    public class TextProcessingConfiguration
    {
        public string EmojiListFileName { get; set; }
        public string RegexMentions     { get; set; }
        public string RegexHashtags     { get; set; }
        public string RegexLinks        { get; set; }
        public string RegexInstagram { get; set; }
        public string RegexTwitter { get; set; }
    }
}
