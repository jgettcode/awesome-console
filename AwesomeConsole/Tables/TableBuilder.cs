using AwesomeConsole.Tables.Interfaces;
using System.Text;

namespace AwesomeConsole.Tables;

public class TableBuilder
{
    private string? _valueDivider = null;
    private string? _newLine = null;
    private readonly StringBuilder _builder;

    public TableFormat Format { get; }

    public TableBuilder(TableFormat format)
    {
        _builder = new StringBuilder();
        Format = format;
    }

    public StringBuilder Build(ITable table)
    {
        if (table.Columns().Length == 0)
            throw new Exception("No columns have been added.");

        _valueDivider = null;
        _newLine = null;

        WriteTopDivider(table);

        if (!Format.HideHeader)
        {
            WriteHeader(table);

            // always show divider between header and rows
            WriteHeaderDivider(table);
        }

        var rows = table.Rows();
        for (var i = 0; i < rows.Length; i++)
        {
            WriteRow(table, rows[i]);

            if (i == rows.Length - 1)
                // special handling for last row
                WriteBottomDivider(table);
            else
                WriteValueDivider(table);
        }

        if (table.Options.EnableCount)
        {
            Append(string.Empty);
            Append($"Count: {rows.Length}");
        }

        return _builder;
    }

    protected virtual void Append(string? value = null)
    {
        if (value == null) return;
        _builder.Append(_newLine + value);
        _newLine = Environment.NewLine;
    }

    protected virtual string GetLine(IEnumerable<object> items, Delimiter pad, Delimiter delimiter)
        => $"{delimiter.Left}{pad.Left}" + string.Join($"{pad.Inner}{delimiter.Inner}{pad.Inner}", items) + $"{pad.Right}{delimiter.Right}";

    protected virtual string? GetValueDivider(ITable table)
    {
        _valueDivider ??= GetDivider(table, Format.ValueDivider, Format.ValueDividerDelimiter);
        return _valueDivider;
    }

    protected virtual string? GetDivider(ITable table, char? divider, Delimiter delimiter)
    {
        if (divider == null) return null;
        var left = Format.Pad.Left == null ? null : divider;
        var inner = Format.Pad.Inner == null ? null : divider;
        var right = Format.Pad.Right == null ? null : divider;
        var pad = new Delimiter(left, inner, right);
        return GetLine(table.Divider(divider.Value), pad, delimiter);
    }

    protected void WriteTopDivider(ITable table)
        => Append(GetDivider(table, Format.TopDivider, Format.TopDividerDelimiter));

    protected void WriteHeaderDivider(ITable table)
        => Append(GetDivider(table, Format.HeaderDivider, Format.HeaderDividerDelimiter));

    protected void WriteValueDivider(ITable table)
        => Append(GetDivider(table, Format.ValueDivider, Format.ValueDividerDelimiter));

    protected void WriteBottomDivider(ITable table)
        => Append(GetDivider(table, Format.BottomDivider, Format.BottomDividerDelimiter));

    protected void WriteHeader(ITable table)
        => Append(GetLine(table.Header(), Format.Pad, Format.HeaderDelimiter));

    protected void WriteRow(ITable table, ITableRow row)
        => Append(GetLine(table.Values(row), Format.Pad, Format.ValueDelimiter));
}