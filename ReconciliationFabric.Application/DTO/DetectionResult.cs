public record DetectionResult(
    IReadOnlyCollection<ReconciliationRecord> Records,
    int MatchedCount,
    int DiscrepantCount,
    int PendingSettlementCount, 
    int DuplicateCount,
    decimal CoveragePercent,
    decimal DiscrepancyRate,    
    IReadOnlyCollection<string> Warnings
)
{
    public bool HasDiscrepancies => DiscrepantCount > 0;

    public bool HasWarnings => Warnings.Count > 0;

    public bool CircuitBreakerShouldTrip(decimal threshold)
        => DiscrepancyRate > threshold;
}