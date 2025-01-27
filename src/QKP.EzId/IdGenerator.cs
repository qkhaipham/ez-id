namespace QKP.EzId;
/// <summary>
/// A predictable 64 bits snowflake inspired ID generator.
///
/// The first bit is not used.
/// The next 41 bits are used to store the timestamp in milliseconds since <see cref="s_epoch"/>.
/// The next 10 bits ( max 1024 unique generators ) are used to store the generatorId.
/// And the last 12 bits ( max 4096 ) are used to store the sequence.
/// </summary>
public class IdGenerator
{
    /// <summary>
    /// The generator identifier.
    ///
    /// Must be an unique identifier across all processes that can generate identifiers concurrently.
    /// This can be unique for example by using machine name or for each thread depending on the use-case.
    /// </summary>
    public long GeneratorId { get; }

    private const int ShiftByForTimestamp = GeneratorIdBits + SequenceBits;
    private const int GeneratorIdBits = 10;
    private const int SequenceBits = 12;
    private const int MaxSequence = 1 << SequenceBits;
    private const int MaxGeneratorId = 1 << GeneratorIdBits;
    private readonly Lock _lockObject = new();
    private long _sequence;
    private long _lastTick = GetTick();

    /// <summary>
    /// Constructs an instance of <see cref="IdGenerator"/>.
    /// </summary>
    /// <param name="generatorId">The generator ID which must be a unique identifier for each concurrent processor.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the generator ID is out of range.</exception>
    public IdGenerator(long generatorId)
    {
        if (generatorId > MaxGeneratorId)
        {
            throw new ArgumentOutOfRangeException(nameof(generatorId), generatorId, $"Generator ID must be less than {MaxGeneratorId}.");
        }

        GeneratorId = generatorId;
    }

    private static readonly DateTimeOffset s_epoch = new(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

    /// <summary>
    /// Gets the next identifier.
    /// </summary>
    /// <returns>A 64 bit incremented identifier represented as <see cref="long"/>.</returns>
    /// <exception cref="DayLightSavingChangedException">Thrown when clock moved backwards.</exception>
    public long GetNextId()
    {
        lock (_lockObject)
        {
            long now = GetTick();
            if (now > _lastTick)
            {
                _lastTick = now;
                _sequence = 0;
            }
            else if (now < _lastTick)
            {
                // clock went backwards
                throw new DayLightSavingChangedException($"Clock changed daylight saving time. Time now:{now} - Last generated:{_lastTick}");
            }

            if (_sequence == MaxSequence)
            {
                SpinWait.SpinUntil(() => _lastTick < GetTick());
                return GetNextId();
            }

            _sequence++;
            return _lastTick << ShiftByForTimestamp | (GeneratorId << GeneratorIdBits) | _sequence;
        }
    }

    private static long GetTick()
    {
        return (long)(DateTimeOffset.UtcNow - s_epoch).TotalMilliseconds;
    }
}
