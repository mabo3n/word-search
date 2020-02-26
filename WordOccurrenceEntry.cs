namespace GutenbergAnalysis
{
    public class WordOccurrenceEntry
    {
        public string Word { get; set; }
        public string FileName { get; set; }
        public long OffsetOnFile { get; set; }
        public long NextOccurrenceOffset { get; set; }
    }
}
