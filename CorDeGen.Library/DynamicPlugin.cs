using System;
using System.Reflection;

namespace CorDeGen.Library
{
    public class DynamicPlugin
    {
        private delegate int TextCountCalculation(int termCount);
        private delegate int[] TermDistributionCalculation(int termIndex);

        private TextCountCalculation _textCountCalculation;
        private TermDistributionCalculation _termDistributionCalculation;

        public DynamicPlugin(object obj) => SetupDelegates(obj);

        public int GetTextCount(int termCount) => _textCountCalculation(termCount);

        public int[] GetTermDistribution(int termIndex) => _termDistributionCalculation(termIndex);

        private void SetupDelegates(object obj)
        {
            var type = obj.GetType();
            var textCountMethod = type.GetMethod("GetTextCount", BindingFlags.Public | BindingFlags.Instance);
            var termDistributionMethod = type.GetMethod("GetTermDistribution", BindingFlags.Public | BindingFlags.Instance);

            _textCountCalculation = (TextCountCalculation)Delegate.CreateDelegate(typeof(TextCountCalculation), obj, textCountMethod);
            _termDistributionCalculation = (TermDistributionCalculation)Delegate.CreateDelegate(typeof(TermDistributionCalculation), obj, termDistributionMethod);
        }
    }
}
