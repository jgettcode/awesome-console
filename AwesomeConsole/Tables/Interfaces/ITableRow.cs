using AwesomeConsole.Tables.Interfaces;

namespace AwesomeConsole.Tables;

public interface ITableRow : IEnumerable<ITableRowValue>
{
    ITableRowValue this[int index] { get; }
    int Count { get; }
    ITableRowValue[] Values();
}