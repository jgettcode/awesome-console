using System.Data;

namespace AwesomeConsole.Tables;

public static class TableExtensions
{
    public static TableFormat HideHeader(this TableFormat format)
    {
        format.HideHeader = true;
        return format;
    }

    public static TableFormat HideAll(this TableFormat format)
    {
        return format
            .HideHeader()
            .HideTopDivider()
            .HideHeaderDivider()
            .HideValueDivider()
            .HideBottomDivider()
            .NoDelimiters()
            .NoPad();
    }

    public static TableFormat HideTopDivider(this TableFormat format)
        => format.ShowTopDivider(null);

    public static TableFormat ShowTopDivider(this TableFormat format, char? divider, Delimiter? delimiter = null)
    {
        format.TopDivider = divider;
        format.TopDividerDelimiter = divider.HasValue ? delimiter.GetValueOrDefault(Delimiter.None) : Delimiter.None;
        return format;
    }

    public static TableFormat HideHeaderDivider(this TableFormat format)
        => format.ShowHeaderDivider(null);

    public static TableFormat ShowHeaderDivider(this TableFormat format, char? divider, Delimiter? delimiter = null)
    {
        if (divider == null || format.TopDivider == null || format.TopDividerDelimiter.SanityCheck(delimiter))
        {
            format.HeaderDivider = divider;
            format.HeaderDividerDelimiter = divider.HasValue ? delimiter.GetValueOrDefault(Delimiter.None) : Delimiter.None;
            return format;
        }

        throw new Exception("Top and Header divider delimiters do not match.");
    }

    public static TableFormat HideValueDivider(this TableFormat format)
        => format.ShowValueDivider(null);

    public static TableFormat ShowValueDivider(this TableFormat format, char? divider, Delimiter? delimiter = null)
    {
        if (divider == null || format.HeaderDivider == null || format.HeaderDividerDelimiter.SanityCheck(delimiter))
        {
            format.ValueDivider = divider;
            format.ValueDividerDelimiter = divider.HasValue ? delimiter.GetValueOrDefault(Delimiter.None) : Delimiter.None;
            return format;
        }

        throw new Exception("Header and Value divider delimiters do not match.");
    }

    public static TableFormat HideBottomDivider(this TableFormat format)
        => format.ShowBottomDivider(null);

    public static TableFormat ShowBottomDivider(this TableFormat format, char? divider, Delimiter? delimiter = null)
    {
        if (divider == null || format.ValueDivider == null || format.ValueDividerDelimiter.SanityCheck(delimiter))
        {
            format.BottomDivider = divider;
            format.BottomDividerDelimiter = divider.HasValue ? delimiter.GetValueOrDefault(Delimiter.None) : Delimiter.None;
            return format;
        }

        throw new Exception("Value and Bottom divider delimiters do not match.");
    }

    public static TableFormat SetDelimiters(this TableFormat format, Delimiter both)
    {
        format.HeaderDelimiter = both;
        format.ValueDelimiter = both;
        return format;
    }

    public static TableFormat NoDelimiters(this TableFormat format)
        => format.SetDelimiters(Delimiter.None, Delimiter.None);

    public static TableFormat SetDelimiters(this TableFormat format, Delimiter header, Delimiter value)
    {
        if (header.SanityCheck(value))
        {
            format.HeaderDelimiter = header;
            format.ValueDelimiter = value;
            return format;
        }

        throw new Exception("Header and Value delimiters do not match.");
    }

    public static TableFormat SetPad(this TableFormat format, char? left = null, char? inner = null, char? right = null)
    {
        format.Pad = new(left, inner, right);
        return format;
    }

    public static TableFormat SetPad(this TableFormat format, Delimiter? delimiter)
    {
        format.Pad = delimiter.GetValueOrDefault(Delimiter.None);
        return format;
    }

    public static TableFormat NoPad(this TableFormat format)
        => format.SetPad(Delimiter.None);

    public static bool SanityCheck(this Delimiter d1, Delimiter? d2)
        => d1.SanityCheck(d2.GetValueOrDefault(Delimiter.None));

    public static bool SanityCheck(this Delimiter d1, Delimiter d2)
    {
        return ((d1.Left == null && d2.Left == null) || (d1.Left != null && d2.Left != null))
            && ((d1.Inner == null && d2.Inner == null) || (d1.Inner != null && d2.Inner != null))
            && ((d1.Right == null && d2.Right == null) || (d1.Right != null && d2.Right != null));
    }

    public static TableFormat SanityCheck(this TableFormat format)
    {
        var sanity = (format.TopDivider == null || format.HeaderDivider == null || format.TopDividerDelimiter.SanityCheck(format.HeaderDividerDelimiter))
            && (format.HeaderDivider == null || format.ValueDivider == null || format.HeaderDividerDelimiter.SanityCheck(format.ValueDividerDelimiter))
            && (format.ValueDivider == null || format.BottomDivider == null || format.ValueDividerDelimiter.SanityCheck(format.BottomDividerDelimiter))
            && (format.BottomDivider == null || format.TopDivider == null || format.BottomDividerDelimiter.SanityCheck(format.TopDividerDelimiter))
            && format.HeaderDelimiter.SanityCheck(format.ValueDelimiter);

        if (sanity)
            return format;
        
        throw new Exception("The divider delimiters do not match.");
    }

    public static Table<T> AddColumn<T>(this Table<T> table, string headerText, Func<T, int, object?> expression)
	{
		table.AddColumn(new TableColumn<T>(headerText, expression));
        return table;
	}

    public static Table AddColumn(this Table table, string headerText, Func<TableItem, int, object?> expression)
    {
        table.AddColumn(new TableColumn<TableItem>(headerText, expression));
        return table;
    }

    public static Table<T> ToConsoleTable<T>(this IEnumerable<T> items, Action<TableOptions<T>>? action = null)
        => new(items, action);

    public static Table ToConsoleTable(this DataTable dt, Action<TableOptions>? action = null)
    {
        var cols = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();

        if (cols.Length == 0)
            throw new Exception("no columns were found.");

        var rows = dt.AsEnumerable().Select(dr => dr.ToTableItem()).ToArray();

        var table = Table.Configure(action ?? (o => o.NumberAlignment = Alignment.Right))
            .AddColumns(cols)
            .AddRows(rows);

        return table;
    }

    public static Table ToConsoleTable(this Dictionary<string, object?>[] data, Action<TableOptions>? action = null)
    {
        if (!data.Any())
            throw new ArgumentException("Data must contain at least one item", nameof(data));

        var cols = data.First().Select(x => x.Key).ToArray();

        if (cols.Length == 0)
            throw new Exception("no columns were found.");

        var rows = data.Select(d => new TableItem(d.Select(v => v.Value).ToArray())).ToArray();

        return Table
            .Configure(action ?? (o => o.NumberAlignment = Alignment.Right))
            .AddColumns(cols)
            .AddRows(rows);
    }

    public static TableItem ToTableItem(this DataRow dr)
        => new(dr.ItemArray);
}