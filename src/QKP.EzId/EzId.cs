using System;
using System.Linq;
using System.Text;
#if NET5_0_OR_GREATER
using System.Text.Json.Serialization;
using QKP.EzId.Json;
#endif

namespace QKP.EzId
{
    /// <summary>
    /// An identifier which encodes a 64-bit value with base32 crockford encoding
    /// to produce human friendly readable identifiers.
    ///
    /// <example>
    /// 070-47XF6Q8-YPA
    /// </example>
    /// The identifier is a 15 character long string. (64 bits / 5) + 2 ( separators ) = 15
    /// </summary>
#if NET5_0_OR_GREATER
    [JsonConverter(typeof(EzIdJsonConverter))]
    public class EzId : IParsable<EzId>, IEquatable<EzId>
#else
    public class EzId: IEquatable<EzId>
#endif
    {
        private readonly long _value;

        /// <summary>
        /// Gets the 64-bit value.
        /// </summary>
        public long Value => _value;

        /// <summary>
        /// Default error value for non parseable EzIds.
        /// </summary>
        public static EzId ErrorId => new EzId(-1);

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
            StringValue = Format(Base32.Base32CrockFord.Encode(_value));
        }


        /// <inheritdoc />
        public override string ToString()
        {
            return StringValue;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((EzId)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(_value, StringValue);
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
        /// <param name="value">The <see cref="string"/> value to parse.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>The parsed <see cref="EzId"/> value.</returns>
        public static EzId Parse(string value, IFormatProvider? formatProvider = null)
        {
            if (value.Length != Length)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"Value must have a length equal to {Length}.");
            }

            string encodedValue = value.Replace(Separator.ToString(), string.Empty);

            foreach (char c in encodedValue)
            {
                if (!Base32.Base32CrockFord.Alphabet.Characters.Contains(c))
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"Value contains illegal character '{c}'.");
                }
            }

            return new EzId(Base32.Base32CrockFord.DecodeLong(encodedValue));
        }

        private const int Length = 15;

        /// <summary>
        /// Parses a <see cref="string"/> value to an instance of <see cref="EzId"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to parse.</param>
        /// <param name="provider">The format provider.</param>
        /// <param name="result">When this method returns, contains the parsed value if successful, otherwise a default value.</param>
        /// <returns>true if parsing succeeded; otherwise, false.</returns>
        public static bool TryParse(string? value, IFormatProvider? provider,
            out EzId result)
        {
            try
            {
                result = Parse(value ?? "");
            }
            catch (ArgumentOutOfRangeException)
            {
                result = ErrorId;
                return false;
            }

            return true;
        }

        private static string Format(string encodedValue)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < encodedValue.Length; i++)
            {
                if (i is 3 || i is 10)
                {
                    sb.Append(Separator);
                }
                sb.Append(encodedValue[i]);
            }
            return sb.ToString();
        }

        private const char Separator = '-';

#if NET7_0_OR_GREATER
        static EzId IParsable<EzId>.Parse(string s, IFormatProvider? provider) => Parse(s, provider);
        static bool IParsable<EzId>.TryParse(string? s, IFormatProvider? provider, out EzId result) => TryParse(s, provider, out result!);
#endif
        /// <summary>
        /// Compares the current instance with another instance of <see cref="EzId"/>.
        /// </summary>
        /// <param name="other">The other instance to compare equality with..</param>
        /// <returns>True indicating when the objects are equal.</returns>
        public bool Equals(EzId? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return _value == other._value && StringValue == other.StringValue;
        }
    }
}
