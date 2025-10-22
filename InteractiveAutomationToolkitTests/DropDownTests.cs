using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skyline.DataMiner.Utils.InteractiveAutomationScript;
using System.Linq;

namespace InteractiveAutomationToolkitTests
{
    [TestClass]
    public class DropDownTests
    {
        [TestMethod]
        public void Clear_Test()
        {
            var options = new[] { "Option1", "Option2", "Option3" };
            var dropDown = new DropDown(options, "Option2");

            Assert.AreEqual("Option2", dropDown.Selected);
            Assert.AreEqual(3, dropDown.Options.Count());

            dropDown.Clear();

            Assert.IsNull(dropDown.Selected);
            Assert.AreEqual(0, dropDown.Options.Count());
        }

        [TestMethod]
        public void Clear_EmptyDropDown_Test()
        {
            var dropDown = new DropDown();

            Assert.IsNull(dropDown.Selected);
            Assert.AreEqual(0, dropDown.Options.Count());

            dropDown.Clear(); // Should not throw

            Assert.IsNull(dropDown.Selected);
            Assert.AreEqual(0, dropDown.Options.Count());
        }

        [TestMethod]
        public void Clear_ThenAddOptions_Test()
        {
            var options = new[] { "Option1", "Option2", "Option3" };
            var dropDown = new DropDown(options, "Option2");

            dropDown.Clear();

            Assert.IsNull(dropDown.Selected);
            Assert.AreEqual(0, dropDown.Options.Count());

            dropDown.AddOption("NewOption1");
            dropDown.AddOption("NewOption2");

            Assert.AreEqual(2, dropDown.Options.Count());
            Assert.IsNull(dropDown.Selected);
        }
    }
}
