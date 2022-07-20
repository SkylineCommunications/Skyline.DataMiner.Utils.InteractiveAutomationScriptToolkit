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
		var app = new InteractiveController(engine);
		var myEventDialog = new MyEventDialog(engine);
		app.Run(myEventDialog);
	}
}

public class MyEventDialog : Dialog<GridPanel>
{
	private readonly TextBox textBox;

	public MyEventDialog(Engine engine)
		: base(engine)
	{
		textBox = new TextBox("Foo");
		Panel.Add(textBox, 0, 0, 1, 2);

		var appendButton = new Button("Append");
		Panel.Add(appendButton, 1, 0);

		var exitButton = new Button("Exit");
		Panel.Add(exitButton, 1, 1);

		textBox.Changed += OnTextBoxChanged;
		appendButton.Pressed += OnAppendButtonPressed;
		exitButton.Pressed += (sender, args) => Engine.ExitSuccess("Operation completed successfully");

		Interacted += OnInteracted;
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

	private void OnTextBoxChanged(object sender, TextBox.ChangedEventArgs args)
	{
		var message = $"Text changed from '{args.Previous}' to '{args.Value}'";
		Engine.GenerateInformation(message);
		Engine.Log(message);
	}
}