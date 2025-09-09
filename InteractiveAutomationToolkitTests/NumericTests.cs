namespace InteractiveAutomationToolkitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Skyline.DataMiner.Utils.InteractiveAutomationScript;
    using System;
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
        }
    }
}
