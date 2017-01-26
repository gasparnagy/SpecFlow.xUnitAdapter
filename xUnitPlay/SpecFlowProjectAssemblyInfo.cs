using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace xUnitPlay
{
    [Serializable]
    public class SpecFlowProjectAssemblyInfo : IAssemblyInfo
    {
        private IAssemblyInfo originalAssemblyInfo;

        public SpecFlowProjectAssemblyInfo(IAssemblyInfo originalAssemblyInfo)
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
            yield return new FeatureFileTypeInfo("Feature1", this);
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
}
