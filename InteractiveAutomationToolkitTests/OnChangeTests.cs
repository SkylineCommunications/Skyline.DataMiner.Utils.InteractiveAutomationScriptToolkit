namespace InteractiveAutomationToolkitTests
{
	using System;
	using System.Collections.Generic;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Moq;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	[TestClass]
	public class OnChangeTests
	{
		/// <summary>
		/// Checks if the WantsOnChange property of a Button is correctly updated when the Pressed event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeButtonOnPressedEvent()
		{
			Button button = new Button("Button 1");
			Assert.IsFalse(button.BlockDefinition.WantsOnChange);

			button.Pressed += Button_Pressed;
			Assert.IsTrue(button.BlockDefinition.WantsOnChange);

			button.Pressed -= Button_Pressed;
			Assert.IsFalse(button.BlockDefinition.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a CheckBox is correctly updated when the Changed event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeCheckBoxOnChangedEvent()
		{
			CheckBox checkBox = new CheckBox();
			Assert.IsFalse(checkBox.BlockDefinition.WantsOnChange);

			checkBox.Changed += CheckBox_Changed;
			Assert.IsTrue(checkBox.BlockDefinition.WantsOnChange);

			checkBox.Changed -= CheckBox_Changed;
			Assert.IsFalse(checkBox.BlockDefinition.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a CheckBox is correctly updated when the Checked event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeCheckBoxOnCheckedEvent()
		{
			CheckBox checkBox = new CheckBox();
			Assert.IsFalse(checkBox.BlockDefinition.WantsOnChange);

			checkBox.Checked += CheckBox_Checked;
			Assert.IsTrue(checkBox.BlockDefinition.WantsOnChange);

			checkBox.Checked -= CheckBox_Checked;
			Assert.IsFalse(checkBox.BlockDefinition.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a CheckBox is correctly updated when the UnChecked event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeCheckBoxOnUnCheckedEvent()
		{
			CheckBox checkBox = new CheckBox();
			Assert.IsFalse(checkBox.BlockDefinition.WantsOnChange);

			checkBox.UnChecked += CheckBox_UnChecked;
			Assert.IsTrue(checkBox.BlockDefinition.WantsOnChange);

			checkBox.UnChecked -= CheckBox_UnChecked;
			Assert.IsFalse(checkBox.BlockDefinition.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a CheckBoxList is correctly updated when the UnChecked event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeCheckBoxListOnChangedEvent()
		{
			string[] options = new string[] { "Option1", "Option2", "Option3" };
			CheckBoxList checkBoxList = new CheckBoxList(options);
			Assert.IsFalse(checkBoxList.BlockDefinition.WantsOnChange);

			checkBoxList.Changed += CheckBoxList_Changed;
			Assert.IsTrue(checkBoxList.BlockDefinition.WantsOnChange);

			checkBoxList.Changed -= CheckBoxList_Changed;
			Assert.IsFalse(checkBoxList.BlockDefinition.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a CollapseButton is correctly updated when the Pressed event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeCollapseButtonOnPressedEvent()
		{
			IEnumerable<Widget> widgets = new Widget[] { new Label("Label1"), new Label("Label2") };
			CollapseButton collapseButton = new CollapseButton(widgets, false);
			Assert.IsTrue(collapseButton.BlockDefinition.WantsOnChange);

			collapseButton.Pressed += CollapseButton_Pressed;
			Assert.IsTrue(collapseButton.BlockDefinition.WantsOnChange);

			collapseButton.Pressed -= CollapseButton_Pressed;
			Assert.IsTrue(collapseButton.BlockDefinition.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a Calendar is correctly updated when the Changed event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeCalendarOnChangedEvent()
		{
			Calendar calendar = new Calendar();
			Assert.IsFalse(calendar.BlockDefinition.WantsOnChange);

			calendar.Changed += Calendar_Changed;
			Assert.IsTrue(calendar.BlockDefinition.WantsOnChange);

			calendar.Changed -= Calendar_Changed;
			Assert.IsFalse(calendar.BlockDefinition.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a DateTimePicker is correctly updated when the Changed event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeDateTimePickerOnChangedEvent()
		{
			DateTimePicker dateTimePicker = new DateTimePicker();
			Assert.IsFalse(dateTimePicker.BlockDefinition.WantsOnChange);

			dateTimePicker.Changed += DateTimePicker_Changed;
			Assert.IsTrue(dateTimePicker.BlockDefinition.WantsOnChange);

			dateTimePicker.Changed -= DateTimePicker_Changed;
			Assert.IsFalse(dateTimePicker.BlockDefinition.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a DropDown is correctly updated when the Changed event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeDropDownOnChangedEvent()
		{
			string[] options = new string[] { "Option1", "Option2", "Option3" };
			DropDown dropDown = new DropDown(options);
			Assert.IsFalse(dropDown.BlockDefinition.WantsOnChange);

			dropDown.Changed += DropDown_Changed;
			Assert.IsTrue(dropDown.BlockDefinition.WantsOnChange);

			dropDown.Changed -= DropDown_Changed;
			Assert.IsFalse(dropDown.BlockDefinition.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a Numeric is correctly updated when the Changed event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeNumericOnChangedEvent()
		{
			Numeric numeric = new Numeric();
			Assert.IsFalse(numeric.BlockDefinition.WantsOnChange);

			numeric.Changed += Numeric_Changed;
			Assert.IsTrue(numeric.BlockDefinition.WantsOnChange);

			numeric.Changed -= Numeric_Changed;
			Assert.IsFalse(numeric.BlockDefinition.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a RadioButtonList is correctly updated when the Changed event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeRadioButtonListOnChangedEvent()
		{
			string[] options = new string[] { "Option1", "Option2", "Option3" };
			RadioButtonList radioButtonList = new RadioButtonList(options);
			Assert.IsFalse(radioButtonList.BlockDefinition.WantsOnChange);

			radioButtonList.Changed += RadioButtonList_Changed;
			Assert.IsTrue(radioButtonList.BlockDefinition.WantsOnChange);

			radioButtonList.Changed -= RadioButtonList_Changed;
			Assert.IsFalse(radioButtonList.BlockDefinition.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a TextBox is correctly updated when the Changed event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeTextBoxOnChangedEvent()
		{
			TextBox textBox = new TextBox();
			Assert.IsFalse(textBox.BlockDefinition.WantsOnChange);

			textBox.Changed += TextBox_Changed;
			Assert.IsTrue(textBox.BlockDefinition.WantsOnChange);

			textBox.Changed -= TextBox_Changed;
			Assert.IsFalse(textBox.BlockDefinition.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a TimePicker is correctly updated when the Changed event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeTimePickerOnChangedEvent()
		{
			TimePicker timePicker = new TimePicker();
			Assert.IsFalse(timePicker.BlockDefinition.WantsOnChange);

			timePicker.Changed += TimePicker_Changed;
			Assert.IsTrue(timePicker.BlockDefinition.WantsOnChange);

			timePicker.Changed -= TimePicker_Changed;
			Assert.IsFalse(timePicker.BlockDefinition.WantsOnChange);
		}

		/// <summary>
		/// Checks if the ReturnWhenDownloadIsStarted property of a DownloadButton is correctly updated when the ReturnWhenDownloadIsStarted event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void ReturnOnDownloadIsStartedOnDownloadStartedEvent()
		{
			DownloadButton downloadButton = new DownloadButton();
			var configOptions = downloadButton.BlockDefinition.ConfigOptions as AutomationDownloadButtonOptions;
			Assert.IsFalse(configOptions.ReturnWhenDownloadIsStarted);

			downloadButton.DownloadStarted += DownloadButton_DownloadStarted;
			Assert.IsTrue(configOptions.ReturnWhenDownloadIsStarted);

			downloadButton.DownloadStarted -= DownloadButton_DownloadStarted;
			Assert.IsFalse(configOptions.ReturnWhenDownloadIsStarted);
		}

		[TestMethod]
		public void DoNotTriggerOnChangeIfValueChangesInCode()
		{
			var startDateTime = DateTime.Now;

			int days = 1;
			var dateTime = startDateTime.AddDays(days);

			var mockedEngine = new Mock<IEngine>();

			DateTimePicker picker = new DateTimePicker(dateTime);
			Numeric numeric = new Numeric(days);

			var mockedUiResults = new Mock<IUIResults>();
			mockedUiResults.Setup(x => x.GetString(numeric.DestVar)).Returns((days + 1).ToString()); // User changes Numeric to 2
			mockedUiResults.Setup(x => x.GetString(picker.DestVar)).Returns(dateTime.ToString("O"));

			var testDialog = new TestDialog(mockedEngine.Object);
			testDialog.AddWidget(picker, 0, 0);
			testDialog.AddWidget(numeric, 1, 0);

			var numericChangedEvents = new List<Numeric.NumericChangedEventArgs>();
			var dateTimePickerChangedEvents = new List<DateTimePicker.DateTimePickerChangedEventArgs>();

			numeric.Changed += (s, e) =>
			{
				numericChangedEvents.Add(e);

				days = (int)e.Value;
				dateTime = startDateTime.AddDays(days);

				picker.DateTime = dateTime;
			};

			picker.Changed += (s, e) =>
			{
				dateTimePickerChangedEvents.Add(e);
			};

			testDialog.LoadChanges(mockedUiResults.Object);
			testDialog.RaiseResultEvents(mockedUiResults.Object);

			mockedUiResults.Setup(x => x.GetString(numeric.DestVar)).Returns((days + 1).ToString()); // User changes Numeric to 3
			mockedUiResults.Setup(x => x.GetString(picker.DestVar)).Returns(dateTime.ToString("O"));

			testDialog.LoadChanges(mockedUiResults.Object);
			testDialog.RaiseResultEvents(mockedUiResults.Object);

			Assert.AreEqual(2, numericChangedEvents.Count);
			Assert.AreEqual(0, dateTimePickerChangedEvents.Count);
		}

		private sealed class TestDialog : Dialog
		{
			public TestDialog(IEngine engine) : base(engine)
			{
			}
		}

		private void Button_Pressed(object sender, EventArgs e)
		{
			// do nothing
		}

		private void CheckBox_Changed(object sender, CheckBox.CheckBoxChangedEventArgs e)
		{
			// do nothing
		}

		private void CheckBox_Checked(object sender, EventArgs e)
		{
			// do nothing
		}

		private void CheckBox_UnChecked(object sender, EventArgs e)
		{
			// do nothing
		}

		private void CheckBoxList_Changed(object sender, CheckBoxList.CheckBoxListChangedEventArgs e)
		{
			// do nothing
		}

		private void CollapseButton_Pressed(object sender, EventArgs e)
		{
			// do nothing
		}

		private void Calendar_Changed(object sender, Calendar.CalendarChangedEventArgs e)
		{
			// do nothing
		}

		private void DateTimePicker_Changed(object sender, DateTimePicker.DateTimePickerChangedEventArgs e)
		{
			// do nothing
		}

		private void DropDown_Changed(object sender, DropDown.DropDownChangedEventArgs e)
		{
			// do nothing
		}

		private void Numeric_Changed(object sender, Numeric.NumericChangedEventArgs e)
		{
			// do nothing
		}

		private void RadioButtonList_Changed(object sender, RadioButtonList.RadioButtonChangedEventArgs e)
		{
			// do nothing
		}

		private void TextBox_Changed(object sender, TextBox.TextBoxChangedEventArgs e)
		{
			// do nothing
		}

		private void TimePicker_Changed(object sender, TimePicker.TimePickerChangedEventArgs e)
		{
			// do nothing
		}

		private void DownloadButton_DownloadStarted(object sender, EventArgs e)
		{
			// do nothing
		}
	}
}
