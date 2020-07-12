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
        [Params(100, 500, 2_500, 12_500, 62_500, 312_500)]
        public int N { get; set; }

        [Benchmark]
        public void Generate()
        {
            var generator = new CorpusGenerator(N, new DynamicPlugin(new DefaultPlugin()));
            generator.GetCorpus();
        }
    }
}
