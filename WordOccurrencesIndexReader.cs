using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace gutenberg_analysis
{
    public class WordOccurrencesIndexReader : IDataReader<KeyValuePair<string, long>>
    {
        private readonly string path;

        public WordOccurrencesIndexReader(string path)
        {
            this.path = path;
        }

        public IEnumerable<KeyValuePair<string, long>> Enumerate()
        {
            using var fileStream = File.OpenRead(path);
            using var binaryReader = new BinaryReader(fileStream);

            while (fileStream.Position < fileStream.Length)
            {
                var word = binaryReader.ReadString();
                var firstOccurrenceOffset = binaryReader.ReadInt64();
                yield return new KeyValuePair<string, long>(word, firstOccurrenceOffset);
            }
        }
    }
}
