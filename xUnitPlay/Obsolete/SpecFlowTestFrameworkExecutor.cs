using System.Collections.Generic;
using System.Reflection;
using xUnitPlay.Artifacts;
using xUnitPlay.Framework;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace xUnitPlay
{
    public class SpecFlowTestFrameworkExecutor : TestFrameworkExecutor<ScenarioTestCase>
    {
        public SpecFlowTestFrameworkExecutor(AssemblyName assemblyName, ISourceInformationProvider sourceInformationProvider, IMessageSink diagnosticMessageSink) : base(assemblyName, sourceInformationProvider, diagnosticMessageSink)
        {
            TestAssembly = new TestAssembly(AssemblyInfo, null, assemblyName.Version);
        }

        protected override ITestFrameworkDiscoverer CreateDiscoverer()
        {
            return new SpecFlowTestDiscoverer(AssemblyInfo, SourceInformationProvider, DiagnosticMessageSink);
        }

        protected TestAssembly TestAssembly { get; set; }

        protected override async void RunTestCases(IEnumerable<ScenarioTestCase> testCases, IMessageSink executionMessageSink,
            ITestFrameworkExecutionOptions executionOptions)
        {
            using (var assemblyRunner = new SpecFlowTestAssemblyRunner(TestAssembly, testCases, DiagnosticMessageSink, executionMessageSink, executionOptions))
                await assemblyRunner.RunAsync();
        }
    }
}