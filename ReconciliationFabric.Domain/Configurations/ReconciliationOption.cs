using System.ComponentModel.DataAnnotations;


public sealed class ReconciliationOptions
{
    public const string SectionName = "Reconciliation";

    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal LowThresholdAmount { get; init; } = 0.01m;

    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal MediumThresholdAmount { get; init; } = 100.00m;

    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal HighThresholdAmount { get; init; } = 10000.00m;

    [Range(1, int.MaxValue)]
    public int DetectionBatchSize { get; init; } = 1000;

    [Range(1, int.MaxValue)]
    public int MaxConcurrentBatches { get; init; } = 1;

    [Range(1, int.MaxValue)]
    public int StaleResolvingThresholdMin { get; init; } = 10;

    [Range(1, 365)]
    public int MaxWindowDays { get; init; } = 7;

    [Range(1, int.MaxValue)]
    public int MaxResolutionAttempts { get; init; } = 3;

    [Range(1, int.MaxValue)]
    public int CriticalSlaHours { get; init; } = 4;

    [Range(1, int.MaxValue)]
    public int HighSlaHours { get; init; } = 24;

    [Range(1, int.MaxValue)]
    public int MediumSlaHours { get; init; } = 72;

    [Range(1, int.MaxValue)]
    public int LowSlaHours { get; init; } = 168;

    [Range(0, int.MaxValue)]
    public int TemporalBufferMinutes { get; init; } = 5;

    [Range(typeof(decimal), "0", "1")]
    public decimal DiscrepancyRateThreshold { get; init; } = 0.05m;

    [Range(1, int.MaxValue)]
    public int OutboxMaxAttempts { get; init; } = 5;
}