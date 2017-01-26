using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly:Xunit.TestFramework("xUnitPlay.MyTestFrameword", "xUnitPlay")]

namespace xUnitPlay
{
    [Serializable]
    public class SpecFlowProject : IAssemblyInfo
    {
        private IAssemblyInfo originalAssemblyInfo;

        public SpecFlowProject(IAssemblyInfo originalAssemblyInfo)
        {
            this.originalAssemblyInfo = originalAssemblyInfo;
        }

        public IEnumerable<IAttributeInfo> GetCustomAttributes(string assemblyQualifiedAttributeTypeName)
        {
            return originalAssemblyInfo.GetCustomAttributes(assemblyQualifiedAttributeTypeName);
        }

        public ITypeInfo GetType(string typeName)
        {
            return GetTypes(false).FirstOrDefault(t => t.Name == typeName);
        }

        public IEnumerable<ITypeInfo> GetTypes(bool includePrivateTypes)
        {
            yield return new FeatureFile("Feature1", this);
            //yield return new FeatureFile("Feature2", this);
        }

        public string AssemblyPath
        {
            get { return originalAssemblyInfo.AssemblyPath; }
        }

        public string Name
        {
            get { return originalAssemblyInfo.Name; }
        }
    }

    [Serializable]
    public class FeatureFile : ITypeInfo, ITestClass
    {
        public IEnumerable<IAttributeInfo> GetCustomAttributes(string assemblyQualifiedAttributeTypeName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ITypeInfo> GetGenericArguments()
        {
            throw new NotImplementedException();
        }

        public IMethodInfo GetMethod(string methodName, bool includePrivateMethod)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMethodInfo> GetMethods(bool includePrivateMethods)
        {
            throw new NotImplementedException();
        }

        public IAssemblyInfo Assembly { get; set; }
        public ITypeInfo BaseType { get; set; }
        public IEnumerable<ITypeInfo> Interfaces { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsGenericParameter { get; set; }
        public bool IsGenericType { get; set; }
        public bool IsSealed { get; set; }
        public bool IsValueType { get; set; }
        public string Name { get; set; }

        public FeatureFile(string name, IAssemblyInfo assembly)
        {
            Name = name;
            Assembly = assembly;

            TestCollection = new TestCollection(new TestAssembly(assembly, null), null, "My Collectin");
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            throw new NotImplementedException();
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            throw new NotImplementedException();
        }

        public ITypeInfo Class { get { return this; } }
        public ITestCollection TestCollection { get; set; }
    }

    [Serializable]
    public class Scenario : ITestCase, ITestMethod, IMethodInfo
    {
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

        public Scenario(ITestClass testClass)
        {
            TestClass = testClass;
            DisplayName = "Scenario 1";
            UniqueID = DisplayName;
        }

        public IMethodInfo Method { get { return this; } }
        public ITestClass TestClass { get; set; }
        public IEnumerable<IAttributeInfo> GetCustomAttributes(string assemblyQualifiedAttributeTypeName)
        {
            throw new NotImplementedException();
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
        public string Name { get { return UniqueID; } }
        public ITypeInfo ReturnType { get; set; }
        public ITypeInfo Type { get { return TestClass.Class; } }
    }

    public class MyTestDiscoverer : TestFrameworkDiscoverer
    {
        public MyTestDiscoverer(IAssemblyInfo assemblyInfo, ISourceInformationProvider sourceProvider, IMessageSink diagnosticMessageSink) : 
            base(new SpecFlowProject(assemblyInfo), sourceProvider, diagnosticMessageSink)
        {
        }

        protected override ITestClass CreateTestClass(ITypeInfo @class)
        {
            return (ITestClass)@class;
        }

        protected override bool FindTestsForType(ITestClass testClass, bool includeSourceInformation, IMessageBus messageBus,
            ITestFrameworkDiscoveryOptions discoveryOptions)
        {
            return messageBus.QueueMessage(new TestCaseDiscoveryMessage(new Scenario(testClass)));
        }
    }

    public class MyTestFrameword : XunitTestFramework
    {
        public MyTestFrameword(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink)
        {
        }

        protected override ITestFrameworkDiscoverer CreateDiscoverer(IAssemblyInfo assemblyInfo)
        {
            return new MyTestDiscoverer(assemblyInfo, SourceInformationProvider, DiagnosticMessageSink);
        }
    }
}
