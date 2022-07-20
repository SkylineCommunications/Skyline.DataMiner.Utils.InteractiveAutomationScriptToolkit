namespace InteractiveAutomationToolkitTests
{
	using System;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.InteractiveAutomationToolkit;

	using static FluentAssertions.FluentActions;

	[TestClass]
	public class GridPanelTests
	{
		[TestMethod]
		public void AddCyclicPanelsTest()
		{
			// Arrange
			var panelA = new GridPanel();
			var panelB = new GridPanel();
			var panelC = new GridPanel();

			// Act & Assert
			panelA.Add(panelB, 0, 0);
			panelB.Add(panelC, 0, 0);
			Invoking(() => panelC.Add(panelA, 0, 0)).Should().Throw<ArgumentException>();
		}

		[TestMethod]
		public void AddMultipleSpanningWidgetTest()
		{
			// Arrange
			var panel = new GridPanel();

			// Act
			panel.Add(new Label(), 0, 0, 1, 2);
			panel.Add(new Label(), 1, 0, 2, 1);
			panel.Add(new Label(), 1, 1);
			panel.Add(new Label(), 2, 1);

			// Assert
			panel.EvaluateRowCount().Should().Be(3);
			panel.EvaluateColumnCount().Should().Be(2);
		}

		[TestMethod]
		public void AddMultipleWidgetTest()
		{
			// Arrange
			var panel = new GridPanel();

			// Act
			panel.Add(new Label(), 0, 0);
			panel.Add(new Label(), 0, 1);
			panel.Add(new Label(), 1, 0);
			panel.Add(new Label(), 2, 0);

			// Assert
			panel.EvaluateRowCount().Should().Be(3);
			panel.EvaluateColumnCount().Should().Be(2);
			panel.GetWidgets().Should().HaveCount(4);
		}

		[TestMethod]
		public void AddNullPanelTest()
		{
			// Arrange
			var panel = new GridPanel();
			panel.Add(new Label(), 0, 0);

			// Act & Assert
			Invoking(() => panel.Add((IPanel)null, 1, 0)).Should().Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void AddNullWidgetTest()
		{
			// Arrange
			var panel = new GridPanel();

			// Act & Assert
			Invoking(() => panel.Add((IWidget)null, 1, 0)).Should().Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void AddPanelTest()
		{
			// Arrange
			var panel = new GridPanel();
			panel.Add(new Label(), 0, 0);

			var subPanel = new GridPanel();
			subPanel.Add(new Label(), 0, 1);

			// Act
			panel.Add(subPanel, 1, 0);

			// Assert
			panel.GetPanels().Should().HaveCount(1, "we added one subPanel to the panel.");
			panel.GetWidgets()
				.Should()
				.HaveCount(2, "the panel has one direct child widget and one from the subPanel.");

			panel.GetWidgets(false).Should().HaveCount(1, "only one widget is a direct child of the panel.");
			panel.EvaluateRowCount().Should().Be(2, "because widgets in sub-panels count towards the row count.");
			panel.EvaluateColumnCount().Should().Be(2, "because widgets in sub-panels count towards the column count.");
		}

		[TestMethod]
		public void OnlySubPanelTest()
		{
			// Arrange
			var panel = new GridPanel();

			var subPanel = new GridPanel();
			subPanel.Add(new Label(), 0, 0);
			subPanel.Add(new Label(), 0, 1);
			subPanel.Add(new Label(), 0, 2);

			// Act
			panel.Add(subPanel, 0, 0);

			// Assert
			panel.GetPanels().Should().HaveCount(1, "we added one subPanel to the panel.");
			panel.GetWidgets()
				.Should()
				.HaveCount(3, "the subPanel has 3 widgets.");

			panel.GetWidgets(false).Should().HaveCount(0, "no widgets are a direct child of the panel.");
			panel.EvaluateRowCount().Should().Be(1, "because widgets in sub-panels count towards the row count.");
			panel.EvaluateColumnCount().Should().Be(3, "because widgets in sub-panels count towards the column count.");
		}

		[TestMethod]
		public void AddPanelToItselfTest()
		{
			// Arrange
			var panel = new GridPanel();
			panel.Add(new Label(), 0, 0);

			// Act & Assert
			Invoking(() => panel.Add(panel, 1, 0)).Should().Throw<ArgumentException>();
		}

		[TestMethod]
		public void AddPanelWithSameWidgetsTest()
		{
			// Arrange
			var panel = new GridPanel();
			var label = new Label();
			panel.Add(label, 0, 0);

			var subPanel = new GridPanel();
			subPanel.Add(label, 0, 0);

			// Act & Assert
			Invoking(() => panel.Add(subPanel, 1, 0)).Should().Throw<ArgumentException>();
		}

		[TestMethod]
		public void AddSingleWidgetTest()
		{
			// Arrange
			var panel = new GridPanel();

			// Act
			panel.Add(new Label(), 0, 0);

			// Assert
			panel.EvaluateRowCount().Should().Be(1, "a single widget was added to row index 0.");
			panel.EvaluateColumnCount().Should().Be(1, "a single widget was added to column index 0.");
			panel.GetWidgets().Should().HaveCount(1, "a single widget was added.");
		}

		[TestMethod]
		public void AddWidgetTwiceTest()
		{
			// Arrange
			var widget = new Label();
			var panel = new GridPanel();
			panel.Add(widget, 0, 0);

			// Act & Assert
			Invoking(() => panel.Add(widget, 1, 0)).Should().Throw<ArgumentException>();
		}

		[TestMethod]
		public void ClearTest()
		{
			// Arrange
			var panel = new GridPanel();
			panel.Add(new Label(), 0, 0);
			panel.Add(new Label(), 1, 0);

			// Act
			panel.Clear();

			// Assert
			panel.EvaluateRowCount().Should().Be(0, "it should no longer have any widget at any row.");
			panel.EvaluateColumnCount().Should().Be(0, "it should no longer have any widget at any column.");
			panel.GetWidgets().Should().BeEmpty("it should not longer have any widget.");
		}

		[TestMethod]
		public void DisableWidgetsTest()
		{
			// Arrange
			var panel = new GridPanel();
			panel.Add(new Button(), 0, 0);
			panel.Add(new Button(), 1, 0);

			// Act
			panel.DisableWidgets();

			// Assert
			panel.GetWidgets()
				.Should()
				.AllSatisfy(widget => ((IInteractiveWidget)widget).IsEnabled.Should().BeFalse());
		}

		[TestMethod]
		public void EnableWidgetsTest()
		{
			// Arrange
			var panel = new GridPanel();
			panel.Add(new Button { IsEnabled = false }, 0, 0);
			panel.Add(new Button(), 1, 0);

			// Act
			panel.EnableWidgets();

			// Assert
			panel.GetWidgets()
				.Should()
				.AllSatisfy(widget => ((IInteractiveWidget)widget).IsEnabled.Should().BeTrue());
		}

		[TestMethod]
		public void GapWidgetTest()
		{
			// Arrange
			var panel = new GridPanel();

			// Act
			panel.Add(new Label(), 2, 3);

			// Assert
			panel.EvaluateRowCount().Should().Be(3);
			panel.EvaluateColumnCount().Should().Be(4);
		}

		[TestMethod]
		public void GetNullPanelLocationTest()
		{
			// Arrange
			var panel = new GridPanel();
			panel.Add(new Label(), 0, 0);

			var subPanel = new GridPanel();
			subPanel.Add(new Label(), 0, 0);

			// Act & Assert
			Invoking(() => panel.GetLocation((IPanel)null))
				.Should()
				.Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void GetNullWidgetLocationTest()
		{
			// Arrange
			var panel = new GridPanel();
			panel.Add(new Label(), 0, 0);

			// Act & Assert
			Invoking(() => panel.GetLocation((IWidget)null))
				.Should()
				.Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void GetUnknownPanelLocationTest()
		{
			// Arrange
			var panel = new GridPanel();
			panel.Add(new Label(), 0, 0);

			var subPanel = new GridPanel();
			subPanel.Add(new Label(), 0, 0);

			// Act & Assert
			Invoking(() => panel.GetLocation(new GridPanel()))
				.Should()
				.Throw<ArgumentException>();
		}

		[TestMethod]
		public void GetUnknownWidgetLocationTest()
		{
			// Arrange
			var panel = new GridPanel();
			panel.Add(new Label(), 0, 0);

			// Act & Assert
			Invoking(() => panel.GetLocation(new Label()))
				.Should()
				.Throw<ArgumentException>();
		}

		[TestMethod]
		public void HideWidgetsTest()
		{
			// Arrange
			var panel = new GridPanel();
			panel.Add(new Label(), 0, 0);
			panel.Add(new Label(), 1, 0);

			// Act
			panel.HideWidgets();

			// Assert
			panel.GetWidgets()
				.Should()
				.HaveCount(2, "the widgets still are part of the panel even if they are hidden.");

			panel.GetWidgets().Should().AllSatisfy(widget => widget.IsVisible.Should().BeFalse());

			panel.EvaluateRowCount().Should().Be(0, "a hidden widget does not take row space on the grid.");
			panel.EvaluateColumnCount().Should().Be(0, "a hidden widget does not take column space on the grid.");
		}

		[TestMethod]
		public void MoveNullPanelTest()
		{
			// Arrange
			var panel = new GridPanel();
			panel.Add(new Label(), 0, 0);

			var subPanel = new GridPanel();
			subPanel.Add(new Label(), 0, 0);

			panel.Add(subPanel, 1, 0);

			// Act & Assert
			Invoking(() => panel.Move((IPanel)null, 1, 1)).Should().Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void MoveNullWidgetTest()
		{
			// Arrange
			var panel = new GridPanel();
			panel.Add(new Label(), 0, 0);

			// Act & Assert
			Invoking(() => panel.Move(null, new WidgetLocation(1, 1)))
				.Should()
				.Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void MovePanelTest()
		{
			// Arrange
			var panel = new GridPanel();
			panel.Add(new Label(), 0, 0);

			var subPanel = new GridPanel();
			subPanel.Add(new Label(), 0, 0);

			panel.Add(subPanel, 1, 0);
			var expectedLocation = new PanelLocation(1, 1);

			// Act
			panel.Move(subPanel, 1, 1);

			// Assert
			panel.GetLocation(subPanel).Should().Be(expectedLocation);
		}

		[TestMethod]
		public void MoveUnknownPanelTest()
		{
			// Arrange
			var panel = new GridPanel();
			panel.Add(new Label(), 0, 0);

			var subPanel = new GridPanel();
			subPanel.Add(new Label(), 0, 0);

			panel.Add(subPanel, 1, 0);

			// Act & Assert
			Invoking(() => panel.Move(new GridPanel(), 1, 1)).Should().Throw<ArgumentException>();
		}

		[TestMethod]
		public void MoveUnknownWidgetTest()
		{
			// Arrange
			var panel = new GridPanel();
			panel.Add(new Label(), 0, 0);

			// Act & Assert
			Invoking(() => panel.Move(new Label(), new WidgetLocation(1, 1)))
				.Should()
				.Throw<ArgumentException>();
		}

		[TestMethod]
		public void MoveWidgetTest()
		{
			// Arrange
			var widget = new Label();
			var panel = new GridPanel();
			panel.Add(widget, 0, 0);
			var expectedLocation = new WidgetLocation(1, 1);

			// Act
			panel.Move(widget, 1, 1);

			// Assert
			panel.GetLocation(widget).Should().Be(expectedLocation);
			panel.GetWidgets().Should().HaveCount(1, "the widget should not disappear.");
			panel.EvaluateRowCount().Should().Be(2, "the widget moved one row down.");
			panel.EvaluateColumnCount().Should().Be(2, "the widget moved one column to the right.");
		}

		[TestMethod]
		public void NoWidgetTest()
		{
			// Act
			var panel = new GridPanel();

			// Assert
			panel.EvaluateRowCount().Should().Be(0, "it cannot have rows if there are no widgets.");
			panel.EvaluateColumnCount().Should().Be(0, "it cannot have columns if there are no widgets");
			panel.GetWidgets().Should().BeEmpty("widgets need to be added first.");
			panel.GetPanels().Should().BeEmpty("sub-panels need to be added first.");
		}

		[TestMethod]
		public void RemoveNullPanelTest()
		{
			// Arrange
			var panel = new GridPanel();
			panel.Add(new Label(), 0, 0);

			var subPanel = new GridPanel();
			subPanel.Add(new Label(), 0, 0);

			panel.Add(subPanel, 1, 0);

			// Act & Assert
			Invoking(() => panel.Remove((IPanel)null))
				.Should()
				.Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void RemoveNullWidgetTest()
		{
			// Arrange
			var panel = new GridPanel();
			panel.Add(new Label(), 0, 0);

			// Act & Assert
			Invoking(() => panel.Remove((IWidget)null)).Should().Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void RemovePanelTest()
		{
			// Arrange
			// Arrange
			var panel = new GridPanel();
			panel.Add(new Label(), 0, 0);

			var subPanel = new GridPanel();
			subPanel.Add(new Label(), 0, 0);

			panel.Add(subPanel, 1, 0);

			// Act
			panel.Remove(subPanel);

			// Assert
			panel.GetPanels().Should().BeEmpty("the only subPanel was removed.");
			panel.GetWidgets().Should().HaveCount(1, "the subPanel with the second widget was removed.");
		}

		[TestMethod]
		public void RemoveUnknownPanelTest()
		{
			// Arrange
			var panel = new GridPanel();
			panel.Add(new Label(), 0, 0);

			var subPanel = new GridPanel();
			subPanel.Add(new Label(), 0, 0);

			panel.Add(subPanel, 1, 0);

			// Act & Assert
			Invoking(() => panel.Remove(new GridPanel()))
				.Should()
				.NotThrow("removing a subPanel that is not part of the panel should just be skipped.");
		}

		[TestMethod]
		public void RemoveUnknownWidgetDoesNotThrowTest()
		{
			// Arrange
			var panel = new GridPanel();
			panel.Add(new Label(), 0, 0);

			// Act & Assert
			Invoking(() => panel.Remove(new Label()))
				.Should()
				.NotThrow("removing should just skip an unknown widget.");

			panel.GetWidgets().Should().HaveCount(1, "the widget could not have been removed.");
		}

		[TestMethod]
		public void RemoveWidgetTest()
		{
			// Arrange
			var panel = new GridPanel();
			var label = new Label();
			panel.Add(label, 0, 0);

			// Act
			panel.Remove(label);

			// Assert
			panel.EvaluateRowCount().Should().Be(0, "it should no longer have any widget at any row.");
			panel.EvaluateColumnCount().Should().Be(0, "it should no longer have any widget at any column.");
			panel.GetWidgets().Should().BeEmpty("it should not longer have any widget.");
		}

		[TestMethod]
		public void ShowWidgetsTest()
		{
			// Arrange
			var panel = new GridPanel();
			panel.Add(new Label { IsVisible = false }, 0, 0);
			panel.Add(new Label(), 1, 0);

			// Act
			panel.ShowWidgets();

			// Assert
			panel.GetWidgets().Should().AllSatisfy(widget => widget.IsVisible.Should().BeTrue());
		}
	}
}