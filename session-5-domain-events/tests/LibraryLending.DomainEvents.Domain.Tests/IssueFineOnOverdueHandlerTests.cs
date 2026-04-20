using FluentAssertions;
using LibraryLending.DomainEvents.Domain;

namespace LibraryLending.DomainEvents.Domain.Tests;

public class IssueFineOnOverdueHandlerTests
{
    private static readonly DateOnly Today = new(2026, 4, 20);

    [Fact(Skip = "Exercise: implement Loan + handler chain")]
    public async Task Handler_CreatesFine_FromOverdueEvent()
    {
        var fines = new InMemoryFineRepository();
        var handler = new IssueFineOnOverdueHandler(fines);

        var memberId = MemberId.New();
        var loanId = LoanId.New();
        var dueOn = Today.AddDays(-3);
        var @event = new LoanOverdue(loanId, memberId, CopyId.New(), DueOn: dueOn, OccurredOn: Today);

        await handler.HandleAsync(@event);

        var saved = await fines.ListAsync();
        saved.Should().ContainSingle();
        var fine = saved.Single();
        fine.MemberId.Should().Be(memberId);
        fine.LoanId.Should().Be(loanId);
        fine.AmountEur.Should().Be(3 * Fine.PerDayEur);
    }
}
