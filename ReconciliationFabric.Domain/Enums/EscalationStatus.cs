public enum EscalationStatus
{
    Open,         // created. not picked up.
    Assigned,     // assigned. not started.
    InProgress,   // operator investigating.
    Resolved,     // operator fixed it.
    Closed,       // resolved + signed off.  TERMINAL ✓
    Rejected      // not real. dismissed.    TERMINAL.
}