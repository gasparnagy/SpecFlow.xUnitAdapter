SpecFlow.xUnitAdapter
=====================

SpecFlow.xUnitAdapter is an xUnit adapter for SpecFlow that allows running 
scenarios without code generation.

For project details, license and donations, please check the GitHub page:
https://github.com/gasparnagy/SpecFlow.xUnitAdapter

The adapter is currently in BETA, with some limitations that you can find on 
the GitHub project page.

Setting up SpecFlow.xUnitAdapter
--------------------------------

1. Disable code-behind code generation

To disable the code-behind code generation, you have to REMOVE (set it to 
empty) the "SpecFlowSingleFileGenerator" "Custom Tool" setting on the file 
properties of all feature files in Visual Studio.

If you do this, Visual Studio will automatically delete the code behind files 
and removes them from the project.

2. Make sure the feature files are copied to the target location.

The SpecFlow.xUnitAdapter currently processes feature files in the folder (or 
subfolders) of the SpecFlow project assembly (typically in bin\Debug) and when
they are embedded into the assembly. 

To achieve that, you should change the `Build Action` setting of 
the feature files to `SpecFlowFeature` if you want them to not be
embedded, or `SpecFlowEmbeddedFeature` if you want them to be embedded
in the assembly.

3. Install xUnit Visual Studio adapter

In order to run the tests from the Visual Studio Test Explorer Window, you 
can install the xUnit Visual Studio adapter. 

    PM> Install-Package xunit.runner.visualstudio


Feedback
--------

If you find any issue please report it on GitHub: 
https://github.com/gasparnagy/SpecFlow.xUnitAdapter/issues

Contribution and pull requests are welcome!
