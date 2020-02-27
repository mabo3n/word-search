using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GutenbergAnalysis.Records;
using GutenbergAnalysis.RW.Reading;
using GutenbergAnalysis.RW.Writing;

namespace GutenbergAnalysis.Indexes
{
    public class WordIndexes
    {
        public string SourceFilePath { get; set; }
        public string IndexFilePath { get; private set; }

        public WordIndexes(string sourceFilePath, string indexFilePath)
        {
            SourceFilePath = sourceFilePath;
            IndexFilePath = indexFilePath;
        }
        public void Create()
        {
            var wordOccurrencesReader = new WordOccurrencesReader(SourceFilePath);

            var wordsFirstOccurrencePosition = new Dictionary<string, long>();
            
            foreach (var occurrence in wordOccurrencesReader.Enumerate().Take(100))
            {
                if (!wordsFirstOccurrencePosition.ContainsKey(occurrence.Word))
                    wordsFirstOccurrencePosition[occurrence.Word] = occurrence.Position;
            }
            
            using var wordOccurrenceIndexesWriter = new WordOccurrenceIndexesWriter(IndexFilePath);

            foreach (var (word, firstOccurrencePosition) in wordsFirstOccurrencePosition)
            {
                wordOccurrenceIndexesWriter.Write(new WordOccurrenceIndexRecord()
                {
                    Word = word, Position = firstOccurrencePosition
                });    
            }
        }

        public IEnumerable<WordOccurrenceIndexRecord> Read()
        {
            var wordOccurrenceIndexesReader = new WordOccurrenceIndexesReader(IndexFilePath);

            foreach (var record in wordOccurrenceIndexesReader.Enumerate().Take(100))
            {
                yield return record;
            }
        }
    }
}
