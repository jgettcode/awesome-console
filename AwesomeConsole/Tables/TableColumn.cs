using AwesomeConsole.Tables.Interfaces;

namespace AwesomeConsole.Tables;

public class TableColumn : ITableColumn
{
    public static TableColumn FromObject(object c)
    {
        if (c is TableColumn result)
            return result;
        else
            return new TableColumn(c.ToString() ?? throw new NullReferenceException("Invalid null column value."));
    }

    private Func<ITableRowValue, string>? _formatter;

    public string HeaderText { get; private set; }
    public Alignment? HeaderAlignment { get; private set; }
    public Alignment? ValueAlignment { get; private set; }
    public bool HasFormatter => _formatter != null;
    
    public TableColumn(string headerText)
        : this(headerText, null, null, null) { }

    public TableColumn(string headerText, Alignment alignment)
        : this(headerText, headerAlignment: alignment, valueAlignment: alignment, null) { }

    public TableColumn(string headerText, Alignment headerAlignment, Alignment valueAlignment)
        : this(headerText, headerAlignment: headerAlignment, valueAlignment: valueAlignment, null) { }

    public TableColumn(string headerText, Func<ITableRowValue, string> formatter)
        : this(headerText, null, null, formatter) { }

    public TableColumn(string headerText, Alignment? headerAlignment = null, Alignment? valueAlignment = null, Func<ITableRowValue, string>? formatter = null)
    {
        HeaderText = headerText;
        HeaderAlignment = headerAlignment;
        ValueAlignment = valueAlignment;
        _formatter = formatter;
    }

    public string Format(ITableRowValue row)
        => (_formatter ?? throw new NullReferenceException("Formatter is null."))(row);

    public void Update(string? headerText = null, Alignment? headerAlignment = null, Alignment? valueAlignment = null, Func<ITableRowValue, string>? formatter = null)
    {
        if (headerText != null)
            HeaderText = headerText;
        
        if (headerAlignment != null)
            HeaderAlignment = headerAlignment;
        
        if (valueAlignment != null)
            ValueAlignment = valueAlignment;

        if (formatter != null)
            _formatter = formatter;
    }
}