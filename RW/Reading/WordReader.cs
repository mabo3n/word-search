using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            long byteOffset = 0;
            
            foreach (var line in File.ReadLines(path))
            {
                var lineCharacters = line.ToCharArray();
                
                var wordBuilder = new StringBuilder();

                for (var i = 0; i < lineCharacters.Length; i++)
                {
                    var currentCharacter = lineCharacters[i];

                    var isIgnoredCharacter = IgnoredCharacters.Contains(currentCharacter);
                    var isLastCharacterOfLine = i == lineCharacters.Length - 1;
                    
                    if (!isIgnoredCharacter)
                    {
                        wordBuilder.Append(currentCharacter);
                    }
                    
                    if (isIgnoredCharacter || isLastCharacterOfLine)
                    {
                        var isEndOfWord = wordBuilder.Length > 0;

                        if (isEndOfWord)
                        {
                            yield return new WordRecord()
                            {
                                Word = wordBuilder.ToString(),
                                Position = byteOffset
                            };
                            byteOffset += wordBuilder.Length;
                            wordBuilder.Clear();
                        }

                        byteOffset++;
                    }
                }

                byteOffset += 2;
            }
        }
    }
}
