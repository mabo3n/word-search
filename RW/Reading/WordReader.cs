using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using GutenbergAnalysis.Records;

namespace GutenbergAnalysis.RW.Reading
{
    public class WordReader : IDataReader<WordRecord>
    {
        private static char[] IgnoredCharacters = new[]
        {
            ' ', '.', ',', '!', '?', '"', '\'',
            '{', '}', ']', '[', '(', ')', '<',
            '>', ';', ':', '\n', '\r'
        };

        private readonly string path;

        public WordReader(string path)
        {
            this.path = path;
        }
        
        private static bool IsNotStopWord(string word) => true;
        
        public IEnumerable<WordRecord> Enumerate()
        {
            // using var fileStream = File.OpenRead(path);
            // using var streamReader = new StreamReader(fileStream);
            //
            // var chunkSize = 1024;
            // var currrentBytes = new byte[chunkSize];
            //
            // while (fileStream.Read(currrentBytes) > 0)
            // {
            //     foreach (var character in currrentBytes)
            //     {
            //         //fileStream.Seek(, SeekOrigin.Begin);
            //     }
            // }

            foreach (string line in File.ReadLines(path))
            {
                var words = line
                    .Split(IgnoredCharacters, StringSplitOptions.RemoveEmptyEntries)
                    .Where(IsNotStopWord);

                foreach (var word in words)
                {
                    yield return new WordRecord()
                    {
                        Word = word,
                        Position = 0 // TODO
                    };
                }
            }
        }
    }
}
