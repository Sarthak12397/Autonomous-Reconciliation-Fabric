using System;

public sealed class OutboxMessage
{
    private OutboxMessage()
    {
    }

    public Guid Id { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public string EventType { get; private set; } = null!;

    public string Payload { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; }

    public int MaxAttempts { get; private set; }

    public OutboxMessageStatus Status { get; private set; }

    public int AttemptCount { get; private set; }

    public DateTime? NextAttemptAt { get; private set; }

    public string? LastError { get; private set; }

    public DateTime? ProcessedAt { get; private set; }


    #region Factory

    public static OutboxMessage Create(
        string eventType,
        string payload,
        Guid correlationId,
        Guid causationId,
        int maxAttempts)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(eventType);
        ArgumentException.ThrowIfNullOrWhiteSpace(payload);

        if (maxAttempts <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxAttempts));

        return new OutboxMessage
        {
            Id = Guid.NewGuid(),
            CorrelationId = correlationId,
            CausationId = causationId,
            EventType = eventType,
            Payload = payload,
            CreatedAt = DateTime.UtcNow,
            MaxAttempts = maxAttempts,
            Status = OutboxMessageStatus.Pending,
            AttemptCount = 0,
            NextAttemptAt = null
        };
    }

    #endregion


    #region Domain Methods

    public void MarkProcessing()
    {
        if (Status != OutboxMessageStatus.Pending &&
            Status != OutboxMessageStatus.Failed)
        {
            throw new InvalidOperationException();
        }

        if (NextAttemptAt.HasValue &&
            NextAttemptAt.Value > DateTime.UtcNow)
        {
            throw new InvalidOperationException(
                "Not yet eligible for retry.");
        }

        Status = OutboxMessageStatus.Processing;
    }


    public void MarkProcessed()
    {
        if (Status != OutboxMessageStatus.Processing)
            throw new InvalidOperationException();

        if (ProcessedAt != null)
        {
            throw new InvalidOperationException(
                "Already processed.");
        }

        Status = OutboxMessageStatus.Published;
        ProcessedAt = DateTime.UtcNow;
    }


    public void MarkFailed(string error)
    {
        if (Status != OutboxMessageStatus.Processing)
            throw new InvalidOperationException();

        ArgumentException.ThrowIfNullOrWhiteSpace(error);

        AttemptCount++;
        LastError = error;

        if (AttemptCount >= MaxAttempts)
        {
            Status = OutboxMessageStatus.DeadLettered;
            NextAttemptAt = null;
            return;
        }

        Status = OutboxMessageStatus.Failed;
        NextAttemptAt =
            DateTime.UtcNow.AddSeconds(
                CalculateBackoffSeconds(AttemptCount));
    }


    public void MarkDeadLettered(string reason)
    {
        if (Status == OutboxMessageStatus.Published)
        {
            throw new InvalidOperationException();
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(reason);

        Status = OutboxMessageStatus.DeadLettered;
        LastError = reason;
        NextAttemptAt = null;
    }


    #endregion


    private static int CalculateBackoffSeconds(int attempt)
    {
        // attempt 1 => 2 seconds
        // attempt 2 => 4 seconds
        // attempt 3 => 8 seconds
        // attempt N => 2^N seconds

        return (int)Math.Pow(2, attempt);
    }
}