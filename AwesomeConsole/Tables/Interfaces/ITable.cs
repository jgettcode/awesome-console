namespace AwesomeConsole.Tables.Interfaces;

public interface ITable
{
    TableOptions Options { get; }
    ITableColumn[] Columns();
    ITableRow[] Rows();
    string[] Values(ITableRow row);
    string[] Header();
    string[] Divider(char c);
    void Write();
    void Write(Action<TableFormat> action);
    void Write(TableFormat format);
    string ToString(Action<TableFormat> action);
    string ToString(TableFormat format);
}