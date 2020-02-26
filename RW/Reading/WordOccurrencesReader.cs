using System.IO;
using System.Collections.Generic;
using GutenbergAnalysis.Records;

namespace GutenbergAnalysis.RW.Reading
{
    public class WordOccurrencesReader : IDataReader<WordOccurrenceRecord>
    {
        private readonly string path;

        public WordOccurrencesReader(string path)
        {
            this.path = path;
        }

        public IEnumerable<WordOccurrenceRecord> Enumerate()
        {
            using var fileStream = File.OpenRead(path);
            using var binaryReader = new BinaryReader(fileStream);

            while (fileStream.Position < fileStream.Length)
            {
                var entry = new WordOccurrenceRecord();

                entry.Word = binaryReader.ReadString();
                entry.FileName = binaryReader.ReadString();
                entry.OffsetOnFile = binaryReader.ReadInt64();
                entry.NextOccurrenceOffset = binaryReader.ReadInt64();

                yield return entry;
            }
        }
    }
}
