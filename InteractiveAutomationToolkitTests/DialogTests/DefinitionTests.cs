namespace InteractiveAutomationToolkitTests
{
	using System;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Moq;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.InteractiveAutomationToolkit;

	using static FluentAssertions.FluentActions;

	[TestClass]
	public class DefinitionTests
	{
		[TestMethod]
		public void EmptyDefinitionsTest()
		{
			// Arrange
			var dialog = new Dialog<GridPanel>(Mock.Of<IEngine>());
			var privateObject = new PrivateObject(dialog);

			// Act
			var columnDefinitions = (string)privateObject.Invoke("GetColumnDefinitions");
			var rowDefinitions = (string)privateObject.Invoke("GetRowDefinitions");

			// Assert
			rowDefinitions.Should().BeEmpty();
			columnDefinitions.Should().BeEmpty();
		}

		[TestMethod]
		public void DefaultDefinitionsTest()
		{
			// Arrange
			var dialog = new Dialog<GridPanel>(Mock.Of<IEngine>());
			dialog.Panel.Add(new Label(), 0, 0);
			dialog.Panel.Add(new Label(), 1, 0);
			dialog.Panel.Add(new Label(), 1, 1);
			dialog.Panel.Add(new Label(), 1, 2);
			var privateObject = new PrivateObject(dialog);

			// Act
			var columnDefinitions = (string)privateObject.Invoke("GetColumnDefinitions");
			var rowDefinitions = (string)privateObject.Invoke("GetRowDefinitions");

			// Assert
			rowDefinitions.Should().Be("auto;auto");
			columnDefinitions.Should().Be("auto;auto;auto");
		}

		[TestMethod]
		public void SetRowHeightTest()
		{
			// Arrange
			var dialog = new Dialog<GridPanel>(Mock.Of<IEngine>());
			dialog.Panel.Add(new Label(), 1, 1);

			// Act
			dialog.SetRowHeight(1, 150);

			// Assert
			((string)new PrivateObject(dialog).Invoke("GetRowDefinitions")).Should().Be("auto;150");
		}

		[TestMethod]
		public void SetInvalidRowHeightHeightTest()
		{
			// Arrange
			var dialog = new Dialog<GridPanel>(Mock.Of<IEngine>());
			dialog.Panel.Add(new Label(), 1, 1);

			// Act & Assert
			Invoking(() => dialog.SetRowHeight(1, -1))
				.Should()
				.Throw<ArgumentOutOfRangeException>("RowHeight cannot be set to zero or lower.");
		}

		[TestMethod]
		public void SetInvalidRowHeightRowTest()
		{
			// Arrange
			var dialog = new Dialog<GridPanel>(Mock.Of<IEngine>());
			dialog.Panel.Add(new Label(), 1, 1);

			// Act & Assert
			Invoking(() => dialog.SetRowHeight(-1, 150))
				.Should()
				.Throw<ArgumentOutOfRangeException>("a row index cannot be set to a negative number.");
		}

		[TestMethod]
		public void SetRowHeightStretchTest()
		{
			// Arrange
			var dialog = new Dialog<GridPanel>(Mock.Of<IEngine>());
			dialog.Panel.Add(new Label(), 1, 1);

			// Act
			dialog.SetRowHeightStretch(1);

			// Assert
			((string)new PrivateObject(dialog).Invoke("GetRowDefinitions")).Should().Be("auto;*");
		}

		[TestMethod]
		public void SetInvalidRowStretchTest()
		{
			// Arrange
			var dialog = new Dialog<GridPanel>(Mock.Of<IEngine>());
			dialog.Panel.Add(new Label(), 1, 1);

			// Act & Assert
			Invoking(() => dialog.SetRowHeightStretch(-1))
				.Should()
				.Throw<ArgumentOutOfRangeException>("a row index cannot be set to a negative number.");
		}

		[TestMethod]
		public void SetRowHeightAutoTest()
		{
			// Arrange
			var dialog = new Dialog<GridPanel>(Mock.Of<IEngine>());
			dialog.Panel.Add(new Label(), 1, 1);
			dialog.SetRowHeight(0, 100);
			dialog.SetRowHeight(1, 200);

			// Act
			dialog.SetRowHeightAuto(1);

			// Assert
			((string)new PrivateObject(dialog).Invoke("GetRowDefinitions")).Should().Be("100;auto");
		}

		[TestMethod]
		public void SetInvalidRowAutoTest()
		{
			// Arrange
			var dialog = new Dialog<GridPanel>(Mock.Of<IEngine>());
			dialog.Panel.Add(new Label(), 1, 1);

			// Act & Assert
			Invoking(() => dialog.SetRowHeightAuto(-1))
				.Should()
				.Throw<ArgumentOutOfRangeException>("a row index cannot be set to a negative number.");
		}

		[TestMethod]
		public void SetColumnWidthTest()
		{
			// Arrange
			var dialog = new Dialog<GridPanel>(Mock.Of<IEngine>());
			dialog.Panel.Add(new Label(), 1, 1);

			// Act
			dialog.SetColumnWidth(1, 150);

			// Assert
			((string)new PrivateObject(dialog).Invoke("GetColumnDefinitions")).Should().Be("auto;150");
		}

		[TestMethod]
		public void SetInvalidColumnWidthHeightTest()
		{
			// Arrange
			var dialog = new Dialog<GridPanel>(Mock.Of<IEngine>());
			dialog.Panel.Add(new Label(), 1, 1);

			// Act & Assert
			Invoking(() => dialog.SetColumnWidth(1, -1))
				.Should()
				.Throw<ArgumentOutOfRangeException>("ColumnWidth cannot be set to a negative number.");
		}

		[TestMethod]
		public void SetInvalidColumnWidthRowTest()
		{
			// Arrange
			var dialog = new Dialog<GridPanel>(Mock.Of<IEngine>());
			dialog.Panel.Add(new Label(), 1, 1);

			// Act & Assert
			Invoking(() => dialog.SetColumnWidth(-1, 150))
				.Should()
				.Throw<ArgumentOutOfRangeException>("a column index cannot be lower than 0.");
		}

		[TestMethod]
		public void SetColumnWidthStretchTest()
		{
			// Arrange
			var dialog = new Dialog<GridPanel>(Mock.Of<IEngine>());
			dialog.Panel.Add(new Label(), 1, 1);

			// Act
			dialog.SetColumnWidthStretch(1);

			// Assert
			((string)new PrivateObject(dialog).Invoke("GetColumnDefinitions")).Should().Be("auto;*");
		}

		[TestMethod]
		public void SetInvalidColumnStretchTest()
		{
			// Arrange
			var dialog = new Dialog<GridPanel>(Mock.Of<IEngine>());
			dialog.Panel.Add(new Label(), 1, 1);

			// Act & Assert
			Invoking(() => dialog.SetColumnWidthStretch(-1))
				.Should()
				.Throw<ArgumentOutOfRangeException>("a column index cannot be lower than 0.");
		}

		[TestMethod]
		public void SetColumnWidthAutoTest()
		{
			// Arrange
			var dialog = new Dialog<GridPanel>(Mock.Of<IEngine>());
			dialog.Panel.Add(new Label(), 1, 1);
			dialog.SetColumnWidth(0, 100);
			dialog.SetColumnWidth(1, 200);

			// Act
			dialog.SetColumnWidthAuto(1);

			// Assert
			((string)new PrivateObject(dialog).Invoke("GetColumnDefinitions")).Should().Be("100;auto");
		}

		[TestMethod]
		public void SetInvalidColumnAutoTest()
		{
			// Arrange
			var dialog = new Dialog<GridPanel>(Mock.Of<IEngine>());
			dialog.Panel.Add(new Label(), 1, 1);

			// Act & Assert
			Invoking(() => dialog.SetColumnWidthAuto(-1))
				.Should()
				.Throw<ArgumentOutOfRangeException>("a column index cannot be lower than 0.");
		}
	}
}