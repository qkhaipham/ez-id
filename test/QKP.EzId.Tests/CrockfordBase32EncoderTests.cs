using System.Text;
using Bogus;
using FluentAssertions;

namespace QKP.EzId.Tests
{
    public class CrockfordBase32EncoderTests
    {
        [Fact]
        public void Given_bytes_when_encoding_it_must_return_expected()
        {
            string data = "ABCDEFG";
            string resultBase = SimpleBase.Base32.Crockford.Encode(Encoding.UTF8.GetBytes(data));
            string result = Base32.Base32CrockFord.Encode(Encoding.UTF8.GetBytes(data));

            result.Should().Be("85146H258S3G");
            result.Should().Be(resultBase);
        }

        [Fact]
        public void Given_large_input_of_bytes_when_encoding_it_must_return_expected()
        {
            string data = new Faker().Lorem.Paragraphs(100);
            string resultBase = SimpleBase.Base32.Crockford.Encode(Encoding.UTF8.GetBytes(data));
            string result = Base32.Base32CrockFord.Encode(Encoding.UTF8.GetBytes(data));

            result.Should().Be(resultBase);
        }
    }
}
