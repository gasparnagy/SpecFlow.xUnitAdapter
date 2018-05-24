using System.Reflection;
using Xunit.Abstractions;

namespace SpecFlow.xUnitAdapter.SpecFlowPlugin.TestArtifacts
{
    public class SpecFlowProjectReflectionAssemblyInfo : SpecFlowProjectAssemblyInfo, IReflectionAssemblyInfo
    {
        public SpecFlowProjectReflectionAssemblyInfo()
        {
        }

        public SpecFlowProjectReflectionAssemblyInfo(IReflectionAssemblyInfo originalAssemblyInfo)
            : base(originalAssemblyInfo)
        {
        }

        public Assembly Assembly => ((IReflectionAssemblyInfo)originalAssemblyInfo).Assembly;
    }
}
