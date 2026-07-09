public record DiscrepancyDto(
    Guid Id,
    Guid RecordId,
    string CorrelationId,
    string CausationId,
    string Type,
    string Severity,
    string Status,
    string Strategy,
    bool AutoResolvable,
    int ResolutionAttempts,
    int MaxResolutionAttempts,
    string? ResolutionNotes,
    DateTime DetectedAt,
    DateTime? ResolvedAt
);