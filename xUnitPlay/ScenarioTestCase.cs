using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace xUnitPlay
{
    [Serializable]
    public class ScenarioTestCase : ITestCase, ITestMethod, IMethodInfo, IXunitTestCase, IReflectionMethodInfo
    {
        public FeatureFileTypeInfo FeatureFile => (FeatureFileTypeInfo)TestClass;

        public void Deserialize(IXunitSerializationInfo info)
        {
            throw new NotImplementedException();
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            throw new NotImplementedException();
        }

        public string DisplayName { get; set; }
        public string SkipReason { get; set; }
        public ISourceInformation SourceInformation { get; set; }
        public ITestMethod TestMethod { get { return this; } }
        public object[] TestMethodArguments { get; set; }
        public Dictionary<string, List<string>> Traits { get; set; }
        public string UniqueID { get; set; }

        public ScenarioTestCase(FeatureFileTypeInfo featureFileTypeInfo, string scenarioTitle)
        {
            TestClass = featureFileTypeInfo;
            Name = scenarioTitle;
            DisplayName = scenarioTitle;
            UniqueID = $"{featureFileTypeInfo.Name};{scenarioTitle}";
        }

        /// <inheritdoc/>
        public virtual Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink,
                                                 IMessageBus messageBus,
                                                 object[] constructorArguments,
                                                 ExceptionAggregator aggregator,
                                                 CancellationTokenSource cancellationTokenSource)
            => new ScenarioTestCaseRunner(this, messageBus, aggregator, cancellationTokenSource).RunAsync();

        public IMethodInfo Method { get { return this; } }
        public ITestClass TestClass { get; set; }
        public IEnumerable<IAttributeInfo> GetCustomAttributes(string assemblyQualifiedAttributeTypeName)
        {
            throw new NotImplementedException("GetCustomAttributes");
        }

        public IEnumerable<ITypeInfo> GetGenericArguments()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IParameterInfo> GetParameters()
        {
            throw new NotImplementedException();
        }

        public IMethodInfo MakeGenericMethod(params ITypeInfo[] typeArguments)
        {
            throw new NotImplementedException();
        }

        public bool IsAbstract { get; set; }
        public bool IsGenericMethodDefinition { get; set; }
        public bool IsPublic { get { return true; } }
        public bool IsStatic { get; set; }
        public string Name { get; set; }
        public ITypeInfo ReturnType { get; set; }
        public ITypeInfo Type { get { return TestClass.Class; } }
        public MethodInfo MethodInfo { get { throw new NotImplementedException("MethodInfo");} }
    }
}