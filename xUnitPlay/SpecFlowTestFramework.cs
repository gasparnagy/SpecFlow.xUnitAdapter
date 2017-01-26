using Xunit.Abstractions;
using Xunit.Sdk;

namespace xUnitPlay
{
    public class SpecFlowTestFramework : XunitTestFramework
    {
        public SpecFlowTestFramework(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink)
        {
        }

        protected override ITestFrameworkDiscoverer CreateDiscoverer(IAssemblyInfo assemblyInfo)
        {
            return new SpecFlowTestDiscoverer(assemblyInfo, SourceInformationProvider, DiagnosticMessageSink);
        }
    }
}