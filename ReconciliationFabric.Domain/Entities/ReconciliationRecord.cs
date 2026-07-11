using System;
using System.Collections.Generic;

public sealed class ReconciliationRecord
{
    private readonly List<Discrepancy> _discrepancies = new();

    private ReconciliationRecord()
    {
    }

    public Guid Id { get; private set; }
    public Guid JobId { get; private set; }
    public Guid CorrelationId { get; private set; }
    public Guid CausationId { get; private set; }

    public string ExternalReference { get; private set; } = null!;
    public string CanonicalKey { get; private set; } = null!;
    public string? InternalReference { get; private set; }

    public DateTime DetectedAt { get; private set; }

    public bool IsNearWindowBoundary { get; private set; }

    public Money? ExternalMoney { get; private set; }
    public Money? InternalMoney { get; private set; }

    public string? ExternalStatus { get; private set; }
    public string? InternalStatus { get; private set; }

    public RecordMatchStatus MatchStatus { get; private set; }

    public DateTime? ResolvedAt { get; private set; }

    public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

    public IReadOnlyCollection<Discrepancy> Discrepancies => _discrepancies.AsReadOnly();

    #region Factory Methods

    public static ReconciliationRecord CreateMatched(
        Guid jobId,
        Guid correlationId,
        CanonicalReferenceKey externalRef,
        string internalReference,
        Money money)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(internalReference);
        ArgumentNullException.ThrowIfNull(money);

        return CreateBase(jobId, correlationId, externalRef, internalReference)
            .SetExternalMoney(money)
            .SetInternalMoney(money)
            .SetStatus(RecordMatchStatus.Matched);
    }

    public static ReconciliationRecord CreateAmountMismatch(
        Guid jobId,
        Guid correlationId,
        CanonicalReferenceKey externalRef,
        string internalReference,
        Money externalMoney,
        Money internalMoney)
    {
        if (!externalMoney.SameCurrencyAs(internalMoney))
            throw new InvalidOperationException(
                "Use CreateCurrencyMismatch for cross-currency records.");

        if (externalMoney == internalMoney)
            throw new ArgumentException(
                "Amounts are equal. Use CreateMatched instead.");

        return CreateBase(jobId, correlationId, externalRef, internalReference)
            .SetExternalMoney(externalMoney)
            .SetInternalMoney(internalMoney)
            .SetStatus(RecordMatchStatus.Discrepant);
    }

    public static ReconciliationRecord CreateCurrencyMismatch(
        Guid jobId,
        Guid correlationId,
        CanonicalReferenceKey externalRef,
        string internalReference,
        Money externalMoney,
        Money internalMoney)
    {
        if (externalMoney.SameCurrencyAs(internalMoney))
            throw new ArgumentException(
                "Currencies are the same. Use CreateAmountMismatch instead.");

        return CreateBase(jobId, correlationId, externalRef, internalReference)
            .SetExternalMoney(externalMoney)
            .SetInternalMoney(internalMoney)
            .SetStatus(RecordMatchStatus.Discrepant);
    }

    public static ReconciliationRecord CreateStatusConflict(
        Guid jobId,
        Guid correlationId,
        CanonicalReferenceKey externalRef,
        string internalReference,
        Money money,
        string externalStatus,
        string internalStatus)
    {
        if (string.Equals(
                externalStatus,
                internalStatus,
                StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                "Statuses are equal. Use CreateMatched instead.");
        }

        return CreateBase(jobId, correlationId, externalRef, internalReference)
            .SetExternalMoney(money)
            .SetInternalMoney(money)
            .SetExternalStatus(externalStatus)
            .SetInternalStatus(internalStatus)
            .SetStatus(RecordMatchStatus.Discrepant);
    }

    public static ReconciliationRecord CreateUnmatchedInternal(
        Guid jobId,
        Guid correlationId,
        CanonicalReferenceKey externalRef,
        Money externalMoney,
        string externalStatus)
    {
        return CreateBase(jobId, correlationId, externalRef, null)
            .SetExternalMoney(externalMoney)
            .SetExternalStatus(externalStatus)
            .SetStatus(RecordMatchStatus.UnmatchedInternal);
    }

    public static ReconciliationRecord CreateUnmatchedExternal(
        Guid jobId,
        Guid correlationId,
        CanonicalReferenceKey externalRef,
        string internalReference,
        Money internalMoney,
        string internalStatus)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(internalReference);

        return CreateBase(jobId, correlationId, externalRef, internalReference)
            .SetInternalMoney(internalMoney)
            .SetInternalStatus(internalStatus)
            .SetStatus(RecordMatchStatus.UnmatchedExternal);
    }

    public static ReconciliationRecord CreateDuplicate(
        Guid jobId,
        Guid correlationId,
        CanonicalReferenceKey externalRef,
        string internalReference,
        Money externalMoney)
    {
        return CreateBase(jobId, correlationId, externalRef, internalReference)
            .SetExternalMoney(externalMoney)
            .SetStatus(RecordMatchStatus.Discrepant);
    }

    public static ReconciliationRecord CreatePendingSettlement(
        Guid jobId,
        Guid correlationId,
        CanonicalReferenceKey externalRef,
        Money externalMoney,
        string externalStatus)
    {
        return CreateBase(jobId, correlationId, externalRef, null)
            .SetExternalMoney(externalMoney)
            .SetExternalStatus(externalStatus)
            .SetPendingSettlement();
    }

    #endregion

    #region Domain Methods

    public Discrepancy AddDiscrepancy(
        DiscrepancyType type,
        DiscrepancySeverity severity,
        bool autoResolvable,
        ResolutionStrategyType strategy,
        int maxAttempts)
    {
        if (MatchStatus == RecordMatchStatus.Matched)
            throw new InvalidOperationException();

        if (MatchStatus == RecordMatchStatus.PendingSettlement)
            throw new InvalidOperationException(
                "PendingSettlement records cannot have discrepancies. They may self-heal. Wait for next reconciliation run.");

        var discrepancy = Discrepancy.Create(
            Id,
            CorrelationId,
            type,
            severity,
            autoResolvable,
            strategy,
            maxAttempts);

        _discrepancies.Add(discrepancy);

        return discrepancy;
    }

    public void MarkResolved()
    {
        if (MatchStatus != RecordMatchStatus.Discrepant &&
            MatchStatus != RecordMatchStatus.UnmatchedInternal &&
            MatchStatus != RecordMatchStatus.UnmatchedExternal)
        {
            throw new InvalidOperationException();
        }

        MatchStatus = RecordMatchStatus.Resolved;
        ResolvedAt = DateTime.UtcNow;
    }

    public void MarkEscalated()
    {
        if (MatchStatus == RecordMatchStatus.Matched ||
            MatchStatus == RecordMatchStatus.Resolved ||
            MatchStatus == RecordMatchStatus.PendingSettlement)
        {
            throw new InvalidOperationException();
        }

        MatchStatus = RecordMatchStatus.Escalated;
    }

    public void MarkPendingSettlementUnresolved()
    {
        if (MatchStatus != RecordMatchStatus.PendingSettlement)
            throw new InvalidOperationException();

        MatchStatus = RecordMatchStatus.UnmatchedInternal;
        IsNearWindowBoundary = false;
    }

    #endregion

    #region Helpers

    private static ReconciliationRecord CreateBase(
        Guid jobId,
        Guid correlationId,
        CanonicalReferenceKey key,
        string? internalReference)
    {
        return new ReconciliationRecord
        {
            Id = Guid.NewGuid(),
            JobId = jobId,
            CorrelationId = correlationId,
            CausationId = jobId,
            ExternalReference = key.Original,
            CanonicalKey = key.Canonical,
            InternalReference = internalReference,
            DetectedAt = DateTime.UtcNow,
            IsNearWindowBoundary = false
        };
    }

    private ReconciliationRecord SetStatus(RecordMatchStatus status)
    {
        MatchStatus = status;
        return this;
    }

    private ReconciliationRecord SetPendingSettlement()
    {
        MatchStatus = RecordMatchStatus.PendingSettlement;
        IsNearWindowBoundary = true;
        return this;
    }

    private ReconciliationRecord SetExternalMoney(Money? money)
    {
        ExternalMoney = money;
        return this;
    }

    private ReconciliationRecord SetInternalMoney(Money? money)
    {
        InternalMoney = money;
        return this;
    }

    private ReconciliationRecord SetExternalStatus(string? status)
    {
        ExternalStatus = status;
        return this;
    }

    private ReconciliationRecord SetInternalStatus(string? status)
    {
        InternalStatus = status;
        return this;
    }

    #endregion
}