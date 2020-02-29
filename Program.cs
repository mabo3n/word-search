using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using GutenbergAnalysis.Indexes;
using GutenbergAnalysis.RW.Reading;

namespace GutenbergAnalysis
{
    class Program
    {
        public static string folderPath = "C:/Users/rober/RiderProjects/word-search/"; 
        public static string SourcePath = folderPath + "data/";
        public static string DatabasePath = folderPath + "index_db/db.txt";
        public static string DatabaseIndexPath = folderPath + "index_db/db_indexes.txt";

        private Dictionary<string, long> WordIndexes = new Dictionary<string, long>();

        static void Main(string[] args)
        {
            var a = new WordReader(folderPath + "teste.txt").Enumerate();
            Console.WriteLine(string.Join('\n', a.Select(r => r.Word + "/" + r.Position)));
            new Program().Run();
        }

        private void Run()
        {
            LoadWordIndexes();
            Menu();
        }

        private void LoadWordIndexes()
        {
            Console.WriteLine("Loading Word indexes into memory...");
            WordIndexes.Clear();

            var wordIndexes = new WordIndexes(DatabasePath, DatabaseIndexPath).Read();

            if (!wordIndexes.Any())
            {
                Console.WriteLine("Couldn't find word indexes to load.");
                return;
            }

            foreach (var record in wordIndexes)
            {
                WordIndexes[record.Word] = record.Position;
            }

            Console.WriteLine("Loaded!");
        }

        static void TimedExecution(Action action)
        {
            var timer = Stopwatch.StartNew();
            try
            {
                action();
            }
            finally
            {
                Console.WriteLine($"Executed in {timer.Elapsed.ToString()}.");
            }
        }

        private string ConsoleMenu(IEnumerable<string> text, Func<string, bool> validOptionFilter)
        {
            string input;

            do
            {
                Console.WriteLine();
                Console.WriteLine(string.Join('\n', text));
                Console.WriteLine();

                input = Console.ReadLine();
            }
            while(!validOptionFilter(input.Trim()));

            return input.Trim();
        }

        public void Menu()
        {
            string input;

            Func<string> MainMenu = () => ConsoleMenu(
                new[] {
                    "What you wanna do?",
                    "(1) Build indexes",
                    "(2) Search for a word",
                    "(0) Exit",
                },
                input => new[] {"0", "1", "2"}.Contains(input)
            );
            if ((input = MainMenu()) != "0")
            {
                if (input == "1")
                {
                    Func<string> IndexesMenu = () => ConsoleMenu(
                        new[] {
                            "Which index do you want to [re]build?",
                            "(1) Word Indexes (higher layer)",
                            "(2) Word Indexes (higher layer) and Word Occurrences Database (lower layer)",
                            "(0) Exit",
                        },
                        input => new[] {"0", "1", "2"}.Contains(input)
                    );

                    if ((input = IndexesMenu()) != "0")
                    {
                        if (input == "1")
                        {
                            if (!File.Exists(DatabasePath))
                            {
                                Console.WriteLine("\nWord Occurrences Database not found!");
                                Menu();
                            }

                            Console.WriteLine("Building Word Indexes...");
                            TimedExecution(
                                new WordIndexes(DatabasePath, DatabaseIndexPath).Create
                            );
                        }
                        else if (input == "2")
                        {
                            Console.WriteLine("Building Word Occurrence Database...");
                            TimedExecution(
                                new WordOccurrenceDatabase(SourcePath, DatabasePath).Create
                            );

                            Console.WriteLine("Building Word Indexes...");
                            TimedExecution(
                                new WordIndexes(DatabasePath, DatabaseIndexPath).Create
                            );
                        }

                        LoadWordIndexes();
                        Menu();

                    } else Menu();
                }
                else if (input == "2")
                {
                    Func<string> WordSearchMenu = () => ConsoleMenu(
                        new[] {
                            "Type the word you want to search for, or 0 to exit:",
                        },
                        input => true
                    );

                    if ((input = WordSearchMenu()) != "0")
                    {
                        Console.WriteLine($"Searching for \"{input}\"...");

                        Search(input);

                        Menu();
                    } else Menu();
                }
            }
        }

        private void Search(string word)
        {
            if (!WordIndexes.ContainsKey(word))
            {
                Console.WriteLine("Word not found in any file!");
                return;
            }
            
            var wordPositionOnWordIndexesFile = WordIndexes[word];

        }
    }
}
