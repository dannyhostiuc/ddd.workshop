namespace LibraryLending.ValueObjects.Domain;

/// <summary>
/// An amount of money in a specific currency. Two <see cref="Money"/> values are
/// equal when both amount and currency are equal. Operations between different
/// currencies are not allowed.
/// </summary>
/// <remarks>
/// Exercise: implement construction (reject negatives, normalize precision per
/// currency), value equality, and the <c>+</c> / <c>-</c> operators that throw
/// when currencies differ.
/// </remarks>
public readonly struct Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency) =>
        throw new NotImplementedException("Exercise: implement Money construction + validation");

    public static Money operator +(Money left, Money right) =>
        throw new NotImplementedException("Exercise: implement Money +");

    public static Money operator -(Money left, Money right) =>
        throw new NotImplementedException("Exercise: implement Money -");

    public bool Equals(Money other) =>
        throw new NotImplementedException("Exercise: implement value equality");

    public override bool Equals(object? obj) => obj is Money other && Equals(other);

    public override int GetHashCode() =>
        throw new NotImplementedException("Exercise: implement hash code");

    public override string ToString() => $"{Amount} {Currency}";
}
