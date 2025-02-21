namespace InteractiveAutomationToolkitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Skyline.DataMiner.Utils.InteractiveAutomationScript;
    using System;
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

        [TestMethod]
        public void ConversionConstructorTest()
        {
            var dropDown = new EnumDropDown<DefaultOption>(ConversionMethod);
            Assert.AreEqual(DefaultOption.None, dropDown.Selected);
            Assert.AreEqual(4, dropDown.Options.Count());

            Assert.AreEqual("No option", dropDown.SelectedOption.DisplayValue);

            dropDown.Selected = DefaultOption.Option1;
            Assert.AreEqual("The first option", dropDown.SelectedOption.DisplayValue);

            dropDown.Selected = DefaultOption.Option2;
            Assert.AreEqual("The second option", dropDown.SelectedOption.DisplayValue);

            dropDown.Selected = DefaultOption.Option3;
            Assert.AreEqual("The last and final option", dropDown.SelectedOption.DisplayValue);
        }

        [TestMethod]
        public void ConversionExcludedConstructorTest()
        {
            var dropDown = new EnumDropDown<DefaultOption>(ConversionMethod, new[] { DefaultOption.None });
            Assert.AreEqual(DefaultOption.Option1, dropDown.Selected);
            Assert.AreEqual(3, dropDown.Options.Count());
        }

        [TestMethod]
        public void ConversionExcludeAllConstructorTest()
        {
            var dropDown = new EnumDropDown<DefaultOption>(ConversionMethod, new[] { DefaultOption.None, DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 });
            Assert.AreEqual(DefaultOption.None, dropDown.Selected);
            Assert.AreEqual(0, dropDown.Options.Count());
            Assert.IsNull(dropDown.SelectedOption);
        }

        [TestMethod]
        public void ConversionNullConstructorTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new EnumDropDown<DefaultOption>(null, new DefaultOption[0]);
            });
        }

        [TestMethod]
        public void SetOptionsMethodTest1()
        {
            var dropdown = new EnumDropDown<DefaultOption>(ConversionMethod, new[] { DefaultOption.None, DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 });
            dropdown.SetOptions(new[] { DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 });

            var option1 = dropdown.Options.Single(x => x.Value == DefaultOption.Option1);
            Assert.AreEqual("The first option", option1.DisplayValue);

            var option2 = dropdown.Options.Single(x => x.Value == DefaultOption.Option2);
            Assert.AreEqual("The second option", option2.DisplayValue);

            var option3 = dropdown.Options.Single(x => x.Value == DefaultOption.Option3);
            Assert.AreEqual("The last and final option", option3.DisplayValue);
        }

        [TestMethod]
        public void SetValuesPropertyTest1()
        {
            var dropdown = new EnumDropDown<DefaultOption>(ConversionMethod, new[] { DefaultOption.None, DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 });
            dropdown.Values = new[] { DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 };

            var option1 = dropdown.Options.Single(x => x.Value == DefaultOption.Option1);
            Assert.AreEqual("The first option", option1.DisplayValue);

            var option2 = dropdown.Options.Single(x => x.Value == DefaultOption.Option2);
            Assert.AreEqual("The second option", option2.DisplayValue);

            var option3 = dropdown.Options.Single(x => x.Value == DefaultOption.Option3);
            Assert.AreEqual("The last and final option", option3.DisplayValue);
        }

        [TestMethod]
        public void SetOptionsMethodTest2()
        {
            var dropdown = new EnumDropDown<DefaultOption>(ConversionMethod);
            dropdown.SetOptions(new[] { DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 });

            var option1 = dropdown.Options.Single(x => x.Value == DefaultOption.Option1);
            Assert.AreEqual("The first option", option1.DisplayValue);

            var option2 = dropdown.Options.Single(x => x.Value == DefaultOption.Option2);
            Assert.AreEqual("The second option", option2.DisplayValue);

            var option3 = dropdown.Options.Single(x => x.Value == DefaultOption.Option3);
            Assert.AreEqual("The last and final option", option3.DisplayValue);
        }

        [TestMethod]
        public void SetValuesPropertyTest2()
        {
            var dropdown = new EnumDropDown<DefaultOption>(ConversionMethod);
            dropdown.Values = new[] { DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 };

            var option1 = dropdown.Options.Single(x => x.Value == DefaultOption.Option1);
            Assert.AreEqual("The first option", option1.DisplayValue);

            var option2 = dropdown.Options.Single(x => x.Value == DefaultOption.Option2);
            Assert.AreEqual("The second option", option2.DisplayValue);

            var option3 = dropdown.Options.Single(x => x.Value == DefaultOption.Option3);
            Assert.AreEqual("The last and final option", option3.DisplayValue);
        }

        [TestMethod]
        public void SetOptionsMethodTest3()
        {
            var dropdown = new EnumDropDown<DefaultOption>();
            dropdown.SetOptions(new[] { DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 });

            var option1 = dropdown.Options.Single(x => x.Value == DefaultOption.Option1);
            Assert.AreEqual("Option 1", option1.DisplayValue);

            var option2 = dropdown.Options.Single(x => x.Value == DefaultOption.Option2);
            Assert.AreEqual("Option2", option2.DisplayValue);

            var option3 = dropdown.Options.Single(x => x.Value == DefaultOption.Option3);
            Assert.AreEqual("Something", option3.DisplayValue);
        }

        [TestMethod]
        public void SetValuesPropertyTest3()
        {
            var dropdown = new EnumDropDown<DefaultOption>();
            dropdown.Values = new[] { DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 };

            var option1 = dropdown.Options.Single(x => x.Value == DefaultOption.Option1);
            Assert.AreEqual("Option 1", option1.DisplayValue);

            var option2 = dropdown.Options.Single(x => x.Value == DefaultOption.Option2);
            Assert.AreEqual("Option2", option2.DisplayValue);

            var option3 = dropdown.Options.Single(x => x.Value == DefaultOption.Option3);
            Assert.AreEqual("Something", option3.DisplayValue);
        }

        private static string ConversionMethod(DefaultOption option)
        {
            switch (option)
            {
                case DefaultOption.None:
                    return "No option";
                case DefaultOption.Option1:
                    return "The first option";
                case DefaultOption.Option2:
                    return "The second option";
                case DefaultOption.Option3:
                    return "The last and final option";
                default:
                    return "Unknown option";
            }
        }
    }
}
