namespace InteractiveAutomationToolkitTests
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Moq;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	[TestClass]
	public class OverlapTests
	{
		/// <summary>
		///     This test will add two different overlapping labels (with column and row spanning) to the dialog and check if an
		///     exception is thrown.
		/// </summary>
		[TestMethod]
		public void TryAddOverlappingColumnAndRowSpanningWidgets()
		{
			var label1 = new Label("Label 1");
			var label2 = new Label("Label 2");
			var dialog = new Dialog<GridPanel>(Mock.Of<IEngine>());
			dialog.Panel.Add(label1, 0, 0, 2, 2);
			dialog.Panel.Add(label2, 1, 1, 2, 3);

			Assert.ThrowsException<OverlappingWidgetsException>(() => dialog.ShowStatic(false));
		}

		/// <summary>
		///     This test will add two different overlapping labels (with column spanning) to the dialog and check if an exception
		///     is thrown.
		/// </summary>
		[TestMethod]
		public void TryAddOverlappingColumnSpanningWidgets()
		{
			var label1 = new Label("Label 1");
			var label2 = new Label("Label 2");
			var dialog = new Dialog<GridPanel>(Mock.Of<IEngine>());
			dialog.Panel.Add(label1, 0, 0, 1, 3);
			dialog.Panel.Add(label2, 0, 2, 1, 2);

			Assert.ThrowsException<OverlappingWidgetsException>(() => dialog.ShowStatic(false));
		}

		/// <summary>
		///     This test will add two different overlapping invisible labels (with column and row spanning) to the dialog and
		///     check if no exception is thrown.
		/// </summary>
		[TestMethod]
		public void TryAddOverlappingInvisibleColumnAndRowSpanningWidgets()
		{
			var label1 = new Label("Label 1");
			var label2 = new Label("Label 2") { IsVisible = false };
			var dialog = new Dialog<GridPanel>(Mock.Of<IEngine>());
			dialog.Panel.Add(label1, 0, 0, 2, 2);
			dialog.Panel.Add(label2, 1, 1, 2, 3);

			try
			{
				dialog.ShowStatic(false);
			}
			catch (Exception ex)
			{
				Assert.Fail("Expected no exception, but got: " + ex.Message);
			}
		}

		/// <summary>
		///     This test will add two different overlapping labels (with row spanning) to the dialog and check if an exception is
		///     thrown.
		/// </summary>
		[TestMethod]
		public void TryAddOverlappingRowSpanningWidgets()
		{
			var label1 = new Label("Label 1");
			var label2 = new Label("Label 2");
			var dialog = new Dialog<GridPanel>(Mock.Of<IEngine>());
			dialog.Panel.Add(label1, 0, 0, 3, 1);
			dialog.Panel.Add(label2, 2, 0, 2, 1);

			Assert.ThrowsException<OverlappingWidgetsException>(() => dialog.ShowStatic(false));
		}

		/// <summary>
		///     This test will add two different labels (without spanning) on the same location and check if an exception is
		///     thrown.
		/// </summary>
		[TestMethod]
		public void TryAddWidgetsSameLocation()
		{
			var label1 = new Label("Label 1");
			var label2 = new Label("Label 2");
			var dialog = new Dialog<GridPanel>(Mock.Of<IEngine>());
			dialog.Panel.Add(label1, 0, 0);
			dialog.Panel.Add(label2, 0, 0);

			Assert.ThrowsException<OverlappingWidgetsException>(() => dialog.ShowStatic(false));
		}
	}
}