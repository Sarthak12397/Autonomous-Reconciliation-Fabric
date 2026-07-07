using System;

public sealed class ToleranceBand
{
    public decimal LowThreshold { get; }
    public decimal MediumThreshold { get; }
    public decimal HighThreshold { get; }

    public ToleranceBand(decimal low, decimal medium, decimal high)
    {
        if (low < 0)
            throw new ArgumentException("Low threshold cannot be negative.", nameof(low));

        if (medium < 0)
            throw new ArgumentException("Medium threshold cannot be negative.", nameof(medium));

        if (high < 0)
            throw new ArgumentException("High threshold cannot be negative.", nameof(high));

        if (low > medium)
            throw new ArgumentException(
                "Low threshold must be less than or equal to the medium threshold.",
                nameof(low));

        if (medium > high)
            throw new ArgumentException(
                "Medium threshold must be less than or equal to the high threshold.",
                nameof(medium));

        LowThreshold = low;
        MediumThreshold = medium;
        HighThreshold = high;
    }

    public static ToleranceBand From(ReconciliationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        return new ToleranceBand(
            options.LowThresholdAmount,
            options.MediumThresholdAmount,
            options.HighThresholdAmount);
    }

    public DiscrepancySeverity Classify(decimal delta)
    {
        if (delta < 0)
            throw new ArgumentException(
                "Delta cannot be negative.",
                nameof(delta));

        if (delta <= LowThreshold)
            return DiscrepancySeverity.Low;

        if (delta <= MediumThreshold)
            return DiscrepancySeverity.Medium;

        if (delta <= HighThreshold)
            return DiscrepancySeverity.High;

        return DiscrepancySeverity.Critical;
    }

    public bool IsAutoResolvable(decimal delta) =>
        Classify(delta) == DiscrepancySeverity.Low;
}