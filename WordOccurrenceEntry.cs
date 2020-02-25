namespace gutenberg_analysis
{
    public class WordOccurrenceEntry
    {
        public string Word { get; set; }
        public string FileName { get; set; }
        public ulong OffsetOnFile { get; set; }
        public ulong NextOccurrenceOffset { get; set; }
    }
}
