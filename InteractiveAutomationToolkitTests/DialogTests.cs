namespace InteractiveAutomationToolkitTests
{
	using System;
	using System.Linq;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	[TestClass]
	public class DialogTests
	{
		/// <summary>
		/// This test will assign a single label to a cell in the dialog.
		/// </summary>
		[TestMethod]
		public void AddSingleWidgetCheckColumnCount()
		{
			TestDialog testDialog = new TestDialog(new Engine());
			testDialog.AddWidget(new Label("Label1"), 0, 0);
			Assert.AreEqual(1, testDialog.ColumnCount);
		}

		/// <summary>
		/// This test will add two labels without spanning on the same row of the dialog and remove them one by one.
		/// </summary>
		[TestMethod]
		public void AddAndRemoveWidgetsCheckColumnCount()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");

			TestDialog testDialog = new TestDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0);
			testDialog.AddWidget(label2, 0, 1);

			Assert.AreEqual(2, testDialog.ColumnCount);

			testDialog.RemoveWidget(label1);
			Assert.AreEqual(1, testDialog.ColumnCount);

			testDialog.RemoveWidget(label2);
			Assert.AreEqual(0, testDialog.ColumnCount);
		}

		/// <summary>
		/// This test will add two labels that span multiple columns on different rows.
		/// </summary>
		[TestMethod]
		public void AddAndRemoveSpanningWidgetsCheckColumnCount()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");

			TestDialog testDialog = new TestDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0, 1, 3);
			testDialog.AddWidget(label2, 1, 0, 1, 2);
			Assert.AreEqual(3, testDialog.ColumnCount);

			testDialog.RemoveWidget(label1);
			Assert.AreEqual(2, testDialog.ColumnCount);
		}

		/// <summary>
		/// This test will add two labels that span multiple columns on the same row of the dialog with multiple "empty" columns between them.
		/// </summary>
		[TestMethod]
		public void AddAndRemoveSpanningWidgetsEmptyColumnsCheckColumnCount()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");

			TestDialog testDialog = new TestDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0, 1, 3);
			testDialog.AddWidget(label2, 0, 100, 1, 2);
			Assert.AreEqual(5, testDialog.ColumnCount);

			testDialog.RemoveWidget(label1);
			Assert.AreEqual(2, testDialog.ColumnCount);
		}

		/// <summary>
		/// This test will assign a single label to a cell in the dialog.
		/// </summary>
		[TestMethod]
		public void AddSingleWidgetCheckRowCount()
		{
			TestDialog testDialog = new TestDialog(new Engine());
			testDialog.AddWidget(new Label("Label1"), 0, 0);
			Assert.AreEqual(1, testDialog.RowCount);
		}

		/// <summary>
		/// This test will add two labels without spanning on different rows of the dialog and remove them one by one.
		/// </summary>
		[TestMethod]
		public void AddAndRemoveWidgetsCheckRowCount()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");

			TestDialog testDialog = new TestDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0);
			testDialog.AddWidget(label2, 1, 0);

			Assert.AreEqual(2, testDialog.RowCount);

			testDialog.RemoveWidget(label1);
			Assert.AreEqual(1, testDialog.RowCount);

			testDialog.RemoveWidget(label2);
			Assert.AreEqual(0, testDialog.RowCount);
		}

		/// <summary>
		/// This test will add two labels that span multiple rows on different columns.
		/// </summary>
		[TestMethod]
		public void AddAndRemoveSpanningWidgetsCheckRowCount()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");

			TestDialog testDialog = new TestDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0, 3, 1);
			testDialog.AddWidget(label2, 0, 1, 2, 1);
			Assert.AreEqual(3, testDialog.RowCount);

			testDialog.RemoveWidget(label1);
			Assert.AreEqual(2, testDialog.RowCount);
		}

		/// <summary>
		/// This test will add two labels that span multiple columns on the same row of the dialog with multiple "empty" rows between them.
		/// </summary>
		[TestMethod]
		public void AddAndRemoveSpanningWidgetsEmptyRowsCheckRowCount()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");

			TestDialog testDialog = new TestDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0, 3, 1);
			testDialog.AddWidget(label2, 100, 0, 2, 1);
			Assert.AreEqual(5, testDialog.RowCount);

			testDialog.RemoveWidget(label1);
			Assert.AreEqual(2, testDialog.RowCount);
		}

		/// <summary>
		/// This test will add the same label to a dialog twice and checks if an exception is thrown when the widget is being added for the second time.
		/// </summary>
		[TestMethod]
		public void TryAddSingleWidgetTwice()
		{
			Label label1 = new Label("Label 1");
			TestDialog testDialog = new TestDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0);

			ArgumentException exception = null;
			try
			{
				testDialog.AddWidget(label1, 0, 1);
			}
			catch (ArgumentException e)
			{
				exception = e;
			}

			Assert.IsNotNull(exception);
		}

		/// <summary>
		/// This test will add two different labels (without spanning) on the same position of the dialog and check if an exception is thrown.
		/// </summary>
		[TestMethod]
		public void TryAddWidgetsSamePosition()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");
			TestDialog testDialog = new TestDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0);
			testDialog.AddWidget(label2, 0, 0);

			OverlappingWidgetsException exception = null;
			try
			{
				testDialog.Show();
			}
			catch (OverlappingWidgetsException e)
			{
				exception = e;
			}

			Assert.IsNotNull(exception);
		}

		/// <summary>
		/// This test will add two different overlapping labels (with column spanning) to the dialog and check if an exception is thrown.
		/// </summary>
		[TestMethod]
		public void TryAddOverlappingColumnSpanningWidgets()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");
			TestDialog testDialog = new TestDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0, 1, 3);
			testDialog.AddWidget(label2, 0, 2, 1, 2);

			OverlappingWidgetsException exception = null;
			try
			{
				testDialog.Show();
			}
			catch (OverlappingWidgetsException e)
			{
				exception = e;
			}

			Assert.IsNotNull(exception);
		}

		/// <summary>
		/// This test will add two different overlapping labels (with row spanning) to the dialog and check if an exception is thrown.
		/// </summary>
		[TestMethod]
		public void TryAddOverlappingRowSpanningWidgets()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");
			TestDialog testDialog = new TestDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0, 3, 1);
			testDialog.AddWidget(label2, 2, 0, 2, 1);

			OverlappingWidgetsException exception = null;
			try
			{
				testDialog.Show();
			}
			catch (OverlappingWidgetsException e)
			{
				exception = e;
			}

			Assert.IsNotNull(exception);
		}

		/// <summary>
		/// This test will add two different overlapping labels (with column and row spanning) to the dialog and check if an exception is thrown.
		/// </summary>
		[TestMethod]
		public void TryAddOverlappingColumnAndRowSpanningWidgets()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");
			TestDialog testDialog = new TestDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0, 2, 2);
			testDialog.AddWidget(label2, 1, 1, 2, 3);

			OverlappingWidgetsException exception = null;
			try
			{
				testDialog.Show();
			}
			catch (OverlappingWidgetsException e)
			{
				exception = e;
			}

			Assert.IsNotNull(exception);
		}

		/// <summary>
		/// This test will add two different overlapping invisble labels (with column and row spanning) to the dialog and check if no exception is thrown.
		/// </summary>
		[TestMethod]
		public void TryAddOverlappingInvisisbleColumnAndRowSpanningWidgets()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2") { IsVisible = false };
			TestDialog testDialog = new TestDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0, 2, 2);
			testDialog.AddWidget(label2, 1, 1, 2, 3);

			OverlappingWidgetsException exception = null;
			try
			{
				testDialog.Show();
			}
			catch (OverlappingWidgetsException e)
			{
				exception = e;
			}
			catch (Exception)
			{
				// Expected exception as no source is available for the Engine provided to the Dialog.
			}

			Assert.IsNull(exception);
		}

		/// <summary>
		/// This test will check if all widgets are removed after the Clear method is called.
		/// </summary>
		[TestMethod]
		public void ClearDialogTest()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2") { IsVisible = false };
			TestDialog testDialog = new TestDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0, 2, 2);
			testDialog.AddWidget(label2, 1, 1, 2, 3);

			Assert.AreEqual(2, testDialog.Widgets.Count());

			testDialog.Clear();

			Assert.AreEqual(0, testDialog.Widgets.Count());
		}
	}

	public class TestDialog : Dialog
	{
		public TestDialog(Engine engine) : base(engine)
		{
		}
	}
}
