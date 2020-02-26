using System.IO;
using System.Collections.Generic;
using GutenbergAnalysis.Records;
using GutenbergAnalysis.RW.Writing;
using System;

namespace GutenbergAnalysis
{
    public class WordOccurrencesWriter : IDataWriter<WordOccurrence>, IDisposable
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

        private bool PreviousEntryExists(WordOccurrence entry)
            => lastOccurrencesNextPointerOffset.ContainsKey(entry.Word);

        public void Write(IEnumerable<WordOccurrence> wordOccurrenceEntries)
        {
            foreach (var entry in wordOccurrenceEntries)
            {
                if (PreviousEntryExists(entry))
                {
                    long currentEntryOffset = fileStream.Position;
                    long previousEntryNextPointerOffset =
                        lastOccurrencesNextPointerOffset[entry.Word];

                    fileStream.Seek(previousEntryNextPointerOffset, SeekOrigin.Begin);
                    binaryWriter.Write(currentEntryOffset);
                    fileStream.Seek(currentEntryOffset, SeekOrigin.Begin);
                }
                binaryWriter.Write(entry.Word);
                binaryWriter.Write(entry.FileName);
                binaryWriter.Write(entry.OffsetOnFile);

                lastOccurrencesNextPointerOffset[entry.Word] = fileStream.Position;
                binaryWriter.Write(NullOffset);
            }
        }

        public void Dispose()
        {
            binaryWriter?.Dispose();
            fileStream?.Dispose();
        }
    }
}
