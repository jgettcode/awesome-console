using System.Diagnostics.Metrics;
using System.Net;

namespace AwesomeConsole.Tables;

public class Table<T>
{
	protected List<T> _items = new();
	protected List<TableColumn<T>> _columns = new();

    public TableOptions<T> Options { get; }

	public Table(IEnumerable<T> items)
		: this(items, typeof(T).GetProperties().Select(t => t.Name).Select(x => new TableColumn<T>(x)).ToArray()) { }
	
	public Table(IEnumerable<T> items, params TableColumn<T>[] columns)
        : this(items, new TableOptions<T> { Columns = columns }) { }

    public Table(IEnumerable<T> items, Action<TableOptions<T>>? action = null)
    {
        var columns = typeof(T).GetProperties().Select(t => t.Name).Select(x => new TableColumn<T>(x)).ToArray();

        var options = new TableOptions<T>
        {
            Columns =  columns,
            NumberAlignment = Alignment.Right
        };

        action?.Invoke(options);

        _items.AddRange(items);
        _columns.AddRange(columns);

        Options = options;
    }

    protected Table(IEnumerable<T> items, TableOptions<T> options) : this(options)
    {
        _items.AddRange(items);
    }

    protected Table(TableOptions<T> options)
    {
        _columns.AddRange(options.Columns);
        Options = options;
    }
	
	public TableColumn<T>[] Columns() => _columns.ToArray();

    public void AddColumn(TableColumn<T> column)
	{
        _columns.Add(column);
	}

	public TableRow<T>[] Rows()
	{
        var result = new List<TableRow<T>>();
        foreach (var i in _items)
        {
            var cells = GetCells(i).ToArray();
            var row = new TableRow<T>(i, cells);
            result.Add(row);
        }
        return result.ToArray();
	}
	
	protected void AddRow(T item)
	{
		_items.Add(item);
	}
	
	protected IEnumerable<TableCell> GetCells(T item)
	{
        var cells = Columns().Select((c, n) =>
        {
            var value = c.Expression.Invoke(item, n);
            string text = (c.Format != null)
                ? string.Format(c.Format, value)
                : value?.ToString() ?? string.Empty;
			return new TableCell(text ?? string.Empty);
        }).ToList();
		
		return cells;
	}

    protected Alignment GetCellAlignment(TableRow<T> row, int index, Alignment defval = Alignment.Left)
    {
        var columns = Columns();
        var cell = row.Cells[index];
        var value = columns[index].Expression.Invoke(row.Item, index);

        if (cell.Alignment != null)
            return cell.Alignment ?? throw new NullReferenceException("Cell alignment is null.");
        else if (columns[index].CellAlignment != null)
            return columns[index].CellAlignment ?? throw new NullReferenceException("Column value alignment is null.");
        else if (Options.NumberAlignment.HasValue && Utilities.IsNumeric(value))
            return Options.NumberAlignment ?? throw new NullReferenceException("Number alignment is null.");
        else
            return defval;
    }

    protected virtual string CellText(TableRow<T> row, int index)
    {
        var text = row.Cells[index].Text;
        var maxlen = MaxLength(index);
        var length = maxlen - (GetTextWidth(text) - text.Length);
        var align = GetCellAlignment(row, index);
        var result = Utilities.PadText(length, align, text);
        return result;
    }

    protected Alignment GetHeaderAlignment(TableColumn<T> column, Alignment defval = Alignment.Left)
    {
        if (column.HeaderAlignment != null)
            return column.HeaderAlignment ?? throw new NullReferenceException("Column header alignment is null.");
        else if (Options.HeaderAlignment != null)
            return Options.HeaderAlignment ?? throw new NullReferenceException("Header alignment is null.");
        else
            return defval;
    }

    public string[] Header()
        => Columns().Select((c, n) => Utilities.PadText(MaxLength(n), GetHeaderAlignment(c), c.HeaderText)).ToArray();

    public string[] Values(TableRow<T> row)
        => Columns().Select((_, n) => CellText(row, n)).ToArray();

    protected int GetTextWidth(string? value)
    {
        if (value == null)
            return 0;

        // this doesn't seem to work
        //var length = value.ToCharArray().Sum(c => c > 127 ? 2 : 1);

        var length = value.Length;

        return length;
    }

    public int[] ColumnLengths()
    {
        var columns = Columns();
        var rows = Rows();

        var result = columns
            .Select((t, i) => rows.Select(r => r.Cells[i].Text)
                .Union(new[] { columns[i].HeaderText })
                .Where(x => x != null)
                .Select(x => GetTextWidth(x)).Max())
            .ToArray();
        return result;
    }

    protected int MaxLength(int index)
    {
        var lengths = ColumnLengths();
        var result = lengths[index];
        return result;
    }

    public string[] Divider(char c)
        => Columns().Select((_, n) => Utilities.Repeat(c, MaxLength(n))).ToArray();
	
    public override string ToString() => ToString(Format.Default);

    public string ToString(Action<TableFormat> action)
    {
        var format = new TableFormat();
        action(format);
        return ToString(format);
    }

    public string ToString(TableFormat format)
    {
        var builder = new TableBuilder<T>(format);
        return builder.Build(this).ToString();
    }
	
    public void Write() => Write(Format.Default);

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
}

public class Table : Table<TableItem>
{
    public static Table<T> From<T>(IEnumerable<T> items, Action<TableOptions<T>>? action = null)
    {
        return new Table<T>(items, action);
    }

    public static Table Configure(Action<TableOptions> action)
    {
        var options = new TableOptions();
        action.Invoke(options);
        return new Table(options);
    }

    public Table(params object?[] columns)
        : base(new TableOptions { Columns = TableColumn.FromObjects(columns).ToArray() }) { }

    private Table(TableOptions options) : base(options) { }
	
	public Table AddRows(params TableItem[] items)
	{
		foreach (var i in items)
			_items.Add(i);
		return this;
	}
	
	public Table AddRow(params object?[] values)
	{
		_items.Add(new TableItem(values));
		return this;
	}
	
	public Table AddColumns(params object?[] columns)
	{
		_columns.AddRange(TableColumn.FromObjects(columns).ToArray());
		return this;
	}
}