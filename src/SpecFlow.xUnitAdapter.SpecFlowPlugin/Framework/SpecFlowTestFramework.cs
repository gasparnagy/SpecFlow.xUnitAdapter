using SpecFlow.xUnitAdapter.SpecFlowPlugin.Framework;
using Xunit.Abstractions;
using Xunit.Sdk;

// ReSharper disable once CheckNamespace
namespace SpecFlow.xUnitAdapter.SpecFlowPlugin
{
    public class SpecFlowTestFramework : XunitTestFramework
    {
        public SpecFlowTestFramework(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink)
        {
        }

        // we only override the discoverer, we can use the built-in executor as it is anyway delegates the actual exectution to the test cases (ScenarioTestCase)
        protected override ITestFrameworkDiscoverer CreateDiscoverer(IAssemblyInfo assemblyInfo)
        {
            return new SpecFlowTestDiscoverer(assemblyInfo, SourceInformationProvider, DiagnosticMessageSink);
        }
    }
}
