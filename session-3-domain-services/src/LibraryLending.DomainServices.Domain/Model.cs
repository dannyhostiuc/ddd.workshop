namespace LibraryLending.DomainServices.Domain;

public readonly record struct MemberId(Guid Value)
{
    public static MemberId New() => new(Guid.NewGuid());
}

public readonly record struct CopyId(Guid Value)
{
    public static CopyId New() => new(Guid.NewGuid());
}

/// <summary>
/// Snapshot of a member's currently relevant lending state. Aggregates are not
/// modeled in this lesson — Session 2 already covered them — we just pass the
/// data the eligibility decision needs.
/// </summary>
public sealed record MemberLendingSnapshot(
    MemberId MemberId,
    int ActiveLoanCount,
    decimal OutstandingFinesEur,
    bool IsSuspended);

/// <summary>A member's reservation on a copy.</summary>
public sealed record Reservation(MemberId MemberId, CopyId CopyId, DateOnly ReservedOn, DateOnly ExpiresOn);

/// <summary>State of a single copy, as far as the eligibility check needs to know.</summary>
public sealed record CopyAvailability(CopyId CopyId, bool IsOnLoan);
