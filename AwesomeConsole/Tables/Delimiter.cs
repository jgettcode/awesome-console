namespace AwesomeConsole.Tables;

public struct Delimiter
{
    public static Delimiter None => new();

    public Delimiter()
    {
        Left = null;
        Inner = null;
        Right = null;
    }

    public Delimiter(char c)
    {
        Left = c;
        Inner = c;
        Right = c;
    }

    public Delimiter(char? left, char? inner, char? right)
    {
        Left = left;
        Inner = inner;
        Right = right;
    }

    public char? Left { get; set; }
    public char? Inner { get; set; }
    public char? Right { get; set; }
}