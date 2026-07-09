public interface IDiscrepancyDetectionEngine
{
    Task<DetectionResult> DetectAsync(
        Guid jobId,
        Guid correlationId,
        IAsyncEnumerable<ExternalRecord> externalRecords,
        IAsyncEnumerable<InternalRecord> internalRecords);
}