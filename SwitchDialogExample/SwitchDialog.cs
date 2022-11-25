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

public class WelcomeDialog : Dialog
{
	public Button NextButton { get; private set; }

	public WelcomeDialog(Engine engine) : base(engine)
	{
		var label = new Label("Click Next to Continue!");
		AddWidget(label, 0, 0);

		NextButton = new Button("Next");
		AddWidget(NextButton, 1, 0);
	}
}

public class SelectionDialog : Dialog
{
	private readonly DropDown dropDown;

	public Button NextButton { get; private set; }

	public SelectionDialog(Engine engine) : base(engine)
	{
		var label = new Label("Please select an option.");
		AddWidget(label, 0, 0);

		dropDown = new DropDown(new[] { "Meat", "Fish" });
		AddWidget(dropDown, 1, 0);

		NextButton = new Button("Next");
		AddWidget(NextButton, 2, 0);
	}

	public string SelectedOption
	{
		get
		{
			return dropDown.Selected;
		}
	}
}

public class ConfirmationDialog : Dialog
{
	private readonly Label label;
	private string option;

	public ConfirmationDialog(Engine engine) : base(engine)
	{
		label = new Label { Style = TextStyle.Bold };
		AddWidget(label, 0, 0);

		var button = new Button("Finish");
		AddWidget(button, 1, 0);

		button.Pressed += (sender, args) => engine.ExitSuccess("Done");
	}

	public string Option
	{
		get => option;
		set
		{
			option = value;
			label.Text = "You have selected: " + option;
		}
	}
}