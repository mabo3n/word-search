using System.IO;
using System.Collections.Generic;
using System;

namespace gutenberg_analysis
{
    public class WordOccurrencesCreator : IDataWriter<WordOccurrenceEntry>
    {
        private const long NullOffset = 0;

        private readonly string writePath;
        private readonly Dictionary<string, long> lastOccurrencesNextPointerOffset;

        public WordOccurrencesCreator(string writePath)
        {
            this.writePath = writePath;
            lastOccurrencesNextPointerOffset = new Dictionary<string, long>();
        }

        private bool PreviousEntryExists(WordOccurrenceEntry entry)
            => lastOccurrencesNextPointerOffset.ContainsKey(entry.Word);

        public void Write(IEnumerable<WordOccurrenceEntry> wordOccurrenceEntries)
        {
            using var fileStream = File.OpenWrite(writePath);
            using var binaryWriter = new BinaryWriter(fileStream);

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
    }
}
