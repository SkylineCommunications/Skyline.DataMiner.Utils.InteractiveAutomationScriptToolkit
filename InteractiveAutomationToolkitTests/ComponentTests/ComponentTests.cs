namespace InteractiveAutomationToolkitTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.Utils.InteractiveAutomationToolkit;

	[TestClass]
	public class ComponentTests
	{
		[TestMethod]
		public void DropDownFirstOptionSelectedAfterAdd()
		{
			var dropDown = new DropDown();
			dropDown.Options.Add("a");
			dropDown.Options.Add("b");
			Assert.AreEqual("a", dropDown.Selected);
		}

		[TestMethod]
		public void DropDownFirstOptionSelectedAfterSet()
		{
			var dropDown = new DropDown();
			dropDown.SetOptions(new[] { "a", "b" });
			Assert.AreEqual("a", dropDown.Selected);
		}

		[TestMethod]
		public void DropDownFirstOptionSelectedAtConstruction()
		{
			var dropDown = new DropDown(new[] { "a", "b" });
			Assert.AreEqual("a", dropDown.Selected);
		}

		[TestMethod]
		public void DropDownNoNullSelectedAfterRemove()
		{
			var dropDown = new DropDown(new[] { "a", "b" }, "a");
			dropDown.Options.Remove("a");
			Assert.AreEqual("b", dropDown.Selected);
		}

		[TestMethod]
		public void DropDownNullSelectedAfterClear()
		{
			var dropDown = new DropDown(new[] { "a", "b" });
			dropDown.Options.Clear();
			Assert.AreEqual(null, dropDown.Selected);
		}

		/// <summary>
		///     Checks if the methods to manipulate the list of options on a dropdown are working as expected.
		/// </summary>
		[TestMethod]
		public void DropdownSetOptionsSelected()
		{
			string[] options = { "option1", "option2", "option3" };

			var dropDown1 = new DropDown(options);
			Assert.AreEqual("option1", dropDown1.Selected);

			var dropDown2 = new DropDown();
			dropDown2.SetOptions(options);
			Assert.AreEqual("option1", dropDown2.Selected);

			dropDown1.RemoveOption("option1");
			Assert.AreNotEqual("option1", dropDown1.Selected);

			dropDown1.SetOptions(options);
			Assert.AreEqual("option2", dropDown1.Selected);
		}

		[TestMethod]
		public void RecreateUiBlockTest()
		{
			Exception exception = null;
			try
			{
				string[] options = { "option 1", "option 2", "option 3" };
				var dropDown = new DropDown();
				dropDown.RemoveOption(options.First());

				dropDown.SetOptions(new[] { "option 4", "option 5", "option 6" });
			}
			catch (Exception e)
			{
				exception = e;
			}

			Assert.IsNull(exception);
		}

		[TestMethod]
		public void TestWidgetMargins()
		{
			var button = new Button("Button");
			Assert.AreEqual(new Margin(4), button.Margin);

			var expected = new Margin(10, 5, 2, 1);
			button.Margin = expected;
			Assert.AreEqual(expected, button.Margin);
		}

		/// <summary>
		///     Checks if the WantsOnChange property of a Button is correctly updated when the Pressed event is subscribed and
		///     unsubscribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeButtonOnPressedEvent()
		{
			var button = new Button("Button 1");
			Assert.IsFalse(button.WantsOnChange);

			button.Pressed += DoNothing;
			Assert.IsTrue(button.WantsOnChange);

			button.Pressed -= DoNothing;
			Assert.IsFalse(button.WantsOnChange);
		}

		/// <summary>
		///     Checks if the WantsOnChange property of a Calendar is correctly updated when the Changed event is subscribed and
		///     unsubscribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeCalendarOnChangedEvent()
		{
			var calendar = new Calendar();
			Assert.IsFalse(calendar.WantsOnChange);

			calendar.Changed += DoNothing;
			Assert.IsTrue(calendar.WantsOnChange);

			calendar.Changed -= DoNothing;
			Assert.IsFalse(calendar.WantsOnChange);
		}

		/// <summary>
		///     Checks if the WantsOnChange property of a CheckBoxList is correctly updated when the UnChecked event is subscribed
		///     and unsubscribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeCheckBoxListOnChangedEvent()
		{
			string[] options = { "Option1", "Option2", "Option3" };
			var checkBoxList = new CheckBoxList(options);
			Assert.IsFalse(checkBoxList.WantsOnChange);

			checkBoxList.Changed += DoNothing;
			Assert.IsTrue(checkBoxList.WantsOnChange);

			checkBoxList.Changed -= DoNothing;
			Assert.IsFalse(checkBoxList.WantsOnChange);
		}

		/// <summary>
		///     Checks if the WantsOnChange property of a CheckBox is correctly updated when the Changed event is subscribed and
		///     unsubscribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeCheckBoxOnChangedEvent()
		{
			var checkBox = new CheckBox();
			Assert.IsFalse(checkBox.WantsOnChange);

			checkBox.Changed += DoNothing;
			Assert.IsTrue(checkBox.WantsOnChange);

			checkBox.Changed -= DoNothing;
			Assert.IsFalse(checkBox.WantsOnChange);
		}

		/// <summary>
		///     Checks if the WantsOnChange property of a CheckBox is correctly updated when the Checked event is subscribed and
		///     unsubscribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeCheckBoxOnCheckedEvent()
		{
			var checkBox = new CheckBox();
			Assert.IsFalse(checkBox.WantsOnChange);

			checkBox.Checked += DoNothing;
			Assert.IsTrue(checkBox.WantsOnChange);

			checkBox.Checked -= DoNothing;
			Assert.IsFalse(checkBox.WantsOnChange);
		}

		/// <summary>
		///     Checks if the WantsOnChange property of a CheckBox is correctly updated when the UnChecked event is subscribed and
		///     unsubscribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeCheckBoxOnUnCheckedEvent()
		{
			var checkBox = new CheckBox();
			Assert.IsFalse(checkBox.WantsOnChange);

			checkBox.UnChecked += DoNothing;
			Assert.IsTrue(checkBox.WantsOnChange);

			checkBox.UnChecked -= DoNothing;
			Assert.IsFalse(checkBox.WantsOnChange);
		}

		/// <summary>
		///     Checks if the WantsOnChange property of a CollapseButton is correctly updated when the Pressed event is subscribed
		///     and unsubscribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeCollapseButtonOnPressedEvent()
		{
			IEnumerable<IWidget> widgets = new IWidget[] { new Label("Label1"), new Label("Label2") };
			var collapseButton = new CollapseButton(widgets, false);
			Assert.IsTrue(collapseButton.WantsOnChange);

			collapseButton.Pressed += DoNothing;
			Assert.IsTrue(collapseButton.WantsOnChange);

			collapseButton.Pressed -= DoNothing;
			Assert.IsTrue(collapseButton.WantsOnChange);
		}

		/// <summary>
		///     Checks if the WantsOnChange property of a DateTimePicker is correctly updated when the Changed event is subscribed
		///     and unsubscribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeDateTimePickerOnChangedEvent()
		{
			var dateTimePicker = new DateTimePicker();
			Assert.IsFalse(dateTimePicker.WantsOnChange);

			dateTimePicker.Changed += DoNothing;
			Assert.IsTrue(dateTimePicker.WantsOnChange);

			dateTimePicker.Changed -= DoNothing;
			Assert.IsFalse(dateTimePicker.WantsOnChange);
		}

		/// <summary>
		///     Checks if the WantsOnChange property of a DropDown is correctly updated when the Changed event is subscribed and
		///     unsubscribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeDropDownOnChangedEvent()
		{
			string[] options = { "Option1", "Option2", "Option3" };
			var dropDown = new DropDown(options);
			Assert.IsFalse(dropDown.WantsOnChange);

			dropDown.Changed += DoNothing;
			Assert.IsTrue(dropDown.WantsOnChange);

			dropDown.Changed -= DoNothing;
			Assert.IsFalse(dropDown.WantsOnChange);
		}

		/// <summary>
		///     Checks if the WantsOnChange property of a Numeric is correctly updated when the Changed event is subscribed and
		///     unsubscribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeNumericOnChangedEvent()
		{
			var numeric = new Numeric();
			Assert.IsFalse(numeric.WantsOnChange);

			numeric.Changed += DoNothing;
			Assert.IsTrue(numeric.WantsOnChange);

			numeric.Changed -= DoNothing;
			Assert.IsFalse(numeric.WantsOnChange);
		}

		/// <summary>
		///     Checks if the WantsOnChange property of a RadioButtonList is correctly updated when the Changed event is subscribed
		///     and unsubscribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeRadioButtonListOnChangedEvent()
		{
			string[] options = { "Option1", "Option2", "Option3" };
			var radioButtonList = new RadioButtonList(options);
			Assert.IsFalse(radioButtonList.WantsOnChange);

			radioButtonList.Changed += DoNothing;
			Assert.IsTrue(radioButtonList.WantsOnChange);

			radioButtonList.Changed -= DoNothing;
			Assert.IsFalse(radioButtonList.WantsOnChange);
		}

		/// <summary>
		///     Checks if the WantsOnChange property of a TextBox is correctly updated when the Changed event is subscribed and
		///     unsubscribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeTextBoxOnChangedEvent()
		{
			var textBox = new TextBox();
			Assert.IsFalse(textBox.WantsOnChange);

			textBox.Changed += DoNothing;
			Assert.IsTrue(textBox.WantsOnChange);

			textBox.Changed -= DoNothing;
			Assert.IsFalse(textBox.WantsOnChange);
		}

		/// <summary>
		///     Checks if the WantsOnChange property of a TimePicker is correctly updated when the Changed event is subscribed and
		///     unsubscribed to.
		/// </summary>
		[TestMethod]
		public void WantsOnChangeTimePickerOnChangedEvent()
		{
			var timePicker = new TimePicker();
			Assert.IsFalse(timePicker.WantsOnChange);

			timePicker.Changed += DoNothing;
			Assert.IsTrue(timePicker.WantsOnChange);

			timePicker.Changed -= DoNothing;
			Assert.IsFalse(timePicker.WantsOnChange);
		}

		private void DoNothing(object sender, EventArgs e)
		{
			// do nothing
		}
	}
}