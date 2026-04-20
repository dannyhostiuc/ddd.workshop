namespace LibraryLending.DomainEvents.Domain;

/// <summary>
/// The <c>Loan</c> aggregate root. Like Session 2, but now it <em>raises
/// domain events</em> when state changes instead of just mutating silently.
/// </summary>
/// <remarks>
/// Exercise: in each operation, raise the appropriate domain event:
/// <list type="bullet">
///   <item><see cref="Start"/> raises a <see cref="LoanStarted"/>.</item>
///   <item><see cref="Return"/> raises a <see cref="LoanReturned"/>.</item>
///   <item><see cref="CheckOverdue"/> raises a <see cref="LoanOverdue"/>
///         <strong>once</strong>, the first time the loan is checked on or
///         after its due date while still active. Subsequent calls do not
///         re-raise.</item>
/// </list>
/// Use <see cref="AggregateRoot.Raise"/> from the base class.
/// </remarks>
public sealed class Loan : AggregateRoot
{
    public LoanId Id { get; }
    public MemberId MemberId { get; }
    public CopyId CopyId { get; }
    public DateOnly StartedOn { get; }
    public DateOnly DueOn { get; }
    public DateOnly? ReturnedOn { get; private set; }
    public bool IsActive => ReturnedOn is null;
    public bool OverdueRaised { get; private set; }

    private Loan(LoanId id, MemberId memberId, CopyId copyId, DateOnly startedOn, DateOnly dueOn)
    {
        Id = id;
        MemberId = memberId;
        CopyId = copyId;
        StartedOn = startedOn;
        DueOn = dueOn;
    }

    public static Loan Start(MemberId memberId, CopyId copyId, DateOnly startedOn, DateOnly dueOn) =>
        throw new NotImplementedException("Exercise: implement Loan.Start (and raise LoanStarted)");

    public void Return(DateOnly on) =>
        throw new NotImplementedException("Exercise: implement Loan.Return (and raise LoanReturned)");

    /// <summary>
    /// Called by the clock advance to give the loan a chance to notice it has
    /// gone overdue. Idempotent: raises <see cref="LoanOverdue"/> at most once.
    /// </summary>
    public void CheckOverdue(DateOnly today) =>
        throw new NotImplementedException("Exercise: implement Loan.CheckOverdue (raise LoanOverdue at most once)");
}
