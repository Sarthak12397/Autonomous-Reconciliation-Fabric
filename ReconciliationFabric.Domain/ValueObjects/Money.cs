using System;


public readonly record struct Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public CurrencyCode Currency { get; }

    public Money(decimal amount, CurrencyCode currency)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));

        ArgumentNullException.ThrowIfNull(currency);

        Amount = Math.Round(amount, 4, MidpointRounding.ToEven);
        Currency = currency;
    }

    public static Money Of(decimal amount, string currencyCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(currencyCode);

        return new Money(amount, new CurrencyCode(currencyCode));
    }

    public static Money Zero(CurrencyCode currency)
    {
        ArgumentNullException.ThrowIfNull(currency);

        return new Money(0m, currency);
    }

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount - other.Amount, Currency);
    }

    public Money AbsoluteDifference(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Math.Abs(Amount - other.Amount), Currency);
    }

    public bool SameCurrencyAs(Money other) =>
        Currency == other.Currency;

    private void EnsureSameCurrency(Money other)
    {
        if (!SameCurrencyAs(other))
        {
            throw new CurrencyMismatchException(
                Currency,
                other.Currency);
        }
    }

    public static Money operator +(Money left, Money right) =>
        left.Add(right);

    public static Money operator -(Money left, Money right) =>
        left.Subtract(right);

    public static bool operator >(Money left, Money right)
    {
        left.EnsureSameCurrency(right);
        return left.Amount > right.Amount;
    }

    public static bool operator <(Money left, Money right)
    {
        left.EnsureSameCurrency(right);
        return left.Amount < right.Amount;
    }

    public static bool operator >=(Money left, Money right)
    {
        left.EnsureSameCurrency(right);
        return left.Amount >= right.Amount;
    }

    public static bool operator <=(Money left, Money right)
    {
        left.EnsureSameCurrency(right);
        return left.Amount <= right.Amount;
    }

    public bool Equals(Money other) =>
        Amount == other.Amount &&
        Currency == other.Currency;

    public override int GetHashCode() =>
        HashCode.Combine(Amount, Currency);

    public override string ToString() =>
        $"{Amount:F4} {Currency}";
}