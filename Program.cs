using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using GutenbergAnalysis.Indexes;

namespace GutenbergAnalysis
{
    class Program
    {
        Dictionary<string, int> Frequencies = new Dictionary<string, int>();
        ulong TotalWordCount = 0;
        public static string SourcePath = "data/";
        public static string DatabasePath = "index_db/db.txt";
        public static string DatabaseIndexPath = "index_db/db_indexes.txt";

        static void Main(string[] args)
        {
            new Program().Run();
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

        public void Run()
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
                            "(2) Word Indexes (higher layer) and Word Ocurrences Database (lower layer)",
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
                            }
                            else
                            {
                                Console.WriteLine("Building Word Indexes...");
                                TimedExecution(
                                    new WordIndexes(DatabasePath, DatabaseIndexPath).Create
                                );
                            }

                            Run();
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

                            Run();
                        }
                    } else Run();
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
                        // execute search for input
                        Console.WriteLine($"Searching for \"{input}\"...");

                        Run();
                    } else Run();
                }
            }
        }
    }
}
