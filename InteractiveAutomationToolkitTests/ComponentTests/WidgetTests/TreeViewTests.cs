namespace InteractiveAutomationToolkitTests
{
	using System;
	using System.Collections;
	using System.Collections.Generic;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.InteractiveAutomationToolkit;

	using static FluentAssertions.FluentActions;

	[TestClass]
	public class TreeViewTests
	{
		[TestMethod]
		public void EmptyTreeTest()
		{
			// Act
			var treeView = new TreeView();

			// Assert
			treeView.Nodes.Should().BeEmpty("there should be no nodes.");
			treeView.CheckedNodes.Should().BeEmpty("there should be no checked nodes.");
			treeView.UnCheckedNodes.Should().BeEmpty("there should be no unchecked nodes.");
			treeView.RootNodes.Should().BeEmpty("there should be no root nodes.");
			treeView.Leaves.Should().BeEmpty("there should be no leaves.");
			treeView.CheckedLeaves.Should().BeEmpty("there should be no checked leaves.");
			treeView.UncheckedLeaves.Should().BeEmpty("there should be no unchecked leaves.");
			treeView.InternalNodes.Should().BeEmpty("there should be no internal nodes.");
			treeView.CheckedInternalNodes.Should().BeEmpty("there should be no checked internal nodes.");
			treeView.UnCheckedInternalNodes.Should().BeEmpty("there should be no checked unchecked internal nodes.");
		}

		[TestMethod]
		public void SingleRootTreeTest()
		{
			// Act
			var treeView = new TreeView(new TreeViewNode());

			// Assert
			treeView.Nodes.Should().HaveCount(1, "there should be one node.");
			treeView.CheckedNodes.Should().BeEmpty("there should be no checked nodes.");
			treeView.UnCheckedNodes.Should().HaveCount(1, "there should be one node.");
			treeView.RootNodes.Should().HaveCount(1, "there should be one root node.");
			treeView.Leaves.Should().HaveCount(1, "there should be one leaf.");
			treeView.CheckedLeaves.Should().BeEmpty("there should be no checked leaves.");
			treeView.UncheckedLeaves.Should().HaveCount(1, "there should be one unchecked leaf.");
			treeView.InternalNodes.Should().BeEmpty("there should be no internal nodes.");
			treeView.CheckedInternalNodes.Should().BeEmpty("there should be no checked internal nodes.");
			treeView.UnCheckedInternalNodes.Should().BeEmpty("there should be no checked unchecked internal nodes.");
		}

		[TestMethod]
		public void MultiRootTreeTest()
		{
			// Act
			var treeView = new TreeView(new[] { new TreeViewNode(), new TreeViewNode() });

			// Assert
			treeView.Nodes.Should().HaveCount(2, "there should be two nodes.");
			treeView.CheckedNodes.Should().BeEmpty("there should be no checked nodes.");
			treeView.UnCheckedNodes.Should().HaveCount(2, "there should be two nodes.");
			treeView.RootNodes.Should().HaveCount(2, "there should be two root nodes.");
			treeView.Leaves.Should().HaveCount(2, "there should be two leaves.");
			treeView.CheckedLeaves.Should().BeEmpty("there should be no checked leaves.");
			treeView.UncheckedLeaves.Should().HaveCount(2, "there should be two unchecked leaves.");
			treeView.InternalNodes.Should().BeEmpty("there should be no internal nodes.");
			treeView.CheckedInternalNodes.Should().BeEmpty("there should be no checked internal nodes.");
			treeView.UnCheckedInternalNodes.Should().BeEmpty("there should be no checked unchecked internal nodes.");
		}

		[TestMethod]
		public void TreeTest()
		{
			// Act
			var treeView = new TreeView(
				new TreeViewNode
				{
					Children =
					{
						new TreeViewNode { IsChecked = true },
						new TreeViewNode(),
						new TreeViewNode
						{
							Children =
							{
								new TreeViewNode { IsChecked = true },
								new TreeViewNode(),
							},
						},
					},
				});

			// Assert
			treeView.Nodes.Should().HaveCount(6, "there should be six nodes.");
			treeView.CheckedNodes.Should().HaveCount(2, "there should be two checked nodes.");
			treeView.UnCheckedNodes.Should().HaveCount(4, "there should be four unchecked nodes.");
			treeView.RootNodes.Should().HaveCount(1, "there should be one root node.");
			treeView.Leaves.Should().HaveCount(4, "there should be four leaves.");
			treeView.CheckedLeaves.Should().HaveCount(2, "there should be two checked leaves.");
			treeView.UncheckedLeaves.Should().HaveCount(2, "there should be two unchecked leaves.");
			treeView.InternalNodes.Should().HaveCount(2, "there should be two internal nodes.");
			treeView.CheckedInternalNodes.Should().BeEmpty("there should be no checked internal nodes.");
			treeView.UnCheckedInternalNodes.Should().HaveCount(2, "there should be two unchecked internal nodes.");
		}

		[TestMethod]
		public void NullConstructorTest()
		{
			Invoking(() => new TreeView((IEnumerable<TreeViewNode>)null))
				.Should()
				.Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void AddRootNodeTest()
		{
			// Arrange
			var treeView = new TreeView();

			// Act
			treeView.RootNodes.Add(new TreeViewNode());

			// Assert
			treeView.Nodes.Should().HaveCount(1);
		}

		[TestMethod]
		public void AddNullRootNodeTest()
		{
			// Arrange
			var treeView = new TreeView();

			// Act & Assert
			Invoking(() => treeView.RootNodes.Add(null)).Should().Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void AddSameRootNodeTest()
		{
			// Arrange
			var treeView = new TreeView();
			var node = new TreeViewNode();
			treeView.RootNodes.Add(node);

			// Act & Assert
			Invoking(() => treeView.RootNodes.Add(node)).Should().Throw<ArgumentException>();
		}

		[TestMethod]
		public void AddChildAsRootNodeTest()
		{
			// Arrange
			var treeView = new TreeView();
			var node = new TreeViewNode();
			var child = new TreeViewNode();
			node.Children.Add(child);
			treeView.RootNodes.Add(node);

			// Act & Assert
			Invoking(() => treeView.RootNodes.Add(child)).Should().Throw<ArgumentException>();
		}

		[TestMethod]
		public void ContainsRootNodeTest()
		{
			// Arrange
			var rootNode = new TreeViewNode();
			var treeView = new TreeView(rootNode);

			// Act
			bool contains = treeView.RootNodes.Contains(rootNode);

			// Assert
			contains.Should().BeTrue();
		}

		[TestMethod]
		public void ContainsUnknownRootNodeTest()
		{
			// Arrange
			var treeView = new TreeView(new TreeViewNode());

			// Act
			bool contains = treeView.RootNodes.Contains(new TreeViewNode());

			// Assert
			contains.Should().BeFalse();
		}

		[TestMethod]
		public void ContainsNullRootNodeTest()
		{
			// Arrange
			var treeView = new TreeView(new TreeViewNode());

			// Act & Assert
			Invoking(() => treeView.RootNodes.Contains(null)).Should().Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void ClearRootNodesTest()
		{
			// Arrange
			var treeView = new TreeView(new TreeViewNode());

			// Act
			treeView.RootNodes.Clear();

			// Assert
			treeView.RootNodes.Should().HaveCount(0);
		}

		[TestMethod]
		public void RemoveRootNodeTest()
		{
			// Arrange
			var rootNode = new TreeViewNode();
			var treeView = new TreeView(rootNode);

			// Act
			treeView.RootNodes.Remove(rootNode);

			// Assert
			treeView.RootNodes.Should().HaveCount(0);
		}

		[TestMethod]
		public void RemoveUnknownRootNodeTest()
		{
			// Arrange
			var treeView = new TreeView(new TreeViewNode());

			// Act & Assert
			Invoking(() => treeView.RootNodes.Remove(new TreeViewNode())).Should().NotThrow();
		}

		[TestMethod]
		public void RemoveNullRootNodeTest()
		{
			// Arrange
			var treeView = new TreeView(new TreeViewNode());

			// Act & Assert
			Invoking(() => treeView.RootNodes.Remove(null)).Should().NotThrow();
		}

		[TestMethod]
		public void RootNodeEnumeratorTest()
		{
			// Arrange
			var rootNode = new TreeViewNode();
			var treeView = new TreeView(rootNode);

			// Act
			IEnumerator enumerator = ((IEnumerable)treeView.RootNodes).GetEnumerator();

			// Assert
			enumerator.MoveNext().Should().BeTrue();
		}

		[TestMethod]
		public void RootNodeCopyToTest()
		{
			// Arrange
			var rootNode = new TreeViewNode();
			var treeView = new TreeView(rootNode);
			var array = new TreeViewNode[1];

			// Act
			treeView.RootNodes.CopyTo(array, 0);

			// Assert
			array[0].Should().Be(rootNode);
		}

		[TestMethod]
		public void SubscribeTest()
		{
			// Arrange
			var treeView = new TreeView();

			// Act
			treeView.Changed += Handler;

			// Assert
			treeView.WantsOnChange.Should().BeTrue();

			void Handler(object sender, TreeView.ChangedEventArgs e)
			{
			}
		}

		[TestMethod]
		public void UnSubscribeTest()
		{
			// Arrange
			var treeView = new TreeView();
			treeView.Changed += Handler;

			// Act
			treeView.Changed -= Handler;

			// Assert
			treeView.WantsOnChange.Should().BeFalse();

			void Handler(object sender, TreeView.ChangedEventArgs e)
			{
			}
		}

		[TestMethod]
		public void ExpandEventTest()
		{
			// Arrange
			var nodeA1 = new TreeViewNode();
			var nodeA2 = new TreeViewNode();
			var nodeA = new TreeViewNode
			{
				Children =
				{
					nodeA1,
					nodeA2,
				},
			};
			var treeView = new TreeView
			{
				RootNodes =
				{
					nodeA,
				},
			};

			treeView.Changed += AssertChangedEvent;
			var changed = false;
			var results = new UIResults();

			void ExpandNodeA()
			{
				var privateObject = new PrivateObject(results);
				privateObject.Invoke("SetValue", treeView.DestVar + "_TreeViewItemsThatNeedExpanding", nodeA.Key);
			}

			// Act
			ExpandNodeA();
			treeView.LoadResult(results);
			treeView.RaiseResultEvents();

			// Assert
			if (!changed)
			{
				Assert.Fail("Change should have been invoked");
			}

			void AssertChangedEvent(object sender, TreeView.ChangedEventArgs args)
			{
				changed = true;
				args.InteractedNode.Should().Be(nodeA);
				args.InteractedNode.IsCollapsed.Should().BeFalse();
			}
		}

		[TestMethod]
		public void CheckEventTest()
		{
			// Arrange
			var nodeA1 = new TreeViewNode();
			var nodeA2 = new TreeViewNode();
			var nodeA = new TreeViewNode
			{
				Children =
				{
					nodeA1,
					nodeA2,
				},
			};
			var treeView = new TreeView
			{
				RootNodes =
				{
					nodeA,
				},
			};

			treeView.Changed += AssertChangedEvent;
			var changed = false;
			var results = new UIResults();

			void CheckNodeA1()
			{
				var privateObject = new PrivateObject(results);
				privateObject.Invoke("SetValue", treeView.DestVar, nodeA1.Key);
			}

			// Act
			CheckNodeA1();
			treeView.LoadResult(results);
			treeView.RaiseResultEvents();

			// Assert
			if (!changed)
			{
				Assert.Fail("Change should have been invoked");
			}

			void AssertChangedEvent(object sender, TreeView.ChangedEventArgs args)
			{
				changed = true;
				args.InteractedNode.Should().Be(nodeA1);
				args.InteractedNode.IsChecked.Should().BeTrue();
			}
		}

		[TestMethod]
		public void RecursiveCheckEventTest()
		{
			// Arrange
			var nodeA1 = new TreeViewNode();
			var nodeA2 = new TreeViewNode();
			var nodeA = new TreeViewNode
			{
				Children =
				{
					nodeA1,
					nodeA2,
				},
			};
			var treeView = new TreeView
			{
				RootNodes =
				{
					nodeA,
				},
			};

			treeView.Changed += AssertChangedEvent;
			var changed = false;
			var results = new UIResults();

			void CheckNodeA()
			{
				var privateObject = new PrivateObject(results);
				privateObject.Invoke("SetValue", treeView.DestVar, String.Join(";", nodeA.Key, nodeA1.Key, nodeA2.Key));
			}

			// Act
			CheckNodeA();
			treeView.LoadResult(results);
			treeView.RaiseResultEvents();

			// Assert
			if (!changed)
			{
				Assert.Fail("Change should have been invoked");
			}

			void AssertChangedEvent(object sender, TreeView.ChangedEventArgs args)
			{
				changed = true;
				args.InteractedNode.Should().Be(nodeA);
				args.InteractedNode.IsChecked.Should().BeTrue();
			}
		}

		[TestMethod]
		public void UnCheckEdgeCaseEventTest()
		{
			// Arrange
			var nodeA1 = new TreeViewNode();
			var nodeA2 = new TreeViewNode();
			var nodeA = new TreeViewNode
			{
				IsChecked = true,
				Children =
				{
					nodeA1,
					nodeA2,
				},
			};
			var treeView = new TreeView
			{
				RootNodes =
				{
					nodeA,
				},
			};

			treeView.Changed += AssertChangedEvent;
			var changed = false;
			var results = new UIResults();

			void UnCheckNodeA1()
			{
				var privateObject = new PrivateObject(results);

				// only checked node left should be A2
				privateObject.Invoke("SetValue", treeView.DestVar, nodeA2.Key);
			}

			// Act
			UnCheckNodeA1();
			treeView.LoadResult(results);
			treeView.RaiseResultEvents();

			// Assert
			if (!changed)
			{
				Assert.Fail("Change should have been invoked");
			}

			void AssertChangedEvent(object sender, TreeView.ChangedEventArgs args)
			{
				changed = true;
				args.InteractedNode.Should().Be(nodeA1);
				args.InteractedNode.IsChecked.Should().BeFalse();
			}
		}
	}
}