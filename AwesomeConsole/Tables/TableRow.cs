namespace AwesomeConsole.Tables;

public class TableRow<T>
{
	private readonly List<TableCell> _cells;
	
	public T Item { get; }
	
	public TableRow(T item, params TableCell[] cells)
	{
		_cells = cells.ToList();
		Item = item;
	}
	
	public TableCell[] Cells => _cells.ToArray();
}