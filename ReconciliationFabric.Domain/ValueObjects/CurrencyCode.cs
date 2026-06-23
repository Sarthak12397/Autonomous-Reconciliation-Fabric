using System;
using System.Collections.Generic;

public sealed class CurrencyCode : IEquatable<CurrencyCode>
{
    private static readonly HashSet<string> SupportedCodes =
        new(StringComparer.Ordinal)
        {
            "USD",
            "GBP",
            "EUR",
            "AUD",
            "NZD",
            "SGD",
            "MYR",
            "NPR",
            "JPY",
            "CHF",
            "CAD",
            "HKD"
        };

    public string Value { get; }

    public CurrencyCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                "Currency code cannot be null or empty.",
                nameof(value));
        }

        if (value.Length != 3)
        {
            throw new ArgumentException(
                "Currency code must be exactly 3 characters.",
                nameof(value));
        }

        var normalized = value.ToUpperInvariant();

        if (!SupportedCodes.Contains(normalized))
        {
            throw new ArgumentException(
                $"Unsupported currency code '{value}'.",
                nameof(value));
        }

        Value = normalized;
    }

    public bool Equals(CurrencyCode? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return string.Equals(Value, other.Value, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj)
    {
        return obj is CurrencyCode other && Equals(other);
    }

    public override int GetHashCode()
    {
        return StringComparer.Ordinal.GetHashCode(Value);
    }

    public static bool operator ==(CurrencyCode? left, CurrencyCode? right)
    {
        if (ReferenceEquals(left, right))
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(CurrencyCode? left, CurrencyCode? right)
    {
        return !(left == right);
    }

    public override string ToString()
    {
        return Value;
    }
}