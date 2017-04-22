using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Parser;

namespace SpecFlow.xUnitAdapter.SpecFlowPlugin.TestArtifacts
{
    public class EmbeddedFeatureFileTestClass : SpecFlowFeatureTestClass
    {
        public EmbeddedFeatureFileTestClass()
            : base()
        {
        }

        public EmbeddedFeatureFileTestClass(SpecFlowProjectAssemblyInfo specFlowProject, string resourceName)
            : base(specFlowProject, resourceName)
        {
        }

        public override string FeatureFilePath => Path.Combine(SpecFlowProject.FeatureFilesFolder, RelativePath);

        public override SpecFlowDocument GetDocument()
        {
            var parser = SpecFlowParserHelper.CreateParser();

            var assembly = Assembly.ReflectionOnlyLoadFrom(this.SpecFlowProject.AssemblyPath);

            using (var stream = assembly.GetManifestResourceStream(this.RelativePath))
            using (var reader = new StreamReader(stream))
            {
                var content = reader.ReadToEnd();

                return parser.Parse(new StringReader(content), this.RelativePath);
            }
        }

        public override async Task<SpecFlowDocument> GetDocumentAsync()
        {
            var parser = SpecFlowParserHelper.CreateParser();

            var assembly = Assembly.ReflectionOnlyLoadFrom(this.SpecFlowProject.AssemblyPath);

            using (var stream = assembly.GetManifestResourceStream(this.RelativePath))
            using (var reader = new StreamReader(stream))
            {
                var content = await reader.ReadToEndAsync();

                return parser.Parse(new StringReader(content), this.RelativePath);
            }
        }
    }
}