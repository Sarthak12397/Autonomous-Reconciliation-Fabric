public sealed class ResolutionSucceededEvent : IDomainEvent
{
    public Guid DiscrepancyId { get; }
    public ResolutionStrategyType Strategy { get; }
    public string Reason { get; }
    public bool WillEscalate { get; }

    public ResolutionSucceededEvent(
        Guid discrepancyId,
        ResolutionStrategyType strategy,
        string reason,
        bool willEscalate)
    {
        DiscrepancyId = discrepancyId;
        Strategy = strategy;
        Reason = reason;
        WillEscalate = willEscalate;
    }
}