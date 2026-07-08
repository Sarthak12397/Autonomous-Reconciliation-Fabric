using System;

namespace Reconciliation.Domain;

public sealed class ReconciliationJob
{
    private ReconciliationJob()
    {
    }


    public Guid Id { get; private set; }

    public string IdempotencyKey { get; private set; } = null!;

    public Guid CorrelationId { get; private set; }

    public Guid? CausationId { get; private set; }

    public Guid? SupersedesJobId { get; private set; }

    public int Revision { get; private set; }

    public ReconciliationJobType JobType { get; private set; }

    public ReconciliationWindow Window { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; }



    public ReconciliationJobStatus Status { get; private set; }

    public int TotalRecordsProcessed { get; private set; }

    public int DiscrepanciesFound { get; private set; }

    public bool CircuitBreakerTripped { get; private set; }

    public DateTime? StartedAt { get; private set; }

    public DateTime? CompletedAt { get; private set; }

    public string? FailureReason { get; private set; }



    public byte[] RowVersion { get; set; } = Array.Empty<byte>();



    public static ReconciliationJob Create(
        ReconciliationJobType jobType,
        ReconciliationWindow window,
        string idempotencyKey,
        Guid? correlationId = null)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey))
            throw new ArgumentException("Idempotency key is required.", nameof(idempotencyKey));

        ArgumentNullException.ThrowIfNull(window);

        return new ReconciliationJob
        {
            Id = Guid.NewGuid(),
            IdempotencyKey = idempotencyKey,
            CorrelationId = correlationId ?? Guid.NewGuid(),
            CausationId = null,
            SupersedesJobId = null,
            Revision = 1,
            JobType = jobType,
            Window = window,
            CreatedAt = DateTime.UtcNow,
            Status = ReconciliationJobStatus.Pending,
            CircuitBreakerTripped = false
        };
    }

    public static ReconciliationJob CreateReplay(
        ReconciliationJobType jobType,
        ReconciliationWindow window,
        string idempotencyKey,
        Guid supersedesJobId,
        int previousRevision,
        Guid? correlationId = null)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey))
            throw new ArgumentException("Idempotency key is required.", nameof(idempotencyKey));

        ArgumentNullException.ThrowIfNull(window);

        if (supersedesJobId == Guid.Empty)
            throw new ArgumentException("Superseded job id is required.", nameof(supersedesJobId));

        if (previousRevision < 1)
            throw new ArgumentException("Previous revision must be >= 1.", nameof(previousRevision));

        if (!idempotencyKey.Contains("-rev", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException(
                "Replay idempotency keys must contain '-rev'.",
                nameof(idempotencyKey));

        return new ReconciliationJob
        {
            Id = Guid.NewGuid(),
            IdempotencyKey = idempotencyKey,
            CorrelationId = correlationId ?? Guid.NewGuid(),
            CausationId = null,
            SupersedesJobId = supersedesJobId,
            Revision = previousRevision + 1,
            JobType = jobType,
            Window = window,
            CreatedAt = DateTime.UtcNow,
            Status = ReconciliationJobStatus.Pending,
            CircuitBreakerTripped = false
        };
    }



    public void Start()
    {
        if (Status != ReconciliationJobStatus.Pending)
        {
            throw new InvalidJobTransitionException(
                Id,
                Status,
                ReconciliationJobStatus.Running);
        }

        Status = ReconciliationJobStatus.Running;
        StartedAt = DateTime.UtcNow;
    }

    public void RecordProcessed(bool hasDiscrepancy)
    {
        if (Status != ReconciliationJobStatus.Running)
            throw new InvalidOperationException("Job must be running.");

        TotalRecordsProcessed++;

        if (hasDiscrepancy)
            DiscrepanciesFound++;
    }

    public void TripCircuitBreaker()
    {
        if (Status != ReconciliationJobStatus.Running)
            throw new InvalidOperationException("Job must be running.");

        if (CircuitBreakerTripped)
            throw new InvalidOperationException("Circuit breaker already tripped.");

        CircuitBreakerTripped = true;
        Status = ReconciliationJobStatus.PartiallyResolved;
        CompletedAt = DateTime.UtcNow;
    }

    public void Complete(
        int actualUnresolvedDiscrepancies,
        int actualOpenEscalations)
    {
        if (Status != ReconciliationJobStatus.Running)
        {
            throw new InvalidJobTransitionException(
                Id,
                Status,
                ReconciliationJobStatus.Completed);
        }

        if (CircuitBreakerTripped)
            throw new InvalidOperationException("Circuit breaker tripped. Cannot Complete.");

        Status =
            actualUnresolvedDiscrepancies > 0 ||
            actualOpenEscalations > 0
                ? ReconciliationJobStatus.PartiallyResolved
                : ReconciliationJobStatus.Completed;

        CompletedAt = DateTime.UtcNow;
    }

    public void Fail(string? reason)
    {
        if (Status == ReconciliationJobStatus.Completed ||
            Status == ReconciliationJobStatus.Failed)
        {
            throw new InvalidOperationException(
                $"Cannot fail a job in '{Status}' state.");
        }

        Status = ReconciliationJobStatus.Failed;
        FailureReason = string.IsNullOrWhiteSpace(reason)
            ? "Unknown."
            : reason;

        CompletedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status != ReconciliationJobStatus.Pending)
            throw new InvalidOperationException("Only pending jobs can be cancelled.");

        Status = ReconciliationJobStatus.Cancelled;
        CompletedAt = DateTime.UtcNow;
    }

}