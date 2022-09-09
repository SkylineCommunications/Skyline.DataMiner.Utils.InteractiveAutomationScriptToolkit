namespace InteractiveAutomationToolkitTests.Generic
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.InteractiveAutomationToolkit;

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
			dropDown.SelectedText.Should().BeNull();
			dropDown.SelectedValue.Should().Be(default);
		}
	}
}