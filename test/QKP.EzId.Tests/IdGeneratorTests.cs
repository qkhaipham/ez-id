using FluentAssertions;

namespace QKP.EzId.Tests;

public class IdGeneratorTests
{
    private readonly IdGenerator _idGenerator = new(12);

    [Fact]
    public void When_generating_it_must_return_expected()
    {
        long id = _idGenerator.GetNextId();
        id.Should().BeGreaterThan(0);
    }

    [Fact]
    public void When_generating_multiple_ids_it_must_return_expected()
    {
        var ids = Enumerable.Range(0, 1000).Select(_ => _idGenerator.GetNextId()).ToList();
        ids.Should().NotBeEmpty();
    }
}
