using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CorDeGen.Library
{
    public class CompilationException : ArgumentException
    {
        internal CompilationException(IEnumerable<Diagnostic> failures) :
            base(string.Join(Environment.NewLine, failures.Select(x => $"{x.Id}: {x.GetMessage()}")))
        {
        }
    }
}
