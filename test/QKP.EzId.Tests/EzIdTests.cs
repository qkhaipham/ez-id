using FluentAssertions;

namespace QKP.EzId.Tests
{
    public class EzIdTests
    {
        [Theory]
        [InlineData(123456789, "2Q6NP1R000000")]
        [InlineData(987654321, "P5MDWEG000000")]
        [InlineData(long.MaxValue, "ZZZZZZZZZZZQY")]
        [InlineData(long.MinValue, "0000000000080")]
        public void Given_string_when_encoding_it_must_return_expected(long value, string expected)
        {
            var ezId = new EzId(value);
            ezId.Value.Should().Be(value);
            ezId.ToString().Should().Be(expected);
        }

        [Fact]
        public void Given_string_when_parsing_it_must_return_expected()
        {
            var ezId = new EzId(124567890);
            string strVal = ezId.ToString();
            EzId.Parse(strVal).Should().Be(ezId);
        }
    }
}
