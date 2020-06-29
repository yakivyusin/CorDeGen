using System;

public class DefaultPlugin
{
    public int GetTextCount(int termCount) => (int)Math.Pow(termCount, 0.25);
}