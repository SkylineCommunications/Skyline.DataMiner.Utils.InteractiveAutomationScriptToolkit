namespace InteractiveAutomationToolkitTests
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Moq;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	[TestClass]
	public class TimeTests
	{
		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void LoadResult_EmptyResult_KeepsValueAndDoesNotRaiseChanged(string received)
		{
			var initial = new TimeSpan(1, 6, 0, 3);
			var time = new Time(initial);
			bool changedRaised = false;
			time.Changed += (s, e) => changedRaised = true;

			var uiResults = new Mock<IUIResults>();
			uiResults.Setup(x => x.GetString(time.DestVar)).Returns(received);

			time.LoadResult(uiResults.Object, null);
			time.RaiseResultEvents();

			Assert.AreEqual(initial, time.TimeSpan);
			Assert.IsFalse(changedRaised);
		}

		[DataTestMethod]
		[DataRow("1.06:00:03", 1, 6, 0, 3)]
		[DataRow("2021-11-16T00:00:16.0000000", 0, 0, 0, 16)]
		public void LoadResult_ValidResult_IsParsed(string received, int days, int hours, int minutes, int seconds)
		{
			var time = new Time(TimeSpan.Zero);

			var uiResults = new Mock<IUIResults>();
			uiResults.Setup(x => x.GetString(time.DestVar)).Returns(received);

			time.LoadResult(uiResults.Object, null);

			Assert.AreEqual(new TimeSpan(days, hours, minutes, seconds), time.TimeSpan);
		}
	}
}
