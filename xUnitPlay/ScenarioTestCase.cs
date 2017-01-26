using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Gherkin.Ast;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace xUnitPlay
{
    [Serializable]
    public class ScenarioTestCase : LongLivedMarshalByRefObject, ITestCase, ITestMethod, IMethodInfo, IXunitTestCase, IReflectionMethodInfo
    {
        public ITestClass TestClass { get; set; }
        public string Name { get; set; }

        public FeatureFileTypeInfo FeatureFile => (FeatureFileTypeInfo)TestClass;
        public string DisplayName => Name;
        public string UniqueID => $"{FeatureFile.Name};{Name}";

        public ITestMethod TestMethod => this;

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

        public string SkipReason { get; set; }
        public ISourceInformation SourceInformation { get; set; }
        public object[] TestMethodArguments { get; set; }
        public Dictionary<string, List<string>> Traits { get; set; }

        public bool IsAbstract { get; set; }
        public bool IsGenericMethodDefinition { get; set; }
        public bool IsPublic { get { return true; } }
        public bool IsStatic { get; set; }
        public ITypeInfo ReturnType { get; set; }
        public MethodInfo MethodInfo { get { throw new NotImplementedException("ScenarioTestCase.MethodInfo"); } }
        public ITypeInfo Type => TestClass.Class;

        public ScenarioTestCase()
        {
            Traits = new Dictionary<string, List<string>>();
        }

        public ScenarioTestCase(FeatureFileTypeInfo featureFileTypeInfo, Scenario scenario, string[] featureTags) : this()
        {
            TestClass = featureFileTypeInfo;
            Name = scenario.Name;
            SourceInformation = new SourceInformation { FileName = featureFileTypeInfo.FeatureFilePath, LineNumber = scenario.Location?.Line };
            Traits.Add("Category", featureTags.Concat(scenario.Tags.GetTags()).ToList());
        }

        /// <inheritdoc/>
        public virtual Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink,
                                                 IMessageBus messageBus,
                                                 object[] constructorArguments,
                                                 ExceptionAggregator aggregator,
                                                 CancellationTokenSource cancellationTokenSource)
            => new ScenarioTestCaseRunner(this, messageBus, aggregator, cancellationTokenSource).RunAsync();

        public IMethodInfo Method { get { return this; } }
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

    }
}