public interface IDomainEventDispatcher
{
    Task DispatchAsync<T>(
        T domainEvent,
        Guid correlationId,
        Guid causationId)
        where T : IDomainEvent;
}