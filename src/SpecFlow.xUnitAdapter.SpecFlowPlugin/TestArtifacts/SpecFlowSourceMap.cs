using System;
using Newtonsoft.Json;

namespace SpecFlow.xUnitAdapter.SpecFlowPlugin.TestArtifacts
{

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
