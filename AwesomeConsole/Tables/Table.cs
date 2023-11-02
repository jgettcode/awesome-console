using System.Collections;

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

    public Table(IEnumerable columns)
        : this(columns.Cast<object>().Select(TableColumn.FromObject)) { }

    public Table(params object?[] columns)
        : this(columns.Select(TableColumn.FromObject)) { }

    public Table(IEnumerable<TableColumn> columns)
        : this(new TableOptions { Columns = columns.ToArray() }) { }

    protected Table(TableOptions options) : base(options) { }

    public Table AddColumn(params object?[] columns)
        => AddColumn(columns.Select(TableColumn.FromObject));

    public Table AddColumn(IEnumerable columns)
        => AddColumn(columns.Cast<object>().Select(TableColumn.FromObject));

    public Table AddColumn(IEnumerable<TableColumn> columns)
    {
        _columns.AddRange(columns);
        return this;
    }

    public Table AddRow(params object?[] values)
        => AddRow(values.Select(TableRowValue.FromObject).ToArray());

    public Table AddRow(params TableRowValue[] values)
        => AddRow(new[] { new TableRow(values) });

    public Table AddRow(params TableRow[] rows)
        => AddRow(rows.AsEnumerable());

    public Table AddRow(IEnumerable<TableRow> rows)
    {
        if (_columns.Count == 0)
            throw new Exception("Please set the columns first.");

        var errors = rows.Select((r, n) => new {row = r, index = n + 1})
            .Where(x => _columns.Count != x.row.Count)
            .Select(x => $"The values count in row #{x.index} ({x.row.Count}) must equal the columns count ({_columns.Count}).");

        if (errors.Any())
        {
            throw new Exception(string.Join(Environment.NewLine, errors));
        }

        _rows.AddRange(rows);
        
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