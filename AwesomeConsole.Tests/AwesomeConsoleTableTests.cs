using Xunit.Sdk;

namespace AwesomeConsole.Tables.Tests;

public class AwesomeConsoleTableTests
{
    [Fact]
    public void CanWriteNullHeader()
    {
        Table table;
        string actual;
        string expected =
        """
        -------------------------------------------------
        | one   |       | 3                             |
        -------------------------------------------------
        | 1     | 2     | three                         |
        -------------------------------------------------
        | hello | world | very long text very long text |
        -------------------------------------------------
        """;

        // mix of types and null
        table = new Table("one", null, 3)
            .AddRow(1, 2, "three")
            .AddRow("hello", "world", "very long text very long text");

        actual = table.ToString();
        Assert.Equal(expected, actual);

        // same type (string) and null
        table = new Table("one", null, "3")
            .AddRow(1, 2, "three")
            .AddRow("hello", "world", "very long text very long text");

        actual = table.ToString();
        Assert.Equal(expected, actual);

        // mix of types (including TableColumn) and null
        table = new Table("one", null, new TableColumn("3"))
            .AddRow(1, 2, "three")
            .AddRow("hello", "world", "very long text very long text");

        // same type (TableColumn) and null
        table = new Table(new TableColumn("one", 0), null, new TableColumn("3"))
            .AddRow(1, 2, "three")
            .AddRow("hello", "world", "very long text very long text");
            
        actual = table.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CanWriteDefault()
    {
        var table = new Table("one", "two", "three");
        table.AddRow(1, 2, 3).AddRow("hello", "world", "very long text very long text");
        var actual = table.ToString();
        var expected =
        """
        -------------------------------------------------
        | one   | two   | three                         |
        -------------------------------------------------
        | 1     | 2     | 3                             |
        -------------------------------------------------
        | hello | world | very long text very long text |
        -------------------------------------------------
        """;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CanWriteCount()
    {
        var actual = Table.Configure(o => o.EnableCount = true)
            .AddColumns("one", "two", "three")
            .AddRow(1, 2, 3)
            .AddRow("hello", "world", "very long text very long text")
            .ToString();
        var expected =
        """
        -------------------------------------------------
        | one   | two   | three                         |
        -------------------------------------------------
        | 1     | 2     | 3                             |
        -------------------------------------------------
        | hello | world | very long text very long text |
        -------------------------------------------------

        Count: 2
        """;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CanWriteNumberAlignment()
    {
        var actual = Table.Configure(o => o.NumberAlignment = Alignment.Right)
            .AddColumns("one", "two", "three")
            .AddRow(1, 2, 3)
            .AddRow("hello", "world", "very long text very long text")
            .ToString();
        var expected =
        """
        -------------------------------------------------
        | one   | two   | three                         |
        -------------------------------------------------
        |     1 |     2 |                             3 |
        -------------------------------------------------
        | hello | world | very long text very long text |
        -------------------------------------------------
        """;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CanWriteAlternative()
    {
        var actual = new Table("one", "two", "three")
            .AddRow(1, 2, 3)
            .AddRow("hello", "world", "very long text very long text")
            .ToString(Format.Alternative);
        var expected =
        """
        +-------+-------+-------------------------------+
        | one   | two   | three                         |
        +-------+-------+-------------------------------+
        | 1     | 2     | 3                             |
        +-------+-------+-------------------------------+
        | hello | world | very long text very long text |
        +-------+-------+-------------------------------+
        """;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CanWriteSimple()
    {
        var actual = Table.Configure(o => o.EnableCount = false)
            .AddColumns("one", "two", "three")
            .AddRow(1, 2, 3)
            .AddRow("hello", "world", "very long text very long text")
            .ToString(Format.Simple);
        var expected =
        """
        +-------+-------+-------------------------------+
        | one   | two   | three                         |
        +-------+-------+-------------------------------+
        | 1     | 2     | 3                             |
        | hello | world | very long text very long text |
        +-------+-------+-------------------------------+
        """;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CanWriteMinimal()
    {
        var actual = Table.Configure(o => o.EnableCount = false)
            .AddColumns("one", "two", "three")
            .AddRow(1, 2, 3)
            .AddRow("hello", "world", "very long text very long text")
            .ToString(Format.Minimal);
        var expected =
        """
        one    two    three                        
        -------------------------------------------
        1      2      3                            
        hello  world  very long text very long text
        """;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CanWriteMarkdown()
    {
        var actual = Table.Configure(o => o.EnableCount = false)
            .AddColumns("one", "two", "three")
            .AddRow(1, 2, 3)
            .AddRow("hello", "world", "very long text very long text")
            .ToString(Format.Markdown);
        var expected =
        """
        | one   | two   | three                         |
        |-------|-------|-------------------------------|
        | 1     | 2     | 3                             |
        |-------|-------|-------------------------------|
        | hello | world | very long text very long text |
        """;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CanWriteCustomFormat()
    {
        var actual = Table.Configure(o => o.EnableCount = false)
            .AddColumns("one", "two", "three")
            .AddRow(1, 2, 3)
            .AddRow("hello", "world", "very long text very long text")
            .ToString(format =>
            {
                format
                    .ShowTopDivider('*', new('*'))
                    .ShowHeaderDivider('*', new('*'))
                    .HideValueDivider()
                    .ShowBottomDivider('-', new('-'))
                    .SetDelimiters(new('*', '|', '*'), new('|'));
            });
        var expected =
        """
        *************************************************
        * one   | two   | three                         *
        *************************************************
        | 1     | 2     | 3                             |
        | hello | world | very long text very long text |
        -------------------------------------------------
        """;
        Assert.Equal(expected, actual);
    }

    [Fact]
    void CanWriteCollection()
    {
        var actual = new []
        {
            new { first = "hello", second = 1, third = 0.00 },
            new { first = "hello world", second = 2, third = 2.34 },
            new { first = "hello pi", second = 3, third = Math.PI },
            new { first = "goodbye world", second = 4, third = 4.56 }
        }.ToConsoleTable(o => o.Columns[2].Format = "{0:0.00}").ToString();
        //}.ToConsoleTable().ToString();
        var expected =
        """
        ----------------------------------
        | first         | second | third |
        ----------------------------------
        | hello         |      1 |  0.00 |
        ----------------------------------
        | hello world   |      2 |  2.34 |
        ----------------------------------
        | hello pi      |      3 |  3.14 |
        ----------------------------------
        | goodbye world |      4 |  4.56 |
        ----------------------------------
        """;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CanWriteCalculatedColumn()
    {
        var t = Table.From(new[]
        {
            new { ID = 1, Text = "hello" },
            new { ID = 2, Text = "world" },
        });
        
        t.AddColumn("Calc", (x, i) => x.ID * 2);
        
        var actual = t.ToString();

        var expected =
        """
        ---------------------
        | ID | Text  | Calc |
        ---------------------
        |  1 | hello |    2 |
        ---------------------
        |  2 | world |    4 |
        ---------------------
        """;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CanAddEmptyColumn()
    {
        var tx = new Table("id", "text").AddRow(1, "hello").AddRow(2, "world");
        tx.Options.NumberAlignment = Alignment.Right;
	    tx.AddColumn(new TableColumn("test"));
	    var actual = tx.ToString();

        var expected =
        """
        ---------------------
        | id | text  | test |
        ---------------------
        |  1 | hello |      |
        ---------------------
        |  2 | world |      |
        ---------------------
        """;

        Assert.Equal(expected, actual);
    }
}