namespace GutenbergAnalysis.Records
{
    public class WordOccurrenceRecord : WordRecord
    {
        public string FileName { get; set; }
        public long OffsetOnFile { get; set; }
        public long NextOccurrenceOffset { get; set; }
    }
}
