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
    }
}