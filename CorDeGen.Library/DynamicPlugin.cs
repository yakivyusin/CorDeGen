using System;
using System.Reflection;

namespace CorDeGen.Library
{
    public class DynamicPlugin
    {
        private delegate int TextCountCalculation(int termCount);

        private TextCountCalculation _textCountCalculation;

        public DynamicPlugin(object obj) => SetupDelegates(obj);

        public int GetTextCount(int termCount) => _textCountCalculation(termCount);

        private void SetupDelegates(object obj)
        {
            var type = obj.GetType();
            var textCountMethod = type.GetMethod("GetTextCount", BindingFlags.Public | BindingFlags.Instance);

            _textCountCalculation = (TextCountCalculation)Delegate.CreateDelegate(typeof(TextCountCalculation), obj, textCountMethod);
        }
    }
}
