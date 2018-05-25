using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Parser;

namespace SpecFlow.xUnitAdapter.SpecFlowPlugin.TestArtifacts
{
    public class EmbeddedFeatureTypeInfo : SpecFlowFeatureTypeInfo
    {
        public EmbeddedFeatureTypeInfo()
        {
        }

        public EmbeddedFeatureTypeInfo(SpecFlowProjectAssemblyInfo specFlowProject, string resourceName)
            : base(specFlowProject, resourceName)
        {
        }

        public override SpecFlowDocument GetDocument()
        {
            var parser = SpecFlowParserHelper.CreateParser();

            var assembly = Assembly.ReflectionOnlyLoadFrom(this.SpecFlowProject.AssemblyPath);

            using (var stream = assembly.GetManifestResourceStream(this.RelativePath))
            using (var reader = new StreamReader(stream))
            {
                var content = reader.ReadToEnd();
                return ParseDocument(content, null, parser);
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
                return ParseDocument(content, null, parser);
            }
        }
    }
}
