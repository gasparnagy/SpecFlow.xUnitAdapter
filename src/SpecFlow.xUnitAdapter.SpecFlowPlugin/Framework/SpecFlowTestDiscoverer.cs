using System;
using System.Linq;
using Gherkin.Ast;
using SpecFlow.xUnitAdapter.SpecFlowPlugin.TestArtifacts;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SpecFlow.xUnitAdapter.SpecFlowPlugin.Framework
{
    public class SpecFlowTestDiscoverer : XunitTestFrameworkDiscoverer
    {
        public SpecFlowTestDiscoverer(IAssemblyInfo assemblyInfo, ISourceInformationProvider sourceProvider, IMessageSink diagnosticMessageSink, IXunitTestCollectionFactory collectionFactory = null) :
            base(CreateSpecFlowProjectAssemblyInfo(assemblyInfo), sourceProvider, diagnosticMessageSink)
        {
        }

        private static SpecFlowProjectAssemblyInfo CreateSpecFlowProjectAssemblyInfo(IAssemblyInfo assemblyInfo)
        {
            if (assemblyInfo is IReflectionAssemblyInfo reflectionAssemblyInfo)
                return new SpecFlowProjectReflectionAssemblyInfo(reflectionAssemblyInfo);
            return new SpecFlowProjectAssemblyInfo(assemblyInfo);
        }

        private bool IsSpecFlowTest(object test)
        {
            return test is SpecFlowFeatureTestClass;
        }

        protected override ITestClass CreateTestClass(ITypeInfo typeInfo)
        {
            if (IsSpecFlowTest(typeInfo))
            {
                //TODO: return (ITestClass)new SpecFlowTestClass(this.TestCollectionFactory.Get(typeInfo), typeInfo);
                ((SpecFlowFeatureTestClass)typeInfo).Hack_SetTestCollection(this.TestCollectionFactory.Get(typeInfo));
                return (ITestClass) typeInfo;
            }
            return base.CreateTestClass(typeInfo);
        }

        protected override bool FindTestsForType(ITestClass testClass, bool includeSourceInformation, IMessageBus messageBus,
            ITestFrameworkDiscoveryOptions discoveryOptions)
        {
            if (!IsSpecFlowTest(testClass))
                return base.FindTestsForType(testClass, includeSourceInformation, messageBus, discoveryOptions);

            var featureTestClass = (SpecFlowFeatureTestClass)testClass;
            var gherkinDocument = featureTestClass.GetDocument();
            if (gherkinDocument?.SpecFlowFeature != null)
            {
                featureTestClass.FeatureName = gherkinDocument.SpecFlowFeature.Name;
                var featureTags = gherkinDocument.SpecFlowFeature.Tags.GetTags().ToArray();
                foreach (var scenarioDefinition in gherkinDocument.SpecFlowFeature.ScenarioDefinitions.Where(sd => !(sd is Background)))
                {
                    var scenario = scenarioDefinition as Scenario;
                    if (scenario != null)
                    {
                        var scenarioTestCase = new ScenarioTestCase(featureTestClass, scenario, featureTags);
                        if (!messageBus.QueueMessage(new TestCaseDiscoveryMessage(scenarioTestCase)))
                            return false;
                    }
                    var scenarioOutline = scenarioDefinition as ScenarioOutline;
                    if (scenarioOutline != null)
                    {
                        foreach (var example in scenarioOutline.Examples)
                        {
                            foreach (var exampleRow in example.TableBody)
                            {
                                var parameters = SpecFlowParserHelper.GetScenarioOutlineParameters(example, exampleRow);
                                var scenarioOutlineTestCase = new ScenarioTestCase(featureTestClass, scenarioOutline, featureTags, parameters, SpecFlowParserHelper.GetExampleRowId(scenarioOutline, exampleRow), exampleRow.Location);
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
