# Skyline.DataMiner.InteractiveAutomationToolkit

This package is an extension to
[Skyline.DataMiner.Automation](https://docs.dataminer.services/develop/api/types/Skyline.DataMiner.Automation.html).

Quickly develop interactive automation scripts for DataMiner.
This package provides an API that more closely resembles other desktop graphical user interface libraries.

## How do I get started?

Create or import an existing DataMiner automation script in Visual Studio.
Then add the NuGet package to the project.
You can now create your first interactive script using the toolkit:

```csharp
using System;

using Skyline.DataMiner.Automation;
using Skyline.DataMiner.InteractiveAutomationToolkit;

/// <summary>
///     DataMiner Script Class.
/// </summary>
internal class Script
{
	/// <summary>
	///     The Script entry point.
	/// </summary>
	/// <param name="engine">Link with SLScripting process.</param>
	public void Run(Engine engine)
	{
		// DO NOT REMOVE THE COMMENTED OUT CODE BELOW OR THE SCRIPT WONT RUN!
		// Interactive scripts need to be launched differently.
		// This is determined by a simple string search looking for "engine.ShowUI" in the source code.
		// However, due to the NuGet package, this string can no longer be detected.
		// This comment is here as a temporary workaround until it has been fixed.
		//// engine.ShowUI(

		try
		{
			// Controls the event loop and switch between dialogs
			var controller = new InteractiveController(engine);

			// Create an instance of the dialog you wish to show.
			var helloWorldDialog = new HelloWorldDialog(engine);

			// Starts the event loop and shows the first dialog.
			controller.Run(helloWorldDialog);
		}
		catch (ScriptAbortException)
		{
			throw;
		}
		catch (ScriptForceAbortException)
		{
			throw;
		}
		catch (ScriptTimeoutException)
		{
			throw;
		}
		catch (InteractiveUserDetachedException)
		{
			throw;
		}
		catch (Exception e)
		{
			engine.ExitFail(e.ToString());
		}
	}
}

// You can define your dialogs by inheriting from the Dialog class
public class HelloWorldDialog : Dialog
{
	public HelloWorldDialog(IEngine engine)
		: base(engine)
	{
		// Create a label widget
		var label = new Label("Hello, World!") { Style = TextStyle.Title };

		// Add it to the dialog grid at position 0,0
		AddWidget(label, 0, 0);

		// Create a button widget
		var button = new Button("OK");

		// Add it to the dialog just below the label
		AddWidget(button, 1, 0);

		// Add an action to be performed whenever the button is clicked
		button.Pressed += (sender, args) => engine.ExitSuccess("Done");
	}
}
```

If you have questions, you can post them to
our [DataMiner community platform](https://community.dataminer.services/questions/).
Or have a look at the guides and video courses listed below.

## Courses and guides

- [All video courses related to DataMiner Automation](https://community.dataminer.services/courses/dataminer-automation/)
- [Getting started with the toolkit](https://community.dataminer.services/documentation/getting-started-with-the-ias-toolkit/)
- [Video course covering the toolkit basics](https://community.dataminer.services/courses/dataminer-automation/lessons/interaction-automation-toolkit/)
  (The first 2 minutes can be skipped as they cover how to acquire the toolkit without NuGet)
- [Create applications using Model View Presenter](https://community.dataminer.services/courses/dataminer-automation/lessons/model-view-presenter/)

## Known Issue

When you use this NuGet package, it is possible you get the following error when launching the script:
`DataMinerException: Show UI Failed: 0x800402F5 (Interactive UI can only be used in interactive mode. )`
This is due to a DataMiner software issue causing the script to be launched in the wrong mode.
The DataMiner software perform a simple string search to detect if the script should be launched in interactive mode.
It is looking for the following string "engine.ShowUI" which is normally always present in the source code of the
script.
This method call is now handled by the NuGet package, so the string can no longer be found in the script source code,
causing the issue.
We are currently working on a fix which will be available in one of our upcoming DataMiner releases.

### Workaround

Add the following comment to your script.

```csharp
// engine.ShowUI(
```

Location of the comment does not matter as long as it is visible in the C# code block view of the DataMiner Automation
Script UI.