using System.IO;
using System.Collections.Generic;
using GutenbergAnalysis.Records;

namespace GutenbergAnalysis.RW.Reading
{
    public class WordOffsetsReader : IDataReader<WordOffset>
    {
        private readonly string path;

        public WordOffsetsReader(string path)
        {
            this.path = path;
        }

        public IEnumerable<WordOffset> Enumerate()
        {
            using var fileStream = File.OpenRead(path);
            using var binaryReader = new BinaryReader(fileStream);

            while (fileStream.Position < fileStream.Length)
            {
                var wordOffset = new WordOffset();
                wordOffset.Word = binaryReader.ReadString();
                wordOffset.Offset = binaryReader.ReadInt64();
                yield return wordOffset;
            }
        }
    }
}
