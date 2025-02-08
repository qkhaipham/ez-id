using FluentAssertions;

namespace QKP.EzId.Tests
{
    public class EzIdTests
    {
        [Fact]
        public void Given_string_when_encoding_it_must_return_expected()
        {
            var ezId = new EzId(1);
            ezId.Value.Should().Be(1);
            ezId.ToString().Should().Be("0400000000000");
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
