using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Loader;

namespace CorDeGen.Library
{
    public static class DynamicPluginLoader
    {
        public static DynamicPlugin Load(FileInfo fileInfo)
        {
            var sourceCode = File.ReadAllText(fileInfo.FullName);

            using var pluginStream = new MemoryStream();
            var result = GenerateCode(sourceCode).Emit(pluginStream);

            if (!result.Success)
            {
                var failures = result.Diagnostics
                    .Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);

                throw new CompilationException(failures);
            }

            pluginStream.Seek(0, SeekOrigin.Begin);

            var assemblyLoadContext = new AssemblyLoadContext("dynamic plugin");
            var assembly = assemblyLoadContext.LoadFromStream(pluginStream);
            var pluginType = assembly.GetExportedTypes()[0];

            return new DynamicPlugin(Activator.CreateInstance(pluginType));
        }

        private static CSharpCompilation GenerateCode(string sourceCode)
        {
            var codeString = SourceText.From(sourceCode);
            var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest);

            var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(codeString, options);

            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location)
            };

            return CSharpCompilation.Create("DynamicPlugin.dll",
                new[] { parsedSyntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Release,
                    assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default));
        }
    }
}
