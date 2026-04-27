namespace InteractiveAutomationToolkitTests
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;
	using System.Collections.Generic;

	[TestClass]
	public class NumericTests
	{
		[TestMethod]
		public void OnChange_InvalidString()
		{
			var numeric = new Skyline.DataMiner.Utils.InteractiveAutomationScript.Numeric(10);

			List<Numeric.NumericChangedEventArgs> changedResults = new List<Numeric.NumericChangedEventArgs>();
			numeric.Changed += (s, e) => changedResults.Add(e);

			var mockedUiResults = new Mock<IUIResults>();
			mockedUiResults.Setup(x => x.GetString(It.IsAny<string>())).Returns("hello world");

			numeric.LoadResult(mockedUiResults.Object);
			numeric.RaiseResultEvents();

			Assert.AreEqual(0, changedResults.Count);
		}

		[TestMethod]
		public void OnChange_NoDecimals()
		{
			var numeric = new Skyline.DataMiner.Utils.InteractiveAutomationScript.Numeric(10);

			List<Numeric.NumericChangedEventArgs> changedResults = new List<Numeric.NumericChangedEventArgs>();
			numeric.Changed += (s, e) => changedResults.Add(e);

			var mockedUiResults = new Mock<IUIResults>();
			mockedUiResults.Setup(x => x.GetString(It.IsAny<string>())).Returns("10.01");

			numeric.LoadResult(mockedUiResults.Object);
			numeric.RaiseResultEvents();

			Assert.AreEqual(1, changedResults.Count);
			Assert.AreEqual(10, changedResults[0].Previous);
			Assert.AreEqual(10.01, changedResults[0].Value);
		}

		[TestMethod]
		public void OnChange_3Decimals()
		{
			var numeric = new Skyline.DataMiner.Utils.InteractiveAutomationScript.Numeric(10)
			{
				Decimals = 3,
			};

			List<Numeric.NumericChangedEventArgs> changedResults = new List<Numeric.NumericChangedEventArgs>();
			numeric.Changed += (s, e) => changedResults.Add(e);

			var mockedUiResults = new Mock<IUIResults>();
			mockedUiResults.Setup(x => x.GetString(It.IsAny<string>())).Returns("10.01");

			numeric.LoadResult(mockedUiResults.Object);
			numeric.RaiseResultEvents();

			Assert.AreEqual(1, changedResults.Count);
			Assert.AreEqual(10, changedResults[0].Previous);
			Assert.AreEqual(10.01, changedResults[0].Value);
		}

		[TestMethod]
		public void OnChange_StepSize()
		{
			var numeric = new Skyline.DataMiner.Utils.InteractiveAutomationScript.Numeric(10)
			{
				StepSize = 3,
			};

			List<Numeric.NumericChangedEventArgs> changedResults = new List<Numeric.NumericChangedEventArgs>();
			numeric.Changed += (s, e) => changedResults.Add(e);

			var mockedUiResults = new Mock<IUIResults>();
			mockedUiResults.Setup(x => x.GetString(It.IsAny<string>())).Returns("10.01");

			numeric.LoadResult(mockedUiResults.Object);
			numeric.RaiseResultEvents();

			Assert.AreEqual(1, changedResults.Count);
			Assert.AreEqual(10, changedResults[0].Previous);
			Assert.AreEqual(10.01, changedResults[0].Value);
		}

		[TestMethod]
		public void OnChange_SameValue_NoChangeExpected_1()
		{
			var numeric = new Skyline.DataMiner.Utils.InteractiveAutomationScript.Numeric(10);
			List<Numeric.NumericChangedEventArgs> changedResults = new List<Numeric.NumericChangedEventArgs>();
			numeric.Changed += (s, e) => changedResults.Add(e);
			var mockedUiResults = new Mock<IUIResults>();
			mockedUiResults.Setup(x => x.GetString(It.IsAny<string>())).Returns("10");
			numeric.LoadResult(mockedUiResults.Object);
			numeric.RaiseResultEvents();
			Assert.AreEqual(0, changedResults.Count);
		}

		[TestMethod]
		public void OnChange_SameValue_NoChangeExpected_2()
		{
			var numeric = new Skyline.DataMiner.Utils.InteractiveAutomationScript.Numeric { Value = 10 };
			List<Numeric.NumericChangedEventArgs> changedResults = new List<Numeric.NumericChangedEventArgs>();
			numeric.Changed += (s, e) => changedResults.Add(e);
			var mockedUiResults = new Mock<IUIResults>();
			mockedUiResults.Setup(x => x.GetString(It.IsAny<string>())).Returns("10");
			numeric.LoadResult(mockedUiResults.Object);
			numeric.RaiseResultEvents();
			Assert.AreEqual(0, changedResults.Count);
		}
	}
}
