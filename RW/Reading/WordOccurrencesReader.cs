using System.IO;
using System.Collections.Generic;
using GutenbergAnalysis.Records;
using GutenbergAnalysis.RW.Writing;

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

                record.Position = fileStream.Position;
                record.Word = binaryReader.ReadString();
                record.FileName = binaryReader.ReadString();
                record.PositionOnFile = binaryReader.ReadInt64();
                record.NextOccurrencePosition = binaryReader.ReadInt64();

                yield return record;
            }
        }
        
        public IEnumerable<WordOccurrenceRecord> Enumerate(WordOccurrenceIndexRecord wordOccurrenceIndexRecord)
        {
            using var fileStream = File.OpenRead(path);
            using var binaryReader = new BinaryReader(fileStream);
            
            var nextOccurrencePosition = wordOccurrenceIndexRecord.Position;
            
            do
            {
                fileStream.Seek(nextOccurrencePosition, SeekOrigin.Begin);
                
                var currentWordOccurrenceRecord = new WordOccurrenceRecord();

                currentWordOccurrenceRecord.Position = fileStream.Position;
                currentWordOccurrenceRecord.Word = binaryReader.ReadString();
                currentWordOccurrenceRecord.FileName = binaryReader.ReadString();
                currentWordOccurrenceRecord.PositionOnFile = binaryReader.ReadInt64();
                currentWordOccurrenceRecord.NextOccurrencePosition = binaryReader.ReadInt64();

                yield return currentWordOccurrenceRecord;

                nextOccurrencePosition = currentWordOccurrenceRecord.NextOccurrencePosition;
                
            } while (nextOccurrencePosition != WordOccurrencesWriter.NullOffset);
        }
    }
}
