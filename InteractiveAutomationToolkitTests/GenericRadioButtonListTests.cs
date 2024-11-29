using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Skyline.DataMiner.Utils.InteractiveAutomationScript;
using System;
using System.Linq;

namespace InteractiveAutomationToolkitTests
{
    [TestClass]
    public class GenericRadioButtonListTests
    {
        [TestMethod]
        public void EmptyConstructorStruct_Test()
        {
            var radioButtonList = new RadioButtonList<int>();

            Assert.IsNull(radioButtonList.SelectedOption);
            Assert.AreEqual(default, radioButtonList.Selected);
        }

        [TestMethod]
        public void EmptyConstructorClass_Test()
        {
            var radioButtonList = new RadioButtonList<string>();

            Assert.IsNull(radioButtonList.SelectedOption);
            Assert.AreEqual(default, radioButtonList.Selected);
        }

        [TestMethod]
        public void OptionConstructor_Test()
        {
            var options = new[] { new Option<int>("1", 1), new Option<int>("2", 2), new Option<int>("3", 3) };
            var radioButtonList = new RadioButtonList<int>(options);

            Assert.AreEqual(null, radioButtonList.SelectedOption);
            Assert.AreEqual(default, radioButtonList.Selected);
        }

        [TestMethod]
        public void OptionConstructorWithSelected_Test()
        {
            var options = new[] { new Option<int>("1", 1), new Option<int>("2", 2), new Option<int>("3", 3) };
            var radioButtonList = new RadioButtonList<int>(options, new Option<int>("3", 3));

            Assert.AreEqual(new Option<int>("3", 3), radioButtonList.SelectedOption);
            Assert.AreEqual(3, radioButtonList.Selected);
        }

        [TestMethod]
        public void ValueConstructor_Test()
        {
            var options = new[] { 1, 2, 3 };
            var radioButtonList = new RadioButtonList<int>(options);

            Assert.AreEqual(null, radioButtonList.SelectedOption);
            Assert.AreEqual(default, radioButtonList.Selected);
        }

        [TestMethod]
        public void ValueConstructorWithSelected_Test()
        {
            var options = new[] { 1, 2, 3 };
            var radioButtonList = new RadioButtonList<int>(options, 3);

            Assert.AreEqual(new Option<int>("3", 3), radioButtonList.SelectedOption);
            Assert.AreEqual(3, radioButtonList.Selected);
        }

        [TestMethod]
        public void EditOptions_Options()
        {
            var radioButtonList = new RadioButtonList<int>();

            var options = new[] { new Option<int>("1", 1), new Option<int>("2", 2) };
            radioButtonList.SetOptions(options);

            Assert.AreEqual(options[0], radioButtonList.SelectedOption);
            Assert.AreEqual(1, radioButtonList.Selected);

            Assert.AreEqual(2, radioButtonList.Options.Count());

            Assert.ThrowsException<InvalidOperationException>(() => radioButtonList.SelectedOption = new Option<int>("3", 3));

            radioButtonList.Selected = 2;
            Assert.AreEqual(new Option<int>("2", 2), radioButtonList.SelectedOption);

            radioButtonList.RemoveOption(new Option<int>("3", 3)); // Removing invalid option does not throw exception

            radioButtonList.RemoveOption(new Option<int>("1", 1));

            Assert.AreEqual(new Option<int>("2", 2), radioButtonList.SelectedOption);

            radioButtonList.AddOption(new Option<int>("1", 1));

            Assert.AreEqual(new Option<int>("2", 2), radioButtonList.SelectedOption);

            radioButtonList.RemoveOption(new Option<int>("2", 2));

            Assert.AreEqual(new Option<int>("1", 1), radioButtonList.SelectedOption);

            radioButtonList.RemoveOption(new Option<int>("1", 1));

            Assert.IsNull(radioButtonList.SelectedOption);
            Assert.AreEqual(default, radioButtonList.Selected);
        }

        [TestMethod]
        public void EditOptions_Values()
        {
            var radioButtonList = new RadioButtonList<int>();

            var options = new[] { 1, 2 };
            radioButtonList.SetOptions(options);

            Assert.AreEqual(new Option<int>("1", 1), radioButtonList.SelectedOption);
            Assert.AreEqual(1, radioButtonList.Selected);

            Assert.AreEqual(2, radioButtonList.Options.Count());

            Assert.ThrowsException<InvalidOperationException>(() => radioButtonList.Selected = 3);

            radioButtonList.Selected = 2;
            Assert.AreEqual(2, radioButtonList.Selected);

            radioButtonList.RemoveOption(3); // Removing invalid option does not throw exception

            radioButtonList.RemoveOption(1);

            Assert.AreEqual(2, radioButtonList.Selected);

            radioButtonList.AddOption(1);

            Assert.AreEqual(2, radioButtonList.Selected);

            radioButtonList.RemoveOption(2);

            Assert.AreEqual(1, radioButtonList.Selected);

            radioButtonList.RemoveOption(1);

            Assert.IsNull(radioButtonList.SelectedOption);
            Assert.AreEqual(default, radioButtonList.Selected);
        }

        [TestMethod]
        public void HandleChange_Test1()
        {
            var radioButtonList = new RadioButtonList<int>(new[] { 1, 2, 3 }, 1);

            RadioButtonList<int>.RadioButtonChangedEventArgs changedResult = null;
            radioButtonList.Changed += (s, e) => changedResult = e;

            // User selects 2
            var mockedUiResults = new Mock<IUIResults>();
            mockedUiResults.Setup(x => x.GetString(It.IsAny<string>())).Returns("2");

            // Show DropDown
            radioButtonList.LoadResult(mockedUiResults.Object);
            radioButtonList.RaiseResultEvents();

            Assert.IsNotNull(changedResult);
            Assert.AreEqual(1, changedResult.Previous);
            Assert.AreEqual(new Option<int>(1), changedResult.PreviousOption);
            Assert.AreEqual(2, changedResult.Selected);
            Assert.AreEqual(new Option<int>(2), changedResult.SelectedOption);

            Assert.AreEqual(2, radioButtonList.Selected);
        }

        [TestMethod]
        public void HandleChange_Test2()
        {
            var radioButtonList = new RadioButtonList<int>(new[] { 1, 2, 3 }, 1);

            RadioButtonList<int>.RadioButtonChangedEventArgs changedResult = null;
            radioButtonList.Changed += (s, e) => changedResult = e;

            // User selects 2
            var mockedUiResults = new Mock<IUIResults>();
            mockedUiResults.Setup(x => x.GetString(It.IsAny<string>())).Returns("1");

            // Show DropDown
            radioButtonList.LoadResult(mockedUiResults.Object);
            radioButtonList.RaiseResultEvents();

            Assert.IsNull(changedResult);

            Assert.AreEqual(1, radioButtonList.Selected);
        }
    }
}
