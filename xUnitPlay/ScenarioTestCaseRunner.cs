using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gherkin.Ast;
using Microsoft.SqlServer.Server;
using TechTalk.SpecFlow.Parser;
using Xunit.Sdk;

namespace xUnitPlay
{
    public class ScenarioTestCaseRunner : TestCaseRunner<ScenarioTestCase>
    {
        public ScenarioTestCaseRunner(ScenarioTestCase testCase, IMessageBus messageBus, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource) : base(testCase, messageBus, aggregator, cancellationTokenSource)
        {
        }

        protected override async Task<RunSummary> RunTestAsync()
        {
            var test = new XunitTest(TestCase, TestCase.DisplayName); //TODO: this is a pickle
            var summary = new RunSummary() { Total = 1 };
            var output = new StringBuilder();

            var gherkinDocument = await SpecFlowParserHelper.ParseSpecFlowDocument(TestCase.FeatureFile.FeatureFilePath);

            Scenario scenario = null;
            if (gherkinDocument.SpecFlowFeature != null)
            {
                scenario = gherkinDocument.SpecFlowFeature.ScenarioDefinitions.OfType<Scenario>().FirstOrDefault(s => s.Name == TestCase.Name);
            }

            var skipped = scenario == null;
            if (skipped)
            {
                summary.Skipped++;

                if (!MessageBus.QueueMessage(new TestSkipped(test, $"Unable to find Scenario: {TestCase.DisplayName}")))
                    CancellationTokenSource.Cancel();
            }
            else
            {
                var aggregator = new ExceptionAggregator(Aggregator);
                if (!aggregator.HasExceptions)
                {
                    foreach (var step in scenario.Steps)
                    {
                        output.AppendLine($"> Running {step.Keyword}{step.Text}");
                    }
                }

                var exception = aggregator.ToException();
                TestResultMessage testResult;
                if (exception == null)
                    testResult = new TestPassed(test, summary.Time, output.ToString());
                else
                {
                    testResult = new TestFailed(test, summary.Time, output.ToString(), exception);
                    summary.Failed++;
                }

                if (!CancellationTokenSource.IsCancellationRequested)
                    if (!MessageBus.QueueMessage(testResult))
                        CancellationTokenSource.Cancel();
            }

            if (!MessageBus.QueueMessage(new TestFinished(test, summary.Time, output.ToString())))
                CancellationTokenSource.Cancel();

            return summary;
        }
    }
}
