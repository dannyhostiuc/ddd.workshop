namespace LibraryLending.DomainEvents.Domain;

/// <summary>
/// Convenience base for aggregate roots. Holds a list of domain events that
/// the aggregate has raised in this transaction. The event dispatcher pulls
/// them off (via <see cref="DequeueEvents"/>) <strong>after</strong> the
/// aggregate has been persisted.
/// </summary>
public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _events = new();

    public IReadOnlyCollection<IDomainEvent> PendingEvents => _events;

    protected void Raise(IDomainEvent @event) => _events.Add(@event);

    public IReadOnlyCollection<IDomainEvent> DequeueEvents()
    {
        var copy = _events.ToArray();
        _events.Clear();
        return copy;
    }
}
