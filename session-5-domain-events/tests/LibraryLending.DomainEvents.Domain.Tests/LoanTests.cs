using FluentAssertions;
using LibraryLending.DomainEvents.Domain;

namespace LibraryLending.DomainEvents.Domain.Tests;

public class LoanTests
{
    private static readonly DateOnly Today = new(2026, 4, 20);

    [Fact(Skip = "Exercise: implement Loan.Start")]
    public void Start_RaisesLoanStarted()
    {
        var loan = Loan.Start(MemberId.New(), CopyId.New(), Today, Today.AddDays(7));

        loan.PendingEvents.Should().ContainSingle()
            .Which.Should().BeOfType<LoanStarted>()
            .Which.LoanId.Should().Be(loan.Id);
    }

    [Fact(Skip = "Exercise: implement Loan.Return")]
    public void Return_RaisesLoanReturned()
    {
        var loan = Loan.Start(MemberId.New(), CopyId.New(), Today, Today.AddDays(7));
        loan.DequeueEvents();

        loan.Return(Today.AddDays(3));

        loan.PendingEvents.Should().ContainSingle()
            .Which.Should().BeOfType<LoanReturned>();
    }

    [Fact(Skip = "Exercise: implement Loan.CheckOverdue")]
    public void CheckOverdue_BeforeDueDate_RaisesNothing()
    {
        var loan = Loan.Start(MemberId.New(), CopyId.New(), Today, Today.AddDays(7));
        loan.DequeueEvents();

        loan.CheckOverdue(Today.AddDays(3));

        loan.PendingEvents.Should().BeEmpty();
    }

    [Fact(Skip = "Exercise: implement Loan.CheckOverdue")]
    public void CheckOverdue_AfterDueDate_RaisesLoanOverdue_Once()
    {
        var loan = Loan.Start(MemberId.New(), CopyId.New(), Today, Today.AddDays(7));
        loan.DequeueEvents();

        loan.CheckOverdue(Today.AddDays(8));
        loan.PendingEvents.Should().ContainSingle().Which.Should().BeOfType<LoanOverdue>();

        loan.DequeueEvents();
        loan.CheckOverdue(Today.AddDays(9));
        loan.PendingEvents.Should().BeEmpty();
    }

    [Fact(Skip = "Exercise: implement Loan.CheckOverdue")]
    public void CheckOverdue_OnReturnedLoan_DoesNothing()
    {
        var loan = Loan.Start(MemberId.New(), CopyId.New(), Today, Today.AddDays(7));
        loan.Return(Today.AddDays(3));
        loan.DequeueEvents();

        loan.CheckOverdue(Today.AddDays(8));

        loan.PendingEvents.Should().BeEmpty();
    }
}
