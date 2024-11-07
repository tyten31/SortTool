using SortTool.Radix;
using SortTool.Merge;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Reflection;
using System.Diagnostics;
using SortTool.Quick;

namespace SortTool
{
    internal class CommandManager
    {
        public int DisplayNumber { get; set; }

        public List<string> SortedWords { get; set; }

        public bool IsValidCommand(string command)
        {
            // Check length
            if (string.IsNullOrEmpty(command))
            {
                return false;
            }

            var options = new List<string> { "-a", "-n", "-u" };
            var commands = command.Trim().Split('|', StringSplitOptions.RemoveEmptyEntries);

            // Check that the first command starts with sort
            if (!commands[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[0].Trim().Equals("sort", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Check that the second command starts with head
            if (commands.Length > 1 && !commands[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)[0].Trim().Equals("head", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            foreach (string arg in commands)
            {
                var split = arg.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (split.Length < 2)
                {
                    return false;
                }

                // Check that options are valid
                foreach (var argOption in split.Where(x => x.StartsWith('-')))
                {
                    if (!options.Contains(argOption))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public async Task RunCommand(string command)
        {
            var timer = new Stopwatch();
            timer.Start();

            SortedWords = [];
            DisplayNumber = -1;
            var commands = command.Trim().Split('|', StringSplitOptions.RemoveEmptyEntries);

            // Sort Command
            var sortCommands = ParseCommand(commands[0].Trim());
            await RunSortCommand(sortCommands);

            // Head Command
            if (commands.Length > 1)
            {
                var headCommand = ParseCommand(commands[1].Trim());
                await RunHeadCommand(headCommand);
            }

            DisplaySortedWords();

            timer.Stop();
            var timeTaken = timer.Elapsed;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{Environment.NewLine}Time Taken: {timeTaken:m\\:ss\\.fff}");
        }

        private void DisplaySortedWords()
        {
            if (DisplayNumber > -1)
            {
                SortedWords = SortedWords.Take(DisplayNumber).ToList();
            }

            foreach (var word in SortedWords)
            {
                Console.WriteLine(word);
            }
        }

        private void ExecuteHeadCommand(int number)
        {
            DisplayNumber = number;
        }

        private async Task ExecuteSortCommand(bool unique, SortAlgorithm algorithm, string file)
        {
            var words = await GetWords(unique, file);

            switch (algorithm)
            {
                case SortAlgorithm.Heap:
                    SortedWords = HeapSort(words);
                    break;

                case SortAlgorithm.Merge:
                    SortedWords = MergeSort.Sort(words);
                    break;

                case SortAlgorithm.Quick:
                    SortedWords = QuickSort.Sort(words);
                    break;

                case SortAlgorithm.Radix:
                    SortedWords = RadixSort.Sort(words);
                    break;

                default:
                    SortedWords = [.. words.Order()];
                    break;
            }
        }

        private async Task<List<string>> GetWords(bool unique, string file)
        {
            var words = new List<string>();
            var wordsFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), file);

            string? line;
            using StreamReader read = new(wordsFilePath);

            while ((line = await read.ReadLineAsync()) != null)
            {
                if (unique)
                {
                    if (!words.Contains(line))
                    {
                        words.Add(line);
                    }
                }
                else
                {
                    words.Add(line);
                }
            }

            return words;
        }

        private List<string> HeapSort(List<string> words)
        {
            return words;
        }

        private List<string> ParseCommand(string command)
        {
            var parts = new List<string>();

            foreach (var item in command.Trim().Split(' '))
            {
                parts.Add(item.Trim());
            }

            return parts;
        }

        private async Task RunHeadCommand(List<string> commands)
        {
            var numberOption = new Option<int>(["--number", "-n"], () => 5, "number of results option");
            var headCommand = new Command("head")
            {
                Handler = CommandHandler.Create<int>(ExecuteHeadCommand)
            };
            headCommand.AddOption(numberOption);

            var rootCommand = new RootCommand();
            rootCommand.AddCommand(headCommand);

            await rootCommand.InvokeAsync([.. commands]);
        }

        private async Task RunSortCommand(List<string> commands)
        {
            var uniqueOption = new Option<bool>(["--unique", "-u"], () => false, "unique flag option");
            var algorithmOption = new Option<SortAlgorithm>(["--algorithm", "-a"], () => SortAlgorithm.Lexi, "algorithm flag option");
            var file = new Argument<string>("file", "a required file argument");

            var sortCommand = new Command("sort")
            {
                Handler = CommandHandler.Create<bool, SortAlgorithm, string>(ExecuteSortCommand)
            };
            sortCommand.AddOption(uniqueOption);
            sortCommand.AddOption(algorithmOption);
            sortCommand.AddArgument(file);

            var rootCommand = new RootCommand();
            rootCommand.AddCommand(sortCommand);

            await rootCommand.InvokeAsync([.. commands]);
        }
    }
}