namespace TextAnalysisTool;

/// <summary>
/// An unbalanced binary search tree that indexes normalised words.
/// </summary>
public sealed class BinaryTree
{
    private Node? root;

    public int Count { get; private set; }

    public void Insert(string word, int lineNumber)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(word);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(lineNumber);

        root = Insert(root, word.ToLowerInvariant(), lineNumber);
    }

    public WordInfo? Find(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
        {
            return null;
        }

        Node? current = root;

        while (current is not null)
        {
            int comparison = StringComparer.OrdinalIgnoreCase.Compare(
                word,
                current.Data.Word);

            if (comparison == 0)
            {
                return current.Data;
            }

            current = comparison < 0 ? current.Left : current.Right;
        }

        return null;
    }

    public IEnumerable<WordInfo> GetWordsInOrder()
    {
        return TraverseInOrder(root);
    }

    public WordInfo? FindMostFrequentWord()
    {
        WordInfo? result = null;

        foreach (WordInfo candidate in GetWordsInOrder())
        {
            if (result is null ||
                candidate.Frequency > result.Frequency ||
                (candidate.Frequency == result.Frequency &&
                 StringComparer.Ordinal.Compare(candidate.Word, result.Word) < 0))
            {
                result = candidate;
            }
        }

        return result;
    }

    public WordInfo? FindLongestWord()
    {
        WordInfo? result = null;

        foreach (WordInfo candidate in GetWordsInOrder())
        {
            if (result is null ||
                candidate.Word.Length > result.Word.Length ||
                (candidate.Word.Length == result.Word.Length &&
                 StringComparer.Ordinal.Compare(candidate.Word, result.Word) < 0))
            {
                result = candidate;
            }
        }

        return result;
    }

    private Node Insert(Node? current, string word, int lineNumber)
    {
        if (current is null)
        {
            Count++;
            return new Node(new WordInfo(word, lineNumber));
        }

        int comparison = StringComparer.OrdinalIgnoreCase.Compare(
            word,
            current.Data.Word);

        if (comparison < 0)
        {
            current.Left = Insert(current.Left, word, lineNumber);
        }
        else if (comparison > 0)
        {
            current.Right = Insert(current.Right, word, lineNumber);
        }
        else
        {
            current.Data.RecordOccurrence(lineNumber);
        }

        return current;
    }

    private static IEnumerable<WordInfo> TraverseInOrder(Node? current)
    {
        if (current is null)
        {
            yield break;
        }

        foreach (WordInfo item in TraverseInOrder(current.Left))
        {
            yield return item;
        }

        yield return current.Data;

        foreach (WordInfo item in TraverseInOrder(current.Right))
        {
            yield return item;
        }
    }
}

