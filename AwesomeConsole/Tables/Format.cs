using System.Text;

namespace AwesomeConsole.Tables;

public static class Format
{
    public static TableFormat Default => new();

    public static TableFormat Alternative => Configure(format =>
    {
        format
            .ShowTopDivider('-', new('+'))
            .ShowHeaderDivider('-', new('+'))
            .ShowValueDivider('-', new('+'))
            .ShowBottomDivider('-', new('+'))
            .SetDelimiters(new('|'));
    });

    public static TableFormat Simple => Configure(format =>
    {
        format
            .ShowTopDivider('-', new('+'))
            .ShowHeaderDivider('-', new('+'))
            .HideValueDivider()
            .ShowBottomDivider('-', new('+'))
            .SetDelimiters(new('|'));
    });

    public static TableFormat Minimal => Configure(format =>
    {
        format
            .HideTopDivider()
            .ShowHeaderDivider('-')
            .HideValueDivider()
            .HideBottomDivider()
            .NoDelimiters()
            .SetPad(inner: ' ');
    });

    public static TableFormat Markdown => Configure(format =>
    {
        format
            .ShowTopDivider(null)
            .ShowHeaderDivider('-', new('|'))
            .ShowValueDivider('-', new('|'))
            .ShowBottomDivider(null)
            .SetDelimiters(new('|'));
    });

    public static TableFormat Unicode => Configure(format =>
    {
        EnsureConsoleOutputEncodingIsUnicode();
        format
            .ShowTopDivider('─', new('┌', '┬', '┐'))
            .ShowHeaderDivider('─', new('├', '┼', '┤'))
            .ShowBottomDivider('─', new('└', '┴', '┘'))
            .ShowValueDivider('─', new('├', '┼', '┤'))
            .SetDelimiters(new('│'));
    });

    public static TableFormat SimpleUnicode => Configure(format =>
    {
        EnsureConsoleOutputEncodingIsUnicode();
        format
            .ShowTopDivider('─', new('┌', '┬', '┐'))
            .ShowHeaderDivider('─', new('├', '┼', '┤'))
            .ShowBottomDivider('─', new('└', '┴', '┘'))
            .HideValueDivider()
            .SetDelimiters(new('│'));
    });

    public static TableFormat Configure(Action<TableFormat> action)
    {
        var format = new TableFormat();
        action(format);
        return format.SanityCheck();
    }

    public static void EnsureConsoleOutputEncodingIsUnicode()
    {
        try
        {
            if (!Console.OutputEncoding.Equals(Encoding.Unicode))
            {
                Console.OutputEncoding = Encoding.Unicode;
            }
        }
        catch(Exception ex)
        {
            throw new FormatException("Failed to set Console output encoding to Unicode.", ex);
        }
    } 
}

public class FormatException : Exception
{
    public FormatException(string message, Exception innerException)
        : base(message, innerException) { }
}