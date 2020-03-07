using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GutenbergAnalysis.Records;
using GutenbergAnalysis.RW.Reading;
using GutenbergAnalysis.RW.Writing;

namespace GutenbergAnalysis.Indexes
{
    public class WordOccurrenceDatabaseMapReduce
    {
        public string SourceDirectoryPath { get; set; }
        public string DatabasePath { get; private set; }
        public WordOccurrenceDatabaseMapReduce(string sourceDirectoryPath, string databasePath)
        {
            SourceDirectoryPath = sourceDirectoryPath;
            DatabasePath = databasePath;
        }


        public IEnumerable<WordOccurrenceMapReduceRecord> Search(WordOccurrenceIndexRecord record)
        {
            return new WordOccurrencesReaderMapReduce(DatabasePath).Enumerate(record);
        }
    }
}
