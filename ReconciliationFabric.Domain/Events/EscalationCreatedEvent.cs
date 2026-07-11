public sealed class EscalationCreatedEvent 
{
    public Guid EscalationId { get; }
    public Guid DiscrepancyId { get; }
    public DiscrepancySeverity Severity { get; }
    public DateTime SlaDeadline { get; }

    public EscalationCreatedEvent(
        Guid escalationId,
        Guid discrepancyId,
        DiscrepancySeverity severity,
        DateTime slaDeadline)
    {
        EscalationId = escalationId;
        DiscrepancyId = discrepancyId;
        Severity = severity;
        SlaDeadline = slaDeadline;
    }
}