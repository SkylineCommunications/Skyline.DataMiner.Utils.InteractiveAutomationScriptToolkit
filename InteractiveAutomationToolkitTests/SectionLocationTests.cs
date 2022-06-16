namespace InteractiveAutomationToolkitTests
{
	using System;
	using System.Collections.Generic;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.InteractiveAutomationToolkit;

	using static FluentAssertions.FluentActions;

	[TestClass]
	public class SectionLocationTests
	{
		[DataTestMethod]
		[DataRow(-1, 0)]
		[DataRow(0, -1)]
		public void InvalidConstructorArgumentsTest(int fromRow, int fromColumn)
		{
			Invoking(() => new SectionLocation(fromRow, fromColumn))
				.Should()
				.Throw<ArgumentOutOfRangeException>();
		}

		[DataTestMethod]
		[DynamicData(nameof(EqualTestData), DynamicDataSourceType.Method)]
		public void EqualTest(SectionLocation location, SectionLocation otherLocation)
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
		public void NotEqualTest(SectionLocation location, SectionLocation otherLocation)
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
			var location = new SectionLocation(2, 1);
			var offset = new SectionLocation(3, 1);
			var expected = new SectionLocation(5, 2);

			// Act
			SectionLocation actual = location.AddOffset(offset);

			// Assert
			actual.Should().Be(expected);
		}

		private static IEnumerable<object[]> EqualTestData()
		{
			yield return new object[] { new SectionLocation(0, 0), new SectionLocation(0, 0) };
			yield return new object[] { new SectionLocation(7, 0), new SectionLocation(7, 0) };
			yield return new object[] { new SectionLocation(0, 9), new SectionLocation(0, 9) };
		}

		private static IEnumerable<object[]> NotEqualTestData()
		{
			yield return new object[] { new SectionLocation(0, 0), new SectionLocation(7, 0) };
			yield return new object[] { new SectionLocation(0, 0), new SectionLocation(0, 9) };
		}
	}
}