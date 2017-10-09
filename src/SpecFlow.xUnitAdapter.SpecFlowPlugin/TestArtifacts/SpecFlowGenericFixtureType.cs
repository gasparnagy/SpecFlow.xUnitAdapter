using System;
using System.Linq;
using Xunit.Abstractions;

namespace SpecFlow.xUnitAdapter.SpecFlowPlugin.TestArtifacts
{
    public class SpecFlowGenericFixtureType
    {
        // This constructor is used to determine the constructor arguments of the scenario
        // since the execution requires the ITestOutputHelper, the parameter is essential.
        // The ITestOutputHelper will be passed in by the runners via ScenarioTestCase.RunAsync.
        public SpecFlowGenericFixtureType(ITestOutputHelper testOutputHelper)
        {
        }
    }
}
