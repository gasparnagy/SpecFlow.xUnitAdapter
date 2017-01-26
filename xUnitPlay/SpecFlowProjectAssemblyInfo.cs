using System;
using System.Collections.Generic;
using System.IO;
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
        public string FeatureFilesFolder { get; private set; }

        private readonly IAssemblyInfo originalAssemblyInfo;

        public SpecFlowProjectAssemblyInfo(IAssemblyInfo originalAssemblyInfo)
        {
            this.originalAssemblyInfo = originalAssemblyInfo;
            FeatureFilesFolder = Path.GetFullPath(Path.GetDirectoryName(originalAssemblyInfo.AssemblyPath));
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
            Console.WriteLine($"    Discovering feature files from folder {FeatureFilesFolder}");
            foreach (var featureFilePath in Directory.GetFiles(FeatureFilesFolder, "*.feature", SearchOption.AllDirectories))
            {
                var relativePath = featureFilePath.Substring(FeatureFilesFolder.Length).TrimStart(Path.DirectorySeparatorChar);
                Console.WriteLine($"      {relativePath}");
                yield return new FeatureFileTypeInfo(relativePath, this);
            }
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
