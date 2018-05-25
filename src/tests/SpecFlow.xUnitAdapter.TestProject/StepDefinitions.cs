using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace SpecFlow.xUnitAdapter.TestProject
{
    [Binding]
    public sealed class StepDefinitions : Steps
    {
        private List<int> numbers = new List<int>();
        private int? result;

        [Given(@"there is a new calcualtor")]
        public void GivenThereIsANewCalcualtor()
        {
            numbers.Clear();
        }

        [Given("I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredSomethingIntoTheCalculator(int number)
        {
            System.Threading.Thread.Sleep(100);
            var outputHelper = ScenarioContext.ScenarioContainer.Resolve<ITestOutputHelper>();
            outputHelper.WriteLine("Sample output through ITestOutputHelper");
            Console.WriteLine("Running Given step");
            numbers.Add(number);
        }

        [Given(@"I have entered the following numbers into the calculator:")]
        public void GivenIHaveEnteredTheFollowingNumbersIntoTheCalculator(Table table)
        {
            foreach (var tableRow in table.Rows)
            {
                GivenIHaveEnteredSomethingIntoTheCalculator(int.Parse(tableRow["number"]));
            }
        }

        [When("I press add")]
        public void WhenIPressAdd()
        {
            Console.WriteLine("Running When step");
            result = numbers.Sum();
        }

        [When(@"there is a failing step")]
        public void WhenThereIsAFailingStep()
        {
            throw new Exception("this has failed");
        }

        [Then("the result should be (.*) on the screen")]
        public void ThenTheResultShouldBe(int expectedResult)
        {
            Console.WriteLine("Running Then step");
            Assert.Equal(expectedResult, result);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Console.WriteLine("after scenario");
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            Console.WriteLine("after feature");
        }
    }
}
