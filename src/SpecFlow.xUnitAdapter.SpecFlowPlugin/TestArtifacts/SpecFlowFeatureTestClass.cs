using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Parser;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SpecFlow.xUnitAdapter.SpecFlowPlugin.TestArtifacts
{
    public abstract class SpecFlowFeatureTestClass : LongLivedMarshalByRefObject, ITypeInfo, IReflectionTypeInfo, ITestClass
    {
        public string FeatureName { get; set; }
        public string RelativePath { get; private set; }
        public SpecFlowProjectAssemblyInfo SpecFlowProject { get; private set; }

        public virtual string FeatureFilePath { get; protected set; }

        IAssemblyInfo ITypeInfo.Assembly => SpecFlowProject;
        string ITypeInfo.Name => (FeatureName ?? RelativePath).Replace(".", "");
        Type IReflectionTypeInfo.Type { get { return typeof(SpecFlowGenericFixtureType); } }

        public ITypeInfo Class => this;
        public ITestCollection TestCollection { get; private set; }

        #region ITypeInfo default implementation
        IEnumerable<IAttributeInfo> ITypeInfo.GetCustomAttributes(string assemblyQualifiedAttributeTypeName) => Enumerable.Empty<IAttributeInfo>();
        IEnumerable<ITypeInfo> ITypeInfo.GetGenericArguments() => Enumerable.Empty<ITypeInfo>();
        IMethodInfo ITypeInfo.GetMethod(string methodName, bool includePrivateMethod) => null;
        IEnumerable<IMethodInfo> ITypeInfo.GetMethods(bool includePrivateMethods) => Enumerable.Empty<IMethodInfo>();
        ITypeInfo ITypeInfo.BaseType => null;
        IEnumerable<ITypeInfo> ITypeInfo.Interfaces => Enumerable.Empty<ITypeInfo>();
        bool ITypeInfo.IsAbstract => false;
        bool ITypeInfo.IsGenericParameter => false;
        bool ITypeInfo.IsGenericType => false;
        bool ITypeInfo.IsSealed => false;
        bool ITypeInfo.IsValueType => false;
        #endregion

        protected SpecFlowFeatureTestClass() { }

        protected SpecFlowFeatureTestClass(SpecFlowProjectAssemblyInfo specFlowProject, string relativePath)
        {
            SpecFlowProject = specFlowProject;
            RelativePath = relativePath;

            TestCollection = new TestCollection(new TestAssembly(specFlowProject, null), null, "Default Collection"); //TODO: support test collections for parallelization
        }

        public virtual void Deserialize(IXunitSerializationInfo data)
        {
            SpecFlowProject = data.GetValue<SpecFlowProjectAssemblyInfo>("SpecFlowProject");
            RelativePath = data.GetValue<string>("RelativePath");
            TestCollection = data.GetValue<ITestCollection>("TestCollection");
        }

        public virtual void Serialize(IXunitSerializationInfo data)
        {
            data.AddValue("SpecFlowProject", SpecFlowProject);
            data.AddValue("RelativePath", RelativePath);
            data.AddValue("TestCollection", TestCollection);
        }
        
        public abstract SpecFlowDocument GetDocument();

        public abstract Task<SpecFlowDocument> GetDocumentAsync();
        
        protected string GetSourceMapping(string content)
        {
            const string Pattern = "#sourceMapping=";

            var index = content.IndexOf(Pattern);
            if (index == -1)
            {
                Console.WriteLine($"Could not find a source mapping for {this.RelativePath}");
                return Path.Combine(SpecFlowProject.FeatureFilesFolder, RelativePath);
            }

            var newLineIndex = content.IndexOf(Environment.NewLine, index);

            return newLineIndex == -1
                ? content.Substring(index + Pattern.Length - 1)
                : content.Substring(index + Pattern.Length, newLineIndex - index - Pattern.Length);
        }
    }
}