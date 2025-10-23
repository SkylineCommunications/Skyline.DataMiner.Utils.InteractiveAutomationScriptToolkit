using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skyline.DataMiner.Utils.InteractiveAutomationScript;
using System.Linq;

namespace InteractiveAutomationToolkitTests
{
	[TestClass]
	public class GenericTreeViewTests
	{
		[TestMethod]
		public void Constructor_Test()
		{
			var rootItems = new[]
			{
				new TreeViewItem<int>("root1", "Root Item 1", 1),
				new TreeViewItem<int>("root2", "Root Item 2", 2)
			};

			var treeView = new TreeView<int>(rootItems);

			Assert.IsNotNull(treeView);
			Assert.AreEqual(2, treeView.Items.Count());
			Assert.IsFalse(treeView.IsReadOnly);
		}

		[TestMethod]
		public void Constructor_WithChildItems_Test()
		{
			var childItems = new[]
			{
				new TreeViewItem<string>("child1", "Child Item 1", "child1data"),
				new TreeViewItem<string>("child2", "Child Item 2", "child2data")
			};

			var rootItem = new TreeViewItem<string>("root", "Root Item", "rootdata", childItems: childItems);
			var treeView = new TreeView<string>(new[] { rootItem });

			Assert.IsNotNull(treeView);
			Assert.AreEqual(1, treeView.Items.Count());
			Assert.AreEqual(3, treeView.GetAllItems().Count()); // 1 root + 2 children
		}

		[TestMethod]
		public void TryFindTreeViewItem_Test()
		{
			var rootItems = new[]
			{
				new TreeViewItem<int>("root1", "Root Item 1", 100),
				new TreeViewItem<int>("root2", "Root Item 2", 200)
			};

			var treeView = new TreeView<int>(rootItems);
			treeView.UpdateItemCache();

			bool found = treeView.TryFindTreeViewItem("root1", out var item);

			Assert.IsTrue(found);
			Assert.IsNotNull(item);
			Assert.AreEqual("root1", item.KeyValue);
			Assert.AreEqual("Root Item 1", item.DisplayValue);
			Assert.AreEqual(100, item.Value);
		}

		[TestMethod]
		public void TryFindTreeViewItem_NotFound_Test()
		{
			var rootItems = new[]
			{
				new TreeViewItem<int>("root1", "Root Item 1", 100)
			};

			var treeView = new TreeView<int>(rootItems);
			treeView.UpdateItemCache();

			bool found = treeView.TryFindTreeViewItem("nonexistent", out var item);

			Assert.IsFalse(found);
			Assert.IsNull(item);
		}

		[TestMethod]
		public void GetAllItems_Test()
		{
			var child1 = new TreeViewItem<int>("child1", "Child 1", 10);
			var child2 = new TreeViewItem<int>("child2", "Child 2", 20);
			var rootItem = new TreeViewItem<int>("root", "Root", 1, childItems: new[] { child1, child2 });

			var treeView = new TreeView<int>(new[] { rootItem });
			treeView.UpdateItemCache();

			var allItems = treeView.GetAllItems().ToList();

			Assert.AreEqual(3, allItems.Count);
			Assert.IsTrue(allItems.Any(x => x.KeyValue == "root"));
			Assert.IsTrue(allItems.Any(x => x.KeyValue == "child1"));
			Assert.IsTrue(allItems.Any(x => x.KeyValue == "child2"));
		}

		[TestMethod]
		public void GetItems_Depth0_Test()
		{
			var child = new TreeViewItem<int>("child", "Child", 10);
			var root = new TreeViewItem<int>("root", "Root", 1, childItems: new[] { child });

			var treeView = new TreeView<int>(new[] { root });
			treeView.UpdateItemCache();

			var depth0Items = treeView.GetItems(0).ToList();

			Assert.AreEqual(1, depth0Items.Count);
			Assert.AreEqual("root", depth0Items[0].KeyValue);
		}

		[TestMethod]
		public void GetItems_Depth1_Test()
		{
			var child = new TreeViewItem<int>("child", "Child", 10);
			var root = new TreeViewItem<int>("root", "Root", 1, childItems: new[] { child });

			var treeView = new TreeView<int>(new[] { root });
			treeView.UpdateItemCache();

			var depth1Items = treeView.GetItems(1).ToList();

			Assert.AreEqual(1, depth1Items.Count);
			Assert.AreEqual("child", depth1Items[0].KeyValue);
		}

		[TestMethod]
		public void CheckedValues_Test()
		{
			var item1 = new TreeViewItem<int>("item1", "Item 1", 100);
			var item2 = new TreeViewItem<int>("item2", "Item 2", 200);
			var item3 = new TreeViewItem<int>("item3", "Item 3", 300);

			var treeView = new TreeView<int>(new[] { item1, item2, item3 });
			treeView.UpdateItemCache();

			// Simulate checking items
			item1.IsChecked = true;
			item3.IsChecked = true;
			treeView.UpdateItemCache();

			var checkedValues = treeView.CheckedValues.ToList();

			Assert.AreEqual(2, checkedValues.Count);
			Assert.IsTrue(checkedValues.Contains(100));
			Assert.IsTrue(checkedValues.Contains(300));
			Assert.IsFalse(checkedValues.Contains(200));
		}

		[TestMethod]
		public void CollapseAndExpand_Test()
		{
			var child = new TreeViewItem<int>("child", "Child", 10);
			var root = new TreeViewItem<int>("root", "Root", 1, childItems: new[] { child });

			var treeView = new TreeView<int>(new[] { root });
			treeView.UpdateItemCache();

			treeView.Collapse();

			Assert.IsTrue(root.IsCollapsed);
			Assert.IsTrue(child.IsCollapsed);

			treeView.Expand();

			Assert.IsFalse(root.IsCollapsed);
			Assert.IsFalse(child.IsCollapsed);
		}

		[TestMethod]
		public void TreeViewItemOption_Equals_Test()
		{
			var item1 = new TreeViewItem<int>("key1", "Display 1", 100);
			var item2 = new TreeViewItem<int>("key1", "Display 1", 100);
			var item3 = new TreeViewItem<int>("key2", "Display 2", 200);

			Assert.AreEqual(item1, item2);
			Assert.AreNotEqual(item1, item3);
		}

		[TestMethod]
		public void TreeViewItemOption_Properties_Test()
		{
			var item = new TreeViewItem<string>("key", "display", "value");

			Assert.AreEqual("key", item.KeyValue);
			Assert.AreEqual("display", item.DisplayValue);
			Assert.AreEqual("value", item.Value);

			item.KeyValue = "newKey";
			item.DisplayValue = "newDisplay";

			Assert.AreEqual("newKey", item.KeyValue);
			Assert.AreEqual("newDisplay", item.DisplayValue);
			Assert.AreEqual("value", item.Value); // Value is read-only
		}

		[TestMethod]
		public void CheckedLeaves_And_CheckedNodes_Test()
		{
			var leaf1 = new TreeViewItem<int>("leaf1", "Leaf 1", 10);
			var leaf2 = new TreeViewItem<int>("leaf2", "Leaf 2", 20);
			var node = new TreeViewItem<int>("node", "Node", 100, childItems: new[] { leaf1, leaf2 });

			var treeView = new TreeView<int>(new[] { node });
			treeView.UpdateItemCache();

			// Check both node and one leaf
			node.IsChecked = true;
			leaf1.IsChecked = true;
			treeView.UpdateItemCache();

			var checkedNodes = treeView.CheckedNodes.ToList();
			var checkedLeaves = treeView.CheckedLeaves.ToList();
			var checkedNodeValues = treeView.CheckedNodeValues.ToList();
			var checkedLeafValues = treeView.CheckedLeafValues.ToList();

			Assert.AreEqual(1, checkedNodes.Count);
			Assert.AreEqual("node", checkedNodes[0].KeyValue);
			Assert.AreEqual(1, checkedNodeValues.Count);
			Assert.AreEqual(100, checkedNodeValues[0]);

			Assert.AreEqual(1, checkedLeaves.Count);
			Assert.AreEqual("leaf1", checkedLeaves[0].KeyValue);
			Assert.AreEqual(1, checkedLeafValues.Count);
			Assert.AreEqual(10, checkedLeafValues[0]);
		}

		[TestMethod]
		public void EmptyConstructor_Test()
		{
			var treeView = new TreeView<string>(System.Linq.Enumerable.Empty<TreeViewItem<string>>());

			Assert.IsNotNull(treeView);
			Assert.AreEqual(0, treeView.Items.Count());
			Assert.AreEqual(0, treeView.GetAllItems().Count());
		}

		[TestMethod]
		public void ParameterlessConstructor_Test()
		{
			var treeView = new TreeView<int>();

			Assert.IsNotNull(treeView);
			Assert.AreEqual(0, treeView.Items.Count());
			Assert.AreEqual(0, treeView.GetAllItems().Count());
			Assert.IsFalse(treeView.IsReadOnly);
		}

		[TestMethod]
		public void NullValue_Test()
		{
			// Test that null values are handled correctly
			var item = new TreeViewItem<string>("key", "display", null);

			Assert.IsNull(item.Value);
			Assert.AreEqual("key", item.KeyValue);
			Assert.AreEqual("display", item.DisplayValue);
		}

		[TestMethod]
		public void ComplexType_Test()
		{
			// Test with a complex custom type
			var customData = new { Id = 123, Name = "Test", Active = true };
			var item = new TreeViewItem<object>("key", "display", customData);

			Assert.IsNotNull(item.Value);
			Assert.AreEqual(customData, item.Value);
		}
	}
}
