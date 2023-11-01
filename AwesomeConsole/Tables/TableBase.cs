using AwesomeConsole.Tables.Interfaces;

namespace AwesomeConsole.Tables;

public abstract class TableBase : ITable
{   
    protected readonly List<ITableColumn> _columns = new();
    protected readonly List<ITableRow> _rows = new();
    protected int[]? _lengths = null;

    public virtual ITableColumn[] Columns() => _columns.ToArray();
    public virtual ITableRow[] Rows() => _rows.ToArray();
    
    protected TableBase(TableOptions options)
    {
        Options = options;
        _columns = Options.Columns.ToList();
    }

    public TableOptions Options { get; }

    public int[] ColumnLengths()
    {
        var columns = Columns();
        var result = columns
            .Select((t, i) => Rows().Select(r => Format(r, i))
                .Union(new[] { columns[i].HeaderText })
                .Where(x => x != null)
                .Select(x => GetTextWidth(x)).Max())
            .ToArray();
        return result;
    }

    protected virtual int GetTextWidth(string? value)
    {
        if (value == null)
            return 0;

        // this doesn't seem to work
        //var length = value.ToCharArray().Sum(c => c > 127 ? 2 : 1);

        var length = value.Length;

        return length;
    }

    public string[] Divider(char c)
        => Columns().Select((_, n) => Utilities.Repeat(c, MaxLength(n))).ToArray();

    public string[] Header()
        => Columns().Select((c, n) => Utilities.PadText(MaxLength(n), GetHeaderAlignment(c), c.HeaderText)).ToArray();

    public string[] Values(ITableRow row)
        => Columns().Select((_, n) => ValueText(row, n)).ToArray();

    protected virtual string ValueText(ITableRow row, int index)
    {
        var text = Format(row, index);
        var maxlen = MaxLength(index);
        var length = maxlen - (GetTextWidth(text) - text.Length);
        var align = GetValueAlignment(row, index);
        var result = Utilities.PadText(length, align, text);
        return result;
    }

    protected int MaxLength(int index)
    {
        _lengths ??= ColumnLengths();
        var result = _lengths[index];
        return result;
    }

    protected Alignment GetHeaderAlignment(ITableColumn column, Alignment defval = Alignment.Left)
    {
        if (column.HeaderAlignment != null)
            return column.HeaderAlignment ?? throw new NullReferenceException("Column header alignment is null.");
        else if (Options.HeaderAlignment != null)
            return Options.HeaderAlignment ?? throw new NullReferenceException("Header alignment is null.");
        else
            return defval;
    }

    protected Alignment GetValueAlignment(ITableRow row, int index, Alignment defval = Alignment.Left)
    {
        var columns = Columns();
        var value = row[index];

        if (value.Alignment != null)
            return value.Alignment ?? throw new NullReferenceException("Value alignment is null.");
        else if (columns[index].ValueAlignment != null)
            return columns[index].ValueAlignment ?? throw new NullReferenceException("Column value alignment is null.");
        else if (Options.NumberAlignment.HasValue && Utilities.IsNumeric(value.Value))
            return Options.NumberAlignment ?? throw new NullReferenceException("Number alignment is null.");
        else
            return defval;  
    }

    protected string Format(ITableRow row, int index)
    {
        string? result;
        
        var value = row[index];
        var columns = Columns();

        if (value.HasFormatter)
            result = value.Format();
        else if (columns[index].HasFormatter)
            result = columns[index].Format(value);
        else
            result = value.Value?.ToString() ?? string.Empty;

        return result;
    }

    public void Write() => Write(Tables.Format.Default);

    public void Write(Action<TableFormat> action)
    {
        var format = new TableFormat();
        action(format);
        Write(format);
    }

    public void Write(TableFormat format)
    {
        Options.OutputTo.Write(ToString(format));
    }

    public override string ToString() => ToString(Tables.Format.Default);

    public string ToString(Action<TableFormat> action)
    {
        var format = new TableFormat();
        action(format);
        return ToString(format);
    }

    public string ToString(TableFormat format)
    {
        var builder = new TableBuilder(format);
        return builder.Build(this).ToString();
    }
}