using System.Collections;
using System.Runtime.CompilerServices;

namespace AwesomeConsole.Tables;

public readonly struct TableValue
{
	public static TableValue Empty => new(null);

	public object? Value { get; }

	public TableValue(object? value)
	{
		Value = value;
	}

    public override string ToString()
    {
        return Value?.ToString() ?? string.Empty;
    }

	public T As<T>()
	{
		if (Value is T result)
			return result;
		else
			throw new InvalidCastException();
	}
}

public class TableItem : IEnumerable<TableValue>
{
	private List<TableValue> _values { get; }
	
	public TableItem(params object?[] values)
	{
		_values = values.Select(x => new TableValue(x)).ToList();
	}

	[IndexerName("Value")]
	public TableValue this[int index]
	{
		get
		{
			if (_values.Count > index)
				return _values[index];
			else
				return TableValue.Empty;
		}
	}

    public IEnumerator<TableValue> GetEnumerator()
    {
		return _values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}