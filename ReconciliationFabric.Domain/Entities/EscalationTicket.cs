using System;

public sealed class EscalationTicket
{
    private EscalationTicket()
    {
    }

    public Guid Id { get; private set; }

    public Guid DiscrepancyId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public DiscrepancySeverity Severity { get; private set; }

    public string Reason { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; }

    public DateTime SlaDeadline { get; private set; }

    public EscalationStatus Status { get; private set; }

    public string? AssignedTo { get; private set; }

    public DateTime? AssignedAt { get; private set; }

    public string? ResolvedBy { get; private set; }

    public string? ResolutionNotes { get; private set; }

    public DateTime? ResolvedAt { get; private set; }

    public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

    public bool IsBreached =>
        (Status == EscalationStatus.Open ||
         Status == EscalationStatus.Assigned)
        && DateTime.UtcNow > SlaDeadline;

    #region Factory

    public static EscalationTicket Create(
        Guid discrepancyId,
        Guid correlationId,
        string reason,
        DiscrepancySeverity severity,
        ReconciliationOptions options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);
        ArgumentNullException.ThrowIfNull(options);

        return new EscalationTicket
        {
            Id = Guid.NewGuid(),
            DiscrepancyId = discrepancyId,
            CorrelationId = correlationId,
            CausationId = discrepancyId,
            Severity = severity,
            Reason = reason,
            CreatedAt = DateTime.UtcNow,
            SlaDeadline = DateTime.UtcNow.AddHours(GetSlaHours(severity, options)),
            Status = EscalationStatus.Open
        };
    }

    #endregion

    #region Domain

    public void Assign(string assignedTo)
    {
        if (Status != EscalationStatus.Open)
            throw new InvalidOperationException();

        ArgumentException.ThrowIfNullOrWhiteSpace(assignedTo);

        AssignedTo = assignedTo;
        AssignedAt = DateTime.UtcNow;
        Status = EscalationStatus.Assigned;
    }

    public void BeginInvestigation()
    {
        if (Status != EscalationStatus.Open &&
            Status != EscalationStatus.Assigned)
        {
            throw new InvalidOperationException();
        }

        Status = EscalationStatus.InProgress;
    }

    public void Resolve(
        string resolvedBy,
        string notes)
    {
        if (Status != EscalationStatus.InProgress)
            throw new InvalidOperationException();

        ArgumentException.ThrowIfNullOrWhiteSpace(resolvedBy);
        ArgumentException.ThrowIfNullOrWhiteSpace(notes);

        Status = EscalationStatus.Resolved;
        ResolvedBy = resolvedBy;
        ResolutionNotes = notes;
        ResolvedAt = DateTime.UtcNow;
    }

    public void Close(string closedBy)
    {
        if (Status != EscalationStatus.Resolved)
            throw new InvalidOperationException();

        ArgumentException.ThrowIfNullOrWhiteSpace(closedBy);

        Status = EscalationStatus.Closed;
        ResolvedBy = closedBy;
    }

    public void Reject(
        string rejectedBy,
        string reason)
    {
        if (Status != EscalationStatus.Open &&
            Status != EscalationStatus.Assigned &&
            Status != EscalationStatus.InProgress)
        {
            throw new InvalidOperationException();
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(rejectedBy);
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);

        Status = EscalationStatus.Rejected;
        ResolvedBy = rejectedBy;
        ResolutionNotes = reason;
        ResolvedAt = DateTime.UtcNow;
    }

    #endregion

    private static double GetSlaHours(
        DiscrepancySeverity severity,
        ReconciliationOptions options)
    {
        return severity switch
        {
            DiscrepancySeverity.Critical => options.CriticalSlaHours,
            DiscrepancySeverity.High => options.HighSlaHours,
            DiscrepancySeverity.Medium => options.MediumSlaHours,
            DiscrepancySeverity.Low => options.LowSlaHours,
            _ => throw new ArgumentOutOfRangeException(nameof(severity))
        };
    }
}