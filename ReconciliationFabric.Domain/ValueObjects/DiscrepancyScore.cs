using System;

public sealed class DiscrepancyScore
{
    public readonly Money Delta;
    public readonly decimal DeltaPercent;
    public readonly DiscrepancySeverity Severity;
    public readonly bool AutoResolvable;

    public DiscrepancyScore(
        Money expected,
        Money actual,
        ToleranceBand band)
    {
        if (!expected.SameCurrencyAs(actual))
        {
            throw new CurrencyMismatchException(
                expected.Currency,
                actual.Currency);
        }

        Delta = expected.AbsoluteDifference(actual);

        DeltaPercent = CalculateDeltaPercent(
            expected.Amount,
            Delta.Amount);

        Severity = band.Classify(Delta.Amount);

        AutoResolvable = band.IsAutoResolvable(Delta.Amount);
    }

    private static decimal CalculateDeltaPercent(
        decimal expectedAmount,
        decimal deltaAmount)
    {
        if (expectedAmount == 0m)
        {
            return deltaAmount == 0m
                ? 0.0000m
                : 100.0000m;
        }

        return Math.Round(
            (deltaAmount / expectedAmount) * 100m,
            4,
            MidpointRounding.AwayFromZero);
    }
}