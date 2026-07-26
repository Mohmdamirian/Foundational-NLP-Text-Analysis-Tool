namespace TextAnalysisTool;

internal sealed class Node
{
    public Node(WordInfo data)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data));
    }

    public WordInfo Data { get; }

    public Node? Left { get; set; }

    public Node? Right { get; set; }
}

