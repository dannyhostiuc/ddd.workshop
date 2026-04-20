namespace LibraryLending.ValueObjects.Domain;

/// <summary>
/// A bounded period during which a copy is on loan. Has a start date (inclusive)
/// and a due date (exclusive). The period must be at least one day and at most
/// the library's maximum loan length.
/// </summary>
/// <remarks>
/// Exercise: implement construction + validation, <see cref="Contains"/>,
/// <see cref="IsOverdueOn"/>, value equality, and a <see cref="Renew"/> method
/// that returns a new <see cref="LoanPeriod"/> extended by the given number of
/// days (without mutating the original).
/// </remarks>
public sealed class LoanPeriod : IEquatable<LoanPeriod>
{
    public static readonly int MaxLoanDays = 28;

    public DateOnly Start { get; }
    public DateOnly Due { get; }

    private LoanPeriod(DateOnly start, DateOnly due)
    {
        Start = start;
        Due = due;
    }

    public static LoanPeriod Create(DateOnly start, int days) =>
        throw new NotImplementedException("Exercise: implement LoanPeriod.Create");

    public bool Contains(DateOnly date) =>
        throw new NotImplementedException("Exercise: implement Contains");

    public bool IsOverdueOn(DateOnly date) =>
        throw new NotImplementedException("Exercise: implement IsOverdueOn");

    public LoanPeriod Renew(int additionalDays) =>
        throw new NotImplementedException("Exercise: implement Renew");

    public bool Equals(LoanPeriod? other) =>
        throw new NotImplementedException("Exercise: implement value equality");

    public override bool Equals(object? obj) => obj is LoanPeriod other && Equals(other);

    public override int GetHashCode() =>
        throw new NotImplementedException("Exercise: implement hash code");

    public override string ToString() => $"{Start:O} → {Due:O}";
}
