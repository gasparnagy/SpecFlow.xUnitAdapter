using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Parser;

namespace SpecFlow.xUnitAdapter.SpecFlowPlugin.TestArtifacts
{
    public class FeatureFileTestClass : SpecFlowFeatureTestClass
    {
        public FeatureFileTestClass()
        {
        }

        public FeatureFileTestClass(SpecFlowProjectAssemblyInfo specFlowProject, string relativePath)
            : base(specFlowProject, relativePath)
        {
        }

        public override SpecFlowDocument GetDocument()
        {
            var parser = SpecFlowParserHelper.CreateParser();
            var path = Path.Combine(SpecFlowProject.FeatureFilesFolder, RelativePath);

            using (var stream = File.OpenText(path))
            {
                var content = stream.ReadToEnd();
                return ParseDocument(content, path, parser);
            }
        }

        public override async Task<SpecFlowDocument> GetDocumentAsync()
        {
            var parser = SpecFlowParserHelper.CreateParser();
            var path = Path.Combine(SpecFlowProject.FeatureFilesFolder, RelativePath);

            using (var stream = File.OpenText(path))
            {
                var content = await stream.ReadToEndAsync();
                return ParseDocument(content, path, parser);
            }
        }
    }
}
