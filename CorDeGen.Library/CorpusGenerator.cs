using System.Linq;
using System.Text;

namespace CorDeGen.Library
{
    public class CorpusGenerator
    {
        private readonly int _termCount;
        private readonly int _textCount;

        public CorpusGenerator(int termCount, DynamicPlugin dynamicPlugin)
        {
            _termCount = termCount;
            _textCount = dynamicPlugin.GetTextCount(termCount);
        }

        public string[] GetCorpus()
        {
            var texts = Enumerable
                .Range(0, _textCount)
                .Select(x => new StringBuilder())
                .ToArray();

            return texts
                .Select(x => x.ToString())
                .ToArray();
        }
    }
}
