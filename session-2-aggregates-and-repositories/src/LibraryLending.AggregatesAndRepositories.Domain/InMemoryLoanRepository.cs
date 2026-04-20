using System.Collections.Concurrent;

namespace LibraryLending.AggregatesAndRepositories.Domain;

/// <summary>
/// In-memory <see cref="ILoanRepository"/> for use in tests and the workshop
/// API host. Not thread-safe across save-then-mutate sequences (mutating an
/// aggregate already inside the dictionary will be visible to other readers);
/// that's a real DDD concern, but out of scope for this lesson.
/// </summary>
public sealed class InMemoryLoanRepository : ILoanRepository
{
    private readonly ConcurrentDictionary<LoanId, Loan> _byId = new();

    public Task<Loan?> FindAsync(LoanId id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_byId.TryGetValue(id, out var loan) ? loan : null);

    public Task<IReadOnlyCollection<Loan>> ListActiveForMemberAsync(MemberId memberId, CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<Loan> result = _byId.Values
            .Where(l => l.MemberId.Equals(memberId) && l.State == LoanState.Active)
            .ToList();
        return Task.FromResult(result);
    }

    public Task SaveAsync(Loan loan, CancellationToken cancellationToken = default)
    {
        _byId[loan.Id] = loan;
        return Task.CompletedTask;
    }
}
