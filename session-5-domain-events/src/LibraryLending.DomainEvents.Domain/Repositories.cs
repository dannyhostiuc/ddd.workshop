using System.Collections.Concurrent;

namespace LibraryLending.DomainEvents.Domain;

public interface ILoanRepository
{
    Task<Loan?> FindAsync(LoanId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Loan>> ListActiveAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(Loan loan, CancellationToken cancellationToken = default);
}

public sealed class InMemoryLoanRepository : ILoanRepository
{
    private readonly ConcurrentDictionary<LoanId, Loan> _byId = new();

    public Task<Loan?> FindAsync(LoanId id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_byId.TryGetValue(id, out var l) ? l : null);

    public Task<IReadOnlyCollection<Loan>> ListActiveAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<Loan> result = _byId.Values.Where(l => l.IsActive).ToList();
        return Task.FromResult(result);
    }

    public Task SaveAsync(Loan loan, CancellationToken cancellationToken = default)
    {
        _byId[loan.Id] = loan;
        return Task.CompletedTask;
    }
}

public interface IFineRepository
{
    Task<IReadOnlyCollection<Fine>> ListAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(Fine fine, CancellationToken cancellationToken = default);
}

public sealed class InMemoryFineRepository : IFineRepository
{
    private readonly ConcurrentDictionary<FineId, Fine> _byId = new();

    public Task<IReadOnlyCollection<Fine>> ListAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<Fine> result = _byId.Values.ToList();
        return Task.FromResult(result);
    }

    public Task SaveAsync(Fine fine, CancellationToken cancellationToken = default)
    {
        _byId[fine.Id] = fine;
        return Task.CompletedTask;
    }
}
