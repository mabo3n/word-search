using System;
using System.IO;
using GutenbergAnalysis.Records;
using GutenbergAnalysis.RW.Reading;

namespace GutenbergAnalysis.Indexes
{
    public class WordOccurrenceDatabase
    {
        public string SourceDirectoryPath { get; set; }
        public string DatabasePath { get; private set; }
        public string DatabaseIndexPath { get; private set; }

        public void Create()
        {
            using var wordOccurrencesWriter = new WordOccurrencesWriter(DatabasePath);
            using var wordOccurrencesIndexWriter = new WordOccurrenceIndexesWriter(DatabasePath);

            foreach (string filePath in Directory.EnumerateFiles(SourceDirectoryPath))
            {
                var wordRecordReader = new WordReader(filePath);

                foreach (var wordRecord in wordRecordReader.Enumerate())
                {
                    var occurrence = new WordOccurrenceRecord();

                    occurrence.FileName = filePath;
                    occurrence.Word = wordRecord.Word;
                    occurrence.OffsetOnFile = wordRecord.Position;

                    wordOccurrencesWriter.Write(occurrence);
                }
            }
        }

        
    }
}
