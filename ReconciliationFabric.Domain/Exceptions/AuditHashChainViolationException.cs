public sealed class AuditHashChainViolationException : Exception
{
    public Guid ViolatingEntryId { get; }
    public string ExpectedHash { get; }
    public string ActualHash { get; }

    public AuditHashChainViolationException(
        Guid violatingEntryId,
        string expectedHash,
        string actualHash)
        : base(
            $"Audit hash chain violation at entry '{violatingEntryId}'.{Environment.NewLine}" +
            $"Expected {expectedHash}. Got {actualHash}.{Environment.NewLine}" +
            "Historical audit data has been tampered with.")
    {
        ViolatingEntryId = violatingEntryId;
        ExpectedHash = expectedHash;
        ActualHash = actualHash;
    }
}