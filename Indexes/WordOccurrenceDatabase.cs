using System;
using System.IO;
using System.Linq;
using GutenbergAnalysis.Records;
using GutenbergAnalysis.RW.Reading;
using GutenbergAnalysis.RW.Writing;

namespace GutenbergAnalysis.Indexes
{
    public class WordOccurrenceDatabase
    {
        public string SourceDirectoryPath { get; set; }
        public string DatabasePath { get; private set; }
        public string DatabaseIndexPath { get; private set; }

        public WordOccurrenceDatabase(string sourceDirectoryPath, string databasePath, string databaseIndexPath = null)
        {
            SourceDirectoryPath = sourceDirectoryPath;
            DatabasePath = databasePath;
            DatabaseIndexPath = databaseIndexPath;
        }
        public void Create()
        {
            using var wordOccurrencesWriter = new WordOccurrencesWriter(DatabasePath);

            foreach (string filePath in Directory.EnumerateFiles(SourceDirectoryPath).Take(10))
            {
                var fileName = Path.GetFileName(filePath);
                var wordRecordReader = new WordReader(filePath);

                foreach (var wordRecord in wordRecordReader.Enumerate())
                {
                    var occurrence = new WordOccurrenceRecord();

                    occurrence.FileName = fileName;
                    occurrence.Word = wordRecord.Word;
                    occurrence.PositionOnFile = wordRecord.Position;

                    wordOccurrencesWriter.Write(occurrence);
                }
            }
        }

        public void Read()
        {
            var wordOccurrencesReader = new WordOccurrencesReader(DatabasePath);

            foreach (var record in wordOccurrencesReader.Enumerate().Take(100))
            {
                Console.WriteLine(record.Word + " " + record.FileName + " " + record.PositionOnFile + " " + record.NextOccurrencePosition);
            }

        }
    }
}
