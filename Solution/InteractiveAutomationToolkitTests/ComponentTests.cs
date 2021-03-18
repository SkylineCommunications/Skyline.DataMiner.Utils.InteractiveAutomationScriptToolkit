namespace InteractiveAutomationToolkitTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit;

	[TestClass]
	public class ComponentTests
	{
		/// <summary>
		/// Checks if the WantsOnChange property of a Button is correctly updated when the Pressed event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeButtonOnPressedEvent()
		{
			Button button = new Button("Button 1");
			Assert.IsFalse(button.WantsOnChange);

			button.Pressed += Button_Pressed;
			Assert.IsTrue(button.WantsOnChange);

			button.Pressed -= Button_Pressed;
			Assert.IsFalse(button.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a CheckBox is correctly updated when the Changed event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeCheckBoxOnChangedEvent()
		{
			CheckBox checkBox = new CheckBox();
			Assert.IsFalse(checkBox.WantsOnChange);

			checkBox.Changed += CheckBox_Changed;
			Assert.IsTrue(checkBox.WantsOnChange);

			checkBox.Changed -= CheckBox_Changed;
			Assert.IsFalse(checkBox.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a CheckBox is correctly updated when the Checked event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeCheckBoxOnCheckedEvent()
		{
			CheckBox checkBox = new CheckBox();
			Assert.IsFalse(checkBox.WantsOnChange);

			checkBox.Checked += CheckBox_Checked;
			Assert.IsTrue(checkBox.WantsOnChange);

			checkBox.Checked -= CheckBox_Checked;
			Assert.IsFalse(checkBox.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a CheckBox is correctly updated when the UnChecked event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeCheckBoxOnUnCheckedEvent()
		{
			CheckBox checkBox = new CheckBox();
			Assert.IsFalse(checkBox.WantsOnChange);

			checkBox.UnChecked += CheckBox_UnChecked;
			Assert.IsTrue(checkBox.WantsOnChange);

			checkBox.UnChecked -= CheckBox_UnChecked;
			Assert.IsFalse(checkBox.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a CheckBoxList is correctly updated when the UnChecked event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeCheckBoxListOnChangedEvent()
		{
			string[] options = new string[] { "Option1", "Option2", "Option3" };
			CheckBoxList checkBoxList = new CheckBoxList(options);
			Assert.IsFalse(checkBoxList.WantsOnChange);

			checkBoxList.Changed += CheckBoxList_Changed;
			Assert.IsTrue(checkBoxList.WantsOnChange);

			checkBoxList.Changed -= CheckBoxList_Changed;
			Assert.IsFalse(checkBoxList.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a CollapseButton is correctly updated when the Pressed event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeCollapseButtonOnPressedEvent()
		{
			IEnumerable<Widget> widgets = new Widget[] { new Label("Label1"), new Label("Label2") };
			CollapseButton collapseButton = new CollapseButton(widgets, false);
			Assert.IsTrue(collapseButton.WantsOnChange);

			collapseButton.Pressed += CollapseButton_Pressed;
			Assert.IsTrue(collapseButton.WantsOnChange);

			collapseButton.Pressed -= CollapseButton_Pressed;
			Assert.IsTrue(collapseButton.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a DateTimePicker is correctly updated when the Changed event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeDateTimePickerOnChangedEvent()
		{
			DateTimePicker dateTimePicker = new DateTimePicker();
			Assert.IsFalse(dateTimePicker.WantsOnChange);

			dateTimePicker.Changed += DateTimePicker_Changed;
			Assert.IsTrue(dateTimePicker.WantsOnChange);

			dateTimePicker.Changed -= DateTimePicker_Changed;
			Assert.IsFalse(dateTimePicker.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a DropDown is correctly updated when the Changed event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeDropDownOnChangedEvent()
		{
			string[] options = new string[] { "Option1", "Option2", "Option3" };
			DropDown dropDown = new DropDown(options);
			Assert.IsFalse(dropDown.WantsOnChange);

			dropDown.Changed += DropDown_Changed;
			Assert.IsTrue(dropDown.WantsOnChange);

			dropDown.Changed -= DropDown_Changed;
			Assert.IsFalse(dropDown.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a Numeric is correctly updated when the Changed event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeNumericOnChangedEvent()
		{
			Numeric numeric = new Numeric();
			Assert.IsFalse(numeric.WantsOnChange);

			numeric.Changed += Numeric_Changed;
			Assert.IsTrue(numeric.WantsOnChange);

			numeric.Changed -= Numeric_Changed;
			Assert.IsFalse(numeric.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a RadioButtonList is correctly updated when the Changed event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeRadioButtonListOnChangedEvent()
		{
			string[] options = new string[] { "Option1", "Option2", "Option3" };
			RadioButtonList radioButtonList = new RadioButtonList(options);
			Assert.IsFalse(radioButtonList.WantsOnChange);

			radioButtonList.Changed += RadioButtonList_Changed;
			Assert.IsTrue(radioButtonList.WantsOnChange);

			radioButtonList.Changed -= RadioButtonList_Changed;
			Assert.IsFalse(radioButtonList.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a TextBox is correctly updated when the Changed event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeTextBoxOnChangedEvent()
		{
			TextBox textBox = new TextBox();
			Assert.IsFalse(textBox.WantsOnChange);

			textBox.Changed += TextBox_Changed;
			Assert.IsTrue(textBox.WantsOnChange);

			textBox.Changed -= TextBox_Changed;
			Assert.IsFalse(textBox.WantsOnChange);
		}

		/// <summary>
		/// Checks if the WantsOnChange property of a TimePicker is correctly updated when the Changed event is subscribed and unsubcribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeTimePickerOnChangedEvent()
		{
			TimePicker timePicker = new TimePicker();
			Assert.IsFalse(timePicker.WantsOnChange);

			timePicker.Changed += TimePicker_Changed;
			Assert.IsTrue(timePicker.WantsOnChange);

			timePicker.Changed -= TimePicker_Changed;
			Assert.IsFalse(timePicker.WantsOnChange);
		}

		/// <summary>
		/// Checks if the methods to manipulate the list of options on a dropdown are working as expected.
		/// </summary>
		[TestMethod]
		public void DropdownSetOptionsSelected()
		{
			string[] options = new string[] { "option1", "option2", "option3" };

			DropDown dropDown1 = new DropDown(options);
			Assert.AreEqual("option1", dropDown1.Selected);

			DropDown dropDown2 = new DropDown();
			dropDown2.SetOptions(options);
			Assert.AreEqual("option1", dropDown2.Selected);

			dropDown1.RemoveOption("option1");
			Assert.AreNotEqual("option1", dropDown1.Selected);

			dropDown1.SetOptions(options);
			Assert.AreEqual("option2", dropDown1.Selected);
		}

		[TestMethod]
		public void TestWidgetMargins()
		{
			Button button = new Button("Button");
			Assert.AreEqual(0, button.Margin.Left);

			button.Margin = new Margin(10, 5, 2, 1);
			Assert.AreEqual(2, button.Margin.Right);
		}

		[TestMethod]
		public void TestSection()
		{
			TestSection section = new TestSection();
			section.AddWidget(new Label("Label 1"), 0, 0);
			section.AddWidget(new Label("Label 2"), 1, 0);

			Assert.AreEqual(2, section.RowCount);
			Assert.AreEqual(1, section.ColumnCount);

			section.AddWidget(new Label("Label 3"), 3, 1);

			Assert.AreEqual(4, section.RowCount);
			Assert.AreEqual(2, section.ColumnCount);

			Assert.AreEqual(3, section.Widgets.Count());

			section.Clear();

			Assert.AreEqual(0, section.Widgets.Count());
		}

		[TestMethod]
		public void RemoveWidgetsFromSection()
		{
			TestSection section = new TestSection();
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");

			section.AddWidget(label1, 0, 0);
			section.AddWidget(label2, 1, 0);

			Assert.AreEqual(2, section.Widgets.Count());
			Assert.AreEqual(2, section.RowCount);
			Assert.AreEqual(1, section.ColumnCount);

			section.RemoveWidget(label2);
			Assert.AreEqual(1, section.Widgets.Count());
			Assert.AreEqual(1, section.RowCount);
			Assert.AreEqual(1, section.ColumnCount);

			section.RemoveWidget(label1);
			Assert.AreEqual(0, section.Widgets.Count());
			Assert.AreEqual(0, section.RowCount);
			Assert.AreEqual(0, section.ColumnCount);
		}

		[TestMethod]
		public void RecreateUiBlockTest()
		{
			Exception exception = null;
			try
			{
				string[] options = new string[] { "option 1", "option 2", "option 3" };
				DropDown dropDown = new DropDown();
				dropDown.RemoveOption(options.First());

				dropDown.SetOptions(new string[] { "option 4", "option 5", "option 6" });
			}
			catch(Exception e)
			{
				exception = e;
			}

			Assert.IsNull(exception);
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
	}

	public class TestSection : Section
	{
	}
}
