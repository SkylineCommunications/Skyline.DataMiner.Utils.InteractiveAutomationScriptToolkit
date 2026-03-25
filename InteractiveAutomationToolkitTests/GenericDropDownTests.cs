using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Skyline.DataMiner.Utils.InteractiveAutomationScript;
using System;
using System.Linq;

namespace InteractiveAutomationToolkitTests
{
    [TestClass]
    public class GenericDropDownTests
    {
        [TestMethod]
        public void EmptyConstructorStruct_Test()
        {
            var dropdown = new DropDown<int>();

            Assert.IsNull(dropdown.SelectedOption);
            Assert.AreEqual(default, dropdown.Selected);
        }

        [TestMethod]
        public void EmptyConstructorClass_Test()
        {
            var dropdown = new DropDown<string>();

            Assert.IsNull(dropdown.SelectedOption);
            Assert.AreEqual(default, dropdown.Selected);
        }

        [TestMethod]
        public void OptionConstructor_Test()
        {
            var options = new[] { new Option<int>("1", 1), new Option<int>("2", 2), new Option<int>("3", 3) };
            var dropDown = new DropDown<int>(options);

            Assert.AreEqual(new Option<int>("1", 1), dropDown.SelectedOption);
            Assert.AreEqual(1, dropDown.Selected);
        }

        [TestMethod]
        public void OptionConstructorWithSelected_Test()
        {
            var options = new[] { new Option<int>("1", 1), new Option<int>("2", 2), new Option<int>("3", 3) };
            var dropDown = new DropDown<int>(options, new Option<int>("3", 3));

            Assert.AreEqual(new Option<int>("3", 3), dropDown.SelectedOption);
            Assert.AreEqual(3, dropDown.Selected);
        }

        [TestMethod]
        public void ValueConstructor_Test()
        {
            var options = new[] { 1, 2, 3 };
            var dropDown = new DropDown<int>(options);

            Assert.AreEqual(new Option<int>("1", 1), dropDown.SelectedOption);
            Assert.AreEqual(1, dropDown.Selected);
        }

        [TestMethod]
        public void ValueConstructorWithSelected_Test()
        {
            var options = new[] { 1, 2, 3 };
            var dropDown = new DropDown<int>(options, 3);

            Assert.AreEqual(new Option<int>("3", 3), dropDown.SelectedOption);
            Assert.AreEqual(3, dropDown.Selected);
        }

        [TestMethod]
        public void EditOptions_Options()
        {
            var dropdown = new DropDown<int>();

            var options = new[] { new Option<int>("1", 1), new Option<int>("2", 2) };
            dropdown.SetOptions(options);

            Assert.AreEqual(options[0], dropdown.SelectedOption);
            Assert.AreEqual(1, dropdown.Selected);

            Assert.AreEqual(2, dropdown.Options.Count());

            Assert.ThrowsExactly<ArgumentException>(() => dropdown.SelectedOption = new Option<int>("3", 3));

            dropdown.Selected = 2;
            Assert.AreEqual(new Option<int>("2", 2), dropdown.SelectedOption);

            dropdown.RemoveOption(new Option<int>("3", 3)); // Removing invalid option does not throw exception

            dropdown.RemoveOption(new Option<int>("1", 1));

            Assert.AreEqual(new Option<int>("2", 2), dropdown.SelectedOption);

            dropdown.AddOption(new Option<int>("1", 1));

            Assert.AreEqual(new Option<int>("2", 2), dropdown.SelectedOption);

            dropdown.RemoveOption(new Option<int>("2", 2));

            Assert.AreEqual(new Option<int>("1", 1), dropdown.SelectedOption);

            dropdown.RemoveOption(new Option<int>("1", 1));

            Assert.IsNull(dropdown.SelectedOption);
            Assert.AreEqual(default, dropdown.Selected);
        }

        [TestMethod]
        public void EditOptions_Values()
        {
            var dropdown = new DropDown<int>();

            var options = new[] { 1, 2 };
            dropdown.SetOptions(options);

            Assert.AreEqual(new Option<int>("1", 1), dropdown.SelectedOption);
            Assert.AreEqual(1, dropdown.Selected);

            Assert.AreEqual(2, dropdown.Options.Count());

            Assert.ThrowsExactly<ArgumentException>(() => dropdown.Selected = 3);

            dropdown.Selected = 2;
            Assert.AreEqual(2, dropdown.Selected);

            dropdown.RemoveOption(3); // Removing invalid option does not throw exception

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

        [TestMethod]
        public void HandleChange_Test1()
        {
            var dropdown = new DropDown<int>(new[] { 1, 2, 3 }, 1);

            DropDown<int>.DropDownChangedEventArgs changedResult = null;
            dropdown.Changed += (s, e) => changedResult = e;

            // User selects 2
            var mockedUiResults = new Mock<IUIResults>();
            mockedUiResults.Setup(x => x.GetString(It.IsAny<string>())).Returns("2");

            // Show DropDown
            dropdown.LoadResult(mockedUiResults.Object);
            dropdown.RaiseResultEvents();

            Assert.IsNotNull(changedResult);
            Assert.AreEqual(1, changedResult.Previous);
            Assert.AreEqual(new Option<int>(1), changedResult.PreviousOption);
            Assert.AreEqual(2, changedResult.Selected);
            Assert.AreEqual(new Option<int>(2), changedResult.SelectedOption);

            Assert.AreEqual(2, dropdown.Selected);
        }

        [TestMethod]
        public void HandleChange_Test2()
        {
            var dropdown = new DropDown<int>(new[] { 1, 2, 3 }, 1);

            DropDown<int>.DropDownChangedEventArgs changedResult = null;
            dropdown.Changed += (s, e) => changedResult = e;

            // User selects 2
            var mockedUiResults = new Mock<IUIResults>();
            mockedUiResults.Setup(x => x.GetString(It.IsAny<string>())).Returns("1");

            // Show DropDown
            dropdown.LoadResult(mockedUiResults.Object);
            dropdown.RaiseResultEvents();

            Assert.IsNull(changedResult);

            Assert.AreEqual(1, dropdown.Selected);
        }

        [TestMethod]
        public void EmptyOption_Test1()
        {
            var options = new[] { Option.Empty<int>() };
            var dropDown = new DropDown<int>(options);

            Assert.AreEqual(Option<int>.Empty, dropDown.SelectedOption);
            Assert.AreEqual(0, dropDown.Selected);
            Assert.IsTrue(dropDown.SelectedOption.IsEmpty);
        }

        [TestMethod]
        public void EmptyOption_Test2()
        {
            var options = new[] { Option.Empty<object>() };
            var dropDown = new DropDown<object>(options);

            Assert.AreEqual(Option<object>.Empty, dropDown.SelectedOption);
            Assert.IsNull(dropDown.Selected);
            Assert.IsTrue(dropDown.SelectedOption.IsEmpty);
        }

        [TestMethod]
        public void ContainsOption_ByValue_ExistingOption_ReturnsTrue()
        {
            var dropDown = new DropDown<int>(new[] { 1, 2, 3 });

            Assert.IsTrue(dropDown.ContainsOption(2));
        }

        [TestMethod]
        public void ContainsOption_ByValue_MissingOption_ReturnsFalse()
        {
            var dropDown = new DropDown<int>(new[] { 1, 2, 3 });

            Assert.IsFalse(dropDown.ContainsOption(99));
        }

        [TestMethod]
        public void ContainsOption_ByOption_ExistingOption_ReturnsTrue()
        {
            var option = new Option<int>("two", 2);
            var dropDown = new DropDown<int>(new[] { new Option<int>("one", 1), option });

            Assert.IsTrue(dropDown.ContainsOption(option));
        }

        [TestMethod]
        public void ContainsOption_ByOption_MissingOption_ReturnsFalse()
        {
            var dropDown = new DropDown<int>(new[] { new Option<int>("one", 1) });

            Assert.IsFalse(dropDown.ContainsOption(new Option<int>("two", 2)));
        }

        [TestMethod]
        public void TrySelectOption_ByValue_ExistingOption_SelectsAndReturnsTrue()
        {
            var dropDown = new DropDown<int>(new[] { 1, 2, 3 }, 1);

            bool result = dropDown.TrySelectOption(3);

            Assert.IsTrue(result);
            Assert.AreEqual(3, dropDown.Selected);
        }

        [TestMethod]
        public void TrySelectOption_ByValue_MissingOption_ReturnsFalseAndKeepsSelection()
        {
            var dropDown = new DropDown<int>(new[] { 1, 2, 3 }, 1);

            bool result = dropDown.TrySelectOption(99);

            Assert.IsFalse(result);
            Assert.AreEqual(1, dropDown.Selected);
        }

        [TestMethod]
        public void TrySelectOption_ByOption_ExistingOption_SelectsAndReturnsTrue()
        {
            var option1 = new Option<int>("one", 1);
            var option2 = new Option<int>("two", 2);
            var dropDown = new DropDown<int>(new[] { option1, option2 }, option1);

            bool result = dropDown.TrySelectOption(option2);

            Assert.IsTrue(result);
            Assert.AreEqual(option2, dropDown.SelectedOption);
        }

        [TestMethod]
        public void TrySelectOption_ByOption_MissingOption_ReturnsFalseAndKeepsSelection()
        {
            var option1 = new Option<int>("one", 1);
            var dropDown = new DropDown<int>(new[] { option1 }, option1);

            bool result = dropDown.TrySelectOption(new Option<int>("missing", 99));

            Assert.IsFalse(result);
            Assert.AreEqual(option1, dropDown.SelectedOption);
        }

        [TestMethod]
        public void ContainsOption_ByOption_NullOption_ThrowsArgumentNullException()
        {
            var dropDown = new DropDown<int>(new[] { 1, 2 });

            Assert.ThrowsExactly<ArgumentNullException>(() => dropDown.ContainsOption((Option<int>)null));
        }

        [TestMethod]
        public void TrySelectOption_ByOption_NullOption_ThrowsArgumentNullException()
        {
            var dropDown = new DropDown<int>(new[] { 1, 2 });

            Assert.ThrowsExactly<ArgumentNullException>(() => dropDown.TrySelectOption((Option<int>)null));
        }
    }
}
