namespace InteractiveAutomationToolkitTests
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Moq;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	[TestClass]
	public class TimePickerTests
	{
		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void LoadResult_EmptyResult_KeepsValueAndDoesNotRaiseChanged(string received)
		{
			var initial = new TimeSpan(13, 45, 0);
			var timePicker = new TimePicker(initial);
			bool changedRaised = false;
			timePicker.Changed += (s, e) => changedRaised = true;

			var uiResults = new Mock<IUIResults>();
			uiResults.Setup(x => x.GetString(timePicker.DestVar)).Returns(received);

			timePicker.LoadResult(uiResults.Object, null);
			timePicker.RaiseResultEvents();

			Assert.AreEqual(initial, timePicker.Time);
			Assert.IsFalse(changedRaised);
		}

		[TestMethod]
		public void LoadResult_ValidResult_IsParsed()
		{
			var timePicker = new TimePicker(TimeSpan.Zero);

			var uiResults = new Mock<IUIResults>();
			uiResults.Setup(x => x.GetString(timePicker.DestVar)).Returns("2021-11-16T13:45:00");

			timePicker.LoadResult(uiResults.Object, null);

			Assert.AreEqual(new TimeSpan(13, 45, 0), timePicker.Time);
		}
	}
}
