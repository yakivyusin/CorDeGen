using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using CorDeGen.Library;

namespace CorDeGen.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmark>();
        }
    }

    public class Benchmark
    {
        [Params(100, 1_000, 10_000, 100_000)]
        public int N { get; set; }

        [Benchmark]
        public void Generate()
        {
            var generator = new CorpusGenerator(N, new DynamicPlugin(new DefaultPlugin()));
            generator.GetCorpus();
        }
    }
}
