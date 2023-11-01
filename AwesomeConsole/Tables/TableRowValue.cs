using AwesomeConsole.Tables.Interfaces;

namespace AwesomeConsole.Tables;

public class TableRowValue : ITableRowValue
{
    public static TableRowValue FromObject(object? obj)
    {
        if (obj is TableRowValue result)
            return result;
        else
            return new TableRowValue(obj);
    }

    private readonly Func<ITableRowValue, string>? _formatter;

    public object? Value { get; }
    public Alignment? Alignment { get; set; }
    public bool HasFormatter => _formatter != null;
    
    public TableRowValue(object? value, Alignment? alignment = null, Func<ITableRowValue, string>? formatter = null)
    {
        Value = value;
        Alignment = alignment;
        _formatter = formatter;
    }

    public string Format()
        => (_formatter ?? throw new NullReferenceException("Formatter is null."))(this);
}