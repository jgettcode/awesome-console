using System.Xml.Linq;
using System.Xml.XPath;
using AwesomeConsole.Tables;
using AwesomeConsole.Tables.Interfaces;

namespace AwesomeConsole.Tools.Table;

public static class Extensions
{
    public static ITable ToConsoleTable(this XDocument xdoc, string xpath)
    {
        var elements = xdoc.XPathSelectElements(xpath);
        if (elements?.Any() ?? false)
        {
            var list = elements.ToList();
            var cols = list[0].Elements().Select(x => x.Name).ToArray();
            var rows = list.Select(el => new TableRow(el.Elements().Select(x => new TableRowValue(x.Value)).ToArray())).ToArray() ?? Array.Empty<TableRow>();
            return new AwesomeConsole.Tables.Table(cols).AddRows(rows);
        }
        else
        {
            throw new Exception($"No elements were found using xpath: {xpath}");
        }
    }
}