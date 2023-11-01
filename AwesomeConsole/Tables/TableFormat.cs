namespace AwesomeConsole.Tables;

public class TableFormat
{
    public static TableFormat Empty => new()
    {
        HideHeader = false,
        TopDivider = null,
        TopDividerDelimiter = Delimiter.None,
        HeaderDelimiter = Delimiter.None,
        HeaderDivider = null,
        HeaderDividerDelimiter = Delimiter.None,
        ValueDelimiter = Delimiter.None,
        ValueDivider = null,
        ValueDividerDelimiter = Delimiter.None,
        BottomDivider = null,
        BottomDividerDelimiter = Delimiter.None,
        Pad = Delimiter.None
    };

    public bool HideHeader { get; set; }

    public char? TopDivider { get; set; } = '-';
    public Delimiter TopDividerDelimiter { get; set; } = new('-');

    public Delimiter HeaderDelimiter { get; set; } = new('|');

    public char? HeaderDivider { get; set; } = '-';
    public Delimiter HeaderDividerDelimiter { get; set; } = new('-');

    public Delimiter ValueDelimiter { get; set; } = new('|');

    public char? ValueDivider { get; set; } = '-';
    public Delimiter ValueDividerDelimiter { get; set; } = new('-');

    public char? BottomDivider { get; set; } = '-';
    public Delimiter BottomDividerDelimiter { get; set; } = new('-');

    public Delimiter Pad { get; set; } = new(' ');
}