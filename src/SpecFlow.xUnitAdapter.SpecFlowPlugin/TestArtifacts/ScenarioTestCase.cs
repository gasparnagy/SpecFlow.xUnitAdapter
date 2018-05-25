﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Gherkin.Ast;
using SpecFlow.xUnitAdapter.SpecFlowPlugin.Runners;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SpecFlow.xUnitAdapter.SpecFlowPlugin.TestArtifacts
{
    public class ScenarioTestCase : LongLivedMarshalByRefObject, ITestMethod, IXunitTestCase, IReflectionMethodInfo
    {
        public SpecFlowFeatureTestClass FeatureTestClass { get; private set; }
        public SpecFlowFeatureTypeInfo FeatureTypeInfo => FeatureTestClass.FeatureTypeInfo;
        public string Name { get; private set; }

        public string DisplayName => GetDisplayName();
        public string UniqueID => $"{FeatureTypeInfo.RelativePath};{Name};{ExampleId}";

        public ISourceInformation SourceInformation { get; set; }
        public string SkipReason { get; set; }
        public Dictionary<string, List<string>> Traits { get; set; }
        public Dictionary<string,string> ScenarioOutlineParameters { get; set; }
        public string ExampleId { get; set; }

        public bool IsScenarioOutline => !string.IsNullOrEmpty(ExampleId);

        object[] ITestCase.TestMethodArguments => ScenarioOutlineParameters?.Cast<object>().ToArray();
        public ITestClass TestClass => FeatureTestClass;
        public ITestMethod TestMethod => this;
        public IMethodInfo Method => this;

        public ScenarioTestCase()
        {
            Traits = new Dictionary<string, List<string>>();
        }

        private ScenarioTestCase(SpecFlowFeatureTestClass featureTestClass, ScenarioDefinition scenario, string[] featureTags, Location location)
        {
            FeatureTestClass = featureTestClass;
            Name = scenario.Name;
            SourceInformation = new SourceInformation { FileName = FeatureTypeInfo.FeatureFilePath, LineNumber = location?.Line };
            Traits = new Dictionary<string, List<string>>();
            Traits.Add("Category", featureTags.Concat(((IHasTags)scenario).Tags.GetTags()).ToList());
        }

        public ScenarioTestCase(SpecFlowFeatureTestClass featureTestClass, Scenario scenario, string[] featureTags)
            : this(featureTestClass, scenario, featureTags, scenario.Location)
        {
        }

        public ScenarioTestCase(SpecFlowFeatureTestClass featureTestClass, ScenarioOutline scenario, string[] featureTags, Dictionary<string, string> scenarioOutlineParameters, string exampleId, Location exampleLocation) 
            : this(featureTestClass, scenario, featureTags, exampleLocation)
        {
            ScenarioOutlineParameters = scenarioOutlineParameters;
            ExampleId = exampleId;
        }

        private string GetDisplayName()
        {
            if (ScenarioOutlineParameters != null)
            {
                return $"{Name} ({string.Join(", ", ScenarioOutlineParameters.Select(p => $"{p.Key}: {p.Value}"))})";
            }
            return Name;
        }

        public void Deserialize(IXunitSerializationInfo data)
        {
            FeatureTestClass = data.GetValue<SpecFlowFeatureTestClass>("FeatureTestClass");
            Name = data.GetValue<string>("Name");
            ExampleId = data.GetValue<string>("ExampleId");
        }

        public void Serialize(IXunitSerializationInfo data)
        {
            data.AddValue("FeatureTestClass", FeatureTestClass);
            data.AddValue("Name", Name);
            data.AddValue("ExampleId", ExampleId);
        }

        public virtual Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink,
                                                 IMessageBus messageBus,
                                                 object[] constructorArguments,
                                                 ExceptionAggregator aggregator,
                                                 CancellationTokenSource cancellationTokenSource)
        {
            var testOutputHelper = constructorArguments.OfType<TestOutputHelper>().FirstOrDefault();
            return new ScenarioTestCaseRunner(this, messageBus, aggregator, cancellationTokenSource, testOutputHelper).RunAsync();
        }

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
