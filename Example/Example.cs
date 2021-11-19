/*
****************************************************************************
*  Copyright (c) 2019,  Skyline Communications NV  All Rights Reserved.    *
****************************************************************************

By using this script, you expressly agree with the usage terms and
conditions set out below.
This script and all related materials are protected by copyrights and
other intellectual property rights that exclusively belong
to Skyline Communications.

A user license granted for this script is strictly for personal use only.
This script may not be used in any way by anyone without the prior
written consent of Skyline Communications. Any sublicensing of this
script is forbidden.

Any modifications to this script by the user are only allowed for
personal use and within the intended purpose of the script,
and will remain the sole responsibility of the user.
Skyline Communications will not be responsible for any damages or
malfunctions whatsoever of the script resulting from a modification
or adaptation by the user.

The content of this script is confidential information.
The user hereby agrees to keep this confidential information strictly
secret and confidential and not to disclose or reveal it, in whole
or in part, directly or indirectly to any person, entity, organization
or administration without the prior written consent of
Skyline Communications.

Any inquiries can be addressed to:

	Skyline Communications NV
	Ambachtenstraat 33
	B-8870 Izegem
	Belgium
	Tel.	: +32 51 31 35 69
	Fax.	: +32 51 31 01 29
	E-mail	: info@skyline.be
	Web		: www.skyline.be
	Contact	: Ben Vandenberghe

****************************************************************************
Revision History:

DATE		VERSION		AUTHOR			COMMENTS

dd/mm/2019	1.0.0.1		XXX, Skyline	Initial Version
****************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Text;

using Skyline.DataMiner.Automation;
using Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit;

/// <summary>
///     DataMiner Script Class.
/// </summary>
internal class Script
{
	private const string WelcomeMessage = @"Interactive Automation Toolkit
--------------------------------
This is an example application to showcase the widgets
";

	/// <summary>
	///     The Script entry point.
	/// </summary>
	/// <param name="engine">Link with SLScripting process.</param>
	public void Run(Engine engine)
	{
		try
		{
			RunSafe(engine);
		}
		catch (ScriptAbortException)
		{
			throw;
		}
		catch (Exception e)
		{
			ShowExceptionDialog(engine, e);
		}
	}

	private static void RunSafe(Engine engine)
	{
		var app = new InteractiveController(engine);

		var welcomeDialog = new WelcomeDialog(engine);
		welcomeDialog.TextBox.Text = WelcomeMessage;

		var demoDialog = new DemoDialog(engine);
		var progressDialog = new ProgressDialog(app);

		welcomeDialog.ContinueButton.Pressed += (sender, args) => app.ShowDialog(demoDialog);
		demoDialog.ExitButton.Pressed += (sender, args) => engine.ExitSuccess("Demo was stopped by user");
		demoDialog.ExecuteButton.Pressed += (sender, args) => app.RequestManualMode(progressDialog.Execute);

		app.ShowDialog(welcomeDialog);
		app.Run(welcomeDialog);
	}

	private static void ShowExceptionDialog(Engine engine, Exception e)
	{
		ExceptionDialog exceptionDialog = new ExceptionDialog(engine, e);
		exceptionDialog.OkButton.Pressed += (sender, args) => engine.ExitFail(e.ToString());
		exceptionDialog.Show();
	}
}

public class WelcomeDialog : Dialog
{
	private readonly Label titleLabel = new Label("Welcome") { Style = TextStyle.Title };

	public WelcomeDialog(Engine engine) : base(engine)
	{
		Title = "Welcome";

		TextBox = new TextBox { IsMultiline = true };
		ContinueButton = new Button("Continue");

		AddWidget(titleLabel, 0, 0, 1, 3, HorizontalAlignment.Center);
		AddWidget(TextBox, 1, 0, 1, 3);
		AddWidget(ContinueButton, 2, 2, HorizontalAlignment.Right);
	}

	public TextBox TextBox { get; private set; }

	public Button ContinueButton { get; private set; }
}

public class DemoDialog : Dialog
{
	/// <inheritdoc />
	public DemoDialog(Engine engine) : base(engine)
	{
		TitleLabel = new Label("Feature Demo") { Style = TextStyle.Title };
		AddWidget(TitleLabel, 0, 0, 1, 2, HorizontalAlignment.Center);
		SetRowHeight(0, 50);

		CheckBox = new CheckBox("checkbox") { WantsOnChange = true };
		AddWidget(CheckBox, 1, 0);
		CheckBox.Changed += OnCheckBoxOnChanged;

		DropDown = new DropDown(new[] { "Title", "Heading", "Bold", "None" }) { WantsOnChange = true };
		AddWidget(DropDown, 1, 1);
		DropDown.Changed += OnDropDownOnChanged;

		CheckBoxList = new CheckBoxList(new[] { "title", "checkbox", "dropdown" }) { WantsOnChange = true };
		CheckBoxList.CheckAll();
		AddWidget(CheckBoxList, 2, 0);
		CheckBoxList.Changed += OnCheckBoxListOnChanged;

		ExecuteButton = new Button("Execute");
		AddWidget(ExecuteButton, 2, 1);

		Numeric = new Numeric(30)
		{
			WantsOnChange = true,
			Decimals = 1,
			Minimum = 30,
			Maximum = 200,
			StepSize = 0.5
		};
		AddWidget(Numeric, 3, 0);
		Numeric.Changed += OnNumericOnChanged;

		AddWidget(new PasswordBox(true), 3, 1);

		RadioButtonList = new RadioButtonList(new[] { "a", "b", "c" });
		AddWidget(RadioButtonList, 4, 0);

		DateTimePicker = new DateTimePicker { WantsOnChange = true };
		AddWidget(DateTimePicker, 4, 1);

		Time = new Time(new TimeSpan(16, 45, 30)) { WantsOnChange = true };
		AddWidget(Time, 5, 0);

		TimePicker = new TimePicker { WantsOnChange = true };
		AddWidget(TimePicker, 5, 1);

		Calendar = new Calendar { WantsOnChange = true };
		AddWidget(Calendar, 6, 1);

		ResetButton = new Button("Reset");
		AddWidget(ResetButton, 7, 0);
		ResetButton.Pressed += OnResetButtonOnPressed;

		ExitButton = new Button("Exit");
		AddWidget(ExitButton, 7, 1);
	}

	public CheckBox CheckBox { get; private set; }

	public CheckBoxList CheckBoxList { get; private set; }

	public Calendar Calendar { get; private set; }

	public DateTimePicker DateTimePicker { get; private set; }

	public DropDown DropDown { get; private set; }

	public Button ExecuteButton { get; private set; }

	public Button ExitButton { get; private set; }

	public Numeric Numeric { get; private set; }

	public Parameter Parameter { get; private set; }

	public RadioButtonList RadioButtonList { get; private set; }

	public Button ResetButton { get; private set; }

	public Label TitleLabel { get; private set; }

	private void OnCheckBoxListOnChanged(object sender, CheckBoxList.CheckBoxListChangedEventArgs args)
	{
		switch (args.Option)
		{
			case "title":
				TitleLabel.IsVisible = args.IsChecked;
				break;

			case "checkbox":
				CheckBox.IsVisible = args.IsChecked;
				break;

			case "dropdown":
				DropDown.IsVisible = args.IsChecked;
				break;
		}
	}

	private void OnCheckBoxOnChanged(object sender, CheckBox.CheckBoxChangedEventArgs args)
	{
		TitleLabel.Text = args.IsChecked ? "Checked" : "UnChecked";
	}

	private void OnDropDownOnChanged(object sender, DropDown.DropDownChangedEventArgs args)
	{
		switch (DropDown.Selected)
		{
			case "Title":
				TitleLabel.Style = TextStyle.Title;
				break;
			case "Heading":
				TitleLabel.Style = TextStyle.Heading;
				break;
			case "Bold":
				TitleLabel.Style = TextStyle.Bold;
				break;
			case "None":
				TitleLabel.Style = TextStyle.None;
				break;
		}
	}

	private void OnNumericOnChanged(object sender, Numeric.NumericChangedEventArgs args)
	{
		var height = (int)args.Value;
		Engine.GenerateInformation(height.ToString());
		SetRowHeight(3, height);
	}

	private void OnResetButtonOnPressed(object sender, EventArgs args)
	{
		TitleLabel.Text = "Feature Demo";
		TitleLabel.Style = TextStyle.Title;
		TitleLabel.IsVisible = true;
		CheckBox.IsChecked = false;
		CheckBox.IsVisible = true;
		DropDown.Selected = "a";
		DropDown.IsVisible = true;
		CheckBoxList.CheckAll();
		SetRowHeightAuto(3);
		Numeric.Value = 30;
	}

	public Time Time { get; private set; }

	public TimePicker TimePicker { get; private set; }
}

public class ProgressDialog : Dialog
{
	private readonly InteractiveController interactiveController;

	private readonly List<string> messages = new List<string>
	{
		"Draining the ocean.",
		"Adding Randomly Mispeled Words Into Text",
		"Attaching Beards to Dwarves",
		"Creating Randomly Generated Feature",
		"Does Anyone Actually Read This?",
		"Doing Something You Don't Wanna Know About",
		"Doing The Impossible",
		"Don't Panic",
		"Ensuring Everything Works Perfektly",
		"Generating Plans for Faster-Than-Light Travel",
		"Hitting Your Keyboard Won't Make This Faster",
		"Loading, Don't Wait If You Don't Want To",
		"Look Out Behind You",
		"Looking For Graphics",
		"Oiling Clockworks",
		"Preparing to Spin You Around Rapidly",
		"Told You It Wasn't Made of Cheese"
	};

	private readonly TextBox progressLog;
	private readonly Random random = new Random();

	/// <inheritdoc />
	public ProgressDialog(InteractiveController interactiveController) : base(interactiveController.Engine)
	{
		this.interactiveController = interactiveController;
		progressLog = new TextBox { IsMultiline = true, Height = 400, Width = 500 };
		AddWidget(progressLog, 0, 0);
	}

	public void Execute()
	{
		Dialog returnDialog = interactiveController.CurrentDialog;
		interactiveController.ShowDialog(this);
		interactiveController.Update();
		var stringBuilder = new StringBuilder();
		for (var i = 0; i < 20; i++)
		{
			Engine.Sleep(random.Next(1000));
			stringBuilder.AppendLine(messages[random.Next(messages.Count - 1)]);
			progressLog.Text = stringBuilder.ToString();
			interactiveController.Update();
		}

		progressLog.Text = String.Empty;
		interactiveController.ShowDialog(returnDialog);
	}
}