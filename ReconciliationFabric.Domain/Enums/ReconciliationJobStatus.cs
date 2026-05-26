public enum ReconciliationJobStatus
{
    Pending,           // Created, not yet started
    Running,           // Actively processing
    Completed,         // All records matched or resolved
    PartiallyResolved, // Some discrepancies remain (escalated)
    Failed,            // Job failed during execution
    Cancelled          // Manually cancelled before completion
}
