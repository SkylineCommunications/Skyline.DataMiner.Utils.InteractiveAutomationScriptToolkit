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
		var welcomeDialog = new WelcomeDialog(engine);
		var selectionDialog = new SelectionDialog(engine);
		var confirmationDialog = new ConfirmationDialog(engine);

		welcomeDialog.NextButton.Pressed += (sender, args) => app.ShowDialog(selectionDialog);
		selectionDialog.NextButton.Pressed += (sender, args) =>
		{
			app.ShowDialog(confirmationDialog);
			confirmationDialog.Option = selectionDialog.SelectedOption;
		};

		app.Run(welcomeDialog);
	}
}

public class WelcomeDialog : Dialog<GridPanel>
{
	public WelcomeDialog(Engine engine)
		: base(engine)
	{
		var label = new Label("Click Next to Continue!");
		Panel.Add(label, 0, 0);

		NextButton = new Button("Next");
		Panel.Add(NextButton, 1, 0);
	}

	public Button NextButton { get; }
}

public class SelectionDialog : Dialog<GridPanel>
{
	private readonly DropDown dropDown;

	public SelectionDialog(Engine engine)
		: base(engine)
	{
		var label = new Label("Please select an option.");
		Panel.Add(label, 0, 0);

		dropDown = new DropDown(new[] { "Meat", "Fish" });
		Panel.Add(dropDown, 1, 0);

		NextButton = new Button("Next");
		Panel.Add(NextButton, 2, 0);
	}

	public Button NextButton { get; }

	public string SelectedOption => dropDown.Selected;
}

public class ConfirmationDialog : Dialog<GridPanel>
{
	private readonly Label label;
	private string option;

	public ConfirmationDialog(Engine engine)
		: base(engine)
	{
		label = new Label { Style = TextStyle.Bold };
		Panel.Add(label, 0, 0);

		var button = new Button("Finish");
		Panel.Add(button, 1, 0);

		button.Pressed += (sender, args) => engine.ExitSuccess("Done");
	}

	public string Option
	{
		set
		{
			option = value;
			label.Text = "You have selected: " + option;
		}
	}
}