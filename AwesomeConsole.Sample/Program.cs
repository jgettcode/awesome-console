using System.Data;
using AwesomeConsole.Tables;
using AwesomeConsole.Tables.Sample;

Console.WriteLine("***** basic default usage");
new Table("first", "second column", "three")
    .AddRow(1, 2, Math.PI)
    .AddRow("hello", "very very very very very very long text", "world")
    .AddRow("github", "AwesomeTables", "https://github.com/jgettcode/awesomeconsole/")
    .Write();

Console.WriteLine();
Console.WriteLine();

Console.WriteLine("***** auto align numbers");
var rand = new Random();
Table.Configure(o => o.NumberAlignment = Alignment.Right)
    .AddColumn("first", "second column", "three")
    .AddRow(1, 2, Math.PI)
    .AddRow(rand.NextDouble(), rand.NextDouble(), rand.NextDouble())
    .AddRow("hello", "very very very very very very long text", "world")
    .AddRow("github", "AwesomeTables", "https://github.com/jgettcode/awesomeconsole/")
    .Write();

Console.WriteLine();
Console.WriteLine();

var table = new Table("first", "second column", "three")
    .AddRow(1, 2, Math.PI)
    .AddRow("hello", "very very very very very very long text", "world")
    .AddRow("github", "AwesomeTables", "https://github.com/jgettcode/awesomeconsole/");

Console.WriteLine("***** preset formats: Alternative");
table.Write(Format.Alternative);

Console.WriteLine();
Console.WriteLine();

Console.WriteLine("***** preset formats: Minimal");
table.Write(Format.Minimal);

Console.WriteLine();
Console.WriteLine();

Console.WriteLine("***** preset formats: Simple");
table.Write(Format.Simple);

Console.WriteLine();
Console.WriteLine();

Console.WriteLine("***** preset formats: Markdown");
table.Write(Format.Markdown);

Console.WriteLine();
Console.WriteLine();

Console.WriteLine("***** centered column headers");
Table.Configure(o => o.HeaderAlignment = Alignment.Center)
    .AddColumn("one", "two", "three")
    .AddRow(1, 2, Math.PI)
    .AddRow("hello", "very very very very very very long text", "world")
    .AddRow("github", "AwesomeTables", "https://github.com/jgettcode/awesomeconsole/")
    .Write(Format.Alternative);

Console.WriteLine();
Console.WriteLine();

Console.WriteLine("***** align entire column");
new Table(new TableColumn("one"), new TableColumn("two", Alignment.Center), new TableColumn("three"))
    .AddRow(1, 2, Math.PI)
    .AddRow("hello", "very very very very very very long text", "world")
    .AddRow("github", "AwesomeTables", "https://github.com/jgettcode/awesomeconsole/")
    .Write(Format.Alternative);

Console.WriteLine();
Console.WriteLine();

Console.WriteLine("***** custom formatting");
Table.Configure(o => o.HeaderAlignment = Alignment.Center)
    .AddColumn("first", "second column", "three")
    .AddRow(1, 2, Math.PI)
    .AddRow("hello", "very very very very very very long text", "world")
    .AddRow("github", "AwesomeTables", "https://github.com/jgettcode/awesomeconsole/")
    .Write(format =>
    {
       format
        .HideTopDivider()
        .ShowHeaderDivider('=', new('=', '^', '='))
        .HideValueDivider()
        .ShowBottomDivider('=', new('=', '^', '='))
        .SetDelimiters(new(' '), new('[', '|', ']'));
    });

Console.WriteLine();
Console.WriteLine();

Console.WriteLine("***** custom value formatting");
new Table("Last Name", "First Name", new TableColumn("Elected", valueAlignment: Alignment.Right), new TableColumn("Birthday", formatter: v => v.As<DateTime>().ToString("MMMM dd, yyyy")))
    .AddRow("Washington", "George", 1788, DateTime.Parse("1732-02-22"))
    .AddRow("Lincoln", "Abraham", 1860, DateTime.Parse("1809-02-12"))
    .AddRow("Roosevelt", "Theodore", 1904, DateTime.Parse("1858-10-27"))
    .AddRow("Kennedy", "John", 1960, DateTime.Parse("1917-05-29"))
    .Write(Format.Simple);

Console.WriteLine();
Console.WriteLine();

var data = new[]
{
    new President { LastName = "Washington", FirstName = "George", YearElected = 1788, Birthday = DateTime.Parse("1732-02-22") },
    new President { LastName = "Lincoln", FirstName = "Abraham", YearElected = 1860, Birthday = DateTime.Parse("1809-02-12") },
    new President { LastName = "Roosevelt", FirstName = "Theodore", YearElected = 1904, Birthday = DateTime.Parse("1858-10-27") },
    new President { LastName = "Kennedy", FirstName = "John", YearElected = 1960, Birthday = DateTime.Parse("1917-05-29") }
};

Console.WriteLine("***** generate from a collection of objects");
data.ToConsoleTable().Write();

Console.WriteLine();
Console.WriteLine();

Console.WriteLine("***** generate from collection with modified column");
new []
{
    new { first = "hello", second = 1, third = 0D },
    new { first = "hello world", second = 2, third = 2.34 },
    new { first = "hello pi", second = 3, third = Math.PI },
    new { first = "goodbye world", second = 4, third = 4.5 }
}.ToConsoleTable(o => o.Columns[2].Update(
    headerText: "number",
    headerAlignment: Alignment.Center,
    formatter: x => x.AsFormat<double>(x => x.ToString("0.00000000"))
)).Write();

Console.WriteLine();
Console.WriteLine();

var dict = new[]
{
    new Dictionary<string, object> { ["Last Name"] = "Washington", ["First Name"] = "George", ["Elected"] = 1788, ["Birthday"] = DateTime.Parse("1732-02-22") },
    new Dictionary<string, object> { ["Last Name"] = "Lincoln", ["First Name"] = "Abraham", ["Elected"] = 1860, ["Birthday"] = DateTime.Parse("1809-02-12") },
    new Dictionary<string, object> { ["Last Name"] = "Roosevelt", ["First Name"] = "Theodore", ["Elected"] = 1904, ["Birthday"] = DateTime.Parse("1858-10-27") },
    new Dictionary<string, object> { ["Last Name"] = "Kennedy", ["First Name"] = "John", ["Elected"] = 1960, ["Birthday"] = DateTime.Parse("1917-05-29") }
};

Console.WriteLine("***** generate from dictionaries");
dict.ToConsoleTable().Write();

Console.WriteLine();
Console.WriteLine();

var dt = new DataTable();
dt.Columns.Add("Last Name", typeof(string));
dt.Columns.Add("First Name", typeof(string));
dt.Columns.Add("Elected", typeof(int));
dt.Columns.Add("Birthday", typeof(DateTime));
dt.Rows.Add("Washington", "George", 1788, DateTime.Parse("1732-02-22"));
dt.Rows.Add("Lincoln", "Abraham", 1860, DateTime.Parse("1809-02-12"));
dt.Rows.Add("Roosevelt", "Theodore", 1904, DateTime.Parse("1858-10-27"));
dt.Rows.Add("Kennedy", "John", 1960, DateTime.Parse("1917-05-29"));

Console.WriteLine("***** generate from DataTable");
dt.ToConsoleTable().Write();

Console.WriteLine();
Console.WriteLine();

Console.WriteLine("***** hide the header");
new [] { new { a = "hello", b = "world" } }.ToConsoleTable().Write(format => format.HideHeader = true);

Console.WriteLine();
Console.WriteLine();

Console.WriteLine("***** unicode format");
new Table("first", "second column", "three")
    .AddRow(1, 2, Math.PI)
    .AddRow("hello", "very very very very very very long text", "world")
    .AddRow("github", "AwesomeTables", "https://github.com/jgettcode/awesomeconsole/")
    .Write(Format.Unicode);