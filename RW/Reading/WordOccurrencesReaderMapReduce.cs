using System.IO;
using System.Collections.Generic;
using GutenbergAnalysis.Records;
using GutenbergAnalysis.RW.Writing;

namespace GutenbergAnalysis.RW.Reading
{
    public class WordOccurrencesReaderMapReduce : IDataReader<WordOccurrenceMapReduceRecord>
    {
        private readonly string path;
        private static char[] separators = new[] {' ', '>'};

        public WordOccurrencesReaderMapReduce(string path)
        {
            this.path = path;
        }

        // Consumed to build Word Indexes
        public IEnumerable<WordOccurrenceMapReduceRecord> Enumerate()
            => Enumerate(null);

        // Consumed to search for a word or above
        public IEnumerable<WordOccurrenceMapReduceRecord> Enumerate(
            WordOccurrenceIndexRecord wordOccurrenceIndexRecord)
        {
            var isSearchingForAWord = wordOccurrenceIndexRecord != null;

            using var fileStream = File.OpenRead(path);

            if (isSearchingForAWord)
            {
                fileStream.Seek(wordOccurrenceIndexRecord.Position, SeekOrigin.Begin);
            }

            using var streamReader = new StreamReader(fileStream);

            var currentWord = string.Empty;
            var currentWordByteOffset = fileStream.Position;
            var currentByteOffset = fileStream.Position;
            
            var rawLine = string.Empty;
            while ((rawLine = streamReader.ReadLine()) != null)
            {
                var line = rawLine.Trim('\t');

                if (!line.StartsWith('>'))
                {
                    currentWord = line;
                    currentWordByteOffset = currentByteOffset;
                    if (isSearchingForAWord && currentWord != wordOccurrenceIndexRecord.Word)
                    {
                        break;
                    }
                }
                else
                {
                    var lineElements = line.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
                    var currentFileName = lineElements[0];

                    for (var i = 1; i < lineElements.Length; i++)
                    {
                        var linePositionOnFile = long.Parse(lineElements[i]);

                        var record = new WordOccurrenceMapReduceRecord();

                        record.Position = currentWordByteOffset;
                        record.Word = currentWord;
                        record.FileName = currentFileName;
                        record.LinePositionOnFile = linePositionOnFile;

                        yield return record;
                    }
                }
                currentByteOffset += rawLine.Length + 1;
            }
        }
    }
}
