using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace gutenberg_analysis
{
    public class WordOcurrencesReader : IDataReader<WordOcurrenceEntry>
    {
        private readonly string path;

        public WordOcurrencesReader(string path)
        {
            this.path = path;
        }

        public IEnumerable<WordOcurrenceEntry> Enumerate()
        {
            using var fileStream = File.OpenRead(path);
            using var binaryReader = new BinaryReader(fileStream);

            while(fileStream.Position < fileStream.Length)
            {
                var entry = new WordOcurrenceEntry();

                entry.Word = binaryReader.ReadString();
                entry.FileName = binaryReader.ReadString();
                entry.OffsetOnFile = binaryReader.ReadUInt64();
                entry.NextOcurrenceOffset = binaryReader.ReadUInt64();

                yield return entry;
            }
        }
    }
}
