namespace InteractiveAutomationToolkitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Skyline.DataMiner.Utils.InteractiveAutomationScript;
    using System.Linq;

    [TestClass]
    public class EnumDropDownTests
    {
        public enum DefaultOption
        {
            None,
            [System.ComponentModel.Description("Option 1")]
            Option1,
            Option2,
            [System.ComponentModel.Description("Something")]
            Option3,
        }

        [TestMethod]
        public void EmptyConstructorTest()
        {
            var dropDown = new EnumDropDown<DefaultOption>();
            Assert.AreEqual(DefaultOption.None, dropDown.Selected);
            Assert.AreEqual(4, dropDown.Options.Count());

            Assert.AreEqual("None", dropDown.SelectedOption.DisplayValue);

            dropDown.Selected = DefaultOption.Option1;
            Assert.AreEqual("Option 1", dropDown.SelectedOption.DisplayValue);

            dropDown.Selected = DefaultOption.Option2;
            Assert.AreEqual("Option2", dropDown.SelectedOption.DisplayValue);

            dropDown.Selected = DefaultOption.Option3;
            Assert.AreEqual("Something", dropDown.SelectedOption.DisplayValue);
        }

        [TestMethod]
        public void ExcludedConstructorTest()
        {
            var dropDown = new EnumDropDown<DefaultOption>(new[] { DefaultOption.None });
            Assert.AreEqual(DefaultOption.Option1, dropDown.Selected);
            Assert.AreEqual(3, dropDown.Options.Count());
        }

        [TestMethod]
        public void ExcludeAllConstructorTest()
        {
            var dropDown = new EnumDropDown<DefaultOption>(new[] { DefaultOption.None, DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 });
            Assert.AreEqual(DefaultOption.None, dropDown.Selected);
            Assert.AreEqual(0, dropDown.Options.Count());
            Assert.IsNull(dropDown.SelectedOption);
        }
    }
}
