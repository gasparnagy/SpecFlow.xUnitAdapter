using System;
using BoDi;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.Tracing;
using Xunit.Abstractions;

namespace SpecFlow.xUnitAdapter.SpecFlowPlugin.Runners
{
    // This listener can be registered into the test thread container, so there will be a new instance
    // created for every execution thread. Therefore it does not have to support parallel scenario executions
    // so it is safe to access the ScenarioContext to get testOutputHelper.
    public class XUnitTraceListener : ITraceListener
    {
        private readonly Lazy<IContextManager> contextManager;

        public XUnitTraceListener(IObjectContainer testThreadContainer)
        {
            contextManager = new Lazy<IContextManager>(testThreadContainer.Resolve<IContextManager>);
        }

        private void Write(string message)
        {
            var scenarioContext = contextManager.Value.ScenarioContext;
            if (scenarioContext == null || !scenarioContext.ScenarioContainer.IsRegistered<ITestOutputHelper>())
            {
                Console.WriteLine(message); // fallback
                return;
            }
            scenarioContext.ScenarioContainer.Resolve<ITestOutputHelper>().WriteLine(message);
        }

        public void WriteTestOutput(string message)
        {
            Write(message);
        }

        public void WriteToolOutput(string message)
        {
            Write("-> " + message);
        }
    }
}
