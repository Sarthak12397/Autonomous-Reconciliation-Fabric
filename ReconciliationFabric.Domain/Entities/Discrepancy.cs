using System;
using System.Collections.Generic;

public sealed class Discrepancy
{
    private readonly List<ResolutionAction> _actions = new();

    private Discrepancy()
    {
    }

    public Guid Id { get; private set; }

    public Guid RecordId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public DiscrepancyType Type { get; private set; }

    public DiscrepancySeverity Severity { get; private set; }

    public bool AutoResolvable { get; private set; }

    public int MaxResolutionAttempts { get; private set; }

    public DateTime DetectedAt { get; private set; }

    public ResolutionStrategyType Strategy { get; private set; }

    public DiscrepancyStatus Status { get; private set; }

    public int ResolutionAttempts { get; private set; }

    public string? ResolutionNotes { get; private set; }

    public DateTime? ResolvedAt { get; private set; }

    public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

    public IReadOnlyCollection<ResolutionAction> Actions =>
        _actions.AsReadOnly();


    #region Factory

    internal static Discrepancy Create(
        Guid recordId,
        Guid correlationId,
        DiscrepancyType type,
        DiscrepancySeverity severity,
        bool autoResolvable,
        ResolutionStrategyType strategy,
        int maxAttempts)
    {
        if (autoResolvable && maxAttempts <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maxAttempts),
                "Auto resolvable discrepancies must have at least one resolution attempt.");
        }

        return new Discrepancy
        {
            Id = Guid.NewGuid(),
            RecordId = recordId,
            CorrelationId = correlationId,
            CausationId = recordId,

            Type = type,
            Severity = severity,

            AutoResolvable = autoResolvable,
            MaxResolutionAttempts =
                autoResolvable ? maxAttempts : 0,

            Strategy = strategy,

            Status = DiscrepancyStatus.Detected,
            ResolutionAttempts = 0,

            DetectedAt = DateTime.UtcNow
        };
    }

    #endregion


    #region Domain Methods

    public bool CanAttemptResolution()
    {
        bool isEligible =
            AutoResolvable &&
            MaxResolutionAttempts > 0;

        bool hasRemainingAttempts =
            ResolutionAttempts < MaxResolutionAttempts;

        bool isValidState =
            Status == DiscrepancyStatus.Detected ||
            Status == DiscrepancyStatus.Failed;

        return isEligible &&
               hasRemainingAttempts &&
               isValidState;
    }


    public void BeginResolution()
    {
        if (!CanAttemptResolution())
        {
            throw new InvalidOperationException(
                $"Discrepancy {Id} cannot resolve. " +
                $"AutoResolvable={AutoResolvable}, " +
                $"Attempts={ResolutionAttempts}/{MaxResolutionAttempts}, " +
                $"Status={Status}");
        }

        Status = DiscrepancyStatus.Resolving;
        ResolutionAttempts++;
    }


    public void MarkResolved(string notes)
    {
        if (Status != DiscrepancyStatus.Resolving)
        {
            throw new InvalidOperationException(
                "Only resolving discrepancies can be marked resolved.");
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(notes);

        Status = DiscrepancyStatus.Resolved;
        ResolvedAt = DateTime.UtcNow;
        ResolutionNotes = notes;
    }


    public void MarkFailed(string reason)
    {
        if (Status != DiscrepancyStatus.Resolving)
        {
            throw new InvalidOperationException(
                "Only resolving discrepancies can fail.");
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(reason);

        ResolutionNotes = reason;

        if (ResolutionAttempts >= MaxResolutionAttempts)
        {
            Status = DiscrepancyStatus.Escalated;
        }
        else
        {
            Status = DiscrepancyStatus.Failed;
        }
    }


    public void ForceEscalate(string reason)
    {
        if (Status == DiscrepancyStatus.Resolved)
        {
            throw new InvalidOperationException(
                "Resolved discrepancies cannot be escalated.");
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(reason);

        Status = DiscrepancyStatus.Escalated;
        ResolutionNotes = reason;
    }


    public void HumanResolved(
        string resolvedBy,
        string notes)
    {
        if (Status != DiscrepancyStatus.Escalated)
        {
            throw new InvalidOperationException(
                "Only escalated discrepancies can be human resolved.");
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(resolvedBy);
        ArgumentException.ThrowIfNullOrWhiteSpace(notes);

        Status = DiscrepancyStatus.Resolved;

        ResolvedAt = DateTime.UtcNow;

        ResolutionNotes =
            $"Human resolved by {resolvedBy}: {notes}";
    }


    public ResolutionAction LogAction(
        ResolutionActionResult result,
        string executionIdempotencyKey,
        string metadata)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            executionIdempotencyKey);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            metadata);

        var action = ResolutionAction.Create(
            discrepancyId: Id,
            correlationId: CorrelationId,
            strategy: Strategy,
            result: result,
            executionIdempotencyKey: executionIdempotencyKey,
            metadata: metadata);

        _actions.Add(action);

        return action;
    }

    #endregion
}