public interface IOutboxRepository
{
    Task AddAsync(OutboxMessage message);

    Task<IEnumerable<OutboxMessage>> GetPendingForProcessingAsync(
        int batchSize);

    Task<IEnumerable<OutboxMessage>> GetDeadLetteredAsync();

    Task UpdateAsync(OutboxMessage entity);
}