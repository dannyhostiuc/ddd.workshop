namespace LibraryLending.DomainServices.Domain;

/// <summary>
/// The decision returned by <see cref="LoanEligibilityService"/>. When
/// <see cref="IsEligible"/> is <c>false</c>, <see cref="Reasons"/> lists every
/// rule that was violated — never just the first one — so the librarian sees
/// the full picture in a single check.
/// </summary>
public sealed record EligibilityDecision(bool IsEligible, IReadOnlyCollection<string> Reasons)
{
    public static EligibilityDecision Eligible() => new(true, Array.Empty<string>());
    public static EligibilityDecision Ineligible(IReadOnlyCollection<string> reasons) => new(false, reasons);
}
