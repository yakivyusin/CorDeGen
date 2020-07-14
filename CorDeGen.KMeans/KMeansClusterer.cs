using CorDeGen.KMeans.Model;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Linq;

namespace CorDeGen.KMeans
{
    public class KMeansClusterer
    {
        public ClusteredTextData[] Clusterize(FeaturizedTextData[] featurizedTexts, int clustersNumber)
        {
            var mlContext = new MLContext();
            var schemaDef = SchemaDefinition.Create(typeof(FeaturizedTextData));
            schemaDef["Features"].ColumnType = new VectorDataViewType(NumberDataViewType.Single, featurizedTexts[0].Features.Length);

            var trainingData = mlContext.Data.LoadFromEnumerable(featurizedTexts, schemaDef);
            var pipeline = mlContext.Clustering.Trainers.KMeans("Features", numberOfClusters: clustersNumber);

            var model = pipeline.Fit(trainingData);
            var outputData = model.Transform(trainingData);

            return mlContext.Data
                .CreateEnumerable<ClusteredTextData>(outputData, reuseRowObject: false)
                .ToArray();
        }
    }
}
