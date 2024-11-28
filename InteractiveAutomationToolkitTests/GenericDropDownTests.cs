using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skyline.DataMiner.Utils.InteractiveAutomationScript;
using System;
using System.Linq;

namespace InteractiveAutomationToolkitTests
{
    [TestClass]
    public class GenericDropDownTests
    {
        [TestMethod]
        public void EmptyConstructor_Test1()
        {
            var dropdown = new DropDown<int>();

            Assert.IsNull(dropdown.SelectedOption);
            Assert.AreEqual(default, dropdown.Selected);
        }

        [TestMethod]
        public void EmptyConstructor_Test2()
        {
            var dropdown = new DropDown<string>();

            Assert.IsNull(dropdown.SelectedOption);
            Assert.AreEqual(default, dropdown.Selected);
        }

        [TestMethod]
        public void EditOptions()
        {
            var dropdown = new DropDown<int>();

            var options = new[] { new Option<int>("1", 1), new Option<int>("2", 2) };
            dropdown.SetOptions(options);

            Assert.AreEqual(options[0], dropdown.SelectedOption);
            Assert.AreEqual(1, dropdown.Selected);

            Assert.AreEqual(2, dropdown.Options.Count());

            Assert.ThrowsException<InvalidOperationException>(() => dropdown.SelectedOption = new Option<int>("3", 3));

            dropdown.Selected = 2;
            Assert.AreEqual(new Option<int>("2", 2), dropdown.SelectedOption);

            dropdown.RemoveOption(new Option<int>("3", 3));

            dropdown.RemoveOption(1);

            Assert.AreEqual(2, dropdown.Selected);

            dropdown.AddOption(1);

            Assert.AreEqual(2, dropdown.Selected);

            dropdown.RemoveOption(2);

            Assert.AreEqual(1, dropdown.Selected);

            dropdown.RemoveOption(1);

            Assert.IsNull(dropdown.SelectedOption);
            Assert.AreEqual(default, dropdown.Selected);
        }
    }
}
