public enum DiscrepancyStatus
{
    Detected,    // Freshly detected, awaiting resolution attempt
    Resolving,   // Resolution job is currently executing
    Resolved,    // Successfully resolved autonomously
    Escalated,   // Could not resolve; escalation ticket created
    Failed,      // Resolution attempted but failed (will retry)
    Ignored      // Deliberately ignored (e.g., test data, known exception)
}
