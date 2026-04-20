using FluentAssertions;
using LibraryLending.AggregatesAndRepositories.Domain;

namespace LibraryLending.AggregatesAndRepositories.Domain.Tests;

public class InMemoryLoanRepositoryTests
{
    private static readonly DateOnly Today = new(2026, 4, 20);

    [Fact(Skip = "Exercise: implement Loan + repository round-trip")]
    public async Task Save_ThenFind_ReturnsSameAggregate()
    {
        var repo = new InMemoryLoanRepository();
        var loan = Loan.Start(MemberId.New(), CopyId.New(), Today, Today.AddDays(14));

        await repo.SaveAsync(loan);
        var found = await repo.FindAsync(loan.Id);

        found.Should().NotBeNull();
        found!.Id.Should().Be(loan.Id);
    }

    [Fact(Skip = "Exercise: implement Loan + repository round-trip")]
    public async Task ListActiveForMember_ExcludesReturnedLoans()
    {
        var repo = new InMemoryLoanRepository();
        var member = MemberId.New();

        var active = Loan.Start(member, CopyId.New(), Today, Today.AddDays(14));
        var returned = Loan.Start(member, CopyId.New(), Today, Today.AddDays(14));
        returned.Return(Today.AddDays(2));

        await repo.SaveAsync(active);
        await repo.SaveAsync(returned);

        var result = await repo.ListActiveForMemberAsync(member);

        result.Should().ContainSingle().Which.Id.Should().Be(active.Id);
    }
}
