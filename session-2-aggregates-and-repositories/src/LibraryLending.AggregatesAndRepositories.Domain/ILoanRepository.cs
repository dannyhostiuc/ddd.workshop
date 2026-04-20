namespace LibraryLending.AggregatesAndRepositories.Domain;

/// <summary>
/// Repository for the <see cref="Loan"/> aggregate. The interface lives in the
/// Domain project so that the Domain layer never depends on infrastructure.
/// </summary>
/// <remarks>
/// Note: there is exactly <strong>one</strong> repository per aggregate root.
/// There is intentionally no <c>ICopyRepository</c> or <c>IMemberRepository</c>
/// here — those are not aggregate roots in this lesson's slice of the model.
/// </remarks>
public interface ILoanRepository
{
    Task<Loan?> FindAsync(LoanId id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Loan>> ListActiveForMemberAsync(MemberId memberId, CancellationToken cancellationToken = default);

    Task SaveAsync(Loan loan, CancellationToken cancellationToken = default);
}
