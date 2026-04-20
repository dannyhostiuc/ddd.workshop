namespace LibraryLending.DomainEvents.Domain;

/// <summary>
/// Reacts to <see cref="LoanOverdue"/> by issuing a <see cref="Fine"/>. The
/// fine is a <strong>separate aggregate</strong> from the loan — that's the
/// whole point of using an event here, rather than mutating the loan.
/// </summary>
public sealed class IssueFineOnOverdueHandler : IDomainEventHandler<LoanOverdue>
{
    private readonly IFineRepository _fines;

    public IssueFineOnOverdueHandler(IFineRepository fines) => _fines = fines;

    public Task HandleAsync(LoanOverdue @event, CancellationToken cancellationToken = default)
    {
        var fine = Fine.ForOverdue(@event.MemberId, @event.LoanId, @event.DueOn, @event.OccurredOn);
        return _fines.SaveAsync(fine, cancellationToken);
    }
}
