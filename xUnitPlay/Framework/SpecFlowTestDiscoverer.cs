using System;
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
            var gherkinDocument = SpecFlowParserHelper.ParseSpecFlowDocument(featureFileTestClass.FeatureFilePath);
            if (gherkinDocument.SpecFlowFeature != null)
            {
                featureFileTestClass.FeatureName = gherkinDocument.SpecFlowFeature.Name;
                var featureTags = gherkinDocument.SpecFlowFeature.Tags.GetTags().ToArray();
                foreach (var scenarioDefinition in gherkinDocument.SpecFlowFeature.ScenarioDefinitions.Where(sd => !(sd is Background)))
                {
                    var scenario = scenarioDefinition as Scenario;
                    if (scenario != null)
                    {
                        var scenarioTestCase = new ScenarioTestCase(featureFileTestClass, scenario, featureTags);
                        if (!messageBus.QueueMessage(new TestCaseDiscoveryMessage(scenarioTestCase)))
                            return false;
                    }
                    var scenarioOutline = scenarioDefinition as ScenarioOutline;
                    if (scenarioOutline != null)
                    {
                        int exampleRowId = 0;
                        foreach (var example in scenarioOutline.Examples)
                        {
                            foreach (var exampleRow in example.TableBody)
                            {
                                var parameters = example.TableHeader.Cells
                                    .Zip(exampleRow.Cells, (keyCell, valueCell) => new { Key = keyCell.Value, valueCell.Value })
                                    .ToDictionary(arg => arg.Key, arg => arg.Value);

                                var scenarioOutlineTestCase = new ScenarioTestCase(featureFileTestClass, scenarioOutline, featureTags, parameters, (++exampleRowId).ToString(), exampleRow.Location);
                                if (!messageBus.QueueMessage(new TestCaseDiscoveryMessage(scenarioOutlineTestCase)))
                                    return false;
                            }
                        }
                    }
                    
                }
            }
            return true;
        }
    }
}