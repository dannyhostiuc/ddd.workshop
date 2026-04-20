namespace LibraryLending.DomainServices.Domain;

/// <summary>
/// Decides whether a member is allowed to borrow a specific copy. This is a
/// <em>domain service</em> because the rule spans multiple aggregates
/// (<c>Member</c>, <c>Loan</c>, <c>Reservation</c>, <c>Copy</c>) and doesn't
/// belong to any one of them.
/// </summary>
/// <remarks>
/// Key properties of a domain service:
/// <list type="bullet">
///   <item><strong>Stateless.</strong> Everything it needs is passed as
///         arguments.</item>
///   <item><strong>Pure.</strong> No persistence, no I/O, no clocks. The
///         caller passes <c>today</c> in.</item>
///   <item><strong>Single decision.</strong> One method, one return value.</item>
/// </list>
/// </remarks>
public sealed class LoanEligibilityService
{
    public const int MaxConcurrentLoans = 5;
    public const decimal MaxOutstandingFinesEur = 10m;

    /// <summary>
    /// Decides eligibility. Returns <see cref="EligibilityDecision.Eligible"/>
    /// or an ineligible decision listing <em>all</em> reasons.
    /// </summary>
    /// <remarks>
    /// Exercise: implement the rules below, accumulating <em>every</em>
    /// failing reason into the decision (don't short-circuit on the first
    /// failure).
    /// <para>
    /// Rules:
    /// </para>
    /// <list type="number">
    ///   <item>The member is not suspended.</item>
    ///   <item>The member has fewer than <see cref="MaxConcurrentLoans"/> active loans.</item>
    ///   <item>The member's outstanding fines do not exceed
    ///         <see cref="MaxOutstandingFinesEur"/>.</item>
    ///   <item>The copy is not currently on loan.</item>
    ///   <item>If the copy is reserved by someone other than the requesting
    ///         member and the reservation has not expired
    ///         (<c>reservation.ExpiresOn &gt; today</c>), the request is
    ///         rejected.</item>
    /// </list>
    /// </remarks>
    public EligibilityDecision Decide(
        MemberLendingSnapshot member,
        CopyAvailability copy,
        IReadOnlyCollection<Reservation> reservationsOnCopy,
        DateOnly today) =>
        throw new NotImplementedException("Exercise: implement LoanEligibilityService.Decide");
}
