using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using SpecFlow.xUnitAdapter.SpecFlowPlugin.TestArtifacts;

namespace SpecFlow.xUnitAdapter.Build
{
    /// <summary>
    /// MSBuild task that applies source maps to SpecFlow feature files.
    /// </summary>
    public class SpecFlowSourceMapAppender : AppDomainIsolatedTask
    {
        private readonly ISpecFlowSourceMapper sourceMapper = new SpecFlowSourceMapperV1();

        /// <summary>
        /// Gets or sets the directory that the source mapped feature files should be output.
        /// This is usually going to be obj/SpecFlow for files that will be embedded and
        /// the target directory for non-embedded files.
        /// </summary>
        [Required]
        public string OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets the collection of feature files being transformed with source map info.
        /// </summary>
        [Required]
        public ITaskItem[] FeatureFiles { get; set; }

        /// <summary>
        /// Gets or sets the output files that have had source maps applied to them.
        /// </summary>
        [Output]
        public ITaskItem[] SourceMappedFiles { get; set; }

        public override bool Execute()
        {
            if (this.FeatureFiles.Length <= 0)
            {
                return true;
            }

            SourceMappedFiles = new TaskItem[this.FeatureFiles.Length];

            Log.LogMessage(MessageImportance.Normal, "Appending SpecFlow SourceMaps:");

            for (var i = 0; i < this.FeatureFiles.Length; i++)
            {
                var item = this.FeatureFiles[i];

                this.SourceMappedFiles[i] = this.AppendSourceMap(item);
            }

            return !this.Log.HasLoggedErrors;
        }

        private ITaskItem AppendSourceMap(ITaskItem item)
        {
            var outputPath = Path.Combine(OutputDirectory, item.ToString());

            this.Log.LogMessage(MessageImportance.Normal, $"    {outputPath}");

            var inputPath = item.GetMetadata("FullPath");
            var outputDirectory = Path.GetDirectoryName(outputPath);

            if (!Directory.Exists(outputDirectory))
            {
                this.Log.LogMessage(MessageImportance.Low, $"Creating directory: {outputDirectory}");
                Directory.CreateDirectory(outputDirectory);
            }
            
            //TODO: If the file modified dates havent changes, consider not copying and appending to the file, and use the existing one.
            File.Copy(inputPath, outputPath, true);
            File.AppendAllLines(outputPath, new[]
            {
                Environment.NewLine,
                this.sourceMapper.GenerateSourceMap(outputPath)
            });

            return new TaskItem(outputPath);
        }
    }

    public class SpecFlowSourceMap
    {
        /// <summary>
        /// Gets or sets the version number that the source map is based off of.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the source file's path.
        /// </summary>
        public string SourcePath { get; set; }
    }
}
