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

        public void Deserialize(IXunitSerializationInfo data)
        {
            TestClass = data.GetValue<FeatureFileTypeInfo>("TestClass");
            Name = data.GetValue<string>("Name");
        }

        public void Serialize(IXunitSerializationInfo data)
        {
            data.AddValue("TestClass", TestClass);
            data.AddValue("Name", Name);
        }

        public string DisplayName => Name;
        public string SkipReason { get; set; }
        public ISourceInformation SourceInformation { get; set; }
        public ITestMethod TestMethod { get { return this; } }
        public object[] TestMethodArguments { get; set; }
        public Dictionary<string, List<string>> Traits { get; set; }
        public string UniqueID => $"{FeatureFile.Name};{Name}";

        public ScenarioTestCase()
        {
            
        }

        public ScenarioTestCase(FeatureFileTypeInfo featureFileTypeInfo, string scenarioTitle)
        {
            TestClass = featureFileTypeInfo;
            Name = scenarioTitle;
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
            throw new NotImplementedException("GetCustomAttributes.GetCustomAttributes");
        }

        public IEnumerable<ITypeInfo> GetGenericArguments()
        {
            throw new NotImplementedException("ScenarioTestCase.GetGenericArguments");
        }

        public IEnumerable<IParameterInfo> GetParameters()
        {
            throw new NotImplementedException("ScenarioTestCase.GetParameters");
        }

        public IMethodInfo MakeGenericMethod(params ITypeInfo[] typeArguments)
        {
            throw new NotImplementedException("ScenarioTestCase.MakeGenericMethod");
        }

        public bool IsAbstract { get; set; }
        public bool IsGenericMethodDefinition { get; set; }
        public bool IsPublic { get { return true; } }
        public bool IsStatic { get; set; }
        public string Name { get; set; }
        public ITypeInfo ReturnType { get; set; }
        public ITypeInfo Type { get { return TestClass.Class; } }
        public MethodInfo MethodInfo { get { throw new NotImplementedException("ScenarioTestCase.MethodInfo");} }
    }
}