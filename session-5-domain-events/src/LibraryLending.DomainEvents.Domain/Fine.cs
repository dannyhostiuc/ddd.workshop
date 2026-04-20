namespace LibraryLending.DomainEvents.Domain;

/// <summary>
/// A fine accrued by a member because of an overdue loan. Created by the
/// <c>LoanOverdue</c> handler — never directly by an API endpoint.
/// </summary>
public sealed class Fine
{
    public const decimal PerDayEur = 0.50m;

    public FineId Id { get; }
    public MemberId MemberId { get; }
    public LoanId LoanId { get; }
    public DateOnly IssuedOn { get; }
    public decimal AmountEur { get; }

    private Fine(FineId id, MemberId memberId, LoanId loanId, DateOnly issuedOn, decimal amountEur)
    {
        Id = id;
        MemberId = memberId;
        LoanId = loanId;
        IssuedOn = issuedOn;
        AmountEur = amountEur;
    }

    public static Fine ForOverdue(MemberId memberId, LoanId loanId, DateOnly dueOn, DateOnly today)
    {
        var daysLate = today.DayNumber - dueOn.DayNumber;
        if (daysLate <= 0) throw new ArgumentException("Loan is not overdue.", nameof(today));
        return new Fine(FineId.New(), memberId, loanId, today, daysLate * PerDayEur);
    }
}
