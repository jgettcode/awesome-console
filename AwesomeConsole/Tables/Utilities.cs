namespace AwesomeConsole.Tables;

public static class Utilities
{
    public static readonly Type[] NumericTypes  = new []
    {
        typeof(int),  typeof(double),  typeof(decimal),
        typeof(long), typeof(short),   typeof(sbyte),
        typeof(byte), typeof(ulong),   typeof(ushort),
        typeof(uint), typeof(float)
    };

    public static bool IsNumeric(object? v) => NumericTypes.Contains(v?.GetType());

    public static string Repeat(char c, int count) => new(c, count);

    public static string PadText(int len, Alignment alignment, string text)
    {
        if (alignment == Alignment.Left)
            return text.PadRight(len, ' ');
        else if (alignment == Alignment.Right)
            return text.PadLeft(len, ' ');
        else if (alignment == Alignment.Center)
        {
            int spaces = len - text.Length;
            int padLeft = spaces / 2 + text.Length;
            return text.PadLeft(padLeft, ' ').PadRight(len, ' ');
        }
        else
            throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null);
    }
}