using AwesomeConsole.Tables.Interfaces;

namespace AwesomeConsole.Tables;

public class TableOptions
{
    public ITableColumn[] Columns { get; set; } = Array.Empty<TableColumn>();
    public bool EnableCount { get; set; } = false;
    public Alignment? NumberAlignment { get; set; } = null;
    public Alignment? HeaderAlignment { get; set; } = null;
    public TextWriter OutputTo { get; set; } = Console.Out;
}