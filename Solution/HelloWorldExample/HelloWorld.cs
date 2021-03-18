using Skyline.DataMiner.Automation;
using Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit;

/// <summary>
/// DataMiner Script Class.
/// </summary>
class Script
{
	/// <summary>
	/// The Script entry point.
	/// </summary>
	/// <param name="engine">Link with SLScripting process.</param>
	public void Run(Engine engine)
	{
		var app = new InteractiveController(engine);
		var helloWorldDialog = new HelloWorldDialog(engine);
		app.ShowDialog(helloWorldDialog);
		app.Run(helloWorldDialog);
	}
}

public class HelloWorldDialog : Dialog
{
	public HelloWorldDialog(Engine engine) : base(engine)
	{
		var label = new Label("Hello, World!") { Style = TextStyle.Title };
		AddWidget(label, 0, 0);

		var button = new Button("OK");
		AddWidget(button, 1, 0);
		button.Pressed += (sender, args) => engine.ExitSuccess("Done");
	}
}