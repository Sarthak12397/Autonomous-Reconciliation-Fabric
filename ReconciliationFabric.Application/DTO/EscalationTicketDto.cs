public record EscalationTicketDto(
    Guid Id,
    Guid DiscrepancyId,
    string CorrelationId,
    string CausationId,
    string Severity,
    string Status,
    string Reason,
    string? AssignedTo,
    DateTime? AssignedAt,
    string? ResolvedBy,
    string? ResolutionNotes,
    DateTime? ResolvedAt,
    DateTime CreatedAt,
    DateTime SlaDeadline,
    bool IsBreached
);