using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skyline.DataMiner.Utils.InteractiveAutomationScript;

namespace InteractiveAutomationToolkitTests
{
    [TestClass]
    public class CheckBoxListTests
    {
        [TestMethod]
        public void ContainsOption_ExistingOption_ReturnsTrue()
        {
            var checkBoxList = new CheckBoxList(new[] { "a", "b", "c" });

            Assert.IsTrue(checkBoxList.ContainsOption("b"));
        }

        [TestMethod]
        public void ContainsOption_MissingOption_ReturnsFalse()
        {
            var checkBoxList = new CheckBoxList(new[] { "a", "b", "c" });

            Assert.IsFalse(checkBoxList.ContainsOption("z"));
        }
    }
}
