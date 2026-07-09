public record ReconciliationReport(
    DateTime GeneratedAt,
    DateTime PeriodStart,
    DateTime PeriodEnd,
    int TotalJobsRun,
    int TotalRecordsProcessed,
    int TotalMatched,
    int TotalDiscrepancies,
    int TotalResolved,
    int TotalEscalated,
    int OpenEscalations,
    int SlaBreaches,
    int CircuitBreakerTrips,
    decimal ResolutionRate,
    decimal CoverageRate
);