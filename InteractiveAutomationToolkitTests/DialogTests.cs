namespace InteractiveAutomationToolkitTests
{
	using System;
	using System.Linq;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Moq;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit;

	[TestClass]
	public class DialogTests
	{
		/// <summary>
		/// This test will assign a single label to a cell in the dialog.
		/// </summary>
		[TestMethod]
		public void AddSingleWidgetCheckColumnCount()
		{
			Dialog dialog = new Dialog(Mock.Of<IEngine>());
			dialog.AddWidget(new Label("Label1"), 0, 0);
			Assert.AreEqual(1, dialog.ColumnCount);
		}

		/// <summary>
		/// This test will add two labels without spanning on the same row of the dialog and remove them one by one.
		/// </summary>
		[TestMethod]
		public void AddAndRemoveWidgetsCheckColumnCount()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");

			Dialog dialog = new Dialog(Mock.Of<IEngine>());
			dialog.AddWidget(label1, 0, 0);
			dialog.AddWidget(label2, 0, 1);

			Assert.AreEqual(2, dialog.ColumnCount);

			dialog.RemoveWidget(label1);
			Assert.AreEqual(1, dialog.ColumnCount);

			dialog.RemoveWidget(label2);
			Assert.AreEqual(0, dialog.ColumnCount);
		}

		/// <summary>
		/// This test will add two labels that span multiple columns on different rows.
		/// </summary>
		[TestMethod]
		public void AddAndRemoveSpanningWidgetsCheckColumnCount()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");

			Dialog dialog = new Dialog(Mock.Of<IEngine>());
			dialog.AddWidget(label1, 0, 0, 1, 3);
			dialog.AddWidget(label2, 1, 0, 1, 2);
			Assert.AreEqual(3, dialog.ColumnCount);

			dialog.RemoveWidget(label1);
			Assert.AreEqual(2, dialog.ColumnCount);
		}

		/// <summary>
		/// This test will add two labels that span multiple columns on the same row of the dialog with multiple "empty" columns between them.
		/// </summary>
		[TestMethod]
		public void AddAndRemoveSpanningWidgetsEmptyColumnsCheckColumnCount()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");

			Dialog dialog = new Dialog(Mock.Of<IEngine>());
			dialog.AddWidget(label1, 0, 0, 1, 3);
			dialog.AddWidget(label2, 0, 100, 1, 2);
			Assert.AreEqual(5, dialog.ColumnCount);

			dialog.RemoveWidget(label1);
			Assert.AreEqual(2, dialog.ColumnCount);
		}

		/// <summary>
		/// This test will assign a single label to a cell in the dialog.
		/// </summary>
		[TestMethod]
		public void AddSingleWidgetCheckRowCount()
		{
			Dialog dialog = new Dialog(Mock.Of<IEngine>());
			dialog.AddWidget(new Label("Label1"), 0, 0);
			Assert.AreEqual(1, dialog.RowCount);
		}

		/// <summary>
		/// This test will add two labels without spanning on different rows of the dialog and remove them one by one.
		/// </summary>
		[TestMethod]
		public void AddAndRemoveWidgetsCheckRowCount()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");

			Dialog dialog = new Dialog(Mock.Of<IEngine>());
			dialog.AddWidget(label1, 0, 0);
			dialog.AddWidget(label2, 1, 0);

			Assert.AreEqual(2, dialog.RowCount);

			dialog.RemoveWidget(label1);
			Assert.AreEqual(1, dialog.RowCount);

			dialog.RemoveWidget(label2);
			Assert.AreEqual(0, dialog.RowCount);
		}

		/// <summary>
		/// This test will add two labels that span multiple rows on different columns.
		/// </summary>
		[TestMethod]
		public void AddAndRemoveSpanningWidgetsCheckRowCount()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");

			Dialog dialog = new Dialog(Mock.Of<IEngine>());
			dialog.AddWidget(label1, 0, 0, 3, 1);
			dialog.AddWidget(label2, 0, 1, 2, 1);
			Assert.AreEqual(3, dialog.RowCount);

			dialog.RemoveWidget(label1);
			Assert.AreEqual(2, dialog.RowCount);
		}

		/// <summary>
		/// This test will add two labels that span multiple columns on the same row of the dialog with multiple "empty" rows between them.
		/// </summary>
		[TestMethod]
		public void AddAndRemoveSpanningWidgetsEmptyRowsCheckRowCount()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");

			Dialog dialog = new Dialog(Mock.Of<IEngine>());
			dialog.AddWidget(label1, 0, 0, 3, 1);
			dialog.AddWidget(label2, 100, 0, 2, 1);
			Assert.AreEqual(5, dialog.RowCount);

			dialog.RemoveWidget(label1);
			Assert.AreEqual(2, dialog.RowCount);
		}

		/// <summary>
		/// This test will add the same label to a dialog twice and checks if an exception is thrown when the widget is being added for the second time.
		/// </summary>
		[TestMethod]
		public void TryAddSingleWidgetTwice()
		{
			Label label1 = new Label("Label 1");
			Dialog dialog = new Dialog(Mock.Of<IEngine>());
			dialog.AddWidget(label1, 0, 0);

			Assert.ThrowsException<ArgumentException>(() => dialog.AddWidget(label1, 0, 1));
		}

		/// <summary>
		/// This test will add two different labels (without spanning) on the same location and check if an exception is thrown.
		/// </summary>
		[TestMethod]
		public void TryAddWidgetsSameLocation()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");
			Dialog dialog = new Dialog(Mock.Of<IEngine>());
			dialog.AddWidget(label1, 0, 0);
			dialog.AddWidget(label2, 0, 0);

			Assert.ThrowsException<OverlappingWidgetsException>(() => dialog.Show(false));
		}

		/// <summary>
		/// This test will add two different overlapping labels (with column spanning) to the dialog and check if an exception is thrown.
		/// </summary>
		[TestMethod]
		public void TryAddOverlappingColumnSpanningWidgets()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");
			Dialog dialog = new Dialog(Mock.Of<IEngine>());
			dialog.AddWidget(label1, 0, 0, 1, 3);
			dialog.AddWidget(label2, 0, 2, 1, 2);

			Assert.ThrowsException<OverlappingWidgetsException>(() => dialog.Show(false));
		}

		/// <summary>
		/// This test will add two different overlapping labels (with row spanning) to the dialog and check if an exception is thrown.
		/// </summary>
		[TestMethod]
		public void TryAddOverlappingRowSpanningWidgets()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");
			Dialog dialog = new Dialog(Mock.Of<IEngine>());
			dialog.AddWidget(label1, 0, 0, 3, 1);
			dialog.AddWidget(label2, 2, 0, 2, 1);

			Assert.ThrowsException<OverlappingWidgetsException>(() => dialog.Show(false));
		}

		/// <summary>
		/// This test will add two different overlapping labels (with column and row spanning) to the dialog and check if an exception is thrown.
		/// </summary>
		[TestMethod]
		public void TryAddOverlappingColumnAndRowSpanningWidgets()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");
			Dialog dialog = new Dialog(Mock.Of<IEngine>());
			dialog.AddWidget(label1, 0, 0, 2, 2);
			dialog.AddWidget(label2, 1, 1, 2, 3);

			Assert.ThrowsException<OverlappingWidgetsException>(() => dialog.Show(false));
		}

		/// <summary>
		/// This test will add two different overlapping invisble labels (with column and row spanning) to the dialog and check if no exception is thrown.
		/// </summary>
		[TestMethod]
		public void TryAddOverlappingInvisibleColumnAndRowSpanningWidgets()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2") { IsVisible = false };
			Dialog dialog = new Dialog(Mock.Of<IEngine>());
			dialog.AddWidget(label1, 0, 0, 2, 2);
			dialog.AddWidget(label2, 1, 1, 2, 3);

			try
			{
				dialog.Show(false);
			}
			catch (Exception ex)
			{
				Assert.Fail("Expected no exception, but got: " + ex.Message);
			}
		}

		/// <summary>
		/// This test will check if all widgets are removed after the Clear method is called.
		/// </summary>
		[TestMethod]
		public void ClearDialogTest()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2") { IsVisible = false };
			Dialog dialog = new Dialog(Mock.Of<IEngine>());
			dialog.AddWidget(label1, 0, 0, 2, 2);
			dialog.AddWidget(label2, 1, 1, 2, 3);

			Assert.AreEqual(2, dialog.Widgets.Count());

			dialog.Clear();

			Assert.AreEqual(0, dialog.Widgets.Count());
		}
	}
}
