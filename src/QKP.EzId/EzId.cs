using System;
using System.Diagnostics.CodeAnalysis;

namespace QKP.EzId
{
    /// <summary>
    /// An identifier which encodes a 64-bit value with base32 crockford encoding
    /// to produce human friendly readable identifiers.
    ///
    /// The identifier is a 13 character long string. (64 bits / 5 = 13)
    /// </summary>
#if NET7_0_OR_GREATER
    public class EzId : IParsable<EzId>
#else
    public class EzId
#endif
    {
        private readonly long _value;

        /// <summary>
        /// Gets the 64-bit value.
        /// </summary>
        public long Value => _value;

        /// <summary>
        /// Gets the base 32 <see cref="string"/> representation of the identifier.
        /// </summary>
        public string StringValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EzId"/> class.
        /// </summary>
        /// <param name="value">The 64-bit value.</param>
        public EzId(long value)
        {
            _value = value;
            StringValue = Base32.Base32CrockFord.Encode(_value);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return StringValue;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is EzId other && Value == other.Value;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Determines whether two specified EzId objects have the same value.
        /// </summary>
        /// <param name="left">The first EzId to compare.</param>
        /// <param name="right">The second EzId to compare.</param>
        /// <returns>true if the value of left is the same as the value of right; otherwise, false.</returns>
        public static bool operator ==(EzId? left, EzId? right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two specified EzId objects have different values.
        /// </summary>
        /// <param name="left">The first EzId to compare.</param>
        /// <param name="right">The second EzId to compare.</param>
        /// <returns>true if the value of left is different from the value of right; otherwise, false.</returns>
        public static bool operator !=(EzId? left, EzId? right)
        {
            return !(left == right);
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
        /// <param name="result">When this method returns, contains the parsed value if successful, otherwise a default value.</param>
        /// <returns>true if parsing succeeded; otherwise, false.</returns>
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

#if NET7_0_OR_GREATER
        // If you want to implement IParsable<EzId> explicitly,
        // the signatures already match the ones above.
        // You can also add explicit interface implementations if desired.
        static EzId IParsable<EzId>.Parse(string s, IFormatProvider? provider) => Parse(s, provider);
        static bool IParsable<EzId>.TryParse(string? s, IFormatProvider? provider, out EzId result) => TryParse(s, provider, out result!);
#endif
    }
}
