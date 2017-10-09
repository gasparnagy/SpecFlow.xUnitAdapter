using System;
using Newtonsoft.Json;

namespace SpecFlow.xUnitAdapter.SpecFlowPlugin.TestArtifacts
{
    public interface ISpecFlowSourceMapper
    {
        /// <summary>
        /// Generates a source map line to add to a feature file.
        /// </summary>
        /// <param name="sourcePath">The path to the source feature file.</param>
        /// <returns>The source map line to add to the feature file.</returns>
        string GenerateSourceMap(string sourcePath);

        /// <summary>
        /// Reads a source map from a feature file.
        /// </summary>
        /// <param name="content">The contents of the feature file with source maps.</param>
        /// <returns>The source map data.</returns>
        SpecFlowSourceMap ReadSourceMap(string content);
    }

    public class SpecFlowSourceMapperV1 : ISpecFlowSourceMapper
    {
        private const int SourceMapVersion = 1;
        private const string SourceMapPattern = "#sourceMap=";

        public string GenerateSourceMap(string sourcePath)
        {
            var sourceMap = JsonConvert.SerializeObject(new SpecFlowSourceMap
            {
                Version = SourceMapVersion,
                SourcePath = sourcePath
            });
            
            return $"{SourceMapPattern}{sourceMap}{Environment.NewLine}";
        }

        public SpecFlowSourceMap ReadSourceMap(string content)
        {
            var index = content.IndexOf(SourceMapPattern);
            if (index == -1)
            {
                Console.WriteLine($"Could not find a source mapping.");
                return null;
            }

            var newLineIndex = content.IndexOf(Environment.NewLine, index);

            var sourceMap = newLineIndex == -1
                ? content.Substring(index + SourceMapPattern.Length)
                : content.Substring(index + SourceMapPattern.Length, newLineIndex - index - SourceMapPattern.Length);

            return JsonConvert.DeserializeObject<SpecFlowSourceMap>(sourceMap);
        }
    }
}
