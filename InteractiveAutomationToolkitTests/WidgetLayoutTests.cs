namespace InteractiveAutomationToolkitTests
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit;

	[TestClass]
	public class WidgetLayoutTests
	{
		[TestMethod]
		public void OverlapAtRootTest()
		{
			bool overlaps = new WidgetLayout(0, 0).Overlaps(new WidgetLayout(0, 0));

			Assert.IsTrue(overlaps);
		}

		[TestMethod]
		public void OverlapTest()
		{
			bool overlaps = new WidgetLayout(5, 3).Overlaps(new WidgetLayout(5, 3));

			Assert.IsTrue(overlaps);
		}

		[TestMethod]
		public void NoOverlapTest()
		{
			bool overlaps = new WidgetLayout(5, 3).Overlaps(new WidgetLayout(7, 2));

			Assert.IsFalse(overlaps);
		}

		[TestMethod]
		public void NoOverlapSameRowTest()
		{
			bool overlaps = new WidgetLayout(0, 0).Overlaps(new WidgetLayout(0, 1));

			Assert.IsFalse(overlaps);
		}

		[TestMethod]
		public void NoOverlapSameColumnTest()
		{
			bool overlaps = new WidgetLayout(0, 0).Overlaps(new WidgetLayout(1, 0));

			Assert.IsFalse(overlaps);
		}

		[TestMethod]
		public void OverlapRowSpanTest()
		{
			bool overlaps = new WidgetLayout(0, 0, 2, 1).Overlaps(new WidgetLayout(1, 0));

			Assert.IsTrue(overlaps);
		}

		[TestMethod]
		public void OverlapColumnSpanTest()
		{
			bool overlaps = new WidgetLayout(0, 0, 1, 2).Overlaps(new WidgetLayout(0, 1));

			Assert.IsTrue(overlaps);
		}

		[TestMethod]
		public void OverlapSpanTest()
		{
			bool overlaps = new WidgetLayout(0, 0, 2, 2).Overlaps(new WidgetLayout(1, 1));

			Assert.IsTrue(overlaps);
		}
	}
}