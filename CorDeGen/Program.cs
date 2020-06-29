using CorDeGen.Library;
using System;
using System.IO;

namespace CorDeGen
{
    class Program
    {
        static void Main(int termCount, FileInfo plugin, DirectoryInfo outputDirectory)
        {
            if (termCount == 0)
            {
                Console.WriteLine("--term-count is required");
                return;
            }

            if (plugin == null || !plugin.Exists)
            {
                Console.WriteLine("--plugin option was not provided or such file does not exist. Default will be used");
                plugin = new FileInfo("Default.cs");
            }

            if (outputDirectory == null || !outputDirectory.Exists)
            {
                Console.WriteLine("--output-directory option was not provided or such directory does not exist. Current directory will be used");
                outputDirectory = new DirectoryInfo(".");
            }    

            var compiledPlugin = DynamicPluginLoader.Load(plugin);
            var corpusGenerator = new CorpusGenerator(termCount, compiledPlugin);
            var corpus = corpusGenerator.GetCorpus();

            for (int i = 0; i < corpus.Length; i++)
            {
                File.WriteAllText(
                    Path.Combine(outputDirectory.FullName, $"{i}.txt"),
                    corpus[i]);
            }
        }
    }
}
