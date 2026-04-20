namespace LibraryLending.ValueObjects.Domain;

/// <summary>
/// International Standard Book Number — a value object identifying a bibliographic
/// title. Two ISBNs are equal when their normalized 13-digit form is equal.
/// </summary>
/// <remarks>
/// Exercise: implement parsing, normalization (strip dashes/spaces, ISBN-10 → 13),
/// validation (length + checksum), and value equality.
/// </remarks>
public sealed class Isbn : IEquatable<Isbn>
{
    public string Value { get; }

    private Isbn(string value) => Value = value;

    /// <summary>Parses the input or throws <see cref="FormatException"/>.</summary>
    public static Isbn Parse(string input) =>
        throw new NotImplementedException("Exercise: implement Isbn.Parse");

    /// <summary>Tries to parse the input. Returns <c>false</c> on invalid input.</summary>
    public static bool TryParse(string? input, out Isbn? isbn) =>
        throw new NotImplementedException("Exercise: implement Isbn.TryParse");

    public bool Equals(Isbn? other) =>
        throw new NotImplementedException("Exercise: implement value equality");

    public override bool Equals(object? obj) => obj is Isbn other && Equals(other);

    public override int GetHashCode() =>
        throw new NotImplementedException("Exercise: implement hash code");

    public override string ToString() => Value;
}
