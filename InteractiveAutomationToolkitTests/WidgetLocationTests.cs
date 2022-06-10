namespace InteractiveAutomationToolkitTests
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit;

	[TestClass]
	public class WidgetLocationTests
	{
		[TestMethod]
		public void NoOverlapSameColumnTest()
		{
			bool overlaps = new WidgetLocation(0, 0).Overlaps(new WidgetLocation(1, 0));

			Assert.IsFalse(overlaps);
		}

		[TestMethod]
		public void NoOverlapSameRowTest()
		{
			bool overlaps = new WidgetLocation(0, 0).Overlaps(new WidgetLocation(0, 1));

			Assert.IsFalse(overlaps);
		}

		[TestMethod]
		public void NoOverlapTest()
		{
			bool overlaps = new WidgetLocation(5, 3).Overlaps(new WidgetLocation(7, 2));

			Assert.IsFalse(overlaps);
		}

		[TestMethod]
		public void OverlapAtRootTest()
		{
			bool overlaps = new WidgetLocation(0, 0).Overlaps(new WidgetLocation(0, 0));

			Assert.IsTrue(overlaps);
		}

		[TestMethod]
		public void OverlapColumnSpanTest()
		{
			bool overlaps = new WidgetLocation(0, 0, 1, 2).Overlaps(new WidgetLocation(0, 1));

			Assert.IsTrue(overlaps);
		}

		[TestMethod]
		public void OverlapRowSpanTest()
		{
			bool overlaps = new WidgetLocation(0, 0, 2, 1).Overlaps(new WidgetLocation(1, 0));

			Assert.IsTrue(overlaps);
		}

		[TestMethod]
		public void OverlapSpanTest()
		{
			bool overlaps = new WidgetLocation(0, 0, 2, 2).Overlaps(new WidgetLocation(1, 1));

			Assert.IsTrue(overlaps);
		}

		[TestMethod]
		public void OverlapTest()
		{
			bool overlaps = new WidgetLocation(5, 3).Overlaps(new WidgetLocation(5, 3));

			Assert.IsTrue(overlaps);
		}
	}
}