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
    }
}