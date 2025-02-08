using System.Linq;
using FluentAssertions;

namespace QKP.EzId.Tests
{
    public class EzIdGeneratorTests
    {
        private readonly EzIdGenerator<EzId> _sut = new(12);

        [Fact]
        public void When_generating_it_must_return_expected()
        {
            EzId id = _sut.GetNextId();
            id.Should().NotBeNull();
        }

        [Fact]
        public void When_generating_multiple_ids_it_must_return_expected()
        {
            var ids = Enumerable.Range(0, 1000).Select(_ => _sut.GetNextId()).ToList();
            ids.Should().NotBeEmpty();
        }
    }
}
