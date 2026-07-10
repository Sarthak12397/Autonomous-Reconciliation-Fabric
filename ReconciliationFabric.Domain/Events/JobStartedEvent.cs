using System;

public sealed class JobStartedEvent : IDomainEvent
{
    public Guid EventId { get; }
    public DateTime OccurredAt { get; }
    public Guid CorrelationId { get; }

    public Guid JobId { get; }
    public DateTime WindowStart { get; }
    public DateTime WindowEnd { get; }

    public JobStartedEvent(
        Guid correlationId,
        Guid jobId,
        DateTime windowStart,
        DateTime windowEnd)
    {
        EventId = Guid.NewGuid();
        OccurredAt = DateTime.UtcNow;
        CorrelationId = correlationId;

        JobId = jobId;
        WindowStart = windowStart;
        WindowEnd = windowEnd;
    }
}