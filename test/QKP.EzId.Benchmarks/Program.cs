using BenchmarkDotNet.Running;

namespace QKP.EzId.Benchmarks;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<IdGeneratorBenchmarks>();
    }
}
