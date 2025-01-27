using System.Diagnostics.CodeAnalysis;

namespace QKP.EzId;

/// <summary>
/// An identifier which encodes a 64 bit value with base32 crockford encoding
/// to produce human friendly readable identifiers.
///
/// The identifier is a 13 character long string. ( 64 bits / 5 = 13 )
/// </summary>
/// <param name="Value">The 64 bit value.</param>
public record EzId(long Value) : IParsable<EzId>
{
    /// <summary>
    /// Gets the base 32 <see cref="string"/> representation of the identifier.
    /// </summary>
    public string StringValue { get; } = Base32.Base32CrockFord.Encode(Value);

    /// <inheritdoc />
    public override string ToString()
    {
        return StringValue;
    }

    /// <summary>
    /// Parses a <see cref="string"/> value to an instance of <see cref="EzId"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> value.</param>
    /// <returns>The parsed <see cref="EzId"/> value.</returns>
    public static EzId Parse(string value)
    {
        return new EzId(Base32.Base32CrockFord.DecodeLong(value));
    }

    /// <summary>
    /// Parses a <see cref="string"/> value to an instance of <see cref="EzId"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> value to parse.</param>
    /// <param name="provider">The format provider.</param>
    /// <returns>The parsed <see cref="EzId"/> value.</returns>
    public static EzId Parse(string value, IFormatProvider? provider) => Parse(value);

    /// <summary>
    /// Parses a <see cref="string"/> value to an instance of <see cref="EzId"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> value to parse.</param>
    /// <param name="provider">The format provider.</param>
    /// <param name="result"></param>
    /// <returns>The parsed <see cref="EzId"/> value.</returns>
    public static bool TryParse([NotNullWhen(true)] string? value, IFormatProvider? provider,
        [MaybeNullWhen(false)] out EzId result)
    {
        if (string.IsNullOrEmpty(value))
        {
            result = new EzId(-1);
            return false;
        }

        result = Parse(value);
        return true;
    }
}
