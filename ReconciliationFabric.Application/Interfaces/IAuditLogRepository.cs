public interface IAuditLogRepository
{
    Task AppendAsync(ReconciliationAuditLog log);

    Task<IEnumerable<ReconciliationAuditLog>> GetByJobIdAsync(
        Guid jobId);

    Task<IEnumerable<ReconciliationAuditLog>> GetByDiscrepancyIdAsync(
        Guid discrepancyId);

    Task<IEnumerable<ReconciliationAuditLog>> GetByCorrelationIdAsync(
        Guid correlationId);

    Task<IEnumerable<ReconciliationAuditLog>> GetByDateRangeAsync(
        DateTime from,
        DateTime to);

    Task<string?> GetLastEntryHashAsync();
}