using AwesomeConsole.Tables.Interfaces;

namespace AwesomeConsole.Tables;

public class Table : TableBase
{
    public static Table Configure(Action<TableOptions> action)
    {
        var options = new TableOptions();
        action(options);
        var table = new Table(options);
        return table;
    }

    public Table(params string[] columns)
        : this(columns.Select(c => new TableColumn(c)).ToArray()) { }

    public Table(params object[] columns)
        : this(columns.Select(TableColumn.FromObject).ToArray()) { }

    public Table(params TableColumn[] columns)
        : this(new TableOptions { Columns = columns }) { }

    protected Table(TableOptions options) : base(options) { }

    public Table AddColumn(params string[] columns)
        => AddColumn(columns.Select(c => new TableColumn(c)).ToArray());

    public Table AddColumn(params object[] columns)
        => AddColumn(columns.Select(TableColumn.FromObject).ToArray());

    public Table AddColumn(params TableColumn[] columns)
    {
        _columns.AddRange(columns);
        return this;
    }

    public Table AddRow(params object?[] values)
        => AddRow(values.Select(TableRowValue.FromObject).ToArray());

    public Table AddRow(params TableRowValue[] values)
        => AddRows(new[] { new TableRow(values) });

    public Table AddRows(params TableRow[] rows)
    {
        if (_columns.Count == 0)
            throw new Exception("Please set the columns first.");

        foreach (var row in rows)
        {
            if (_columns.Count != row.Count)
                throw new Exception($"The values count ({row.Count}) must equal the columns count ({_columns.Count})");

            _rows.Add(row);    
        }

        return this;
    }
}

public class Table<T> : TableBase
{
    private readonly List<T> _items;
    private readonly List<TableRow<T>> _trows;

    public override ITableRow[] Rows() => _trows.ToArray();

    public static Table<T> From(IEnumerable<T> items)
    {
        return new(items, o => o.NumberAlignment = Alignment.Right);
    }

    public static Table<T> From(IEnumerable<T> items, Action<TableOptions> action)
        => new(items, action);

    private static TableOptions GetOptions(Action<TableOptions> action)
    {
        var result = new TableOptions
        {
            Columns = typeof(T).GetProperties().Select(x => new TableColumn(x.Name)).ToArray(),
            NumberAlignment = Alignment.Right
        };

        action(result);

        return result;
    }

    private Table(IEnumerable<T> items, Action<TableOptions> action) : base(GetOptions(action))
    {
        _items = items.ToList();
        _trows = _items.Select(x => new TableRow<T>(x)).ToList();
    }

    public T[] Items() => _items.ToArray();   
}