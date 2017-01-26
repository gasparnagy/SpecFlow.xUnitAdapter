using System;
using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace xUnitPlay
{
    [Serializable]
    public class FeatureFileTypeInfo : ITypeInfo, ITestClass, IReflectionTypeInfo
    {
        public IEnumerable<IAttributeInfo> GetCustomAttributes(string assemblyQualifiedAttributeTypeName)
        {
            yield break;
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

        public FeatureFileTypeInfo(string name, IAssemblyInfo assembly)
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
        public Type Type => typeof (SpecFlowGenericFixtureType);
    }
}