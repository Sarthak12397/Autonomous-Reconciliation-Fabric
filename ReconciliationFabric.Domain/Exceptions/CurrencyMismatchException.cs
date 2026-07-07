using System;

namespace YourNamespace;

public sealed class CurrencyMismatchException : Exception
{
    public CurrencyCode Expected { get; }
    public CurrencyCode Actual { get; }

    public CurrencyMismatchException(
        CurrencyCode expected,
        CurrencyCode actual)
        : base(
            $"Currency mismatch: expected {expected}, got {actual}.{Environment.NewLine}" +
            "Cross-currency arithmetic is forbidden.")
    {
        ArgumentNullException.ThrowIfNull(expected);
        ArgumentNullException.ThrowIfNull(actual);

        Expected = expected;
        Actual = actual;
    }
}