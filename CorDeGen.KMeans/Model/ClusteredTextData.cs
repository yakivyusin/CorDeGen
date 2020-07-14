using Microsoft.ML.Data;

namespace CorDeGen.KMeans.Model
{
    public class ClusteredTextData : FeaturizedTextData
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedClusterId;

        [ColumnName("Score")]
        public float[] Distances;
    }
}
