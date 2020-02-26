using System;
using System.IO;
using System.Collections.Generic;
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

        public IEnumerable<WordRecord> Enumerate()
        {
            foreach (string line in File.ReadLines(path))
            {
                var words = line
                    .Split(IgnoredCharacters, StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in words)
                {
                    yield return new WordRecord()
                    {
                        Word = word,
                        Position = 0
                    };
                }
            }
        }
    }
}
