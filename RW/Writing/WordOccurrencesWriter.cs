using System;
using System.Collections.Generic;
using System.IO;
using GutenbergAnalysis.Records;

namespace GutenbergAnalysis.RW.Writing
{
    public class WordOccurrencesWriter : IDataWriter<WordOccurrenceRecord>, IDisposable
    {
        private const long NullOffset = 0;
        private readonly Dictionary<string, long> lastOccurrencesNextPointerOffset;

        private readonly string writePath;
        private readonly FileStream fileStream;
        private readonly BinaryWriter binaryWriter;

        public WordOccurrencesWriter(string writePath)
        {
            this.writePath = writePath;

            fileStream = File.OpenWrite(writePath);
            binaryWriter = new BinaryWriter(fileStream);

            lastOccurrencesNextPointerOffset = new Dictionary<string, long>();
        }

        private bool PreviousEntryExists(WordOccurrenceRecord entry)
            => lastOccurrencesNextPointerOffset.ContainsKey(entry.Word);

        public void Write(WordOccurrenceRecord wordOccurrenceRecord)
        {
            if (PreviousEntryExists(wordOccurrenceRecord))
            {
                long currentEntryOffset = fileStream.Position;
                long previousEntryNextPointerOffset =
                    lastOccurrencesNextPointerOffset[wordOccurrenceRecord.Word];

                fileStream.Seek(previousEntryNextPointerOffset, SeekOrigin.Begin);
                binaryWriter.Write(currentEntryOffset);
                fileStream.Seek(currentEntryOffset, SeekOrigin.Begin);
            }
            binaryWriter.Write(wordOccurrenceRecord.Word);
            binaryWriter.Write(wordOccurrenceRecord.FileName);
            binaryWriter.Write(wordOccurrenceRecord.PositionOnFile);

            lastOccurrencesNextPointerOffset[wordOccurrenceRecord.Word] = fileStream.Position;
            binaryWriter.Write(NullOffset);
        }

        public void Dispose()
        {
            binaryWriter?.Dispose();
            fileStream?.Dispose();
        }
    }
}
