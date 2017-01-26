using System;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace xUnitPlay
{
    [Serializable]
    public class ScenarioTestCase : ITestCase, ITestMethod, IMethodInfo
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

        public ScenarioTestCase(ITestClass testClass)
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
}