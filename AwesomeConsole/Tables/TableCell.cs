namespace AwesomeConsole.Tables;

public class TableCell
{
	public string Text { get; }
    public Alignment? Alignment { get; set; } = null;
	
	public TableCell(string text)
	{
		Text = text;
	}
}