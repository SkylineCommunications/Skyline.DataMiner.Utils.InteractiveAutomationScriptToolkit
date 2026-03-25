using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skyline.DataMiner.Utils.InteractiveAutomationScript;

namespace InteractiveAutomationToolkitTests
{
    [TestClass]
    public class DropDownTests
    {
        [TestMethod]
        public void ContainsOption_ExistingOption_ReturnsTrue()
        {
            var dropDown = new DropDown(new[] { "a", "b", "c" });

            Assert.IsTrue(dropDown.ContainsOption("b"));
        }

        [TestMethod]
        public void ContainsOption_MissingOption_ReturnsFalse()
        {
            var dropDown = new DropDown(new[] { "a", "b", "c" });

            Assert.IsFalse(dropDown.ContainsOption("z"));
        }

        [TestMethod]
        public void TrySelectOption_ExistingOption_SelectsAndReturnsTrue()
        {
            var dropDown = new DropDown(new[] { "a", "b", "c" }, "a");

            bool result = dropDown.TrySelectOption("c");

            Assert.IsTrue(result);
            Assert.AreEqual("c", dropDown.Selected);
        }

        [TestMethod]
        public void TrySelectOption_MissingOption_ReturnsFalseAndKeepsSelection()
        {
            var dropDown = new DropDown(new[] { "a", "b", "c" }, "a");

            bool result = dropDown.TrySelectOption("z");

            Assert.IsFalse(result);
            Assert.AreEqual("a", dropDown.Selected);
        }
    }
}
