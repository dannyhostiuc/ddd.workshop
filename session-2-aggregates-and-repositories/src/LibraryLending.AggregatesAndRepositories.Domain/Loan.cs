namespace LibraryLending.AggregatesAndRepositories.Domain;

/// <summary>
/// The <c>Loan</c> aggregate root. Owns the lifecycle of a single member's
/// possession of a single copy: <see cref="Start"/> → optional
/// <see cref="Renew"/> → <see cref="Return"/>.
/// </summary>
/// <remarks>
/// Exercise: enforce the following invariants <em>inside</em> the aggregate
/// (not in the API handler):
/// <list type="number">
///   <item>A loan starts in the <c>Active</c> state with a due date in the
///         future.</item>
///   <item><see cref="Renew"/> is only allowed while <c>Active</c> and at most
///         <see cref="MaxRenewals"/> times. Each renewal extends the due date
///         by <see cref="RenewalDays"/>.</item>
///   <item><see cref="Return"/> is only allowed while <c>Active</c>. After
///         return the state is <c>Returned</c> and further operations are
///         rejected.</item>
/// </list>
/// </remarks>
public sealed class Loan
{
    public const int MaxRenewals = 2;
    public const int RenewalDays = 7;

    public LoanId Id { get; }
    public MemberId MemberId { get; }
    public CopyId CopyId { get; }
    public DateOnly StartedOn { get; }
    public DateOnly DueOn { get; private set; }
    public DateOnly? ReturnedOn { get; private set; }
    public int RenewalCount { get; private set; }
    public LoanState State { get; private set; }

    private Loan(LoanId id, MemberId memberId, CopyId copyId, DateOnly startedOn, DateOnly dueOn)
    {
        Id = id;
        MemberId = memberId;
        CopyId = copyId;
        StartedOn = startedOn;
        DueOn = dueOn;
        State = LoanState.Active;
    }

    /// <summary>Starts a new loan. Throws if <paramref name="dueOn"/> is not after <paramref name="startedOn"/>.</summary>
    public static Loan Start(MemberId memberId, CopyId copyId, DateOnly startedOn, DateOnly dueOn) =>
        throw new NotImplementedException("Exercise: implement Loan.Start");

    /// <summary>Extends the due date by <see cref="RenewalDays"/>. Throws if not allowed.</summary>
    public void Renew() =>
        throw new NotImplementedException("Exercise: implement Loan.Renew");

    /// <summary>Marks the copy as returned on the given date. Throws if already returned.</summary>
    public void Return(DateOnly on) =>
        throw new NotImplementedException("Exercise: implement Loan.Return");
}

public enum LoanState
{
    Active = 1,
    Returned = 2,
}
