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
                var record = new WordOccurrenceRecord();

                record.Word = binaryReader.ReadString();
                record.FileName = binaryReader.ReadString();
                record.PositionOnFile = binaryReader.ReadInt64();
                record.NextOccurrencePosition = binaryReader.ReadInt64();

                yield return record;
            }
        }
    }
}
