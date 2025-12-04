using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Skyline.DataMiner.Utils.InteractiveAutomationScript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InteractiveAutomationToolkitTests
{
    [TestClass]
    public class GenericCheckBoxListTests
    {
        [TestMethod]
        public void EmptyConstructorStruct_Test()
        {
            var checkBoxList = new CheckBoxList<int>();

            Assert.AreEqual(0, checkBoxList.Options.Count());
            Assert.AreEqual(0, checkBoxList.Values.Count());
            Assert.AreEqual(0, checkBoxList.CheckedOptions.Count());
            Assert.AreEqual(0, checkBoxList.Checked.Count());
            Assert.AreEqual(0, checkBoxList.UncheckedOptions.Count());
            Assert.AreEqual(0, checkBoxList.Unchecked.Count());
        }

        [TestMethod]
        public void EmptyConstructorClass_Test()
        {
            var checkBoxList = new CheckBoxList<string>();

            Assert.AreEqual(0, checkBoxList.Options.Count());
            Assert.AreEqual(0, checkBoxList.Values.Count());
            Assert.AreEqual(0, checkBoxList.CheckedOptions.Count());
            Assert.AreEqual(0, checkBoxList.Checked.Count());
            Assert.AreEqual(0, checkBoxList.UncheckedOptions.Count());
            Assert.AreEqual(0, checkBoxList.Unchecked.Count());
        }

        [TestMethod]
        public void OptionConstructor_Test()
        {
            var options = new[] { new Option<int>("1", 1), new Option<int>("2", 2), new Option<int>("3", 3) };
            var checkBoxList = new CheckBoxList<int>(options);

            Assert.AreEqual(3, checkBoxList.Options.Count());
            Assert.AreEqual(3, checkBoxList.Values.Count());
            Assert.AreEqual(0, checkBoxList.CheckedOptions.Count());
            Assert.AreEqual(0, checkBoxList.Checked.Count());
            Assert.AreEqual(3, checkBoxList.UncheckedOptions.Count());
            Assert.AreEqual(3, checkBoxList.Unchecked.Count());

            Assert.IsTrue(Enumerable.SequenceEqual(options, checkBoxList.Options));
        }

        [TestMethod]
        public void ValueConstructor_Test()
        {
            var options = new[] { 1, 2, 3 };
            var checkBoxList = new CheckBoxList<int>(options);

            Assert.AreEqual(3, checkBoxList.Options.Count());
            Assert.AreEqual(3, checkBoxList.Values.Count());
            Assert.AreEqual(0, checkBoxList.CheckedOptions.Count());
            Assert.AreEqual(0, checkBoxList.Checked.Count());
            Assert.AreEqual(3, checkBoxList.UncheckedOptions.Count());
            Assert.AreEqual(3, checkBoxList.Unchecked.Count());

            Assert.IsTrue(Enumerable.SequenceEqual(options, checkBoxList.Values));
        }

        [TestMethod]
        public void EditOptions_Options()
        {
            var checkBoxList = new CheckBoxList<int>();

            var options = new[] { new Option<int>("1", 1), new Option<int>("2", 2) };
            checkBoxList.SetOptions(options);

            Assert.AreEqual(2, checkBoxList.UncheckedOptions.Count());
            Assert.AreEqual(0, checkBoxList.CheckedOptions.Count());

            Assert.AreEqual(2, checkBoxList.Options.Count());

            Assert.ThrowsExactly<ArgumentException>(() => checkBoxList.Uncheck(new Option<int>("3", 3)));
            Assert.ThrowsExactly<ArgumentException>(() => checkBoxList.Check(new Option<int>("3", 3)));

            checkBoxList.Check(new Option<int>("2", 2));

            Assert.AreEqual(new Option<int>("2", 2), checkBoxList.CheckedOptions.Single());
            Assert.AreEqual(2, checkBoxList.Options.Count());
            Assert.AreEqual(1, checkBoxList.UncheckedOptions.Count());
            Assert.AreEqual(1, checkBoxList.CheckedOptions.Count());

            checkBoxList.Check(new Option<int>("1", 1));

            Assert.IsTrue(checkBoxList.CheckedOptions.Contains(new Option<int>("1", 1)));
            Assert.AreEqual(2, checkBoxList.Options.Count());
            Assert.AreEqual(0, checkBoxList.UncheckedOptions.Count());
            Assert.AreEqual(2, checkBoxList.CheckedOptions.Count());

            checkBoxList.RemoveOption(new Option<int>("3", 3)); // Removing invalid option does not throw exception

            checkBoxList.RemoveOption(new Option<int>("1", 1));

            Assert.AreEqual(1, checkBoxList.Options.Count());
            Assert.AreEqual(0, checkBoxList.UncheckedOptions.Count());
            Assert.AreEqual(1, checkBoxList.CheckedOptions.Count());

            Assert.AreEqual(new Option<int>("2", 2), checkBoxList.CheckedOptions.Single());

            checkBoxList.AddOption(new Option<int>("1", 1));

            Assert.AreEqual(2, checkBoxList.Options.Count());
            Assert.AreEqual(1, checkBoxList.UncheckedOptions.Count());
            Assert.AreEqual(1, checkBoxList.CheckedOptions.Count());

            checkBoxList.RemoveOption(new Option<int>("2", 2));

            Assert.AreEqual(1, checkBoxList.Options.Count());
            Assert.AreEqual(1, checkBoxList.UncheckedOptions.Count());
            Assert.AreEqual(0, checkBoxList.CheckedOptions.Count());

            checkBoxList.RemoveOption(new Option<int>("1", 1));

            Assert.AreEqual(0, checkBoxList.Options.Count());
            Assert.AreEqual(0, checkBoxList.UncheckedOptions.Count());
            Assert.AreEqual(0, checkBoxList.CheckedOptions.Count());
        }

        [TestMethod]
        public void EditOptions_Values()
        {
            var checkBoxList = new CheckBoxList<int>();

            var options = new[] { 1, 2 };
            checkBoxList.SetOptions(options);

            Assert.AreEqual(2, checkBoxList.Unchecked.Count());
            Assert.AreEqual(0, checkBoxList.Checked.Count());

            Assert.AreEqual(2, checkBoxList.Values.Count());

            Assert.ThrowsExactly<ArgumentException>(() => checkBoxList.Uncheck(3));
            Assert.ThrowsExactly<ArgumentException>(() => checkBoxList.Check(3));

            checkBoxList.Check(2);

            Assert.AreEqual(2, checkBoxList.Checked.Single());
            Assert.AreEqual(2, checkBoxList.Values.Count());
            Assert.AreEqual(1, checkBoxList.Unchecked.Count());
            Assert.AreEqual(1, checkBoxList.Checked.Count());

            checkBoxList.Check(1);

            Assert.IsTrue(checkBoxList.Checked.Contains(1));
            Assert.AreEqual(2, checkBoxList.Values.Count());
            Assert.AreEqual(0, checkBoxList.Unchecked.Count());
            Assert.AreEqual(2, checkBoxList.Checked.Count());

            checkBoxList.RemoveOption(3); // Removing invalid option does not throw exception

            checkBoxList.RemoveOption(1);

            Assert.AreEqual(1, checkBoxList.Values.Count());
            Assert.AreEqual(0, checkBoxList.Unchecked.Count());
            Assert.AreEqual(1, checkBoxList.Checked.Count());

            Assert.AreEqual(2, checkBoxList.Checked.Single());

            checkBoxList.AddOption(1);

            Assert.AreEqual(2, checkBoxList.Values.Count());
            Assert.AreEqual(1, checkBoxList.Unchecked.Count());
            Assert.AreEqual(1, checkBoxList.Checked.Count());

            checkBoxList.RemoveOption(2);

            Assert.AreEqual(1, checkBoxList.Values.Count());
            Assert.AreEqual(1, checkBoxList.Unchecked.Count());
            Assert.AreEqual(0, checkBoxList.Checked.Count());

            checkBoxList.RemoveOption(1);

            Assert.AreEqual(0, checkBoxList.Values.Count());
            Assert.AreEqual(0, checkBoxList.Unchecked.Count());
            Assert.AreEqual(0, checkBoxList.Checked.Count());
        }

        [TestMethod]
        public void Interaction_CheckOneItem()
        {
            var checkBoxList = new CheckBoxList<int>(new[] { 1, 2, 3 });

            List<CheckBoxList<int>.CheckBoxListChangedEventArgs> changedResults = new List<CheckBoxList<int>.CheckBoxListChangedEventArgs>();
            checkBoxList.Changed += (s, e) => changedResults.Add(e);

			// User checks 2
			var option2 = checkBoxList.Options.Single(x => x.Value == 2);

			var mockedUiResults = new Mock<IUIResults>();
            mockedUiResults.Setup(x => x.GetString(It.IsAny<string>())).Returns(checkBoxList.GetRawValue(option2));

            // Show CheckBoxList
            checkBoxList.LoadResult(mockedUiResults.Object);
            checkBoxList.RaiseResultEvents();

            Assert.AreEqual(1, changedResults.Count);

            var changedResult = changedResults.Single();
            Assert.IsNotNull(changedResult);
            Assert.AreEqual(2, changedResult.Value);
            Assert.AreEqual(new Option<int>(2), changedResult.Option);
            Assert.IsTrue(changedResult.IsChecked);

            Assert.AreEqual(2, checkBoxList.Checked.Single());
        }

        [TestMethod]
        public void Interaction_CheckTwoItems()
        {
            var checkBoxList = new CheckBoxList<int>(new[] { 1, 2, 3 });

            List<CheckBoxList<int>.CheckBoxListChangedEventArgs> changedResults = new List<CheckBoxList<int>.CheckBoxListChangedEventArgs>();
            checkBoxList.Changed += (s, e) => changedResults.Add(e);

			// User checks 2 and 3
			var option2 = checkBoxList.Options.Single(x => x.Value == 2);
			var option3 = checkBoxList.Options.Single(x => x.Value == 3);

			var mockedUiResults = new Mock<IUIResults>();
            mockedUiResults.Setup(x => x.GetString(It.IsAny<string>()))
                .Returns($"{checkBoxList.GetRawValue(option2)};{checkBoxList.GetRawValue(option3)}");

            // Show CheckBoxList
            checkBoxList.LoadResult(mockedUiResults.Object);
            checkBoxList.RaiseResultEvents();

            Assert.AreEqual(2, changedResults.Count);

            var changedResult1 = changedResults[0];
            Assert.IsNotNull(changedResult1);
            Assert.AreEqual(2, changedResult1.Value);
            Assert.AreEqual(new Option<int>(2), changedResult1.Option);
            Assert.IsTrue(changedResult1.IsChecked);

            var changedResult2 = changedResults[1];
            Assert.IsNotNull(changedResult2);
            Assert.AreEqual(3, changedResult2.Value);
            Assert.AreEqual(new Option<int>(3), changedResult2.Option);
            Assert.IsTrue(changedResult2.IsChecked);

            Assert.AreEqual(2, checkBoxList.Checked.Count());
        }

        [TestMethod]
        public void Interaction_UncheckOneItem()
        {
            var checkBoxList = new CheckBoxList<int>(new[] { 1, 2, 3 });
            checkBoxList.Check(2);

            List<CheckBoxList<int>.CheckBoxListChangedEventArgs> changedResults = new List<CheckBoxList<int>.CheckBoxListChangedEventArgs>();
            checkBoxList.Changed += (s, e) => changedResults.Add(e);

            // User unchecks 2
            var mockedUiResults = new Mock<IUIResults>();
            mockedUiResults.Setup(x => x.GetString(It.IsAny<string>())).Returns(String.Empty);

            // Show CheckBoxList
            checkBoxList.LoadResult(mockedUiResults.Object);
            checkBoxList.RaiseResultEvents();

            Assert.AreEqual(1, changedResults.Count);

            var changedResult = changedResults.Single();
            Assert.IsNotNull(changedResult);
            Assert.AreEqual(2, changedResult.Value);
            Assert.AreEqual(new Option<int>(2), changedResult.Option);
            Assert.IsFalse(changedResult.IsChecked);

            Assert.AreEqual(0, checkBoxList.Checked.Count());
        }

        [TestMethod]
        public void Interaction_CheckAndUncheckTwoItems()
        {
            var checkBoxList = new CheckBoxList<int>(new[] { 1, 2, 3 });
            checkBoxList.Check(2);

            List<CheckBoxList<int>.CheckBoxListChangedEventArgs> changedResults = new List<CheckBoxList<int>.CheckBoxListChangedEventArgs>();
            checkBoxList.Changed += (s, e) => changedResults.Add(e);

			// User unchecks 2 and checks 3
			var option3 = checkBoxList.Options.Single(x => x.Value == 3);

			var mockedUiResults = new Mock<IUIResults>();
            mockedUiResults.Setup(x => x.GetString(It.IsAny<string>())).Returns(checkBoxList.GetRawValue(option3));

            // Show CheckBoxList
            checkBoxList.LoadResult(mockedUiResults.Object);
            checkBoxList.RaiseResultEvents();

            Assert.AreEqual(2, changedResults.Count);

            var changedResult1 = changedResults[0];
            Assert.IsNotNull(changedResult1);
            Assert.AreEqual(2, changedResult1.Value);
            Assert.AreEqual(new Option<int>(2), changedResult1.Option);
            Assert.IsFalse(changedResult1.IsChecked);

            var changedResult2 = changedResults[1];
            Assert.IsNotNull(changedResult2);
            Assert.AreEqual(3, changedResult2.Value);
            Assert.AreEqual(new Option<int>(3), changedResult2.Option);
            Assert.IsTrue(changedResult2.IsChecked);

            Assert.AreEqual(3, checkBoxList.Checked.Single());
        }

        [TestMethod]
        public void Interaction_NoChanges()
        {
            var dropdown = new CheckBoxList<int>(new[] { 1, 2, 3 });

            CheckBoxList<int>.CheckBoxListChangedEventArgs changedResult = null;
            dropdown.Changed += (s, e) => changedResult = e;

            // Nothing gets selected
            var mockedUiResults = new Mock<IUIResults>();
            mockedUiResults.Setup(x => x.GetString(It.IsAny<string>())).Returns(String.Empty);

            // Show DropDown
            dropdown.LoadResult(mockedUiResults.Object);
            dropdown.RaiseResultEvents();

            Assert.IsNull(changedResult);

            Assert.AreEqual(0, dropdown.Checked.Count());
        }

        [TestMethod]
        public void DuplicateOptions_ValueConstructor()
        {
            var options = new[] { 1, 1, 1 };

            var checkboxlist = new CheckBoxList<int>(options);

            Assert.AreEqual(1, checkboxlist.Options.Count());
        }

        [TestMethod]
        public void DuplicateOptions_OptionConstructor()
        {
            var options = new[] { new Option<int>(1), new Option<int>(1), new Option<int>(1) };

            var checkboxlist = new CheckBoxList<int>(options);

            Assert.AreEqual(1, checkboxlist.Options.Count());
        }

        [TestMethod]
        public void DuplicateValues_OptionConstructor()
        {
            var options = new[] { new Option<int>("option 1", 1), new Option<int>("option 2", 1), new Option<int>("option 3", 1) };

            var checkboxlist = new CheckBoxList<int>(options);

            Assert.AreEqual(3, checkboxlist.Options.Count());
        }

        [TestMethod]
        public void DuplicateOptions_AddValue()
        {
            var options = new[] { 1 };

            var checkboxlist = new CheckBoxList<int>(options);

            Assert.AreEqual(1, checkboxlist.Options.Count());

            checkboxlist.AddOption(1);

            Assert.AreEqual(1, checkboxlist.Options.Count());
        }

        [TestMethod]
        public void DuplicateOptions_AddOption()
        {
            var options = new[] { new Option<int>(1) };

            var checkboxlist = new CheckBoxList<int>(options);

            Assert.AreEqual(1, checkboxlist.Options.Count());

            checkboxlist.AddOption(new Option<int>(1));

            Assert.AreEqual(1, checkboxlist.Options.Count());
        }

        [TestMethod]
        public void DuplicateValues_AddOption()
        {
            var options = new[] { new Option<int>("option 1", 1) };

            var checkboxlist = new CheckBoxList<int>(options);

            Assert.AreEqual(1, checkboxlist.Options.Count());

            checkboxlist.AddOption(new Option<int>("option 2", 1));

            Assert.AreEqual(2, checkboxlist.Options.Count());
        }

        [TestMethod]
        public void DuplicateValues_CheckOptions()
        {
            var options = new[] { new Option<int>("option 1", 1), new Option<int>("option 2", 1), new Option<int>("option 3", 1) };

            var checkboxlist = new CheckBoxList<int>(options);

            Assert.AreEqual(3, checkboxlist.Unchecked.Count());

            checkboxlist.Check(1);

            Assert.AreEqual(3, checkboxlist.Checked.Count());
        }

        [TestMethod]
        public void NullValueOptions()
        {
            var options = new[] { new Option<string>("option 1", "1"), new Option<string>("option 2", null) };

            var checkboxlist = new CheckBoxList<string>(options);

            Assert.AreEqual(2, checkboxlist.Options.Count());

            checkboxlist.RemoveOption((String)null);

            Assert.AreEqual(1, checkboxlist.Options.Count());

            checkboxlist.SetOptions(new[] { new Option<string>("option 1", "1"), new Option<string>("option 2", null) });

            Assert.AreEqual(2, checkboxlist.Options.Count());
        }

        [TestMethod]
        public void EmptyOption_Test1()
        {
            var options = new[] { Option.Empty<int>() };
            var checkboxlist = new CheckBoxList<int>(options);

            checkboxlist.Check(Option<int>.Empty);

            Assert.IsTrue(checkboxlist.CheckedOptions.Single().IsEmpty);
        }

        [TestMethod]
        public void EmptyOption_Test2()
        {
            var options = new[] { Option.Empty<object>() };
            var checkboxlist = new CheckBoxList<object>(options);

            checkboxlist.Check(Option.Empty<object>());

            Assert.IsTrue(checkboxlist.CheckedOptions.Single().IsEmpty);
        }
    }
}
