namespace LibraryLending.AggregatesAndRepositories.Domain;

/// <summary>
/// Identifier for a member. A wrapper around <see cref="Guid"/> to avoid
/// passing a bare Guid where a member is expected.
/// </summary>
public readonly record struct MemberId(Guid Value)
{
    public static MemberId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}

/// <summary>Identifier for a copy of a book.</summary>
public readonly record struct CopyId(Guid Value)
{
    public static CopyId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}

/// <summary>Identifier for a loan aggregate.</summary>
public readonly record struct LoanId(Guid Value)
{
    public static LoanId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
