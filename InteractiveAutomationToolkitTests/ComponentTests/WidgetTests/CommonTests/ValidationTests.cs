namespace InteractiveAutomationToolkitTests
{
	using System.ComponentModel;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Moq;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.InteractiveAutomationToolkit;

	using static FluentAssertions.FluentActions;

	[TestClass]
	public class ValidationTests
	{
		[TestMethod]
		public void InitializeTest()
		{
			// Arrange
			var widgetMock = Mock.Of<IWidget>(widget => widget.BlockDefinition == new UIBlockDefinition());

			// Act
			var validation = new Validation(widgetMock);

			// Assert
			widgetMock.BlockDefinition.ValidationText.Should().Be(validation.ValidationText);
			widgetMock.BlockDefinition.ValidationState.Should().Be(validation.ValidationState);
		}

		[TestMethod]
		public void SetTest()
		{
			// Arrange
			var widgetMock = Mock.Of<IWidget>(widget => widget.BlockDefinition == new UIBlockDefinition());
			var validation = new Validation(widgetMock);

			// Act
			validation.ValidationText = "foo";
			validation.ValidationState = UIValidationState.Invalid;

			// Assert
			widgetMock.BlockDefinition.ValidationText.Should().Be("foo");
			widgetMock.BlockDefinition.ValidationState.Should().Be(UIValidationState.Invalid);
		}

		[TestMethod]
		public void SetNullTextTest()
		{
			// Arrange
			var widgetMock = Mock.Of<IWidget>(widget => widget.BlockDefinition == new UIBlockDefinition());
			var validation = new Validation(widgetMock);

			// Act
			validation.ValidationText = null;

			// Assert
			widgetMock.BlockDefinition.ValidationText.Should().BeEmpty();
		}

		[TestMethod]
		public void SetInvalidEnumTest()
		{
			// Arrange
			var widgetMock = Mock.Of<IWidget>(widget => widget.BlockDefinition == new UIBlockDefinition());
			var validation = new Validation(widgetMock);

			// Act & Assert
			Invoking(() => validation.ValidationState = (UIValidationState)99)
				.Should()
				.Throw<InvalidEnumArgumentException>();
		}
	}
}