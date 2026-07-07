public sealed class CircuitBreakerOpenException : Exception
{
    public Guid JobId { get; }
    public int DiscrepancyCount { get; }
    public int TotalCount { get; }
    public decimal Rate { get; }
    public decimal Threshold { get; }

    public CircuitBreakerOpenException(
        Guid jobId,
        int discrepancyCount,
        int totalCount,
        decimal rate,
        decimal threshold)
        : base(
            $"Circuit breaker tripped for job '{jobId}'.{Environment.NewLine}" +
            $"Discrepancy rate {rate:P1} exceeds threshold {threshold:P1} " +
            $"({discrepancyCount}/{totalCount} records).{Environment.NewLine}" +
            "Autonomous resolution has been frozen.")
    {
        JobId = jobId;
        DiscrepancyCount = discrepancyCount;
        TotalCount = totalCount;
        Rate = rate;
        Threshold = threshold;
    }
}