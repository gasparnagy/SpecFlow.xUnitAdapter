using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gherkin.Ast;
using Microsoft.SqlServer.Server;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist.ValueRetrievers;
using TechTalk.SpecFlow.Parser;
using Xunit.Sdk;

namespace xUnitPlay
{
    public class ScenarioTestCaseRunner : TestCaseRunner<ScenarioTestCase>
    {
        private ITestRunner testRunner;

        public ScenarioTestCaseRunner(ScenarioTestCase testCase, IMessageBus messageBus, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource) : base(testCase, messageBus, aggregator, cancellationTokenSource)
        {
        }

        public void FeatureSetup(GherkinDocument gherkinDocument)
        {
            Debug.Assert(gherkinDocument.Feature != null);
            var feature = gherkinDocument.Feature;

            var assembly = Assembly.LoadFrom(TestCase.FeatureFile.Assembly.AssemblyPath);
            testRunner = TestRunnerManager.GetTestRunner(assembly);
            var featureInfo = new FeatureInfo(new CultureInfo("en-US"), feature.Name, feature.Description, ProgrammingLanguage.CSharp, feature.Tags.Select(t => t.Name).ToArray());
            testRunner.OnFeatureStart(featureInfo);
        }

        public void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }

        public void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }

        public virtual void ScenarioSetup(ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }

        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
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
                FeatureSetup(gherkinDocument);

                var aggregator = new ExceptionAggregator(Aggregator);
                if (!aggregator.HasExceptions)
                {
                    var scenarioInfo = new ScenarioInfo(scenario.Name, scenario.Tags.Select(t => t.Name).ToArray());
                    ScenarioSetup(scenarioInfo);

                    foreach (var step in scenario.Steps.Cast<SpecFlowStep>())
                    {
                        output.AppendLine($"> Running {step.Keyword}{step.Text}");
                        ExecuteStep(step);
                    }

                    ScenarioCleanup();
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

            ScenarioTearDown();
            FeatureTearDown(); //TODO: call in finally?

            return summary;
        }

        private void ExecuteStep(SpecFlowStep step)
        {
            var docStringArg = step.Argument as DocString;
            string docString = docStringArg?.Content;
            var dataTableArg = step.Argument as DataTable;
            Table dataTable = null;
            if (dataTableArg != null && dataTableArg.Rows.Any())
            {
                dataTable = new Table(dataTableArg.Rows.First().Cells.Select(c => c.Value).ToArray());
                foreach (var row in dataTableArg.Rows.Skip(1))
                {
                    dataTable.AddRow(row.Cells.Select(c => c.Value).ToArray());
                }
            }
            switch (step.StepKeyword)
            {
                case StepKeyword.Given:
                    testRunner.Given(step.Text, docString, dataTable, step.Keyword);
                    break;
                case StepKeyword.When:
                    testRunner.When(step.Text, docString, dataTable, step.Keyword);
                    break;
                case StepKeyword.Then:
                    testRunner.Then(step.Text, docString, dataTable, step.Keyword);
                    break;
                case StepKeyword.And:
                    testRunner.And(step.Text, docString, dataTable, step.Keyword);
                    break;
                case StepKeyword.But:
                    testRunner.But(step.Text, docString, dataTable, step.Keyword);
                    break;
            }
        }
    }
}
