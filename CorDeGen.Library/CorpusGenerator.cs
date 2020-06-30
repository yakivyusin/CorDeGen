using System;
using System.Linq;
using System.Text;

namespace CorDeGen.Library
{
    public class CorpusGenerator
    {
        private readonly int _termCount;
        private readonly int _textCount;
        private readonly DynamicPlugin _dynamicPlugin;
        private readonly Random _random;

        public CorpusGenerator(int termCount, DynamicPlugin dynamicPlugin)
        {
            _termCount = termCount;
            _textCount = dynamicPlugin.GetTextCount(termCount);
            _dynamicPlugin = dynamicPlugin;
            _random = new Random();
        }

        public string[] GetCorpus()
        {
            var texts = Enumerable
                .Range(0, _textCount)
                .Select(x => new StringBuilder())
                .ToArray();

            for (int i = 0; i < _termCount; i++)
            {
                var term = Convert.ToString(i, 16);
                var distribution = _dynamicPlugin.GetTermDistribution(i);

                WriteTerm(texts, distribution, term, i);
            }

            return texts
                .Select(x => x.ToString())
                .ToArray();
        }

        private void WriteTerm(StringBuilder[] texts, int[] distribution, string term, int index)
        {
            var totalProbability = distribution.Sum();
            var totalCount = _textCount * ((index % _textCount) + 1);

            for (int i = 0; i < distribution.Length; i++)
            {
                var count = totalCount * ((double)distribution[i] / totalProbability);

                for (int j = 0; j < count; j++)
                {
                    texts[i].Append(term);
                    texts[i].Append(" ");

                    if (_random.Next(10) == 0)
                    {
                        texts[i].AppendLine();
                    }
                }
            }
        }
    }
}
