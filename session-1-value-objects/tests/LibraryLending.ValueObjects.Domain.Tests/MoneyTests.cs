using FluentAssertions;
using LibraryLending.ValueObjects.Domain;

namespace LibraryLending.ValueObjects.Domain.Tests;

public class MoneyTests
{
    [Fact(Skip = "Exercise: implement Money")]
    public void Construction_RejectsNegativeAmounts()
    {
        var act = () => new Money(-1m, "EUR");
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact(Skip = "Exercise: implement Money")]
    public void Construction_RejectsUnknownCurrencyCode()
    {
        var act = () => new Money(1m, "ZZZ");
        act.Should().Throw<ArgumentException>();
    }

    [Fact(Skip = "Exercise: implement Money")]
    public void Addition_OfSameCurrency_Sums()
    {
        var sum = new Money(3m, "EUR") + new Money(2m, "EUR");
        sum.Should().Be(new Money(5m, "EUR"));
    }

    [Fact(Skip = "Exercise: implement Money")]
    public void Addition_OfDifferentCurrencies_Throws()
    {
        var act = () => new Money(1m, "EUR") + new Money(1m, "USD");
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact(Skip = "Exercise: implement Money")]
    public void Equality_IsByAmountAndCurrency()
    {
        new Money(1m, "EUR").Should().Be(new Money(1m, "EUR"));
        new Money(1m, "EUR").Should().NotBe(new Money(1m, "USD"));
    }
}
