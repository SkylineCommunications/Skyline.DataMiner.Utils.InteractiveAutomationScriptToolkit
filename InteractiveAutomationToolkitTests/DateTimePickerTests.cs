namespace InteractiveAutomationToolkitTests
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	[TestClass]
	public class DateTimePickerTests
	{
		[TestMethod]
		public void VerifyDefaultValues()
		{
			var dateTime = DateTime.Now;
			var dateTimePicker = new DateTimePicker(dateTime);

			Assert.AreEqual(false, dateTimePicker.DisplayServerTime);
			Assert.AreEqual(dateTime, dateTimePicker.DateTime);
			Assert.AreEqual(false, dateTimePicker.AutoCloseCalendar);
			Assert.AreEqual(DateTime.MinValue, dateTimePicker.Minimum);
			Assert.AreEqual(DateTime.MaxValue, dateTimePicker.Maximum);

			Assert.AreEqual(null, dateTimePicker.Tooltip);

			Assert.AreEqual(CalendarMode.Month, dateTimePicker.CalendarDisplayMode);
			Assert.AreEqual(true, dateTimePicker.HasDropDownButton);
			Assert.AreEqual(true, dateTimePicker.IsTimePickerVisible);
			Assert.AreEqual(true, dateTimePicker.HasTimePickerSpinnerButton);
			Assert.AreEqual(true, dateTimePicker.IsTimePickerSpinnerButtonEnabled);

			// InteractiveWidget
			Assert.AreEqual(true, dateTimePicker.IsEnabled);

			// Widget
			Assert.AreEqual(-1, dateTimePicker.Height);
			Assert.AreEqual(-1, dateTimePicker.MinHeight);
			Assert.AreEqual(-1, dateTimePicker.MaxHeight);
			Assert.AreEqual(-1, dateTimePicker.Width);
			Assert.AreEqual(-1, dateTimePicker.MaxWidth);
			Assert.AreEqual(-1, dateTimePicker.MinWidth);

			Assert.AreEqual(true, dateTimePicker.IsVisible);
			Assert.AreEqual(null, dateTimePicker.DebugTag);

			// IIsReadonlyWidget
			Assert.AreEqual(false, dateTimePicker.IsReadOnly);
			Assert.AreEqual(false, dateTimePicker.RequiresResponse);

			// TimePickerBase
			Assert.AreEqual(true, dateTimePicker.IsSpinnerButtonEnabled);
			Assert.AreEqual(true, dateTimePicker.HasSpinnerButton);
			Assert.AreEqual(false, dateTimePicker.ShowSeconds);
			Assert.AreEqual(false, dateTimePicker.UpdateOnEnter);
			Assert.AreEqual(DateTimeFormat.Custom, dateTimePicker.DateTimeFormat);
			Assert.AreEqual("G", dateTimePicker.CustomDateTimeFormat);
			Assert.AreEqual(DateTimeKind.Unspecified, dateTimePicker.Kind);
			Assert.AreEqual(false, dateTimePicker.ClipValueToRange);

			// IValidationWidget
			Assert.AreEqual(UIValidationState.NotValidated, dateTimePicker.ValidationState);
			Assert.AreEqual("Invalid Input", dateTimePicker.ValidationText);
		}
	}
}
