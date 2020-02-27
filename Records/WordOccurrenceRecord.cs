namespace GutenbergAnalysis.Records
{
    public class WordOccurrenceRecord : WordRecord
    {
        public string FileName { get; set; }
        public long PositionOnFile { get; set; }
        public long NextOccurrencePosition { get; set; }
    }
}
