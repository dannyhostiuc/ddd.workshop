namespace LibraryLending.DomainEvents.Domain;

public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}

/// <summary>
/// Synchronous in-process dispatcher. Note the dispatch order: callers
/// <strong>save the aggregate first</strong>, then call
/// <see cref="DispatchAsync"/> with whatever events the aggregate raised. If
/// persistence throws, no events are emitted.
/// </summary>
public sealed class DomainEventDispatcher
{
    private readonly Func<Type, IEnumerable<object>> _resolveHandlers;

    /// <param name="resolveHandlers">
    /// Given a closed <see cref="IDomainEventHandler{TEvent}"/> type, returns
    /// every registered handler for it. The composition root wires this up to
    /// its DI container; the domain layer stays container-agnostic.
    /// </param>
    public DomainEventDispatcher(Func<Type, IEnumerable<object>> resolveHandlers) =>
        _resolveHandlers = resolveHandlers;

    public async Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default)
    {
        foreach (var @event in events)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(@event.GetType());
            foreach (var handler in _resolveHandlers(handlerType))
            {
                var method = handlerType.GetMethod(nameof(IDomainEventHandler<IDomainEvent>.HandleAsync))!;
                var task = (Task)method.Invoke(handler, new object[] { @event, cancellationToken })!;
                await task.ConfigureAwait(false);
            }
        }
    }
}
