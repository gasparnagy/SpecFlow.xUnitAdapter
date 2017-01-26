Feature: SpecFlow Feature 2
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: 
	Given there is a background

@mytag
Scenario: Add two numbers C
	Given I have entered 50 into the calculator
	And I have entered 70 into the calculator
	When I press add
	Then the result should be 120 on the screen

Scenario Outline: Add two numbers with SO
	Given I have entered <a> into the calculator
	And I have entered <b> into the calculator
	When I press add
	Then the result should be <result> on the screen

Examples: 
	| a | b | result |
	| 1 | 2 | 3      |
	| 3 | 4 | 7      |
	| 3 | 4 | 42     |

@ignore
Scenario: Ignored scenario
	Given something wrong