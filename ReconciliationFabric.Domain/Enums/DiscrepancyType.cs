public enum DiscrepancyType
{
    AmountMismatch,       // Same reference, different amount
    StatusConflict,       // Same reference, different status
    MissingInInternal,    // External has record, internal does not
    MissingInExternal,    // Internal has record, external does not
    DuplicateDetected,    // Same reference appears more than once
    TimestampAnomaly,     // Record exists but timing is outside expected window
    CurrencyMismatch      // Same reference, different currency
}