using System.IO;
using System.Collections.Generic;
using System;

namespace gutenberg_analysis
{
    public class WordOcurrencesCreator : IDataWriter<WordOcurrenceEntry>
    {
        private const long NullOffset = 0;

        private readonly string writePath;
        private readonly Dictionary<string, long> lastOcurrencesNextPointerOffset;

        public WordOcurrencesCreator(string writePath)
        {
            this.writePath = writePath;
            lastOcurrencesNextPointerOffset = new Dictionary<string, long>();
        }

        private bool PreviousEntryExists(WordOcurrenceEntry entry)
            => lastOcurrencesNextPointerOffset.ContainsKey(entry.Word);

        public void Write(IEnumerable<WordOcurrenceEntry> wordOcurrenceEntries)
        {
            using var fileStream = File.OpenWrite(writePath);
            using var binaryWriter = new BinaryWriter(fileStream);

            foreach (var entry in wordOcurrenceEntries)
            {
                if (PreviousEntryExists(entry))
                {
                    long currentEntryOffset = fileStream.Position;
                    long previousEntryNextPointerOffset =
                        lastOcurrencesNextPointerOffset[entry.Word];

                    fileStream.Seek(previousEntryNextPointerOffset, SeekOrigin.Begin);
                    binaryWriter.Write(currentEntryOffset);
                    fileStream.Seek(currentEntryOffset, SeekOrigin.Begin);
                }
                binaryWriter.Write(entry.Word);
                binaryWriter.Write(entry.FileName);
                binaryWriter.Write(entry.OffsetOnFile);

                lastOcurrencesNextPointerOffset[entry.Word] = fileStream.Position;
                binaryWriter.Write(NullOffset);
            }
        }
    }
}
