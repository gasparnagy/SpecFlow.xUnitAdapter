Feature: SpecFlow Feature 2
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: 
	Given I have entered 50 into the calculator

@mytag
Scenario: Add two numbers with Background
	Given I have entered 70 into the calculator
	When I press add
	Then the result should be 120 on the screen

@mytag
Scenario: Add two numbers with DataTable
	Given there is a new calcualtor
	And I have entered the following numbers into the calculator:
		| number |
		| 50     |
		| 70     |
	When I press add
	Then the result should be 120 on the screen

Scenario Outline: Add two numbers as Scenario Outline
	Given there is a new calcualtor
	And I have entered <a> into the calculator
	And I have entered <b> into the calculator
	When I press add
	Then the result should be <result> on the screen

Examples: 
	| case    | a | b | result |
	| basic   | 1 | 2 | 3      |
	| seven   | 3 | 4 | 7      |
	| failing | 3 | 4 | 42     |

@ignore
Scenario: Ignored scenario
	Given something wrong

Scenario: Undefined scenario
	Given there is an undefined step