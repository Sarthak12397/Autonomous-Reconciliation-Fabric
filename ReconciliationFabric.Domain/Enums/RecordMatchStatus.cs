public enum RecordMatchStatus
{
    Unprocessed,  // Not yet matched
    Matched,      // Clean match, no discrepancy
    Discrepant,   // Matched but values differ
    UnmatchedInternal,  // Found in internal, not in external
    UnmatchedExternal,  // Found in external, not in internal
    Resolved,     // Was discrepant, now resolved
    Escalated     // Sent to escalation, awaiting human action
}
