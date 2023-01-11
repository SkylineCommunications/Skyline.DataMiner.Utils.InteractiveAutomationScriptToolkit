namespace InteractiveAutomationToolkitTests
{
	using System;
	using System.Collections.Generic;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.Utils.InteractiveAutomationToolkit;

	using static FluentAssertions.FluentActions;

	[TestClass]
	public class WidgetLocationTests
	{
		[TestMethod]
		public void NoOverlapSameColumnTest()
		{
			// Arrange
			var location = new WidgetLocation(0, 0);
			var otherLocation = new WidgetLocation(1, 0);

			// Act
			bool overlaps = location.Overlaps(otherLocation);

			// Assert
			overlaps.Should().BeFalse();
		}

		[TestMethod]
		public void NoOverlapSameRowTest()
		{
			// Arrange
			var location = new WidgetLocation(0, 0);
			var otherLocation = new WidgetLocation(0, 1);

			// Act
			bool overlaps = location.Overlaps(otherLocation);

			// Assert
			overlaps.Should().BeFalse();
		}

		[TestMethod]
		public void NoOverlapTest()
		{
			// Arrange
			var location = new WidgetLocation(5, 3);
			var otherLocation = new WidgetLocation(7, 2);

			// Act
			bool overlaps = location.Overlaps(otherLocation);

			// Assert
			overlaps.Should().BeFalse();
		}

		[TestMethod]
		public void OverlapAtRootTest()
		{
			// Arrange
			var location = new WidgetLocation(0, 0);
			var otherLocation = new WidgetLocation(0, 0);

			// Act
			bool overlaps = location.Overlaps(otherLocation);

			// Assert
			overlaps.Should().BeTrue();
		}

		[TestMethod]
		public void OverlapColumnSpanTest()
		{
			// Arrange
			var location = new WidgetLocation(0, 0, 1, 2);
			var otherLocation = new WidgetLocation(0, 1);

			// Act
			bool overlaps = location.Overlaps(otherLocation);

			// Assert
			overlaps.Should().BeTrue();
		}

		[TestMethod]
		public void OverlapRowSpanTest()
		{
			// Arrange
			var location = new WidgetLocation(0, 0, 2, 1);
			var otherLocation = new WidgetLocation(1, 0);

			// Act
			bool overlaps = location.Overlaps(otherLocation);

			// Assert
			overlaps.Should().BeTrue();
		}

		[TestMethod]
		public void OverlapSpanTest()
		{
			// Arrange
			var location = new WidgetLocation(0, 0, 2, 2);
			var otherLocation = new WidgetLocation(1, 1);

			// Act
			bool overlaps = location.Overlaps(otherLocation);

			// Assert
			overlaps.Should().BeTrue();
		}

		[TestMethod]
		public void OverlapTest()
		{
			// Arrange
			var location = new WidgetLocation(5, 3);
			var otherLocation = new WidgetLocation(5, 3);

			// Act
			bool overlaps = location.Overlaps(otherLocation);

			// Assert
			overlaps.Should().BeTrue();
		}

		[DataTestMethod]
		[DataRow(-1, 0, 1, 1)]
		[DataRow(0, -1, 1, 1)]
		[DataRow(0, 0, 0, 1)]
		[DataRow(0, 0, -1, 1)]
		[DataRow(0, 0, 1, 0)]
		[DataRow(0, 0, 1, -1)]
		public void InvalidConstructorArgumentsTest(int fromRow, int fromColumn, int rowSpan, int columnSpan)
		{
			Invoking(() => new WidgetLocation(fromRow, fromColumn, rowSpan, columnSpan))
				.Should()
				.Throw<ArgumentOutOfRangeException>();
		}

		[DataTestMethod]
		[DynamicData(nameof(EqualTestData), DynamicDataSourceType.Method)]
		public void EqualTest(WidgetLocation location, WidgetLocation otherLocation)
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
		public void NotEqualTest(WidgetLocation location, WidgetLocation otherLocation)
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
			var location = new WidgetLocation(2, 1);
			var offset = new PanelLocation(3, 1);
			var expected = new WidgetLocation(5, 2);

			// Act
			WidgetLocation actual = location.AddOffset(offset);

			// Assert
			actual.Should().Be(expected);
		}

		private static IEnumerable<object[]> EqualTestData()
		{
			yield return new object[] { new WidgetLocation(0, 0), new WidgetLocation(0, 0) };
			yield return new object[] { new WidgetLocation(12, 0), new WidgetLocation(12, 0) };
			yield return new object[] { new WidgetLocation(0, 7), new WidgetLocation(0, 7) };
			yield return new object[] { new WidgetLocation(0, 0, 1, 1), new WidgetLocation(0, 0, 1, 1) };
			yield return new object[] { new WidgetLocation(0, 0, 9, 1), new WidgetLocation(0, 0, 9, 1) };
			yield return new object[] { new WidgetLocation(0, 0, 1, 14), new WidgetLocation(0, 0, 1, 14) };
			yield return new object[] { new WidgetLocation(0, 0, 1, 1), new WidgetLocation(0, 0) };
		}

		private static IEnumerable<object[]> NotEqualTestData()
		{
			yield return new object[] { new WidgetLocation(0, 0), new WidgetLocation(8, 0) };
			yield return new object[] { new WidgetLocation(0, 0), new WidgetLocation(0, 16) };
			yield return new object[] { new WidgetLocation(0, 0), new WidgetLocation(0, 0, 5, 3) };
			yield return new object[] { new WidgetLocation(0, 0, 1, 1), new WidgetLocation(0, 0, 9, 1) };
			yield return new object[] { new WidgetLocation(0, 0, 1, 1), new WidgetLocation(0, 0, 1, 14) };
		}
	}
}