using FluentAssertions;
using LibraryLending.ValueObjects.Domain;

namespace LibraryLending.ValueObjects.Domain.Tests;

public class LoanPeriodTests
{
    private static readonly DateOnly Today = new(2026, 4, 20);

    [Fact(Skip = "Exercise: implement LoanPeriod")]
    public void Create_RejectsZeroOrNegativeDays()
    {
        var act = () => LoanPeriod.Create(Today, 0);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact(Skip = "Exercise: implement LoanPeriod")]
    public void Create_RejectsMoreThanMaxDays()
    {
        var act = () => LoanPeriod.Create(Today, LoanPeriod.MaxLoanDays + 1);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact(Skip = "Exercise: implement LoanPeriod")]
    public void Contains_IsInclusiveOfStart_ExclusiveOfDue()
    {
        var period = LoanPeriod.Create(Today, 7);

        period.Contains(period.Start).Should().BeTrue();
        period.Contains(period.Due).Should().BeFalse();
    }

    [Fact(Skip = "Exercise: implement LoanPeriod")]
    public void IsOverdue_TrueOnAndAfterDueDate()
    {
        var period = LoanPeriod.Create(Today, 7);

        period.IsOverdueOn(period.Due).Should().BeTrue();
        period.IsOverdueOn(period.Due.AddDays(1)).Should().BeTrue();
        period.IsOverdueOn(period.Due.AddDays(-1)).Should().BeFalse();
    }

    [Fact(Skip = "Exercise: implement LoanPeriod")]
    public void Renew_ReturnsNewInstance_AndDoesNotMutateOriginal()
    {
        var original = LoanPeriod.Create(Today, 7);
        var renewed = original.Renew(7);

        renewed.Should().NotBeSameAs(original);
        renewed.Due.Should().Be(original.Due.AddDays(7));
        original.Due.Should().Be(Today.AddDays(7));
    }
}
