using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Skyline.DataMiner.Utils.InteractiveAutomationScript;

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

			Assert.IsNull(radioButtonList.SelectedOption);
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

			Assert.IsNull(radioButtonList.SelectedOption);
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
		public void SetValuesTest_NothingSelected()
		{
			var options = new[] { 1, 2, 3 };
			var radioButtonList = new RadioButtonList<int>(options);

			Assert.IsNull(radioButtonList.SelectedOption);
			Assert.AreEqual(0, radioButtonList.Selected);

			radioButtonList.SetOptions(new[] { 4, 5, 6 });

			Assert.IsNull(radioButtonList.SelectedOption);
			Assert.AreEqual(0, radioButtonList.Selected);
		}

		[TestMethod]
		public void SetOptionsTest_NothingSelected()
		{
			var options = new[] { new Option<int>("1", 1), new Option<int>("2", 2), new Option<int>("3", 3) };
			var radioButtonList = new RadioButtonList<int>(options);

			Assert.IsNull(radioButtonList.SelectedOption);
			Assert.AreEqual(0, radioButtonList.Selected);

			radioButtonList.SetOptions(new[] { new Option<int>("4", 4), new Option<int>("5", 5), new Option<int>("6", 6) });

			Assert.IsNull(radioButtonList.SelectedOption);
			Assert.AreEqual(0, radioButtonList.Selected);
		}

		[TestMethod]
		public void EditOptions_Options()
		{
			var radioButtonList = new RadioButtonList<int>();

			var options = new[] { new Option<int>("1", 1), new Option<int>("2", 2) };
			radioButtonList.SetOptions(options);

			Assert.IsNull(radioButtonList.SelectedOption);
			Assert.AreEqual(0, radioButtonList.Selected);

			Assert.AreEqual(2, radioButtonList.Options.Count());

			Assert.ThrowsExactly<ArgumentException>(() => radioButtonList.SelectedOption = new Option<int>("3", 3));

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

			Assert.IsNull(radioButtonList.SelectedOption);
			Assert.AreEqual(0, radioButtonList.Selected);

			Assert.AreEqual(2, radioButtonList.Options.Count());

			Assert.ThrowsExactly<ArgumentException>(() => radioButtonList.Selected = 3);

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
			var option2 = radioButtonList.Options.Single(x => x.Value == 2);

			var mockedUiResults = new Mock<IUIResults>();
			mockedUiResults.Setup(x => x.GetString(It.IsAny<string>())).Returns(radioButtonList.GetRawValue(option2));

			// Show RadioButtonList
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

			// User selects 1 (which is already selected)
			var option1 = radioButtonList.Options.Single(x => x.Value == 1);

			var mockedUiResults = new Mock<IUIResults>();
			mockedUiResults.Setup(x => x.GetString(It.IsAny<string>())).Returns(radioButtonList.GetRawValue(option1));

			// Show RadioButtonList
			radioButtonList.LoadResult(mockedUiResults.Object);
			radioButtonList.RaiseResultEvents();

			Assert.IsNull(changedResult);

			Assert.AreEqual(1, radioButtonList.Selected);
		}

		[TestMethod]
		public void EmptyOption_Test1()
		{
			var options = new[] { Option.Empty<int>() };
			var radioButtonList = new RadioButtonList<int>(options, Option<int>.Empty);

			Assert.AreEqual(Option<int>.Empty, radioButtonList.SelectedOption);
			Assert.AreEqual(0, radioButtonList.Selected);
			Assert.IsTrue(radioButtonList.SelectedOption.IsEmpty);
		}

		[TestMethod]
		public void EmptyOption_Test2()
		{
			var options = new[] { Option.Empty<object>() };
			var radioButtonList = new RadioButtonList<object>(options, Option<object>.Empty);

            Assert.AreEqual(Option<object>.Empty, radioButtonList.SelectedOption);
            Assert.IsNull(radioButtonList.Selected);
            Assert.IsTrue(radioButtonList.SelectedOption.IsEmpty);
        }

        [TestMethod]
        public void ContainsOption_ByValue_ExistingOption_ReturnsTrue()
        {
            var radioButtonList = new RadioButtonList<int>(new[] { 1, 2, 3 });

            Assert.IsTrue(radioButtonList.ContainsOption(2));
        }

        [TestMethod]
        public void ContainsOption_ByValue_MissingOption_ReturnsFalse()
        {
            var radioButtonList = new RadioButtonList<int>(new[] { 1, 2, 3 });

            Assert.IsFalse(radioButtonList.ContainsOption(99));
        }

        [TestMethod]
        public void ContainsOption_ByOption_ExistingOption_ReturnsTrue()
        {
            var option = new Option<int>("two", 2);
            var radioButtonList = new RadioButtonList<int>(new[] { new Option<int>("one", 1), option });

            Assert.IsTrue(radioButtonList.ContainsOption(option));
        }

        [TestMethod]
        public void ContainsOption_ByOption_MissingOption_ReturnsFalse()
        {
            var radioButtonList = new RadioButtonList<int>(new[] { new Option<int>("one", 1) });

            Assert.IsFalse(radioButtonList.ContainsOption(new Option<int>("two", 2)));
        }

        [TestMethod]
        public void TrySelectOption_ByValue_ExistingOption_SelectsAndReturnsTrue()
        {
            var radioButtonList = new RadioButtonList<int>(new[] { 1, 2, 3 }, 1);

            bool result = radioButtonList.TrySelectOption(3);

            Assert.IsTrue(result);
            Assert.AreEqual(3, radioButtonList.Selected);
        }

        [TestMethod]
        public void TrySelectOption_ByValue_MissingOption_ReturnsFalseAndKeepsSelection()
        {
            var radioButtonList = new RadioButtonList<int>(new[] { 1, 2, 3 }, 1);

            bool result = radioButtonList.TrySelectOption(99);

            Assert.IsFalse(result);
            Assert.AreEqual(1, radioButtonList.Selected);
        }

        [TestMethod]
        public void TrySelectOption_ByOption_ExistingOption_SelectsAndReturnsTrue()
        {
            var option1 = new Option<int>("one", 1);
            var option2 = new Option<int>("two", 2);
            var radioButtonList = new RadioButtonList<int>(new[] { option1, option2 }, option1);

            bool result = radioButtonList.TrySelectOption(option2);

            Assert.IsTrue(result);
            Assert.AreEqual(option2, radioButtonList.SelectedOption);
        }

        [TestMethod]
        public void TrySelectOption_ByOption_MissingOption_ReturnsFalseAndKeepsSelection()
        {
            var option1 = new Option<int>("one", 1);
            var radioButtonList = new RadioButtonList<int>(new[] { option1 }, option1);

            bool result = radioButtonList.TrySelectOption(new Option<int>("missing", 99));

            Assert.IsFalse(result);
            Assert.AreEqual(option1, radioButtonList.SelectedOption);
        }

        [TestMethod]
        public void ContainsOption_ByOption_NullOption_ThrowsArgumentNullException()
        {
            var radioButtonList = new RadioButtonList<int>(new[] { 1, 2 });

            Assert.ThrowsExactly<ArgumentNullException>(() => radioButtonList.ContainsOption((Option<int>)null));
        }

        [TestMethod]
        public void TrySelectOption_ByOption_NullOption_ThrowsArgumentNullException()
        {
            var radioButtonList = new RadioButtonList<int>(new[] { 1, 2 });

            Assert.ThrowsExactly<ArgumentNullException>(() => radioButtonList.TrySelectOption((Option<int>)null));
        }
    }
}
