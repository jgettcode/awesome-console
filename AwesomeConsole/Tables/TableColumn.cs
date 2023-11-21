using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

namespace AwesomeConsole.Tables;

public class TableColumn<T>
{
	public string HeaderText { get; set; }
	public string PropertyName { get; set; } = string.Empty;
    public Alignment? HeaderAlignment { get; set; }
    public Alignment? CellAlignment { get; set; }
    public string? Format { get; set; }
    public Func<T, int, object?> Expression { get; set; }

	public TableColumn(string headerText, string? propertyName = null, Alignment? headerAlignment = null, Alignment? cellAlignment = null, string? format = null)
	{
		HeaderText = headerText;
		PropertyName = propertyName ?? headerText;
        HeaderAlignment = headerAlignment;
        CellAlignment = cellAlignment;
        Format = format;
        Expression = (x, i) => typeof(T).GetProperty(PropertyName)?.GetValue(x);
	}

    public TableColumn(string headerText, Func<T, int, object?> expression, Alignment? headerAlignment = null, Alignment? cellAlignment = null, string? format = null)
    {
        HeaderText = headerText;
        HeaderAlignment = headerAlignment;
        CellAlignment = cellAlignment;
        Format = format;
        Expression = expression;
    }
}

public class TableColumn : TableColumn<TableItem>
{
    public static IEnumerable<TableColumn> FromObjects(object?[] objects)
    {
        var result = objects.Select((o, n) =>
        {
            if (o == null)
                return new TableColumn(string.Empty);
            else if (o is TableColumn col)
                return col;
            else
                return new TableColumn(o.ToString() ?? string.Empty);
        }).ToList();

        return result;
    }

    public TableColumn(string headerText)
        : this(headerText, (x, i) => x[i].Value, null, null, null) { }

    public TableColumn(string headerText, string? format)
        : this(headerText, (x, i) => x[i].Value, null, null, format) { }

    public TableColumn(string headerText, Alignment? alignment)
        : this(headerText, (x, i) => x[i].Value, alignment, alignment, null) { }

    public TableColumn(string headerText, Alignment? alignment, string? format)
        : this(headerText, (x, i) => x[i].Value, alignment, alignment, format) { }

    public TableColumn(string headerText, Alignment? headerAlignment, Alignment? cellAlignment)
        : this(headerText, (x, i) => x[i].Value, headerAlignment, cellAlignment, null) { }

    public TableColumn(string headerText, Alignment? headerAlignment, Alignment? cellAlignment, string format)
        : this(headerText, (x, i) => x[i].Value, headerAlignment, cellAlignment, format) { }

    public TableColumn(string headerText, Func<TableItem, int, object?> expression, Alignment? headerAlignment = null, Alignment? cellAlignment = null, string? format = null) : base(headerText)
    {
        HeaderAlignment = headerAlignment;
        CellAlignment = cellAlignment;
        Format = format;
        Expression = expression;
    }
}