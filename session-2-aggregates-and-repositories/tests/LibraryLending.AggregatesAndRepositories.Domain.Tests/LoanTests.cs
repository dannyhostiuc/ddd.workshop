using FluentAssertions;
using LibraryLending.AggregatesAndRepositories.Domain;

namespace LibraryLending.AggregatesAndRepositories.Domain.Tests;

public class LoanTests
{
    private static readonly DateOnly Today = new(2026, 4, 20);

    [Fact(Skip = "Exercise: implement Loan.Start")]
    public void Start_RejectsDueDate_NotAfterStart()
    {
        var act = () => Loan.Start(MemberId.New(), CopyId.New(), Today, Today);
        act.Should().Throw<ArgumentException>();
    }

    [Fact(Skip = "Exercise: implement Loan.Start")]
    public void Start_NewLoan_IsActive()
    {
        var loan = Loan.Start(MemberId.New(), CopyId.New(), Today, Today.AddDays(14));

        loan.State.Should().Be(LoanState.Active);
        loan.RenewalCount.Should().Be(0);
        loan.ReturnedOn.Should().BeNull();
    }

    [Fact(Skip = "Exercise: implement Loan.Renew")]
    public void Renew_ExtendsDueDate_UpToMaxRenewals()
    {
        var loan = Loan.Start(MemberId.New(), CopyId.New(), Today, Today.AddDays(14));
        var originalDue = loan.DueOn;

        loan.Renew();
        loan.DueOn.Should().Be(originalDue.AddDays(Loan.RenewalDays));
        loan.RenewalCount.Should().Be(1);

        loan.Renew();
        loan.RenewalCount.Should().Be(Loan.MaxRenewals);

        var third = () => loan.Renew();
        third.Should().Throw<InvalidOperationException>();
    }

    [Fact(Skip = "Exercise: implement Loan.Return")]
    public void Return_TransitionsToReturned_AndRejectsFurtherOps()
    {
        var loan = Loan.Start(MemberId.New(), CopyId.New(), Today, Today.AddDays(14));

        loan.Return(Today.AddDays(3));

        loan.State.Should().Be(LoanState.Returned);
        loan.ReturnedOn.Should().Be(Today.AddDays(3));

        var renewAfter = () => loan.Renew();
        renewAfter.Should().Throw<InvalidOperationException>();

        var returnAgain = () => loan.Return(Today.AddDays(4));
        returnAgain.Should().Throw<InvalidOperationException>();
    }
}
