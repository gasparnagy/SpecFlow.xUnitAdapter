Feature: Complex Addition
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers
 
Scenario: Add two positive numbers
	Given I have entered the following numbers
		| number |
		| 29     |
		| 13     |
	When I press add
	Then the result should be 42 on the screen

Scenario Outline: Add two numbers (outline)
	Given I have entered <a> into the calculator
	And I have entered <b> into the calculator
	When I press add
	Then the result should be <result> on the screen

Examples: 
	| case          | a  | b  | result |
	| classic       | 50 | 70 | 120    |
	| commutativity | 70 | 50 | 120    |
	| zero          | 0  | 42 | 42     |
	