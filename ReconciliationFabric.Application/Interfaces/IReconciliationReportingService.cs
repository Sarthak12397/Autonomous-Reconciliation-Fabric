public interface IReconciliationReportingService
{
    Task<ReconciliationReport> GenerateAsync(
        DateTime from,
        DateTime to);
}