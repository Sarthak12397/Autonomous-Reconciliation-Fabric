using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace YourNamespace;

public sealed class CanonicalReferenceKey : IEquatable<CanonicalReferenceKey>
{
    public string Original { get; }
    public string Canonical { get; }
    public byte[] HashKey { get; }

    public CanonicalReferenceKey(string original)
    {
        if (string.IsNullOrWhiteSpace(original))
            throw new ArgumentException("Reference cannot be null or empty.", nameof(original));

        Original = original;
        Canonical = Normalise(original);
        HashKey = SHA256.HashData(Encoding.UTF8.GetBytes(Canonical));
    }

    private static string Normalise(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var builder = new StringBuilder(value.Trim().ToUpperInvariant().Length);

        foreach (var c in value.Trim().ToUpperInvariant())
        {
            if (c == '_' || c == '-')
                continue;

            if (char.IsLetterOrDigit(c))
                builder.Append(c);
        }

        return builder.ToString();
    }

    public bool Equals(CanonicalReferenceKey? other)
    {
        if (ReferenceEquals(null, other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return string.Equals(Canonical, other.Canonical, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj) =>
        obj is CanonicalReferenceKey other && Equals(other);

    public override int GetHashCode() =>
        StringComparer.Ordinal.GetHashCode(Canonical);

    public static bool operator ==(
        CanonicalReferenceKey? left,
        CanonicalReferenceKey? right) =>
        Equals(left, right);

    public static bool operator !=(
        CanonicalReferenceKey? left,
        CanonicalReferenceKey? right) =>
        !Equals(left, right);

    public override string ToString() => Canonical;
}