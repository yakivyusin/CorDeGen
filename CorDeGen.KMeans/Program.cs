using CorDeGen.KMeans.Model;
using System;
using System.IO;
using System.Linq;

namespace CorDeGen.KMeans
{
    class Program
    {
        static void Main(DirectoryInfo textsDirectory)
        {
            var texts = textsDirectory.GetFiles("*.txt")
                .OrderBy(file => int.Parse(Path.GetFileNameWithoutExtension(file.Name)))
                .Select(file => new TextData
                {
                    FileName = file.Name,
                    Text = File.ReadAllText(file.FullName)
                })
                .ToArray();

            var featurizer = new TextFeaturizer();
            var clusterer = new KMeansClusterer();

            var featurizedData = featurizer.Featurize(texts);

#if BUG
            for (int i = 0; i < featurizedData[0].Features.Length; i++)
            {
                featurizedData[0].Features[i] = 1f;
            }
#endif

            var clusteredData = clusterer.Clusterize(featurizedData, 2);

            textsDirectory.CreateSubdirectory("result");

            using var resultFile = File.CreateText(Path.Combine(textsDirectory.FullName, $"result\\{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.txt"));
            foreach (var text in clusteredData)
            {
                resultFile.WriteLine($"{text.FileName} | {text.PredictedClusterId}");
            }
        }
    }
}
