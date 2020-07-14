using CorDeGen.KMeans.Model;
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
                    Text = File.ReadAllText(file.FullName)
                })
                .ToArray();

            var featurizer = new TextFeaturizer();
            var clusterer = new KMeansClusterer();

            var featurizedData = featurizer.Featurize(texts);
            var clusteredData = clusterer.Clusterize(featurizedData, 2);
        }
    }
}
