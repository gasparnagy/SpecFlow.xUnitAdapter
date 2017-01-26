using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace xUnitPlay.SpecFlowProject
{
    [Binding]
    public sealed class StepDefinition1
    {
        [Given("I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredSomethingIntoTheCalculator(int number)
        {
            Console.WriteLine("Running Given step");
        }

        [When("I press add")]
        public void WhenIPressAdd()
        {
            Console.WriteLine("Running When step");
        }

        [Then("the result should be (.*) on the screen")]
        public void ThenTheResultShouldBe(int result)
        {
            Console.WriteLine("Running Then step");
        }
    }
}
