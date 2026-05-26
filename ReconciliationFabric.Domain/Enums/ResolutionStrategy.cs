public enum ResolutionStrategy
{
    AutoAdjustEntry,     // Create a correcting ledger entry
    CreateMissingRecord, // Create the missing internal record
    MarkAsReconciled,    // No action needed, mark as reconciled
    FlagForReview,       // Cannot safely resolve — escalate
    Retry                // Transient failure — retry resolution
}