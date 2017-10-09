using System;
using Xunit;

namespace SpecFlow.xUnitAdapter.TestProject
{
    public class StandardXUnitTest
    {
        [Fact]
        public void ThisIsAStrandardPassingXUnitTest()
        {
            Console.WriteLine("This should be tested too");
        }

        [Fact(DisplayName = "Failing standard test with custom display name")]
        public void ThisIsAStrandardFailingXUnitTest()
        {
            Assert.True(false);
        }
    }
}
