using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace gutenberg_analysis
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

        public async IAsyncEnumerable<string> EnumerateAsync()
        {
            foreach(var word in GetWordsFromFile(path))
            {
                yield return word;
            }
        }

        public IEnumerable<string> GetWordsFromFile(string path)
        {
            foreach (string line in File.ReadLines(path))
            {
                var words = line
                    .Split(IgnoredCharacters, StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in words.Select(word => word.ToLower()))
                {
                    yield return word;
                }
            }
        }
    }
}
