using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace xUnitPlay
{
    public class ScenarioTestCaseRunner : TestCaseRunner<ScenarioTestCase>
    {
        public ScenarioTestCaseRunner(ScenarioTestCase testCase, IMessageBus messageBus, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource) : base(testCase, messageBus, aggregator, cancellationTokenSource)
        {
        }

        protected override Task<RunSummary> RunTestAsync()
        {
            throw new NotImplementedException("ScenarioTestCaseRunner.RunTestAsync");
        }
    }
}
