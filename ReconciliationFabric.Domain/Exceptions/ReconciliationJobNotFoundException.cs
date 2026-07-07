public sealed class ReconciliationJobNotFoundException: Exception
{
    public Guid JobId{get;}
    public ReconciliationJobNotFoundException(
        Guid jobId
    ):base($"Reconciliation job '{jobId}' was not found.")
    {
        JobId = jobId;
    }
}