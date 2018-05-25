using System;
using Xunit;
using Xunit.Abstractions;

namespace SpecFlow.xUnitAdapter.SpecFlowPlugin.TestArtifacts
{
    public class SpecFlowFeatureTestClass : LongLivedMarshalByRefObject, ITestClass
    {
        public ITypeInfo Class { get; private set; }
        public ITestCollection TestCollection { get; private set; }

        public SpecFlowFeatureTypeInfo FeatureTypeInfo => (SpecFlowFeatureTypeInfo)Class;

        [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
        public SpecFlowFeatureTestClass()
        {
        }

        public SpecFlowFeatureTestClass(ITestCollection testCollection, ITypeInfo typeInfo)
        {
            if (!(typeInfo is SpecFlowFeatureTypeInfo))
                throw new ArgumentException($"Must me an instance of {nameof(SpecFlowFeatureTypeInfo)}", "typeInfo");

            TestCollection = testCollection;
            Class = typeInfo;
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue("TestCollection", TestCollection);
            info.AddValue("SpecFlowProject", FeatureTypeInfo.SpecFlowProject);
            info.AddValue("RelativePath", FeatureTypeInfo.RelativePath);
            info.AddValue("TypeInfoKind", Class.GetType().Name);
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            TestCollection = info.GetValue<ITestCollection>("TestCollection");
            var specFlowProject = info.GetValue<SpecFlowProjectAssemblyInfo>("SpecFlowProject");
            var relativePath = info.GetValue<string>("RelativePath");
            var typeInfoKind = info.GetValue<string>("TypeInfoKind");
            if (typeInfoKind == nameof(FeatureFileTypeInfo))
                Class = new FeatureFileTypeInfo(specFlowProject, relativePath);
            else if (typeInfoKind == nameof(EmbeddedFeatureTypeInfo))
                Class = new EmbeddedFeatureTypeInfo(specFlowProject, relativePath);
        }
    }
}
