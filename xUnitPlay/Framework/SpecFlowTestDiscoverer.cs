using System.Linq;
using System.Threading.Tasks;
using Gherkin.Ast;
using TechTalk.SpecFlow.Parser;
using xUnitPlay.TestArtifacts;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace xUnitPlay.Framework
{
    public class SpecFlowTestDiscoverer : TestFrameworkDiscoverer
    {
        public SpecFlowTestDiscoverer(IAssemblyInfo assemblyInfo, ISourceInformationProvider sourceProvider, IMessageSink diagnosticMessageSink) : 
            base(new SpecFlowProjectAssemblyInfo(assemblyInfo), sourceProvider, diagnosticMessageSink)
        {
        }

        protected override ITestClass CreateTestClass(ITypeInfo typeInfo)
        {
            return (ITestClass)typeInfo;
        }

        protected override bool FindTestsForType(ITestClass testClass, bool includeSourceInformation, IMessageBus messageBus,
            ITestFrameworkDiscoveryOptions discoveryOptions)
        {
            var featureFileTestClass = (FeatureFileTestClass)testClass;
            var gherkinDocument = SpecFlowParserHelper.ParseSpecFlowDocument(featureFileTestClass.FeatureFilePath).Result;
            if (gherkinDocument.SpecFlowFeature != null)
            {
                featureFileTestClass.FeatureName = gherkinDocument.SpecFlowFeature.Name;
                var featureTags = gherkinDocument.SpecFlowFeature.Tags.GetTags().ToArray();
                foreach (var scenario in gherkinDocument.SpecFlowFeature.ScenarioDefinitions.OfType<Scenario>())
                {
                    var scenarioTestCase = new ScenarioTestCase(featureFileTestClass, scenario, featureTags);
                    if (!messageBus.QueueMessage(new TestCaseDiscoveryMessage(scenarioTestCase)))
                        return false;
                }
            }
            return true;
        }
    }
}