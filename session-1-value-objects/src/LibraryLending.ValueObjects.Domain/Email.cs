namespace LibraryLending.ValueObjects.Domain;

/// <summary>
/// An e-mail address for a library member. Equality is case-insensitive on the
/// domain part and case-sensitive on the local part (per RFC 5321).
/// </summary>
/// <remarks>
/// Exercise: implement parsing, normalization, validation, and value equality.
/// </remarks>
public sealed class Email : IEquatable<Email>
{
    public string Value { get; }

    private Email(string value) => Value = value;

    public static Email Parse(string input) =>
        throw new NotImplementedException("Exercise: implement Email.Parse");

    public static bool TryParse(string? input, out Email? email) =>
        throw new NotImplementedException("Exercise: implement Email.TryParse");

    public bool Equals(Email? other) =>
        throw new NotImplementedException("Exercise: implement value equality");

    public override bool Equals(object? obj) => obj is Email other && Equals(other);

    public override int GetHashCode() =>
        throw new NotImplementedException("Exercise: implement hash code");

    public override string ToString() => Value;
}
