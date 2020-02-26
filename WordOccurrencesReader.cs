using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace GutenbergAnalysis
{
    public class WordOccurrencesReader : IDataReader<WordOccurrenceEntry>
    {
        private readonly string path;

        public WordOccurrencesReader(string path)
        {
            this.path = path;
        }

        public IEnumerable<WordOccurrenceEntry> Enumerate()
        {
            using var fileStream = File.OpenRead(path);
            using var binaryReader = new BinaryReader(fileStream);

            while (fileStream.Position < fileStream.Length)
            {
                var entry = new WordOccurrenceEntry();

                entry.Word = binaryReader.ReadString();
                entry.FileName = binaryReader.ReadString();
                entry.OffsetOnFile = binaryReader.ReadInt64();
                entry.NextOccurrenceOffset = binaryReader.ReadInt64();

                yield return entry;
            }
        }
    }
}
