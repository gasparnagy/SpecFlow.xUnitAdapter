using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace xUnitPlay
{
    public class Disc : ITestFrameworkDiscoverer
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Find(bool includeSourceInformation, IMessageSink discoveryMessageSink,
            ITestFrameworkDiscoveryOptions discoveryOptions)
        {
            throw new NotImplementedException();
        }

        public void Find(string typeName, bool includeSourceInformation, IMessageSink discoveryMessageSink,
            ITestFrameworkDiscoveryOptions discoveryOptions)
        {
            throw new NotImplementedException();
        }

        public string Serialize(ITestCase testCase)
        {
            throw new NotImplementedException();
        }

        public string TargetFramework { get; }
        public string TestFrameworkDisplayName { get; }
    }
}
