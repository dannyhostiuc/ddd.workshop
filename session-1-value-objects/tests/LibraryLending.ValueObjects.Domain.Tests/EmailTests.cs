using FluentAssertions;
using LibraryLending.ValueObjects.Domain;

namespace LibraryLending.ValueObjects.Domain.Tests;

public class EmailTests
{
    [Theory(Skip = "Exercise: implement Email")]
    [InlineData("alice@example.com")]
    [InlineData("a.b+tag@sub.example.co.uk")]
    public void Parse_AcceptsValidAddress(string input)
    {
        var act = () => Email.Parse(input);
        act.Should().NotThrow();
    }

    [Theory(Skip = "Exercise: implement Email")]
    [InlineData("")]
    [InlineData("no-at-sign")]
    [InlineData("@no-local.com")]
    [InlineData("no-domain@")]
    public void Parse_RejectsInvalidAddress(string input)
    {
        var act = () => Email.Parse(input);
        act.Should().Throw<FormatException>();
    }

    [Fact(Skip = "Exercise: implement Email")]
    public void DomainPart_IsCaseInsensitive_ForEquality()
    {
        var a = Email.Parse("alice@Example.com");
        var b = Email.Parse("alice@EXAMPLE.COM");

        a.Should().Be(b);
    }

    [Fact(Skip = "Exercise: implement Email")]
    public void LocalPart_IsCaseSensitive_ForEquality()
    {
        var a = Email.Parse("Alice@example.com");
        var b = Email.Parse("alice@example.com");

        a.Should().NotBe(b);
    }
}
