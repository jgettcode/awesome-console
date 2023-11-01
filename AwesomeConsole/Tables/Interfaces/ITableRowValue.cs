namespace AwesomeConsole.Tables.Interfaces;

public interface ITableRowValue
{
    object? Value { get; }
    bool HasFormatter { get; }
    Alignment? Alignment { get; }
    string Format();
}