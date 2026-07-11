using System;

public sealed class ResolutionAction
{
    private ResolutionAction()
    {
    }

    public Guid Id { get; private set; }

    public Guid DiscrepancyId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public ResolutionStrategyType Strategy { get; private set; }

    public ResolutionActionResult Result { get; private set; }

    public string ExecutionIdempotencyKey { get; private set; } = null!;

    public string Metadata { get; private set; } = null!;

    public DateTime ExecutedAt { get; private set; }

    internal static ResolutionAction Create(
        Guid discrepancyId,
        Guid correlationId,
        ResolutionStrategyType strategy,
        ResolutionActionResult result,
        string executionIdempotencyKey,
        string metadata)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(executionIdempotencyKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(metadata);

        return new ResolutionAction
        {
            Id = Guid.NewGuid(),
            DiscrepancyId = discrepancyId,
            CorrelationId = correlationId,
            CausationId = discrepancyId,
            Strategy = strategy,
            Result = result,
            ExecutionIdempotencyKey = executionIdempotencyKey,
            Metadata = metadata,
            ExecutedAt = DateTime.UtcNow
        };
    }
}