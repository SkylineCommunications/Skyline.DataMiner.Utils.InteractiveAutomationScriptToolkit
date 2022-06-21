namespace InteractiveAutomationToolkitTests
{
	using System;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.InteractiveAutomationToolkit;

	using static FluentAssertions.FluentActions;

	[TestClass]
	public class SectionTests
	{
		[TestMethod]
		public void AddCyclicSectionsTest()
		{
			// Arrange
			var sectionA = new Section();
			var sectionB = new Section();
			var sectionC = new Section();

			// Act & Assert
			sectionA.AddSection(sectionB, 0, 0);
			sectionB.AddSection(sectionC, 0, 0);
			Invoking(() => sectionC.AddSection(sectionA, 0, 0)).Should().Throw<ArgumentException>();
		}

		[TestMethod]
		public void AddMultipleSpanningWidgetTest()
		{
			// Arrange
			var section = new Section();

			// Act
			section.AddWidget(new Label(), 0, 0, 1, 2);
			section.AddWidget(new Label(), 1, 0, 2, 1);
			section.AddWidget(new Label(), 1, 1);
			section.AddWidget(new Label(), 2, 1);

			// Assert
			section.RowCount.Should().Be(3);
			section.ColumnCount.Should().Be(2);
		}

		[TestMethod]
		public void AddMultipleWidgetTest()
		{
			// Arrange
			var section = new Section();

			// Act
			section.AddWidget(new Label(), 0, 0);
			section.AddWidget(new Label(), 0, 1);
			section.AddWidget(new Label(), 1, 0);
			section.AddWidget(new Label(), 2, 0);

			// Assert
			section.RowCount.Should().Be(3);
			section.ColumnCount.Should().Be(2);
			section.GetWidgets().Should().HaveCount(4);
		}

		[TestMethod]
		public void AddNullSectionTest()
		{
			// Arrange
			var section = new Section();
			section.AddWidget(new Label(), 0, 0);

			// Act & Assert
			Invoking(() => section.AddSection(null, 1, 0)).Should().Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void AddNullWidgetTest()
		{
			// Arrange
			var section = new Section();

			// Act & Assert
			Invoking(() => section.AddWidget(null, 1, 0)).Should().Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void AddSectionTest()
		{
			// Arrange
			var section = new Section();
			section.AddWidget(new Label(), 0, 0);

			var subsection = new Section();
			subsection.AddWidget(new Label(), 0, 1);

			// Act
			section.AddSection(subsection, 1, 0);

			// Assert
			section.GetSections().Should().HaveCount(1, "we added one subsection to the section.");
			section.GetWidgets()
				.Should()
				.HaveCount(2, "the section has one direct child widget and one from the subsection.");

			section.GetWidgets(false).Should().HaveCount(1, "only one widget is a direct child of the section.");
			section.RowCount.Should().Be(2, "because widgets in subsections count towards the row count.");
			section.ColumnCount.Should().Be(2, "because widgets in subsections count towards the column count.");
		}

		[TestMethod]
		public void OnlySubsectionTest()
		{
			// Arrange
			var section = new Section();

			var subsection = new Section();
			subsection.AddWidget(new Label(), 0, 0);
			subsection.AddWidget(new Label(), 0, 1);
			subsection.AddWidget(new Label(), 0, 2);

			// Act
			section.AddSection(subsection, 0, 0);

			// Assert
			section.GetSections().Should().HaveCount(1, "we added one subsection to the section.");
			section.GetWidgets()
				.Should()
				.HaveCount(3, "the subsection has 3 widgets.");

			section.GetWidgets(false).Should().HaveCount(0, "no widgets are a direct child of the section.");
			section.RowCount.Should().Be(1, "because widgets in subsections count towards the row count.");
			section.ColumnCount.Should().Be(3, "because widgets in subsections count towards the column count.");
		}

		[TestMethod]
		public void AddSectionToItselfTest()
		{
			// Arrange
			var section = new Section();
			section.AddWidget(new Label(), 0, 0);

			// Act & Assert
			Invoking(() => section.AddSection(section, 1, 0)).Should().Throw<ArgumentException>();
		}

		[TestMethod]
		public void AddSectionWithSameWidgetsTest()
		{
			// Arrange
			var section = new Section();
			var label = new Label();
			section.AddWidget(label, 0, 0);

			var subsection = new Section();
			subsection.AddWidget(label, 0, 0);

			// Act & Assert
			Invoking(() => section.AddSection(subsection, 1, 0)).Should().Throw<ArgumentException>();
		}

		[TestMethod]
		public void AddSingleWidgetTest()
		{
			// Arrange
			var section = new Section();

			// Act
			section.AddWidget(new Label(), 0, 0);

			// Assert
			section.RowCount.Should().Be(1, "a single widget was added to row index 0.");
			section.ColumnCount.Should().Be(1, "a single widget was added to column index 0.");
			section.GetWidgets().Should().HaveCount(1, "a single widget was added.");
		}

		[TestMethod]
		public void AddWidgetTwiceTest()
		{
			// Arrange
			var widget = new Label();
			var section = new Section();
			section.AddWidget(widget, 0, 0);

			// Act & Assert
			Invoking(() => section.AddWidget(widget, 1, 0)).Should().Throw<ArgumentException>();
		}

		[TestMethod]
		public void ClearTest()
		{
			// Arrange
			var section = new Section();
			section.AddWidget(new Label(), 0, 0);
			section.AddWidget(new Label(), 1, 0);

			// Act
			section.Clear();

			// Assert
			section.RowCount.Should().Be(0, "it should no longer have any widget at any row.");
			section.ColumnCount.Should().Be(0, "it should no longer have any widget at any column.");
			section.GetWidgets().Should().BeEmpty("it should not longer have any widget.");
		}

		[TestMethod]
		public void DisableWidgetsTest()
		{
			// Arrange
			var section = new Section();
			section.AddWidget(new Button(), 0, 0);
			section.AddWidget(new Button(), 1, 0);

			// Act
			section.DisableWidgets();

			// Assert
			section.GetWidgets()
				.Should()
				.AllSatisfy(widget => ((IInteractiveWidget)widget).IsEnabled.Should().BeFalse());
		}

		[TestMethod]
		public void EnableWidgetsTest()
		{
			// Arrange
			var section = new Section();
			section.AddWidget(new Button { IsEnabled = false }, 0, 0);
			section.AddWidget(new Button(), 1, 0);

			// Act
			section.EnableWidgets();

			// Assert
			section.GetWidgets()
				.Should()
				.AllSatisfy(widget => ((IInteractiveWidget)widget).IsEnabled.Should().BeTrue());
		}

		[TestMethod]
		public void GapWidgetTest()
		{
			// Arrange
			var section = new Section();

			// Act
			section.AddWidget(new Label(), 2, 3);

			// Assert
			section.RowCount.Should().Be(3);
			section.ColumnCount.Should().Be(4);
		}

		[TestMethod]
		public void GetNullSectionLocationTest()
		{
			// Arrange
			var section = new Section();
			section.AddWidget(new Label(), 0, 0);

			var subsection = new Section();
			subsection.AddWidget(new Label(), 0, 0);

			// Act & Assert
			Invoking(() => section.GetSectionLocation(null))
				.Should()
				.Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void GetNullWidgetLocationTest()
		{
			// Arrange
			var section = new Section();
			section.AddWidget(new Label(), 0, 0);

			// Act & Assert
			Invoking(() => section.GetWidgetLocation(null))
				.Should()
				.Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void GetUnknownSectionLocationTest()
		{
			// Arrange
			var section = new Section();
			section.AddWidget(new Label(), 0, 0);

			var subsection = new Section();
			subsection.AddWidget(new Label(), 0, 0);

			// Act & Assert
			Invoking(() => section.GetSectionLocation(new Section()))
				.Should()
				.Throw<ArgumentException>();
		}

		[TestMethod]
		public void GetUnknownWidgetLocationTest()
		{
			// Arrange
			var section = new Section();
			section.AddWidget(new Label(), 0, 0);

			// Act & Assert
			Invoking(() => section.GetWidgetLocation(new Label()))
				.Should()
				.Throw<ArgumentException>();
		}

		[TestMethod]
		public void HideWidgetsTest()
		{
			// Arrange
			var section = new Section();
			section.AddWidget(new Label(), 0, 0);
			section.AddWidget(new Label(), 1, 0);

			// Act
			section.HideWidgets();

			// Assert
			section.GetWidgets()
				.Should()
				.HaveCount(2, "the widgets still are part of the section even if they are hidden.");

			section.GetWidgets().Should().AllSatisfy(widget => widget.IsVisible.Should().BeFalse());

			section.RowCount.Should().Be(0, "a hidden widget does not take row space on the grid.");
			section.ColumnCount.Should().Be(0, "a hidden widget does not take column space on the grid.");
		}

		[TestMethod]
		public void MoveNullSectionTest()
		{
			// Arrange
			var section = new Section();
			section.AddWidget(new Label(), 0, 0);

			var subsection = new Section();
			subsection.AddWidget(new Label(), 0, 0);

			section.AddSection(subsection, 1, 0);

			// Act & Assert
			Invoking(() => section.MoveSection(null, 1, 1)).Should().Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void MoveNullWidgetTest()
		{
			// Arrange
			var section = new Section();
			section.AddWidget(new Label(), 0, 0);

			// Act & Assert
			Invoking(() => section.MoveWidget(null, new WidgetLocation(1, 1)))
				.Should()
				.Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void MoveSectionTest()
		{
			// Arrange
			var section = new Section();
			section.AddWidget(new Label(), 0, 0);

			var subsection = new Section();
			subsection.AddWidget(new Label(), 0, 0);

			section.AddSection(subsection, 1, 0);
			var expectedLocation = new SectionLocation(1, 1);

			// Act
			section.MoveSection(subsection, 1, 1);

			// Assert
			section.GetSectionLocation(subsection).Should().Be(expectedLocation);
		}

		[TestMethod]
		public void MoveUnknownSectionTest()
		{
			// Arrange
			var section = new Section();
			section.AddWidget(new Label(), 0, 0);

			var subsection = new Section();
			subsection.AddWidget(new Label(), 0, 0);

			section.AddSection(subsection, 1, 0);

			// Act & Assert
			Invoking(() => section.MoveSection(new Section(), 1, 1)).Should().Throw<ArgumentException>();
		}

		[TestMethod]
		public void MoveUnknownWidgetTest()
		{
			// Arrange
			var section = new Section();
			section.AddWidget(new Label(), 0, 0);

			// Act & Assert
			Invoking(() => section.MoveWidget(new Label(), new WidgetLocation(1, 1)))
				.Should()
				.Throw<ArgumentException>();
		}

		[TestMethod]
		public void MoveWidgetTest()
		{
			// Arrange
			var widget = new Label();
			var section = new Section();
			section.AddWidget(widget, 0, 0);
			var expectedLocation = new WidgetLocation(1, 1);

			// Act
			section.MoveWidget(widget, 1, 1);

			// Assert
			section.GetWidgetLocation(widget).Should().Be(expectedLocation);
			section.GetWidgets().Should().HaveCount(1, "the widget should not disappear.");
			section.RowCount.Should().Be(2, "the widget moved one row down.");
			section.ColumnCount.Should().Be(2, "the widget moved one column to the right.");
		}

		[TestMethod]
		public void NoWidgetTest()
		{
			// Act
			var section = new Section();

			// Assert
			section.RowCount.Should().Be(0, "it cannot have rows if there are no widgets.");
			section.ColumnCount.Should().Be(0, "it cannot have columns if there are no widgets");
			section.GetWidgets().Should().BeEmpty("widgets need to be added first.");
			section.GetSections().Should().BeEmpty("subsections need to be added first.");
		}

		[TestMethod]
		public void RemoveNullSectionTest()
		{
			// Arrange
			var section = new Section();
			section.AddWidget(new Label(), 0, 0);

			var subsection = new Section();
			subsection.AddWidget(new Label(), 0, 0);

			section.AddSection(subsection, 1, 0);

			// Act & Assert
			Invoking(() => section.RemoveSection(null))
				.Should()
				.Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void RemoveNullWidgetTest()
		{
			// Arrange
			var section = new Section();
			section.AddWidget(new Label(), 0, 0);

			// Act & Assert
			Invoking(() => section.RemoveWidget(null)).Should().Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void RemoveSectionTest()
		{
			// Arrange
			// Arrange
			var section = new Section();
			section.AddWidget(new Label(), 0, 0);

			var subsection = new Section();
			subsection.AddWidget(new Label(), 0, 0);

			section.AddSection(subsection, 1, 0);

			// Act
			section.RemoveSection(subsection);

			// Assert
			section.GetSections().Should().BeEmpty("the only subsection was removed.");
			section.GetWidgets().Should().HaveCount(1, "the subsection with the second widget was removed.");
		}

		[TestMethod]
		public void RemoveUnknownSectionTest()
		{
			// Arrange
			var section = new Section();
			section.AddWidget(new Label(), 0, 0);

			var subsection = new Section();
			subsection.AddWidget(new Label(), 0, 0);

			section.AddSection(subsection, 1, 0);

			// Act & Assert
			Invoking(() => section.RemoveSection(new Section()))
				.Should()
				.NotThrow("removing a subsection that is not part of the section should just be skipped.");
		}

		[TestMethod]
		public void RemoveUnknownWidgetDoesNotThrowTest()
		{
			// Arrange
			var section = new Section();
			section.AddWidget(new Label(), 0, 0);

			// Act & Assert
			Invoking(() => section.RemoveWidget(new Label()))
				.Should()
				.NotThrow("removing should just skip an unknown widget.");

			section.GetWidgets().Should().HaveCount(1, "the widget could not have been removed.");
		}

		[TestMethod]
		public void RemoveWidgetTest()
		{
			// Arrange
			var section = new Section();
			var label = new Label();
			section.AddWidget(label, 0, 0);

			// Act
			section.RemoveWidget(label);

			// Assert
			section.RowCount.Should().Be(0, "it should no longer have any widget at any row.");
			section.ColumnCount.Should().Be(0, "it should no longer have any widget at any column.");
			section.GetWidgets().Should().BeEmpty("it should not longer have any widget.");
		}

		[TestMethod]
		public void ShowWidgetsTest()
		{
			// Arrange
			var section = new Section();
			section.AddWidget(new Label { IsVisible = false }, 0, 0);
			section.AddWidget(new Label(), 1, 0);

			// Act
			section.ShowWidgets();

			// Assert
			section.GetWidgets().Should().AllSatisfy(widget => widget.IsVisible.Should().BeTrue());
		}
	}
}