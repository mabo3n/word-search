using System.IO;
using System.Collections.Generic;

namespace GutenbergAnalysis.RW.Reading
{
    public class WordOffsetsReader : IDataReader<KeyValuePair<string, long>>
    {
        private readonly string path;

        public WordOffsetsReader(string path)
        {
            this.path = path;
        }

        public IEnumerable<KeyValuePair<string, long>> Enumerate()
        {
            using var fileStream = File.OpenRead(path);
            using var binaryReader = new BinaryReader(fileStream);

            while (fileStream.Position < fileStream.Length)
            {
                var word = binaryReader.ReadString();
                var firstOccurrenceOffset = binaryReader.ReadInt64();
                yield return new KeyValuePair<string, long>(word, firstOccurrenceOffset);
            }
        }
    }
}
