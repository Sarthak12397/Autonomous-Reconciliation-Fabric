public enum RecordMatchStatus
{
    Unprocessed,         // not yet processed.
    Matched,             // both sides agree.                         TERMINAL ✓
    PendingSettlement,   // near window boundary. may self-heal.
                         // NOT escalated. re-evaluated next run.
    Discrepant,          // found on both sides. values differ.
    UnmatchedInternal,   // external has it. internal does not.
    UnmatchedExternal,   // internal has it. external does not.
    Resolved,            // fixed.                                    TERMINAL ✓
    Escalated            // human queue.                              TERMINAL.
}