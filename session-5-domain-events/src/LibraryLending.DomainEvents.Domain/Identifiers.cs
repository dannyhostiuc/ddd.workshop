namespace LibraryLending.DomainEvents.Domain;

public readonly record struct MemberId(Guid Value)
{
    public static MemberId New() => new(Guid.NewGuid());
}

public readonly record struct CopyId(Guid Value)
{
    public static CopyId New() => new(Guid.NewGuid());
}

public readonly record struct LoanId(Guid Value)
{
    public static LoanId New() => new(Guid.NewGuid());
}

public readonly record struct FineId(Guid Value)
{
    public static FineId New() => new(Guid.NewGuid());
}
