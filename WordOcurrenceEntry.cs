namespace gutenberg_analysis
{
    public class WordOcurrenceEntry
    {
        public string Word { get; set; }
        public string FileName { get; set; }
        public ulong OffsetOnFile { get; set; }
        public ulong NextOcurrenceOffset { get; set; }
    }
}
