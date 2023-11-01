using AwesomeConsole.Tables;
using AwesomeConsole.Tools.Table;
using CommandLine;
using System.Xml.Linq;

Parser.Default.ParseArguments<Options>(args).WithParsed(options =>
{
    var format = options.Format switch
    {
        "alternative" => Format.Alternative,
        "simple" => Format.Simple,
        "minimal" => Format.Minimal,
        "markdown" => Format.Markdown,
        "unicode" => Format.Unicode,
        "usimple" => Format.SimpleUnicode,
        _ => Format.Default,
    };

    var xml = GetPipeInput() ?? options.XML;

    if (!string.IsNullOrEmpty(xml))
    {
        try
        {
            XDocument xdoc = XDocument.Parse(xml);
            xdoc.ToConsoleTable(options.XPath).Write(format);
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }
    else
    {
        ShowError("No XML was found.");
    }
});

static string? GetPipeInput()
{
    if (Console.IsInputRedirected)  
    {
        using var s = Console.OpenStandardInput();
        using var reader = new StreamReader(s);
        var xml = reader.ReadToEnd();
        return xml;
    }

    return null;
}

static void ShowError(string errmsg)
{
    var clr = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(errmsg);
    Console.ForegroundColor = clr;
}