using SpecFlow.xUnitAdapter.SpecFlowPlugin.Framework;
using Xunit.Abstractions;
using Xunit.Sdk;
using System.Runtime.Loader;
using System.IO;
using System;

namespace SpecFlow.xUnitAdapter.SpecFlowPlugin
{
    public class SpecFlowTestFramework : XunitTestFramework
    {
        public SpecFlowTestFramework(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink)
        {
        }

        private string GetAssemblyFolder(string assemblyPath)
        {
            var relativePath = Path.GetDirectoryName(assemblyPath);
            if (string.IsNullOrEmpty(relativePath))
                relativePath = ".";
            return Path.GetFullPath(relativePath);
        }

        protected override ITestFrameworkDiscoverer CreateDiscoverer(IAssemblyInfo assemblyInfo)
        {
            string assemblyFolder = GetAssemblyFolder(assemblyInfo.AssemblyPath);
            string featureFilesFolder = assemblyFolder;

            AssemblyLoadContext.Default.Resolving += (ctx, an) =>
            {
                var assemblyPath = Path.Combine(assemblyFolder, an.Name + ".dll");
                if (File.Exists(assemblyPath))
                {
                    try
                    {
                        return AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
                return null;
            };

            return new SpecFlowTestDiscoverer(assemblyInfo, featureFilesFolder, SourceInformationProvider, DiagnosticMessageSink);
        }

        //protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName)
        //{
        //    return new SpecFlowTestFrameworkExecutor(assemblyName, SourceInformationProvider, DiagnosticMessageSink);
        //}
    }
}