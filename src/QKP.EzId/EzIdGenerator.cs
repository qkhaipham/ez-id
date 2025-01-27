namespace QKP.EzId;

/// <summary>
/// A
/// </summary>
/// <param name="generatorId"></param>
/// <typeparam name="T"></typeparam>
public class EzIdGenerator<T>(long generatorId) where T : EzId
{
    private readonly IdGenerator _generator = new(generatorId);

    public virtual T GetNextId()
    {
        return (T)Activator.CreateInstance(typeof(T), _generator.GetNextId())! ?? throw new InvalidOperationException($"Could not construct type {typeof(T).FullName}.");
    }
}
