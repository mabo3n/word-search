using System;
using System.Collections.Generic;
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
        public WordOccurrenceDatabase(string sourceDirectoryPath, string databasePath)
        {
            SourceDirectoryPath = sourceDirectoryPath;
            DatabasePath = databasePath;
        }
        public void Create()
        {
            using var wordOccurrencesWriter = new WordOccurrencesWriter(DatabasePath);

            var counter = 1;
            foreach (string filePath in Directory.EnumerateFiles(SourceDirectoryPath))
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
                Console.WriteLine($"{filePath} indexed. ({counter})");
                counter++;
            }
        }

        public IEnumerable<WordOccurrenceRecord> Search(WordOccurrenceIndexRecord record)
        {
            return new WordOccurrencesReader(DatabasePath).Enumerate(record);
        }
    }
}
