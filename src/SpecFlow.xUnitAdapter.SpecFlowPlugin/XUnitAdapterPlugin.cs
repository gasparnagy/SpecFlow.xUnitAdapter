using System;
using SpecFlow.xUnitAdapter.SpecFlowPlugin;
using SpecFlow.xUnitAdapter.SpecFlowPlugin.Runners;
using TechTalk.SpecFlow.Plugins;
using TechTalk.SpecFlow.Tracing;

[assembly: RuntimePlugin(typeof(XUnitAdapterPlugin))]

namespace SpecFlow.xUnitAdapter.SpecFlowPlugin
{
    public class XUnitAdapterPlugin : IRuntimePlugin
    {
        public void Initialize(RuntimePluginEvents runtimePluginEvents, RuntimePluginParameters runtimePluginParameters)
        {
            runtimePluginEvents.CustomizeTestThreadDependencies += (sender, args) =>
            {
                args.ObjectContainer.RegisterTypeAs<XUnitTraceListener, ITraceListener>();
            };
        }
    }
}
