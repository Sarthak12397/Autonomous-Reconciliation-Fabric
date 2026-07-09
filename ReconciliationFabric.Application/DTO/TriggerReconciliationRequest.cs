public record TriggerReconciliationRequest(
    SourceSystem SourceSystem,
    SourceSystem TargetSystem,
    DateTime WindowStart,
    DateTime WindowEnd,
    string IdempotencyKey
);