using System;

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
		var myEventDialog = new MyEventDialog(engine);
		app.Run(myEventDialog);
	}
}

public class MyEventDialog : Dialog
{
	private readonly TextBox textBox;

	public MyEventDialog(Engine engine) : base(engine)
	{
		textBox = new TextBox("Foo");
		AddWidget(textBox, 0, 0, 1, 2);

		var appendButton = new Button("Append");
		AddWidget(appendButton, 1, 0);

		var exitButton = new Button("Exit");
		AddWidget(exitButton, 1, 1);

		textBox.Changed += OnTextBoxChanged;
		appendButton.Pressed += OnAppendButtonPressed;
		exitButton.Pressed += (sender, args) => Engine.ExitSuccess("Operation completed successfully");

		Interacted += OnInteracted;
	}

	private void OnTextBoxChanged(object sender, TextBox.TextBoxChangedEventArgs args)
	{
		string message = String.Format("Text changed from '{0}' to '{1}'", args.Previous, args.Value);
		Engine.GenerateInformation(message);
		Engine.Log(message);
	}

	private void OnAppendButtonPressed(object sender, EventArgs args)
	{
		string text = textBox.Text + Environment.NewLine + "Bar";
		textBox.Text = text;
	}

	private void OnInteracted(object sender, EventArgs args)
	{
		const string Message = "User interaction occured";
		Engine.GenerateInformation(Message);
		Engine.Log(Message);
	}
}