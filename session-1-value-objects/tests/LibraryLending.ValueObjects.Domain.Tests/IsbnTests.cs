using FluentAssertions;
using LibraryLending.ValueObjects.Domain;

namespace LibraryLending.ValueObjects.Domain.Tests;

public class IsbnTests
{
    [Theory(Skip = "Exercise: implement Isbn")]
    [InlineData("978-3-16-148410-0")]
    [InlineData("9783161484100")]
    [InlineData("978 3 16 148410 0")]
    public void Parse_AcceptsValidIsbn13(string input)
    {
        var act = () => Isbn.Parse(input);
        act.Should().NotThrow();
    }

    [Theory(Skip = "Exercise: implement Isbn")]
    [InlineData("")]
    [InlineData("not-an-isbn")]
    [InlineData("978-3-16-148410-1")] // bad checksum
    public void Parse_RejectsInvalidIsbn(string input)
    {
        var act = () => Isbn.Parse(input);
        act.Should().Throw<FormatException>();
    }

    [Fact(Skip = "Exercise: implement Isbn")]
    public void EqualIsbns_CompareEqual_RegardlessOfFormatting()
    {
        var a = Isbn.Parse("978-3-16-148410-0");
        var b = Isbn.Parse("9783161484100");

        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }
}
