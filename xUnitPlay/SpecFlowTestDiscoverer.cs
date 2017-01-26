using System.Linq;
using Gherkin.Ast;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace xUnitPlay
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
                foreach (var scenario in gherkinDocument.SpecFlowFeature.ScenarioDefinitions.OfType<Scenario>())
                {
                    if (!messageBus.QueueMessage(new TestCaseDiscoveryMessage(new ScenarioTestCase(featureFileTypeInfo, scenario.Name))))
                        return false;
                }
            }
            return true;
        }
    }
}