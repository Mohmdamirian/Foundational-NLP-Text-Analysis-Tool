using System.Text.RegularExpressions;

namespace TextAnalysisTool;

internal static partial class Program
{
    private static int Main(string[] args)
    {
        Console.WriteLine("Text Analysis Tool");
        Console.WriteLine("==================");

        string? filePath = args.Length > 0 ? args[0] : PromptForFilePath();
        if (string.IsNullOrWhiteSpace(filePath))
        {
            Console.Error.WriteLine("No file was selected.");
            return 1;
        }

        filePath = Path.GetFullPath(filePath);
        if (!File.Exists(filePath))
        {
            Console.Error.WriteLine($"File not found: {filePath}");
            return 1;
        }

        var tree = new BinaryTree();

        try
        {
            int totalWords = BuildIndex(filePath, tree);
            if (totalWords == 0)
            {
                Console.Error.WriteLine("The file does not contain any recognised words.");
                return 1;
            }

            Console.WriteLine($"\nLoaded: {Path.GetFileName(filePath)}");
            Console.WriteLine($"Words analysed: {totalWords:N0}");
            Console.WriteLine($"Unique words: {tree.Count:N0}");
        }
        catch (IOException exception)
        {
            Console.Error.WriteLine($"The file could not be read: {exception.Message}");
            return 1;
        }
        catch (UnauthorizedAccessException)
        {
            Console.Error.WriteLine("You do not have permission to read this file.");
            return 1;
        }

        ShowMenu(tree);
        return 0;
    }

    private static string PromptForFilePath()
    {
        string samplePath = Path.Combine(
            AppContext.BaseDirectory,
            "sample-data",
            "sample.txt");

        Console.Write("Enter a text-file path, or press Enter for the sample: ");
        string? input = Console.ReadLine();

        return string.IsNullOrWhiteSpace(input)
            ? samplePath
            : input.Trim().Trim('"');
    }

    private static int BuildIndex(string filePath, BinaryTree tree)
    {
        int totalWords = 0;
        int lineNumber = 0;

        foreach (string line in File.ReadLines(filePath))
        {
            lineNumber++;

            foreach (Match match in WordPattern().Matches(line))
            {
                tree.Insert(match.Value, lineNumber);
                totalWords++;
            }
        }

        return totalWords;
    }

    private static void ShowMenu(BinaryTree tree)
    {
        while (true)
        {
            Console.WriteLine("\n1. Display all words");
            Console.WriteLine("2. Show most frequent word");
            Console.WriteLine("3. Show longest word");
            Console.WriteLine("4. Find the frequency of a word");
            Console.WriteLine("5. Find the line numbers of a word");
            Console.WriteLine("6. Exit");
            Console.Write("Choose an option (1-6): ");

            switch (Console.ReadLine())
            {
                case "1":
                    DisplayAllWords(tree);
                    break;
                case "2":
                    DisplayMostFrequentWord(tree);
                    break;
                case "3":
                    DisplayLongestWord(tree);
                    break;
                case "4":
                    FindWord(tree, showLineNumbers: false);
                    break;
                case "5":
                    FindWord(tree, showLineNumbers: true);
                    break;
                case "6":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid input. Enter a number from 1 to 6.");
                    break;
            }
        }
    }

    private static void DisplayAllWords(BinaryTree tree)
    {
        Console.WriteLine("\nWords in alphabetical order:");

        foreach (WordInfo wordInfo in tree.GetWordsInOrder())
        {
            Console.WriteLine($"{wordInfo.Word} ({wordInfo.Frequency})");
        }
    }

    private static void DisplayMostFrequentWord(BinaryTree tree)
    {
        WordInfo? result = tree.FindMostFrequentWord();

        Console.WriteLine(
            result is null
                ? "No words are available."
                : $"\nMost frequent word: {result.Word} ({result.Frequency} times)");
    }

    private static void DisplayLongestWord(BinaryTree tree)
    {
        WordInfo? result = tree.FindLongestWord();

        Console.WriteLine(
            result is null
                ? "No words are available."
                : $"\nLongest word: {result.Word} ({result.Frequency} times)");
    }

    private static void FindWord(BinaryTree tree, bool showLineNumbers)
    {
        Console.Write("\nEnter a word: ");
        string? searchTerm = Console.ReadLine();
        WordInfo? result = searchTerm is null ? null : tree.Find(searchTerm);

        if (result is null)
        {
            Console.WriteLine("Word not found.");
            return;
        }

        if (showLineNumbers)
        {
            Console.WriteLine(
                $"'{result.Word}' appears on line(s): " +
                string.Join(", ", result.LineNumbers));
        }
        else
        {
            Console.WriteLine(
                $"'{result.Word}' appears {result.Frequency} time(s).");
        }
    }

    [GeneratedRegex(@"[\p{L}]+(?:['’][\p{L}]+)*")]
    private static partial Regex WordPattern();
}

