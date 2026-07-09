public interface IDiscrepancyRepository
{
    Task<Discrepancy?> GetByIdAsync(Guid id);

    Task<IEnumerable<Discrepancy>> GetPendingResolutionAsync();

    Task<IEnumerable<Discrepancy>> GetByRecordIdAsync(Guid recordId);

    Task<IEnumerable<Discrepancy>> GetStaleResolvingAsync(
        TimeSpan staleAfter);

    Task<int> CountResolvedByJobAsync(Guid jobId);

    Task<int> CountEscalatedByJobAsync(Guid jobId);

    Task UpdateAsync(Discrepancy entity);
}