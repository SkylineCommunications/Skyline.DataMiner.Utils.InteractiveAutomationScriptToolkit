using Microsoft.VisualStudio.TestTools.UnitTesting;

using Skyline.DataMiner.Utils.InteractiveAutomationScript;

namespace InteractiveAutomationToolkitTests
{
	[TestClass]
	public class RadioButtonListTests
	{
		[TestMethod]
		public void SetOptionsTest_NothingSelected()
		{
			var options = new[] { "1", "2" };
			var radioButtonList = new RadioButtonList(options);

			Assert.IsNull(radioButtonList.Selected);

			radioButtonList.SetOptions(new[] { "3", "4" });

			Assert.IsNull(radioButtonList.Selected);
		}

		[TestMethod]
		public void SetOptionsTest_SameOptions()
		{
			var options = new[] { "1", "2" };
			var radioButtonList = new RadioButtonList(options);

			Assert.IsNull(radioButtonList.Selected);

			radioButtonList.SetOptions(new[] { "1", "2" });

			Assert.IsNull(radioButtonList.Selected);

			radioButtonList.Selected = "2";

			radioButtonList.SetOptions(new[] { "1", "2" });

			Assert.AreEqual("2", radioButtonList.Selected);
		}

		[TestMethod]
		public void SetOptionsTest_OverwriteSelected()
		{
			var options = new[] { "1", "2" };
			var radioButtonList = new RadioButtonList(options, "1");

			Assert.AreEqual("1", radioButtonList.Selected);

			radioButtonList.SetOptions(new[] { "3", "4" });

            Assert.IsNull(radioButtonList.Selected);
        }

        [TestMethod]
        public void ContainsOption_ExistingOption_ReturnsTrue()
        {
            var radioButtonList = new RadioButtonList(new[] { "a", "b", "c" });

            Assert.IsTrue(radioButtonList.ContainsOption("b"));
        }

        [TestMethod]
        public void ContainsOption_MissingOption_ReturnsFalse()
        {
            var radioButtonList = new RadioButtonList(new[] { "a", "b", "c" });

            Assert.IsFalse(radioButtonList.ContainsOption("z"));
        }

        [TestMethod]
        public void TrySelectOption_ExistingOption_SelectsAndReturnsTrue()
        {
            var radioButtonList = new RadioButtonList(new[] { "a", "b", "c" }, "a");

            bool result = radioButtonList.TrySelectOption("c");

            Assert.IsTrue(result);
            Assert.AreEqual("c", radioButtonList.Selected);
        }

        [TestMethod]
        public void TrySelectOption_MissingOption_ReturnsFalseAndKeepsSelection()
        {
            var radioButtonList = new RadioButtonList(new[] { "a", "b", "c" }, "a");

            bool result = radioButtonList.TrySelectOption("z");

            Assert.IsFalse(result);
            Assert.AreEqual("a", radioButtonList.Selected);
        }
    }
}