/*
****************************************************************************
*  Copyright (c) 2020,  Skyline Communications NV  All Rights Reserved.    *
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

dd/mm/2020	1.0.0.1		XXX, Skyline	Initial Version
****************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;

using Skyline.DataMiner.Automation;
using Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit;

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
		var interactiveController = new InteractiveController(engine);

		var protocolDialog = new ProtocolDialog(engine);
		var elementDialog = new ElementDialog(engine);
		var setParameterDialog = new SetParameterDialog(engine);

		protocolDialog.NextButton.Pressed += (sender, args) =>
		{
			Element[] elements = engine.FindElementsByProtocol(protocolDialog.TextBox.Text);
			if (elements.Any())
			{
				protocolDialog.ErrorLabel.IsVisible = false;
				elementDialog.SetElements(elements);
				interactiveController.ShowDialog(elementDialog);
			}
			else
			{
				protocolDialog.ErrorLabel.IsVisible = true;
			}
		};

		elementDialog.BackButton.Pressed += (sender, args) => interactiveController.ShowDialog(protocolDialog);
		elementDialog.NextButton.Pressed += (sender, args) =>
		{
			setParameterDialog.SetElements(elementDialog.GetSelectedElements());
			interactiveController.ShowDialog(setParameterDialog);
		};

		setParameterDialog.BackButton.Pressed += (sender, args) => interactiveController.ShowDialog(elementDialog);

		interactiveController.Run(protocolDialog);
	}
}

public class ProtocolDialog : Dialog
{
	public ProtocolDialog(Engine engine)
		: base(engine)
	{
		var label = new Label("Please enter a protocol");
		AddWidget(label, 0, 0);

		TextBox = new TextBox();
		AddWidget(TextBox, 1, 0, 1, 2);

		ErrorLabel = new Label("Could not find protocol") { IsVisible = false };
		AddWidget(ErrorLabel, 2, 0);

		NextButton = new Button("Next");
		AddWidget(NextButton, 3, 1);
	}

	public Label ErrorLabel { get; private set; }

	public Button NextButton { get; private set; }

	public TextBox TextBox { get; private set; }
}

public class ElementDialog : Dialog
{
	private readonly CheckBoxList checkBoxList;
	private Element[] elementOptions;

	public ElementDialog(Engine engine)
		: base(engine)
	{
		var label = new Label("Please select the elements");
		AddWidget(label, 0, 0);

		checkBoxList = new CheckBoxList();
		AddWidget(checkBoxList, 1, 0);

		BackButton = new Button("Back");
		AddWidget(BackButton, 2, 0);

		NextButton = new Button("Next");
		AddWidget(NextButton, 2, 1);
	}

	public Button BackButton { get; private set; }

	public Button NextButton { get; private set; }

	public IEnumerable<Element> GetSelectedElements()
	{
		var selectedElements = new HashSet<string>(checkBoxList.Checked);
		return elementOptions.Where(element => selectedElements.Contains(element.ElementName));
	}

	public void SetElements(Element[] elements)
	{
		elementOptions = elements;
		checkBoxList.SetOptions(elements.Select(element => element.ElementName));
	}
}

public class SetParameterDialog : Dialog
{
	private readonly Numeric numeric;
	private readonly TextBox textBox;
	private IEnumerable<Element> selectedElements;

	public SetParameterDialog(Engine engine)
		: base(engine)
	{
		AddWidget(new Label("PID:"), 0, 0);

		numeric = new Numeric(1) { Minimum = 0 };
		AddWidget(numeric, 0, 1);

		AddWidget(new Label("Value:"), 1, 0);

		textBox = new TextBox();
		AddWidget(textBox, 1, 1);

		var setButton = new Button("Set");
		AddWidget(setButton, 2, 1);
		setButton.Pressed += SetParameters;

		BackButton = new Button("Back");
		AddWidget(BackButton, 3, 0);
	}

	public Button BackButton { get; private set; }

	public void SetElements(IEnumerable<Element> elements)
	{
		selectedElements = elements;
	}

	private void SetParameters(object sender, EventArgs e)
	{
		foreach (Element element in selectedElements)
		{
			element.SetParameter((int)numeric.Value, textBox.Text);
		}

		Engine.ExitSuccess("Parameters have been set");
	}
}