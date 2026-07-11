using System;
using System.Security.Cryptography;
using System.Text;

public sealed class ReconciliationAuditLog
{
    private ReconciliationAuditLog()
    {
    }

    public Guid Id { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid? CausationId { get; private set; }

    public AuditEventType EventType { get; private set; }

    public string Actor { get; private set; } = null!;

    public string Payload { get; private set; } = null!;

    public int SchemaVersion { get; private set; }

    public DateTime OccurredAt { get; private set; }

    public Guid? JobId { get; private set; }

    public Guid? RecordId { get; private set; }

    public Guid? DiscrepancyId { get; private set; }

    public Guid? EscalationId { get; private set; }

    public string? PreviousHash { get; private set; }

    public string CurrentHash { get; private set; } = null!;

    #region Factory

    public static ReconciliationAuditLog Record(
        AuditEventType eventType,
        string actor,
        string payload,
        Guid correlationId,
        string? previousHash,
        Guid? causationId = null,
        Guid? jobId = null,
        Guid? recordId = null,
        Guid? discrepancyId = null,
        Guid? escalationId = null,
        int schemaVersion = 1)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(actor);
        ArgumentException.ThrowIfNullOrWhiteSpace(payload);

        var now = DateTime.UtcNow;

        var log = new ReconciliationAuditLog
        {
            Id = Guid.NewGuid(),
            CorrelationId = correlationId,
            CausationId = causationId,
            EventType = eventType,
            Actor = actor,
            Payload = payload,
            SchemaVersion = schemaVersion,
            OccurredAt = now,
            JobId = jobId,
            RecordId = recordId,
            DiscrepancyId = discrepancyId,
            EscalationId = escalationId,
            PreviousHash = previousHash
        };

        log.CurrentHash = ComputeHash(log);

        return log;
    }

    #endregion

    internal static string ComputeHash(ReconciliationAuditLog entry)
    {
        var input =
            entry.Id.ToString() +
            entry.OccurredAt.ToString("O") +
            entry.EventType +
            entry.Payload +
            (entry.PreviousHash ?? "GENESIS");

        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));

        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}