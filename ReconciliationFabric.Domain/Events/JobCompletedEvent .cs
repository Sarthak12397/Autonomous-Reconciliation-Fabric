public sealed class JobCompletedEvent : IDomainEvent
{
    public Guid JobId { get; }
    public ReconciliationJobStatus Status { get; }
    public int TotalRecordsProcessed { get; }
    public int DiscrepanciesFound { get; }

    public JobCompletedEvent(
        Guid jobId,
        ReconciliationJobStatus status,
        int totalRecordsProcessed,
        int discrepanciesFound)
    {
        if (status != ReconciliationJobStatus.Completed &&
            status != ReconciliationJobStatus.PartiallyResolved)
        {
            throw new ArgumentException(
                "JobCompletedEvent can only be raised for Completed or PartiallyResolved jobs.",
                nameof(status));
        }

        JobId = jobId;
        Status = status;
        TotalRecordsProcessed = totalRecordsProcessed;
        DiscrepanciesFound = discrepanciesFound;
    }
}