using System.IO;
using System.Collections.Generic;
using GutenbergAnalysis.RW.Writing;

namespace GutenbergAnalysis
{
    public class WordOccurrencesIndexCreator : IDataWriter<KeyValuePair<string, long>>
    {
        private readonly string writePath;

        public WordOccurrencesIndexCreator(string writePath)
        {
            this.writePath = writePath;
        }

        public void Write(IEnumerable<KeyValuePair<string, long>> wordOccurrenceIndexes)
        {
            using var fileStream = File.OpenWrite(writePath);
            using var binaryWriter = new BinaryWriter(fileStream);

            foreach (var entry in wordOccurrenceIndexes)
            {
                binaryWriter.Write(entry.Key);
                binaryWriter.Write(entry.Value);
            }
        }
    }
}
