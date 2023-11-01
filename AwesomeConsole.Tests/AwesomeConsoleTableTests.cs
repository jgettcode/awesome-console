using System.Runtime.Serialization;

namespace AwesomeConsole.Tables.Tests;

public class AwesomeConsoleTableTests
{
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
            .AddColumn("one", "two", "three")
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
            .AddColumn("one", "two", "three")
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
            .AddColumn("one", "two", "three")
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
            .AddColumn("one", "two", "three")
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
            .AddColumn("one", "two", "three")
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
            .AddColumn("one", "two", "three")
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
        }.ToConsoleTable(o => o.Columns[2].Update(formatter: x => x.As<double>().ToString("0.00"))).ToString();
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
}