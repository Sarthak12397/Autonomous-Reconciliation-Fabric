using System;

public sealed class ReconciliationWindow : IEquatable<ReconciliationWindow>
{
    public DateTime Start { get; }
    public DateTime End { get; }
    public SourceSystem Source { get; }
    public SourceSystem Target { get; }
    public TimeSpan TemporalBuffer { get; }

    public TimeSpan Duration => End - Start;

    public DateTime BufferedStart => Start - TemporalBuffer;

    public DateTime BufferedEnd => End + TemporalBuffer;

    public ReconciliationWindow(
        DateTime start,
        DateTime end,
        SourceSystem source,
        SourceSystem target,
        int maxWindowDays,
        int temporalBufferMin)
    {
        if (start.Kind != DateTimeKind.Utc)
            throw new ArgumentException("Start must be UTC.", nameof(start));

        if (end.Kind != DateTimeKind.Utc)
            throw new ArgumentException("End must be UTC.", nameof(end));

        if (end <= start)
            throw new ArgumentException("End must be after Start.", nameof(end));

        if (end - start > TimeSpan.FromDays(maxWindowDays))
            throw new ArgumentException(
                $"Window cannot exceed {maxWindowDays} days.",
                nameof(end));

        if (source.Equals(target))
            throw new ArgumentException(
                "Source and Target cannot be the same system.");

        if (end > DateTime.UtcNow.AddMinutes(5))
            throw new ArgumentException(
                "End cannot be more than 5 minutes in the future. Future data has not settled yet.",
                nameof(end));

        if (temporalBufferMin < 0)
            throw new ArgumentException(
                "Temporal buffer cannot be negative.",
                nameof(temporalBufferMin));

        Start = start;
        End = end;
        Source = source;
        Target = target;
        TemporalBuffer = TimeSpan.FromMinutes(temporalBufferMin);
    }

    public bool IsNearBoundary(DateTime timestamp)
    {
        var nearStart =
            timestamp >= Start - TemporalBuffer &&
            timestamp <= Start + TemporalBuffer;

        var nearEnd =
            timestamp >= End - TemporalBuffer &&
            timestamp <= End + TemporalBuffer;

        return nearStart || nearEnd;
    }

    public string ToIdempotencyKey(string prefix)
    {
        if (string.IsNullOrWhiteSpace(prefix))
            throw new ArgumentException(
                "Prefix is required.",
                nameof(prefix));

        return $"{prefix}-{Source}-{Target}-{Start:yyyyMMddHHmm}-{End:yyyyMMddHHmm}";
    }

    public bool Equals(ReconciliationWindow? other)
    {
        if (other is null)
            return false;

        return Start == other.Start &&
               End == other.End &&
               EqualityComparer<SourceSystem>.Default.Equals(Source, other.Source) &&
               EqualityComparer<SourceSystem>.Default.Equals(Target, other.Target);
    }

    public override bool Equals(object? obj)
        => Equals(obj as ReconciliationWindow);

    public override int GetHashCode()
        => HashCode.Combine(Start, End, Source, Target);

    public static bool operator ==(
        ReconciliationWindow? left,
        ReconciliationWindow? right)
        => Equals(left, right);

    public static bool operator !=(
        ReconciliationWindow? left,
        ReconciliationWindow? right)
        => !Equals(left, right);
}

