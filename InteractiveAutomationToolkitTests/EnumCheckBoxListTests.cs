namespace InteractiveAutomationToolkitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Skyline.DataMiner.Utils.InteractiveAutomationScript;
    using System;
    using System.Linq;

    [TestClass]
    public class EnumCheckBoxListTests
    {
        [Flags]
        public enum DefaultOption
        {
            None = 0,
            [System.ComponentModel.Description("Option 1")]
            Option1 = 1,
            Option2 = 2,
            [System.ComponentModel.Description("Something")]
            Option3 = 4,
            Option4 = 8,
        }

        [TestMethod]
        public void EmptyConstructorTest()
        {
            var checkBoxList = new EnumCheckBoxList<DefaultOption>();
            Assert.AreEqual(DefaultOption.None, checkBoxList.CheckedFlags);
            Assert.AreEqual(5, checkBoxList.Options.Count());
            Assert.AreEqual(0, checkBoxList.Checked.Count());
        }

        [TestMethod]
        public void CheckedFlagsTest()
        {
            var checkBoxList = new EnumCheckBoxList<DefaultOption>();
            
            // Check multiple options
            checkBoxList.Check(DefaultOption.Option1);
            checkBoxList.Check(DefaultOption.Option2);
            
            Assert.AreEqual(DefaultOption.Option1 | DefaultOption.Option2, checkBoxList.CheckedFlags);
            Assert.AreEqual(2, checkBoxList.Checked.Count());
        }

        [TestMethod]
        public void SetCheckedFlagsTest()
        {
            var checkBoxList = new EnumCheckBoxList<DefaultOption>();
            
            // Set combined flags
            checkBoxList.CheckedFlags = DefaultOption.Option1 | DefaultOption.Option3;
            
            Assert.AreEqual(DefaultOption.Option1 | DefaultOption.Option3, checkBoxList.CheckedFlags);
            Assert.IsTrue(checkBoxList.Checked.Contains(DefaultOption.Option1));
            Assert.IsTrue(checkBoxList.Checked.Contains(DefaultOption.Option3));
            Assert.IsFalse(checkBoxList.Checked.Contains(DefaultOption.Option2));
            Assert.AreEqual(2, checkBoxList.Checked.Count());
        }

        [TestMethod]
        public void SetCheckedFlagsToNoneTest()
        {
            var checkBoxList = new EnumCheckBoxList<DefaultOption>();
            
            checkBoxList.Check(DefaultOption.Option1);
            checkBoxList.Check(DefaultOption.Option2);
            
            // Set to None
            checkBoxList.CheckedFlags = DefaultOption.None;
            
            Assert.AreEqual(DefaultOption.None, checkBoxList.CheckedFlags);
            Assert.AreEqual(0, checkBoxList.Checked.Count());
        }

        [TestMethod]
        public void CheckAllFlagsTest()
        {
            var checkBoxList = new EnumCheckBoxList<DefaultOption>();
            
            checkBoxList.CheckedFlags = DefaultOption.Option1 | DefaultOption.Option2 | DefaultOption.Option3 | DefaultOption.Option4;
            
            Assert.AreEqual(4, checkBoxList.Checked.Count());
            Assert.IsTrue(checkBoxList.Checked.Contains(DefaultOption.Option1));
            Assert.IsTrue(checkBoxList.Checked.Contains(DefaultOption.Option2));
            Assert.IsTrue(checkBoxList.Checked.Contains(DefaultOption.Option3));
            Assert.IsTrue(checkBoxList.Checked.Contains(DefaultOption.Option4));
        }

        [TestMethod]
        public void ExcludedConstructorTest()
        {
            var checkBoxList = new EnumCheckBoxList<DefaultOption>(new[] { DefaultOption.None });
            Assert.AreEqual(DefaultOption.None, checkBoxList.CheckedFlags);
            Assert.AreEqual(4, checkBoxList.Options.Count());
            
            // None should be excluded
            Assert.IsFalse(checkBoxList.Values.Contains(DefaultOption.None));
        }

        [TestMethod]
        public void ExcludeAllConstructorTest()
        {
            var checkBoxList = new EnumCheckBoxList<DefaultOption>(new[] { DefaultOption.None, DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3, DefaultOption.Option4 });
            Assert.AreEqual(DefaultOption.None, checkBoxList.CheckedFlags);
            Assert.AreEqual(0, checkBoxList.Options.Count());
        }

        [TestMethod]
        public void ConversionConstructorTest()
        {
            var checkBoxList = new EnumCheckBoxList<DefaultOption>(ConversionMethod);
            Assert.AreEqual(DefaultOption.None, checkBoxList.CheckedFlags);
            Assert.AreEqual(5, checkBoxList.Options.Count());

            checkBoxList.Check(DefaultOption.Option1);
            var option1 = checkBoxList.Options.Single(x => x.Value.Equals(DefaultOption.Option1));
            Assert.AreEqual("The first option", option1.DisplayValue);

            checkBoxList.Check(DefaultOption.Option2);
            var option2 = checkBoxList.Options.Single(x => x.Value.Equals(DefaultOption.Option2));
            Assert.AreEqual("The second option", option2.DisplayValue);

            checkBoxList.Check(DefaultOption.Option3);
            var option3 = checkBoxList.Options.Single(x => x.Value.Equals(DefaultOption.Option3));
            Assert.AreEqual("The third option", option3.DisplayValue);
        }

        [TestMethod]
        public void ConversionExcludedConstructorTest()
        {
            var checkBoxList = new EnumCheckBoxList<DefaultOption>(ConversionMethod, new[] { DefaultOption.None });
            Assert.AreEqual(DefaultOption.None, checkBoxList.CheckedFlags);
            Assert.AreEqual(4, checkBoxList.Options.Count());
        }

        [TestMethod]
        public void ConversionExcludeAllConstructorTest()
        {
            var checkBoxList = new EnumCheckBoxList<DefaultOption>(ConversionMethod, new[] { DefaultOption.None, DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3, DefaultOption.Option4 });
            Assert.AreEqual(DefaultOption.None, checkBoxList.CheckedFlags);
            Assert.AreEqual(0, checkBoxList.Options.Count());
        }

        [TestMethod]
        public void ConversionNullConstructorTest()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                new EnumCheckBoxList<DefaultOption>(null, new DefaultOption[0]);
            });
        }

        [TestMethod]
        public void SetOptionsMethodTest1()
        {
            var checkBoxList = new EnumCheckBoxList<DefaultOption>(ConversionMethod, new[] { DefaultOption.None, DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3, DefaultOption.Option4 });
            checkBoxList.SetOptions(new[] { DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 });

            var option1 = checkBoxList.Options.Single(x => x.Value.Equals(DefaultOption.Option1));
            Assert.AreEqual("The first option", option1.DisplayValue);

            var option2 = checkBoxList.Options.Single(x => x.Value.Equals(DefaultOption.Option2));
            Assert.AreEqual("The second option", option2.DisplayValue);

            var option3 = checkBoxList.Options.Single(x => x.Value.Equals(DefaultOption.Option3));
            Assert.AreEqual("The third option", option3.DisplayValue);
        }

        [TestMethod]
        public void SetValuesPropertyTest1()
        {
            var checkBoxList = new EnumCheckBoxList<DefaultOption>(ConversionMethod, new[] { DefaultOption.None, DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3, DefaultOption.Option4 });
            checkBoxList.Values = new[] { DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 };

            var option1 = checkBoxList.Options.Single(x => x.Value.Equals(DefaultOption.Option1));
            Assert.AreEqual("The first option", option1.DisplayValue);

            var option2 = checkBoxList.Options.Single(x => x.Value.Equals(DefaultOption.Option2));
            Assert.AreEqual("The second option", option2.DisplayValue);

            var option3 = checkBoxList.Options.Single(x => x.Value.Equals(DefaultOption.Option3));
            Assert.AreEqual("The third option", option3.DisplayValue);
        }

        [TestMethod]
        public void SetOptionsMethodTest2()
        {
            var checkBoxList = new EnumCheckBoxList<DefaultOption>(ConversionMethod);
            checkBoxList.SetOptions(new[] { DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 });

            var option1 = checkBoxList.Options.Single(x => x.Value.Equals(DefaultOption.Option1));
            Assert.AreEqual("The first option", option1.DisplayValue);

            var option2 = checkBoxList.Options.Single(x => x.Value.Equals(DefaultOption.Option2));
            Assert.AreEqual("The second option", option2.DisplayValue);

            var option3 = checkBoxList.Options.Single(x => x.Value.Equals(DefaultOption.Option3));
            Assert.AreEqual("The third option", option3.DisplayValue);
        }

        [TestMethod]
        public void SetValuesPropertyTest2()
        {
            var checkBoxList = new EnumCheckBoxList<DefaultOption>(ConversionMethod);
            checkBoxList.Values = new[] { DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 };

            var option1 = checkBoxList.Options.Single(x => x.Value.Equals(DefaultOption.Option1));
            Assert.AreEqual("The first option", option1.DisplayValue);

            var option2 = checkBoxList.Options.Single(x => x.Value.Equals(DefaultOption.Option2));
            Assert.AreEqual("The second option", option2.DisplayValue);

            var option3 = checkBoxList.Options.Single(x => x.Value.Equals(DefaultOption.Option3));
            Assert.AreEqual("The third option", option3.DisplayValue);
        }

        [TestMethod]
        public void SetOptionsMethodTest3()
        {
            var checkBoxList = new EnumCheckBoxList<DefaultOption>();
            checkBoxList.SetOptions(new[] { DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 });

            var option1 = checkBoxList.Options.Single(x => x.Value.Equals(DefaultOption.Option1));
            Assert.AreEqual("Option 1", option1.DisplayValue);

            var option2 = checkBoxList.Options.Single(x => x.Value.Equals(DefaultOption.Option2));
            Assert.AreEqual("Option2", option2.DisplayValue);

            var option3 = checkBoxList.Options.Single(x => x.Value.Equals(DefaultOption.Option3));
            Assert.AreEqual("Something", option3.DisplayValue);
        }

        [TestMethod]
        public void SetValuesPropertyTest3()
        {
            var checkBoxList = new EnumCheckBoxList<DefaultOption>();
            checkBoxList.Values = new[] { DefaultOption.Option1, DefaultOption.Option2, DefaultOption.Option3 };

            var option1 = checkBoxList.Options.Single(x => x.Value.Equals(DefaultOption.Option1));
            Assert.AreEqual("Option 1", option1.DisplayValue);

            var option2 = checkBoxList.Options.Single(x => x.Value.Equals(DefaultOption.Option2));
            Assert.AreEqual("Option2", option2.DisplayValue);

            var option3 = checkBoxList.Options.Single(x => x.Value.Equals(DefaultOption.Option3));
            Assert.AreEqual("Something", option3.DisplayValue);
        }

        [TestMethod]
        public void CheckedFlagsWithExcludedOptionTest()
        {
            // Exclude Option2 but try to set a value that includes it
            var checkBoxList = new EnumCheckBoxList<DefaultOption>(new[] { DefaultOption.Option2 });
            
            // This should only check Option1 and Option3, skipping Option2 since it's excluded
            checkBoxList.CheckedFlags = DefaultOption.Option1 | DefaultOption.Option2 | DefaultOption.Option3;
            
            // Option2 should not be checked since it's excluded
            Assert.IsTrue(checkBoxList.Checked.Contains(DefaultOption.Option1));
            Assert.IsFalse(checkBoxList.Checked.Contains(DefaultOption.Option2));
            Assert.IsTrue(checkBoxList.Checked.Contains(DefaultOption.Option3));
        }

        [TestMethod]
        public void UncheckAndRecheckTest()
        {
            var checkBoxList = new EnumCheckBoxList<DefaultOption>();
            
            checkBoxList.Check(DefaultOption.Option1);
            checkBoxList.Check(DefaultOption.Option2);
            Assert.AreEqual(2, checkBoxList.Checked.Count());
            
            checkBoxList.Uncheck(DefaultOption.Option1);
            Assert.AreEqual(1, checkBoxList.Checked.Count());
            Assert.AreEqual(DefaultOption.Option2, checkBoxList.CheckedFlags);
            
            checkBoxList.Check(DefaultOption.Option3);
            Assert.AreEqual(DefaultOption.Option2 | DefaultOption.Option3, checkBoxList.CheckedFlags);
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
                    return "The third option";
                case DefaultOption.Option4:
                    return "The fourth option";
                default:
                    return "Unknown option";
            }
        }
    }
}
