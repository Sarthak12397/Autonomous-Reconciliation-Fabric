public record AuditHashVerificationResult(
    bool IsValid,
    int TotalChecked,
    int ViolationCount,
    Guid? FirstViolation,
    DateTime CheckedAt
);