using System;
using System.Linq;

public class DefaultPlugin
{
    private int _textCount;

    public int GetTextCount(int termCount)
    {
        _textCount = (int)Math.Pow(termCount, 0.25);
        return _textCount;
    }

    public int[] GetTermDistribution(int termIndex)
    {
        var arr = new int[_textCount];
        var range = (_textCount / 5) + 1;
        var centralText = termIndex % _textCount;
        var occurenceIndexes = Enumerable.Range(centralText - range, 2 * range + 1).Select(x => GetSafeIndex(x, arr));

        foreach (var occurenceIndex in occurenceIndexes)
        {
            arr[occurenceIndex] = occurenceIndex == centralText ? 2 : 1;
        }

        return arr;
    }

    private int GetSafeIndex(int index, Array array)
    {
        if (index < 0)
        {
            return array.Length + index;
        }

        if (index >= array.Length)
        {
            return index % array.Length;
        }

        return index;
    }
}