public interface IReconciliationRecordRepository
{
    Task<ReconciliationRecord?> GetByIdAsync(Guid id);

    Task<IEnumerable<ReconciliationRecord>> GetByJobIdAsync(Guid jobId);

    Task<IEnumerable<ReconciliationRecord>> GetDiscrepantByJobIdAsync(Guid jobId);

    Task<IEnumerable<ReconciliationRecord>> GetPendingSettlementAsync(Guid jobId);

    Task<ReconciliationRecord?> GetByCanonicalKeyAsync(string canonical);

    Task<int> CountUnresolvedByJobIdAsync(Guid jobId);

    Task AddRangeAsync(IEnumerable<ReconciliationRecord> records);

    Task UpdateAsync(ReconciliationRecord entity);
}