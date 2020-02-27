using System.IO;
using System.Collections.Generic;
using GutenbergAnalysis.Records;
using System.Linq;

namespace GutenbergAnalysis.RW.Reading
{
    public class WordOccurrenceIndexesReader : IDataReader<WordOccurrenceIndexRecord>
    {
        private readonly string path;

        public WordOccurrenceIndexesReader(string path)
        {
            this.path = path;
        }

        public IEnumerable<WordOccurrenceIndexRecord> Enumerate()
        {
            if (!File.Exists(path))
            {
                yield break;
            }

            using var fileStream = File.OpenRead(path);
            using var binaryReader = new BinaryReader(fileStream);

            while (fileStream.Position < fileStream.Length)
            {
                var wordOccurrenceIndex = new WordOccurrenceIndexRecord();
                wordOccurrenceIndex.Word = binaryReader.ReadString();
                wordOccurrenceIndex.Position = binaryReader.ReadInt64();
                yield return wordOccurrenceIndex;
            }
        }
    }
}
