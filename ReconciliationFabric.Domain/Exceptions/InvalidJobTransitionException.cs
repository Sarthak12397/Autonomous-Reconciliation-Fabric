public sealed class InvalidJobTransitionException: Exception
{
    public Guid JobId{get;}
    public ReconciliationJobStatus From{get;}
    public ReconciliationJobStatus To{get;}

    public InvalidJobTransitionException(
        Guid jobId,
        ReconciliationJobStatus from,ReconciliationJobStatus to

    ):base($"Job '{jobId}' cannot transition from {from} to {to}")
    {
        JobId = jobId;
        From = from;
        To = to;
    }
}