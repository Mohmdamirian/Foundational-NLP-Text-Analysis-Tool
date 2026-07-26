namespace TextAnalysisTool;

/// <summary>
/// Stores the frequency and unique line locations of one normalised word.
/// </summary>
public sealed class WordInfo
{
    private readonly LinkedList<int> lineNumbers = new();

    public WordInfo(string word, int lineNumber)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(word);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(lineNumber);

        Word = word.ToLowerInvariant();
        Frequency = 1;
        lineNumbers.AddLast(lineNumber);
    }

    public string Word { get; }

    public int Frequency { get; private set; }

    public IEnumerable<int> LineNumbers => lineNumbers;

    public void RecordOccurrence(int lineNumber)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(lineNumber);
        Frequency++;

        if (!lineNumbers.Contains(lineNumber))
        {
            lineNumbers.AddLast(lineNumber);
        }
    }
}

