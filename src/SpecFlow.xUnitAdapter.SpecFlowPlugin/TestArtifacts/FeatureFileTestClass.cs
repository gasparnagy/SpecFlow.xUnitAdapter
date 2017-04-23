using System.IO;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Parser;

namespace SpecFlow.xUnitAdapter.SpecFlowPlugin.TestArtifacts
{
    public class FeatureFileTestClass : SpecFlowFeatureTestClass
    {
        public FeatureFileTestClass()
            : base()
        {
        }

        public FeatureFileTestClass(SpecFlowProjectAssemblyInfo specFlowProject, string relativePath)
            : base(specFlowProject, relativePath)
        {
        }

        //public override string FeatureFilePath => Path.Combine(SpecFlowProject.FeatureFilesFolder, RelativePath);

        public override SpecFlowDocument GetDocument()
        {
            var parser = SpecFlowParserHelper.CreateParser();
            var path = Path.Combine(SpecFlowProject.FeatureFilesFolder, RelativePath);

            using (var stream = File.OpenText(path))
            {
                var content = stream.ReadToEnd();
                var sourceMap = this.SpecFlowSourceMapper.ReadSourceMap(content);

                this.FeatureFilePath = sourceMap.SourcePath;

                return parser.Parse(new StringReader(content), this.FeatureFilePath);
            }
        }

        public override async Task<SpecFlowDocument> GetDocumentAsync()
        {
            var parser = SpecFlowParserHelper.CreateParser();
            var path = Path.Combine(SpecFlowProject.FeatureFilesFolder, RelativePath);

            using (var stream = File.OpenText(path))
            {
                var content = await stream.ReadToEndAsync();
                var sourceMap = this.SpecFlowSourceMapper.ReadSourceMap(content);

                this.FeatureFilePath = sourceMap.SourcePath;

                return parser.Parse(new StringReader(content), this.FeatureFilePath);
            }
        }
    }
}