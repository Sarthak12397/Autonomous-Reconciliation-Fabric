public interface IAuditHashVerifier
{
    Task<AuditHashVerificationResult> VerifyChainAsync(
        DateTime from,
        DateTime to);

    Task<bool> VerifyEntryAsync(
        Guid entryId);
}