using System.IO;
using System.Collections.Generic;
using GutenbergAnalysis.RW.Writing;
using GutenbergAnalysis.Records;

namespace GutenbergAnalysis
{
    public class WordOffsetsWriter : IDataWriter<WordOffset>
    {
        private readonly string writePath;
        private readonly FileStream fileStream;
        private readonly BinaryWriter binaryWriter;

        public WordOffsetsWriter(string writePath)
        {
            this.writePath = writePath;

            fileStream = File.OpenWrite(writePath);
            binaryWriter = new BinaryWriter(fileStream);
        }

        public void Write(IEnumerable<WordOffset> wordOffsets)
        {
            foreach (var entry in wordOffsets)
            {
                binaryWriter.Write(entry.Word);
                binaryWriter.Write(entry.Offset);
            }
        }

        public void Dispose()
        {
            binaryWriter?.Dispose();
            fileStream?.Dispose();
        }
    }
}
