# Debugging SpecFlow xUnitAdapter
In order to debug the SpecFlow xUnitAdapter, you will need to be able to attach the debugger to the process running/discovering tests. Unfortunatly, `vstest.console.exe` kicks off child processes when it runs/discovers tests. The easiest solution to this is to use an extension created by an employee at Microsoft that instructs the Visual Studio Debugger to attach to and child process spawned from the process currently attached to the debugger. You can download and install the extension from: [Microsoft Child Process Debugging Power Tool](https://visualstudiogallery.msdn.microsoft.com/a1141bff-463f-465f-9b6d-d29b7b503d7a).

Once the extension is installed open the `SpecFlow.xUnitAdapter.SpecFlowPlugin` project's properties and go to the `Debug` property page.

## Start external program:
Visual Studio 2015:
```
C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe
```

Visual Studio 2017:
```
C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\Extensions\TestPlatform\vstest.console.exe
```

## Command line arguments:
This property will change depending on what you are trying to debug. The most common scenarios are:

Test Discovery:
```
SpecFlow.xUnitAdapter.TestProject.dll /ListTests /TestAdapterPath:.
```

Test Run:
```
SpecFlow.xUnitAdapter.TestProject.dll /TestAdapterPath:.
```

## Working directory:
This should be set to the bin directory of the test project which may be different on your machine. An example might be:
```
C:\Projects\SpecFlow.xUnitAdapter\src\tests\SpecFlow.xUnitAdapter.TestProject\bin\Debug\
```

## Enable native code debugging:
This option needt to be enabled so that the child process debugging tools work.

## Microsoft Child Process Debugging Power Tool Configuration
The settings for the child process debugging are stored in the `src\SpecFlow.xUnitAdapter.2017.ChildProcessDbgSettings` file. These settings should be already configured to work correctly with the SpecFlow adapter, however if you need to modify the settings you can do so in Visual Studio using the following menu: `Debug -> Other Debug Targets -> Child Process Debugging Settings...`