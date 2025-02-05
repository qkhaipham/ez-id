using FluentAssertions;
using System.Collections.Concurrent;

namespace QKP.EzId.Tests;

public class IdGeneratorTests
{
    private readonly IdGenerator _idGenerator = new(12);

    [Fact]
    public void When_generating_it_must_return_expected()
    {
        // ACT
        long id = _idGenerator.GetNextId();

        // ASSERT
        id.Should().BeGreaterThan(0);
    }

    [Fact]
    public void When_generating_multiple_ids_it_must_return_expected()
    {
        // ACT
        var ids = Enumerable.Range(0, 1000).Select(_ => _idGenerator.GetNextId()).ToList();

        // ASSERT
        ids.Should().NotBeEmpty();
        ids.Should().OnlyHaveUniqueItems();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1023)]
    [InlineData(1024)]
    [InlineData(1025)]
    public void When_generator_id_is_out_of_range_it_must_throw_exception(long generatorId)
    {
        // ACT
        Action act = () => new IdGenerator(generatorId);

        // ASSERT
        if (generatorId < 0 || generatorId >= 1024)
        {
            act.Should().Throw<ArgumentOutOfRangeException>();
        }
        else
        {
            act.Should().NotThrow();
        }
    }

    [Fact]
    public void When_clock_moves_backwards_it_must_throw_exception()
    {
        // arrange
        var idGenerator = new IdGenerator(1);
        typeof(IdGenerator).GetField("_lastTick", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(idGenerator, IdGenerator.GetTick() + 1000);

        // ACT
        Action act = () => idGenerator.GetNextId();

        // ASSERT
        act.Should().Throw<DayLightSavingChangedException>();
    }

    [Fact]
    public void When_generating_max_sequence_in_same_millisecond_it_must_handle_correctly()
    {
        // ARRANGE
        var idGenerator = new IdGenerator(1);
        var ids = new HashSet<long>();

        // ACT & ASSERT
        // Generate 4096 IDs (max sequence number) + 1 more
        for (int i = 0; i < 4097; i++)
        {
            var id = idGenerator.GetNextId();
            ids.Add(id).Should().BeTrue("All IDs should be unique");
        }
        
            var firstTimestamp = firstId >> (10 + 12); // 10 for GeneratorId, 12 for Sequence
            var lastTimestamp = lastId >> (10 + 12);
            lastTimestamp.Should().BeGreaterThan(firstTimestamp, "Last ID should have a newer timestamp");        I
    }

    [Fact]
    public void When_generating_ids_concurrently_they_must_be_unique()
    {
        // ARRANGE
        const int numThreads = 4;
        const int idsPerThread = 1000;
        var idGenerator = new IdGenerator(1);
        var ids = new ConcurrentBag<long>();
        var tasks = new List<Task>();

        // ACT
        for (int i = 0; i < numThreads; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                for (int j = 0; j < idsPerThread; j++)
                {
                    ids.Add(idGenerator.GetNextId());
                }
            }));
        }
        Task.WaitAll(tasks.ToArray());

        // ASSERT
        ids.Should().HaveCount(numThreads * idsPerThread);
        ids.Distinct().Should().HaveCount(numThreads * idsPerThread);
    }

    [Fact]
    public void When_generating_ids_bit_structure_must_be_correct()
    {
        // ARRANGE
        const long generatorId = 123;
        var idGenerator = new IdGenerator(generatorId);

        // ACT
        long id = idGenerator.GetNextId();

        // ASSERT
        // Extract components using bit operations
        long timestamp = (id >> 22); // 41 bits for timestamp (after first unused bit)
        long extractedGeneratorId = (id >> 12) & 0x3FF; // 10 bits for generator ID
        long sequence = id & 0xFFF; // 12 bits for sequence

        extractedGeneratorId.Should().Be(generatorId);
        sequence.Should().Be(1); // First sequence number
        timestamp.Should().BeGreaterThan(0);
        
        // Verify first bit is unused (should be 0)
        ((id >> 63) & 1).Should().Be(0);
    }

    [Fact]
    public void When_multiple_generators_run_concurrently_ids_must_be_unique()
    {
        // ARRANGE
        const int numGenerators = 4;
        const int idsPerGenerator = 1000;
        var generators = Enumerable.Range(0, numGenerators)
            .Select(i => new IdGenerator(i))
            .ToList();
        var ids = new ConcurrentBag<long>();
        var tasks = new List<Task>();

        // ACT
        foreach (var generator in generators)
        {
            tasks.Add(Task.Run(() =>
            {
                for (int i = 0; i < idsPerGenerator; i++)
                {
                    ids.Add(generator.GetNextId());
                }
            }));
        }
        Task.WaitAll(tasks.ToArray());

        // ASSERT
        ids.Should().HaveCount(numGenerators * idsPerGenerator);
        ids.Distinct().Should().HaveCount(numGenerators * idsPerGenerator);
    }
}
