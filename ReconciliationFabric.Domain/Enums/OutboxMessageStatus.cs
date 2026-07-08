public enum OutboxMessageStatus
{
    Pending,       // created. eligible for immediate processing.
    Processing,    // currently being dispatched.
    Published,     // successfully delivered.   TERMINAL ✓
    Failed,        // dispatch failed. will retry after NextAttemptAt.
    DeadLettered   // max attempts exhausted. never retried. TERMINAL ✗
}
