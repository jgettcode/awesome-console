namespace AwesomeConsole.Tables;

public class TableOptions<T>
{
    public TableColumn<T>[] Columns { get; set; } = Array.Empty<TableColumn<T>>();
    public bool EnableCount { get; set; } = false;
    public Alignment? NumberAlignment { get; set; } = null;
    public Alignment? HeaderAlignment { get; set; } = null;
    public TextWriter OutputTo { get; set; } = Console.Out;
}

public class TableOptions : TableOptions<TableItem> { }