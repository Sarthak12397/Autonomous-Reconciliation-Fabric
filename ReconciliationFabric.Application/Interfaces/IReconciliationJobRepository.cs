using Reconciliation.Domain;

public interface IReconciliationJobRepository
{
    Task<ReconciliationJob?> GetByIdAsync(Guid id);

    Task<ReconciliationJob?> GetByIdempotencyKeyAsync(string key);

    Task<IEnumerable<ReconciliationJob>> GetByStatusAsync(
        ReconciliationJobStatus status);

    Task<PagedResult<ReconciliationJob>> GetPagedAsync(
        ReconciliationJobStatus? status,
        Guid? afterCursor,
        int size);

    Task AddAsync(ReconciliationJob entity);

    Task UpdateAsync(ReconciliationJob entity);
}