﻿# SpecFlow.xUnitAdapter

SpecFlow.xUnitAdapter is an xUnit adapter for SpecFlow that allows running 
scenarios without code generation.

Supports:

* SpecFlow v2.2
* xUnit v2.2 or above

Requires:

* .NET Framework 4.5 or above
* Visual Studio for Windows

License: Apache (https://github.com/gasparnagy/SpecFlow.xUnitAdapter/blob/master/LICENSE)

NuGet: https://www.nuget.org/packages/SpecFlow.xUnitAdapter

See my blog post (http://gasparnagy.com/2017/04/specflow-without-code-behind-files/) for more information and background, 
you can also look at the complete example at https://github.com/gasparnagy/SpecFlow.xUnitAdapter/tree/master/sample/MyCalculator or watch the 
video demo on [YouTube](https://youtu.be/wGuoVqG3I8M).

[![Build status](https://ci.appveyor.com/api/projects/status/oshtcr06501euoih/branch/master?svg=true)](https://ci.appveyor.com/project/gasparnagy/specflow-xunitadapter/branch/master)

## Donate

If you like this plugin and would like to support developing this or similar plugins, please consider donating the project. (For receiving an invoice for your donation, please [contact me](http://gasparnagy.com/about/) upfront.)

[![Donate](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=2FHWS4JADYFZN)

## Usage

### Install plugin from NuGet into your SpecFlow project.

    PM> Install-Package SpecFlow.xUnitAdapter
  
### Disable code-behind code generation

To disable the code-behind code generation, you have to REMOVE (set it to 
empty) the "SpecFlowSingleFileGenerator" "Custom Tool" setting on the file 
properties of all feature files in Visual Studio.

If you do this, Visual Studio will automatically delete the code behind files 
and removes them from the project.

### Make sure the feature files have a `SpecFlowFeature` or `SpecFlowEmbeddedFeature` build action.

The SpecFlow.xUnitAdapter currently processes feature files in the folder (or 
subfolders) of the SpecFlow project assembly (typically in bin\Debug) and when
they are embedded into the assembly. 

To achieve that, you should change the `Build Action` setting of 
the feature files to `SpecFlowFeature` if you want them to not be
embedded, or `SpecFlowEmbeddedFeature` if you want them to be embedded
in the assembly.

### Install xUnit Visual Studio adapter 

In order to run the tests from the Visual Studio Test Explorer Window, you 
can install the xUnit Visual Studio adapter. 

    PM> Install-Package xunit.runner.visualstudio

## Limitations

The adapter is currently in BETA and there are some limitations.

1. If you use `SpecFlowFeature`: As Visual Studio Test Explorer window only triggers the re-discovery of tests when the output assembly changes, if you only change the feature files, but nothing else in the code, you need to _Rebuild_ the project in order to see the changes. (This is important only if the change is related to the discovered tests, so for example if you change the scenario name or the examples section of a scenario outline.)
2. The adapter currently does not load the project-level feature file language setting (but assumes `en-US`).
3. For feature files with `#language` setting, the setting will only be used for data conversions if it is a specific culture (e.g. `de-AT`). For neutral languages (e.g. `de`), currently `en-US` is used for conversions.

For diagnosing test discovery errors, you can add the following setting to the `App.config` file of the SpecFlow project. With this setting, the test discovery errors are displayed in the `Tests` pane of the output window.

    <appSettings>
      <add key="xunit.diagnosticMessages" value="true"/>
    </appSettings>

## Release History

* See https://github.com/gasparnagy/SpecFlow.xUnitAdapter/releases
