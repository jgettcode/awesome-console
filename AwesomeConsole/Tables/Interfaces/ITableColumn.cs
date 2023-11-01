
namespace AwesomeConsole.Tables.Interfaces;

public interface ITableColumn
{
    string HeaderText { get; }
    bool HasFormatter { get; }
    Alignment? HeaderAlignment { get; }
    Alignment? ValueAlignment { get; }
    string Format(ITableRowValue value);
    void Update(string? headerText = null, Alignment? headerAlignment = null, Alignment? valueAlignment = null, Func<ITableRowValue, string>? formatter = null);
}