using CorDeGen.KMeans.Model;
using Microsoft.ML;
using Microsoft.ML.Transforms.Text;
using System.Linq;

namespace CorDeGen.KMeans
{
    public class TextFeaturizer
    {
        public FeaturizedTextData[] Featurize(TextData[] texts)
        {
            var mlContext = new MLContext();
            var dataView = mlContext.Data.LoadFromEnumerable(texts);

            var options = new TextFeaturizingEstimator.Options
            {
                CaseMode = TextNormalizingEstimator.CaseMode.Lower,
                WordFeatureExtractor = new WordBagEstimator.Options
                {
                    NgramLength = 1,
                    UseAllLengths = true
                },
                CharFeatureExtractor = null,
                KeepDiacritics = false,
                KeepPunctuations = false,
                KeepNumbers = true
            };

            var textPipeline = mlContext.Transforms.Text.FeaturizeText("Features", options, "Text");
            var textTransformer = textPipeline.Fit(dataView);
            var featurizedData = textTransformer.Transform(dataView);
            
            return mlContext.Data
                .CreateEnumerable<FeaturizedTextData>(featurizedData, reuseRowObject: false)
                .ToArray();
        }
    }
}
