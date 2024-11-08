using System.Reflection;
using System.Text.RegularExpressions;

namespace SortTool
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var exit = false;
            var commandManager = new CommandManager();

            var words = await GetWords();
            await CreateWordsFile(words);
            ShowHelp();

            while (!exit)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(">");

                var input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input))
                {
                    exit = input.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase);

                    if (!exit)
                    {
                        //Check if the command is valid
                        if (commandManager.IsValidCommand(input))
                        {
                            await commandManager.RunCommand(input);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid Command");
                            Console.WriteLine("");

                            ShowHelp();
                        }

                        Console.WriteLine("");
                    }
                }
            }
        }

        private static async Task CreateWordsFile(List<string> words)
        {
            var wordFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "words.txt");

            if (File.Exists(wordFilePath))
            {
                File.Delete(wordFilePath);
            }

            await using StreamWriter wordsFile = new(wordFilePath, true);
            foreach (var word in words)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    await wordsFile.WriteLineAsync(word);
                }
            }
        }

        private static async Task<List<string>> GetWords()
        {
            var words = new List<string>();
            var textFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "text.txt");

            string? line;
            using StreamReader read = new(textFilePath);

            while ((line = await read.ReadLineAsync()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    // Expression to remove anything that is not a letter
                    var reg_exp = new Regex("[^a-zA-Z]");

                    foreach (var word in reg_exp.Replace(line, " ").ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries))
                    {
                        // a & i are the only single letter words in english
                        if (word.Length > 1 || word.Equals("a") || word.Equals("i"))
                        {
                            words.Add(word.Trim());
                        }
                    }
                }
            }

            return words;
        }

        private static void ShowHelp()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Usage: sort -u -a <algorithm> <file> | head -n<#>");
            Console.WriteLine("Commands:");
            Console.WriteLine(" -a <Sorting algorithm [Bubble, Heap, Lexi, Merge, Quick, Radix]>");
            Console.WriteLine(" -u <Unique words>");
            Console.WriteLine(" -n <Number of words>");
            Console.WriteLine("");
        }
    }
}