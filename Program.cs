using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Diagnostics;
using GutenbergAnalysis.RW.Reading;
using GutenbergAnalysis.Records;

namespace GutenbergAnalysis
{
    class Program
    {
        Dictionary<string, int> Frequencies = new Dictionary<string, int>();
        ulong TotalWordCount = 0;
        string Root = "./data/";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // new Program().WordFrequencyAsync().Wait();
            new Program().Run();
        }

        private string ConsoleMenu(IEnumerable<string> text, Func<string, bool> validOptionFilter)
        {
            string input;

            do
            {
                Console.WriteLine(string.Join('\n', text));

                input = Console.ReadLine();
            }
            while(!validOptionFilter(input.Trim()));

            return input.Trim();
        }

        private string MainMenu()
        {
            var text = new[]
            {
                "What you wanna do?",
                "(1) Build indexes",
                "(2) Search for a word",
                "(0) Exit",
            };
            var options = new[] {"0", "1", "2"};

            return ConsoleMenu(text, input => options.Contains(input));
        }

        private string IndexesMenu()
        {
            var text = new[]
            {
                "Which index do you want to [re]build?",
                "(1) Word Ocurrences Index (higher layer)",
                "(2) Word Ocurrences Index (higher layer) and Word Ocurrences (lower layer)",
                "(0) Exit",
            };
            var options = new[] {"0", "1", "2"};

            return ConsoleMenu(text, input => options.Contains(input));
        }

        private string WordSearchMenu()
        {
            var text = new[]
            {
                "Type the word you want to search for, or 0 to exit:",
            };

            return ConsoleMenu(text, input => true);
        }

        private void Run()
        {
            string input;

            if ((input = MainMenu()) != "0")
            {
                if (input == "1")
                {
                    if ((input = IndexesMenu()) != "0")
                    {
                        if (input == "1")
                        {
                            // build higher layer index
                            Run();
                        }
                        else if (input == "2")
                        {
                            // build lower and higher layer index
                            Run();
                        }
                    } else Run();
                }
                else if (input == "2")
                {
                    if ((input = WordSearchMenu()) != "0")
                    {
                        // execute search for input
                    } else Run();
                }
            }
        }

        public IEnumerable<string> GetFilePaths(string root)
        {
            foreach (string file in Directory.EnumerateFiles(root))
            {
                yield return file;
            }
        }

        public Dictionary<string, int> BuildWordFrequency(IEnumerable<WordRecord> wordRecords)
        {
            var frequencies = new Dictionary<string, int>();

            foreach (var record in wordRecords)
            {
                if (frequencies.ContainsKey(record.Word))
                {
                    frequencies[record.Word] += 1;
                }
                else
                {
                    frequencies[record.Word] = 1;
                }
            }

            return frequencies;
        }

        public void UpdateGlobalFrequenciesFromFileFrequencies(Dictionary<string, int> fileFrequencies)
        {
            lock (Frequencies)
            {
                foreach (var wordFrequency in fileFrequencies)
                {
                    var word = wordFrequency.Key;
                    if (Frequencies.ContainsKey(word))
                    {
                        Frequencies[word] += wordFrequency.Value;
                    }
                    else
                    {
                        Frequencies[word] = wordFrequency.Value;
                    }

                    TotalWordCount += (ulong)wordFrequency.Value;
                }
            }
        }

        public void PrintFrequenciesSorted()
        {
            Console.WriteLine("Total word count: " + TotalWordCount);
            Console.WriteLine("Unique word count: " + Frequencies.Count);
            foreach (var freq in Frequencies.OrderByDescending(key => key.Value).Take(10))
            {
                Console.WriteLine(freq.Key + ": " + freq.Value);
            }
        }

        private void UpdateWordFrequenciesFromFile(string filePath)
        {
            var words = new WordReader(filePath).Enumerate();
            var fileWordFrequencies = BuildWordFrequency(words);
            // Console.WriteLine("Done for " + filePath);
            UpdateGlobalFrequenciesFromFileFrequencies(fileWordFrequencies);
        }

        public async Task WordFrequencyAsync()
        {
            var executionBlock = new ActionBlock<string>
            (
                UpdateWordFrequenciesFromFile,
                new ExecutionDataflowBlockOptions {
                    MaxDegreeOfParallelism = 4,
                    BoundedCapacity = 4
                }
            );

            var timer = Stopwatch.StartNew();
            Console.WriteLine("Starting");

            foreach (var filePath in GetFilePaths(Root))
            {
                await executionBlock.SendAsync(filePath);
            }

            executionBlock.Complete();
            await executionBlock.Completion;

            timer.Stop();
            Console.WriteLine("Completed in " + timer.Elapsed.ToString());
            PrintFrequenciesSorted();
        }
    }
}
