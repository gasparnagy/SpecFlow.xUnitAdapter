using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Gherkin.Ast;
using xUnitPlay.Runners;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace xUnitPlay.TestArtifacts
{
    public class ScenarioTestCase : LongLivedMarshalByRefObject, ITestMethod, IXunitTestCase, IReflectionMethodInfo
    {
        public FeatureFileTestClass FeatureFile { get; private set; }
        public string Name { get; private set; }

        public string DisplayName => Name;
        public string UniqueID => $"{FeatureFile.RelativePath};{Name}";

        public ISourceInformation SourceInformation { get; set; }
        public string SkipReason { get; set; }
        public Dictionary<string, List<string>> Traits { get; set; }
        object[] ITestCase.TestMethodArguments => null;

        public ITestClass TestClass => FeatureFile;
        public ITestMethod TestMethod => this;
        public IMethodInfo Method => this;

        public ScenarioTestCase()
        {
            Traits = new Dictionary<string, List<string>>();
        }

        public ScenarioTestCase(FeatureFileTestClass featureFileTestClass, Scenario scenario, string[] featureTags) : this()
        {
            FeatureFile = featureFileTestClass;
            Name = scenario.Name;
            SourceInformation = new SourceInformation { FileName = featureFileTestClass.FeatureFilePath, LineNumber = scenario.Location?.Line };
            Traits.Add("Category", featureTags.Concat(scenario.Tags.GetTags()).ToList());
        }

        public void Deserialize(IXunitSerializationInfo data)
        {
            FeatureFile = data.GetValue<FeatureFileTestClass>("FeatureFile");
            Name = data.GetValue<string>("Name");
        }

        public void Serialize(IXunitSerializationInfo data)
        {
            data.AddValue("FeatureFile", FeatureFile);
            data.AddValue("Name", Name);
        }

        public virtual Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink,
                                                 IMessageBus messageBus,
                                                 object[] constructorArguments,
                                                 ExceptionAggregator aggregator,
                                                 CancellationTokenSource cancellationTokenSource)
            => new ScenarioTestCaseRunner(this, messageBus, aggregator, cancellationTokenSource).RunAsync();

        #region IMethodInfo default implementation
        bool IMethodInfo.IsAbstract => false;
        bool IMethodInfo.IsGenericMethodDefinition => false;
        bool IMethodInfo.IsPublic => true;
        bool IMethodInfo.IsStatic => false;
        ITypeInfo IMethodInfo.ReturnType => null;
        ITypeInfo IMethodInfo.Type => TestClass.Class;
        IEnumerable<IAttributeInfo> IMethodInfo.GetCustomAttributes(string assemblyQualifiedAttributeTypeName) => Enumerable.Empty<IAttributeInfo>();
        IEnumerable<ITypeInfo> IMethodInfo.GetGenericArguments() => Enumerable.Empty<ITypeInfo>();
        IEnumerable<IParameterInfo> IMethodInfo.GetParameters() => Enumerable.Empty<IParameterInfo>();
        IMethodInfo IMethodInfo.MakeGenericMethod(params ITypeInfo[] typeArguments)
        {
            throw new NotSupportedException("ScenarioTestCase.MakeGenericMethod");
        }
        MethodInfo IReflectionMethodInfo.MethodInfo { get { throw new NotSupportedException("MethodInfo is not available for scenarios"); } }
        #endregion
    }
}