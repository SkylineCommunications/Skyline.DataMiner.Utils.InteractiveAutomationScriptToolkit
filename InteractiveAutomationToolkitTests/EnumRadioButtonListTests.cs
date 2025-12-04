namespace InteractiveAutomationToolkitTests
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;
	using System;
	using System.Linq;

	[TestClass]
    public class EnumRadioButtonListTests
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
            var radioButtonList = new EnumRadioButtonList<DefaultOption>();
            Assert.AreEqual(DefaultOption.None, radioButtonList.Selected);
            Assert.AreEqual(4, radioButtonList.Options.Count());

            Assert.IsNull(radioButtonList.SelectedOption);

            radioButtonList.Selected = DefaultOption.None;
            Assert.AreEqual("None", radioButtonList.SelectedOption.DisplayValue);

            radioButtonList.Selected = DefaultOption.Option1;
            Assert.AreEqual("Option 1", radioButtonList.SelectedOption.DisplayValue);

            radioButtonList.Selected = DefaultOption.Option2;
            Assert.AreEqual("Option2", radioButtonList.SelectedOption.DisplayValue);

            radioButtonList.Selected = DefaultOption.Option3;
            Assert.AreEqual("Something", radioButtonList.SelectedOption.DisplayValue);
        }

        [TestMethod]
        public void ExcludedConstructorTest()
        {
            var radioButtonList = new EnumRadioButtonList<DefaultOption>(new[] { DefaultOption.None });
            Assert.AreEqual(DefaultOption.None, radioButtonList.Selected);
            Assert.AreEqual(3, radioButtonList.Options.Count());
        }

        [TestMethod]
        public void ExcludeAllConstructorTest()
        {
            var radioButtonList = new EnumRadioButtonList<DefaultOption>(new[] { DefaultOption.None, DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 });
            Assert.AreEqual(DefaultOption.None, radioButtonList.Selected);
            Assert.AreEqual(0, radioButtonList.Options.Count());
            Assert.IsNull(radioButtonList.SelectedOption);
        }

        [TestMethod]
        public void ConversionConstructorTest()
        {
            var radioButtonList = new EnumRadioButtonList<DefaultOption>(ConversionMethod);
            Assert.AreEqual(DefaultOption.None, radioButtonList.Selected);
            Assert.AreEqual(4, radioButtonList.Options.Count());

            Assert.IsNull(radioButtonList.SelectedOption);

            radioButtonList.Selected = DefaultOption.None;
            Assert.AreEqual("No option", radioButtonList.SelectedOption.DisplayValue);

            radioButtonList.Selected = DefaultOption.Option1;
            Assert.AreEqual("The first option", radioButtonList.SelectedOption.DisplayValue);

            radioButtonList.Selected = DefaultOption.Option2;
            Assert.AreEqual("The second option", radioButtonList.SelectedOption.DisplayValue);

            radioButtonList.Selected = DefaultOption.Option3;
            Assert.AreEqual("The last and final option", radioButtonList.SelectedOption.DisplayValue);
        }

        [TestMethod]
        public void ConversionExcludedConstructorTest()
        {
            var radioButtonList = new EnumRadioButtonList<DefaultOption>(ConversionMethod, new[] { DefaultOption.None });
            Assert.AreEqual(DefaultOption.None, radioButtonList.Selected);
            Assert.AreEqual(3, radioButtonList.Options.Count());
        }

        [TestMethod]
        public void ConversionExcludeAllConstructorTest()
        {
            var radioButtonList = new EnumRadioButtonList<DefaultOption>(ConversionMethod, new[] { DefaultOption.None, DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 });
            Assert.AreEqual(DefaultOption.None, radioButtonList.Selected);
            Assert.AreEqual(0, radioButtonList.Options.Count());
            Assert.IsNull(radioButtonList.SelectedOption);
        }

        [TestMethod]
        public void ConversionNullConstructorTest()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                new EnumRadioButtonList<DefaultOption>(null, new DefaultOption[0]);
            });
        }

        [TestMethod]
        public void SetOptionsMethodTest1()
        {
            var radioButtonList = new EnumRadioButtonList<DefaultOption>(ConversionMethod, new[] { DefaultOption.None, DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 });
            radioButtonList.SetOptions(new[] { DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 });

            var option1 = radioButtonList.Options.Single(x => x.Value == DefaultOption.Option1);
            Assert.AreEqual("The first option", option1.DisplayValue);

            var option2 = radioButtonList.Options.Single(x => x.Value == DefaultOption.Option2);
            Assert.AreEqual("The second option", option2.DisplayValue);

            var option3 = radioButtonList.Options.Single(x => x.Value == DefaultOption.Option3);
            Assert.AreEqual("The last and final option", option3.DisplayValue);
        }

        [TestMethod]
        public void SetValuesPropertyTest1()
        {
            var radioButtonList = new EnumRadioButtonList<DefaultOption>(ConversionMethod, new[] { DefaultOption.None, DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 });
            radioButtonList.Values = new[] { DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 };

            var option1 = radioButtonList.Options.Single(x => x.Value == DefaultOption.Option1);
            Assert.AreEqual("The first option", option1.DisplayValue);

            var option2 = radioButtonList.Options.Single(x => x.Value == DefaultOption.Option2);
            Assert.AreEqual("The second option", option2.DisplayValue);

            var option3 = radioButtonList.Options.Single(x => x.Value == DefaultOption.Option3);
            Assert.AreEqual("The last and final option", option3.DisplayValue);
        }

        [TestMethod]
        public void SetOptionsMethodTest2()
        {
            var radioButtonList = new EnumRadioButtonList<DefaultOption>(ConversionMethod);
            radioButtonList.SetOptions(new[] { DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 });

            var option1 = radioButtonList.Options.Single(x => x.Value == DefaultOption.Option1);
            Assert.AreEqual("The first option", option1.DisplayValue);

            var option2 = radioButtonList.Options.Single(x => x.Value == DefaultOption.Option2);
            Assert.AreEqual("The second option", option2.DisplayValue);

            var option3 = radioButtonList.Options.Single(x => x.Value == DefaultOption.Option3);
            Assert.AreEqual("The last and final option", option3.DisplayValue);
        }

        [TestMethod]
        public void SetValuesPropertyTest2()
        {
            var radioButtonList = new EnumRadioButtonList<DefaultOption>(ConversionMethod);
            radioButtonList.Values = new[] { DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 };

            var option1 = radioButtonList.Options.Single(x => x.Value == DefaultOption.Option1);
            Assert.AreEqual("The first option", option1.DisplayValue);

            var option2 = radioButtonList.Options.Single(x => x.Value == DefaultOption.Option2);
            Assert.AreEqual("The second option", option2.DisplayValue);

            var option3 = radioButtonList.Options.Single(x => x.Value == DefaultOption.Option3);
            Assert.AreEqual("The last and final option", option3.DisplayValue);
        }

        [TestMethod]
        public void SetOptionsMethodTest3()
        {
            var radioButtonList = new EnumRadioButtonList<DefaultOption>();
            radioButtonList.SetOptions(new[] { DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 });

            var option1 = radioButtonList.Options.Single(x => x.Value == DefaultOption.Option1);
            Assert.AreEqual("Option 1", option1.DisplayValue);

            var option2 = radioButtonList.Options.Single(x => x.Value == DefaultOption.Option2);
            Assert.AreEqual("Option2", option2.DisplayValue);

            var option3 = radioButtonList.Options.Single(x => x.Value == DefaultOption.Option3);
            Assert.AreEqual("Something", option3.DisplayValue);
        }

        [TestMethod]
        public void SetValuesPropertyTest3()
        {
            var radioButtonList = new EnumRadioButtonList<DefaultOption>();
            radioButtonList.Values = new[] { DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 };

            var option1 = radioButtonList.Options.Single(x => x.Value == DefaultOption.Option1);
            Assert.AreEqual("Option 1", option1.DisplayValue);

            var option2 = radioButtonList.Options.Single(x => x.Value == DefaultOption.Option2);
            Assert.AreEqual("Option2", option2.DisplayValue);

            var option3 = radioButtonList.Options.Single(x => x.Value == DefaultOption.Option3);
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
