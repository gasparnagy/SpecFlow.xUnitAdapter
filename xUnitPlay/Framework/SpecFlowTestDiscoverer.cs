using System.Linq;
using Gherkin.Ast;
using xUnitPlay.Artifacts;
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

        protected override ITestClass CreateTestClass(ITypeInfo @class)
        {
            return (ITestClass)@class;
        }

        protected override bool FindTestsForType(ITestClass testClass, bool includeSourceInformation, IMessageBus messageBus,
            ITestFrameworkDiscoveryOptions discoveryOptions)
        {
            var featureFileTypeInfo = (FeatureFileTypeInfo)testClass;
            var gherkinDocument = SpecFlowParserHelper.ParseSpecFlowDocument(featureFileTypeInfo.FeatureFilePath).Result;
            if (gherkinDocument.SpecFlowFeature != null)
            {
                var featureTags = gherkinDocument.SpecFlowFeature.Tags.GetTags().ToArray();
                foreach (var scenario in gherkinDocument.SpecFlowFeature.ScenarioDefinitions.OfType<Scenario>())
                {
                    var scenarioTestCase = new ScenarioTestCase(featureFileTypeInfo, scenario, featureTags);
                    if (!messageBus.QueueMessage(new TestCaseDiscoveryMessage(scenarioTestCase)))
                        return false;
                }
            }
            return true;
        }
    }
}