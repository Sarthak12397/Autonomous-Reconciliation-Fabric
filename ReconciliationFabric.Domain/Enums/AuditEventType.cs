public enum AuditEventType
{
    JobCreated, JobStarted, JobCompleted, JobFailed, JobCancelled,
    BatchIngested, DiscrepancyDetected,
    ResolutionStarted, ResolutionSucceeded, ResolutionFailed,
    EscalationCreated, EscalationAssigned, EscalationResolved,
    EscalationRejected, EscalationClosed, SlaBreached
}