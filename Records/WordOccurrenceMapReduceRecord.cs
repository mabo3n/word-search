namespace GutenbergAnalysis.Records
{
    public class WordOccurrenceMapReduceRecord : WordRecord
    {
        public string FileName { get; set; }
        public long LinePositionOnFile { get; set; }
    }
}
