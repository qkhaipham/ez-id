using FluentAssertions;

namespace QKP.EzId.Tests
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class FooId(long value) : EzId(value);

    public class EzIdGeneratorTests
    {
        private readonly EzIdGenerator<EzId> _sut = new(12);

        [Fact]
        public void Given_ez_id_type_when_generating_it_must_return_expected()
        {
            EzId id = _sut.GetNextId();
            id.Should().NotBeNull();
        }

        [Fact]
        public void Given_derived_ez_id_type_when_generating_it_must_return_expected()
        {
            var sut = new EzIdGenerator<FooId>(12);
            FooId id = sut.GetNextId();
            id.Should().NotBeNull();
        }
    }
}
