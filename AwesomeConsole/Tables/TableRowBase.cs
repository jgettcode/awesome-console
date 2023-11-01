using AwesomeConsole.Tables.Interfaces;
using System.Collections;

namespace AwesomeConsole.Tables;

public abstract class TableRowBase : ITableRow
{
    protected List<ITableRowValue> _values = new();

    public ITableRowValue this[int index] => _values[index];

    public int Count => _values.Count;

    public void Add(params ITableRowValue[] values)
        => _values.AddRange(values);

    public ITableRowValue[] Values()
    {
        return _values.ToArray();
    }

    public IEnumerator<ITableRowValue> GetEnumerator()
    {
        return _values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}