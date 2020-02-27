using System;
using System.IO;
using GutenbergAnalysis.Records;

namespace GutenbergAnalysis.RW.Writing
{
    public class WordOccurrenceIndexesWriter : IDataWriter<WordOccurrenceIndexRecord>, IDisposable
    {
        private readonly string writePath;
        private readonly FileStream fileStream;
        private readonly BinaryWriter binaryWriter;

        public WordOccurrenceIndexesWriter(string writePath)
        {
            this.writePath = writePath;

            fileStream = File.OpenWrite(writePath);
            binaryWriter = new BinaryWriter(fileStream);
        }

        public void Write(WordOccurrenceIndexRecord wordOccurrenceIndex)
        {
            binaryWriter.Write(wordOccurrenceIndex.Word);
            binaryWriter.Write(wordOccurrenceIndex.Position);
        }

        public void Dispose()
        {
            binaryWriter?.Dispose();
            fileStream?.Dispose();
        }
    }
}
