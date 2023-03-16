namespace InteractiveAutomationToolkitTests.Generic
{
	using System;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	using static FluentAssertions.FluentActions;

	[TestClass]
	public class DropDownTests
	{
		[TestMethod]
		public void EmptyDropDownTest()
		{
			// Act
			var dropDown = new DropDown<int>();

			// Assert
			dropDown.Options.Should().BeEmpty();
			dropDown.Selected.Should().Be(default(Option<int>), "an empty dropdown should not have a selected option.");
			dropDown.SelectedName.Should().BeNull("an empty dropdown should not have a selected text.");
			dropDown.SelectedValue.Should().Be(default, "an empty dropdown should not have a selected value");
		}

		[TestMethod]
		public void InitDropDownWithOptionsTest()
		{
			// Act
			var dropDown = new DropDown<int>(new[] { Option.Create("1", 1), Option.Create("2", 2) });

			// Assert
			dropDown.Options.Should().HaveCount(2);
			dropDown.SelectedName.Should().Be("1");
			dropDown.SelectedValue.Should().Be(1);
		}

		[TestMethod]
		public void ConstuctorNullArgumentTest()
		{
			// Act & Assert
			Invoking(() => new DropDown<int>(null)).Should().Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void SetSelectedOptionTest()
		{
			// Arrange
			var dropDown = new DropDown<int>(new[] { Option.Create("1", 1), Option.Create("2", 2) });

			// Act
			dropDown.Selected = Option.Create("2", 2);

			// Assert
			dropDown.SelectedName.Should().Be("2");
			dropDown.SelectedValue.Should().Be(2);
		}

		[TestMethod]
		public void SetInvalidSelectedOptionTest()
		{
			// Arrange
			var dropDown = new DropDown<int>(new[] { Option.Create("1", 1), Option.Create("2", 2) });

			// Act
			dropDown.Selected = Option.Create("2", 1);

			// Assert
			dropDown.SelectedName.Should().Be("1");
			dropDown.SelectedValue.Should().Be(1);
		}

		[TestMethod]
		public void SetSelectedOptionByValueTest()
		{
			// Arrange
			var dropDown = new DropDown<int>(new[] { Option.Create("1", 1), Option.Create("2", 2) });

			// Act
			dropDown.SelectedValue = 2;

			// Assert
			dropDown.SelectedName.Should().Be("2");
			dropDown.SelectedValue.Should().Be(2);
		}

		[TestMethod]
		public void SetInvalidSelectedOptionByValueTest()
		{
			// Arrange
			var dropDown = new DropDown<int>(new[] { Option.Create("1", 1), Option.Create("2", 2) });

			// Act
			dropDown.SelectedValue = 0;

			// Assert
			dropDown.SelectedName.Should().Be("1");
			dropDown.SelectedValue.Should().Be(1);
		}

		[TestMethod]
		public void AddNullRangeTest()
		{
			// Act & Assert
			Invoking(() => new DropDown<int>().Options.AddRange(null)).Should().Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void AddOptionWithDuplicateTextTest()
		{
			// Arrange
			var dropDown = new DropDown<int>(new[] { Option.Create("foo", 1) });

			// Act & Assert
			Invoking(() => dropDown.Options.Add("foo", 2)).Should().Throw<ArgumentException>();
		}

		[TestMethod]
		public void AddOptionWithDuplicateValueTest()
		{
			// Arrange
			var dropDown = new DropDown<int>(new[] { Option.Create("foo", 1) });

			// Act & Assert
			Invoking(() => dropDown.Options.Add("bar", 1)).Should().Throw<ArgumentException>();
		}

		[TestMethod]
		public void WantsOnChangeTest()
		{
			var dropDown = new DropDown<int>();
			Assert.IsFalse(dropDown.WantsOnChange);

			dropDown.Changed += DoNothing;
			Assert.IsTrue(dropDown.WantsOnChange);

			dropDown.Changed -= DoNothing;
			Assert.IsFalse(dropDown.WantsOnChange);

			void DoNothing(object o, DropDown<int>.ChangedEventArgs args)
			{
			}
		}
	}
}