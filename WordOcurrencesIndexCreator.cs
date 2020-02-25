using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace gutenberg_analysis
{
    public class WordOcurrencesIndexCreator : IDataWriter<KeyValuePair<string, long>>
    {
        private readonly string writePath;

        public WordOcurrencesIndexCreator(string writePath)
        {
            this.writePath = writePath;
        }

        public void Write(IEnumerable<KeyValuePair<string, long>> wordOcurrenceIndexes)
        {
            using var fileStream = File.OpenWrite(writePath);
            using var binaryWriter = new BinaryWriter(fileStream);

            foreach (var entry in wordOcurrenceIndexes)
            {
                binaryWriter.Write(entry.Key);
                binaryWriter.Write(entry.Value);
            }
        }
    }
}
