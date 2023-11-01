using CommandLine;

namespace AwesomeConsole.Tools.Table;

public class Options
{
    [Value(0, Required = false, Default = "", HelpText = "XML to parse and generate a table for. You can also pipe it in (e.g. command-that-produces-xml | table).")]
    public string XML { get; set; } = string.Empty;

    [Option('f', "format", Required = false, Default = "default", HelpText = "The AwesomeConsole Table preset format to use.")]
    public string Format { get; set; } = "default";

    [Option('p', "xpath", Required = false, Default = "/root/item", HelpText = "The xpath to select items with. Should return a list of elements with matching schemas.")]
    public string XPath { get; set; } = "/root/item";
}