using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using Xunit;

namespace xUnitPlay.SpecFlowProject
{
    [Binding]
    public sealed class StepDefinition1
    {
        private List<int> numbers = new List<int>();
        private int? result;
            
        [Given(@"there is a background")]
        public void GivenThereIsABackground()
        {
            Console.WriteLine("Running Background step");
        }

        [Given("I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredSomethingIntoTheCalculator(int number)
        {
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

        [When(@"I press multiply")]
        public void WhenIPressMultiply()
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
