using System;
using System.IO;
using System.Collections.Generic;

namespace GutenbergAnalysis.RW.Reading
{
    public class WordReader : IDataReader<string>
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

        public IEnumerable<string> Enumerate()
        {
            foreach (string line in File.ReadLines(path))
            {
                var words = line
                    .Split(IgnoredCharacters, StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in words)
                {
                    yield return word.ToLower();
                }
            }
        }
    }
}
