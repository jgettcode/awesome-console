using AwesomeConsole.Tables.Interfaces;
using System.Collections;

namespace AwesomeConsole.Tables;

public class TableRow : TableRowBase
{
    public TableRow(params ITableRowValue[] values) : base()
    {
        Add(values);
    }
}

public class TableRow<T> : TableRowBase
{
    private readonly T _item;

    public TableRow(T item)
    {
        _item = item;
        _values.AddRange(typeof(T).GetProperties().Select(x => new TableRowValue(x.GetValue(_item))));
    }

    public T Data => _item;
}