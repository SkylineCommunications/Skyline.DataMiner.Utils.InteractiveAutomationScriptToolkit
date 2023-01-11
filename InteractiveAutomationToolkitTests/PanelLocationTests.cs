namespace InteractiveAutomationToolkitTests
{
	using System;
	using System.Collections.Generic;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.Utils.InteractiveAutomationToolkit;

	using static FluentAssertions.FluentActions;

	[TestClass]
	public class PanelLocationTests
	{
		[DataTestMethod]
		[DataRow(-1, 0)]
		[DataRow(0, -1)]
		public void InvalidConstructorArgumentsTest(int fromRow, int fromColumn)
		{
			Invoking(() => new PanelLocation(fromRow, fromColumn))
				.Should()
				.Throw<ArgumentOutOfRangeException>();
		}

		[DataTestMethod]
		[DynamicData(nameof(EqualTestData), DynamicDataSourceType.Method)]
		public void EqualTest(PanelLocation location, PanelLocation otherLocation)
		{
			location.Equals(otherLocation)
				.Should()
				.BeTrue("The object should be the equal if all values are equal");

			(location == otherLocation)
				.Should()
				.BeTrue("Because the equal operators should also perform value equality.");

			location.GetHashCode()
				.Should()
				.Be(otherLocation.GetHashCode(), "The hashcode should be the same if the structs are equal.");
		}

		[DataTestMethod]
		[DynamicData(nameof(NotEqualTestData), DynamicDataSourceType.Method)]
		public void NotEqualTest(PanelLocation location, PanelLocation otherLocation)
		{
			location.Equals(otherLocation)
				.Should()
				.BeFalse("The object should not be the equal if some values are different");

			(location != otherLocation)
				.Should()
				.BeTrue("Because the equal operators should also perform value equality.");

			location.GetHashCode()
				.Should()
				.NotBe(otherLocation.GetHashCode(), "The hashcode should be the same if the structs are equal.");
		}

		[TestMethod]
		public void AddOffsetTest()
		{
			// Arrange
			var location = new PanelLocation(2, 1);
			var offset = new PanelLocation(3, 1);
			var expected = new PanelLocation(5, 2);

			// Act
			PanelLocation actual = location.AddOffset(offset);

			// Assert
			actual.Should().Be(expected);
		}

		private static IEnumerable<object[]> EqualTestData()
		{
			yield return new object[] { new PanelLocation(0, 0), new PanelLocation(0, 0) };
			yield return new object[] { new PanelLocation(7, 0), new PanelLocation(7, 0) };
			yield return new object[] { new PanelLocation(0, 9), new PanelLocation(0, 9) };
		}

		private static IEnumerable<object[]> NotEqualTestData()
		{
			yield return new object[] { new PanelLocation(0, 0), new PanelLocation(7, 0) };
			yield return new object[] { new PanelLocation(0, 0), new PanelLocation(0, 9) };
		}
	}
}