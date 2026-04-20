namespace LibraryLending.DomainEvents.Domain;

/// <summary>Marker interface for everything that is a domain event.</summary>
public interface IDomainEvent
{
    DateOnly OccurredOn { get; }
}

public sealed record LoanStarted(LoanId LoanId, MemberId MemberId, CopyId CopyId, DateOnly OccurredOn) : IDomainEvent;

public sealed record LoanReturned(LoanId LoanId, MemberId MemberId, CopyId CopyId, DateOnly OccurredOn) : IDomainEvent;

/// <summary>Raised once when a loan first crosses its due date without being returned.</summary>
public sealed record LoanOverdue(LoanId LoanId, MemberId MemberId, CopyId CopyId, DateOnly DueOn, DateOnly OccurredOn) : IDomainEvent;
