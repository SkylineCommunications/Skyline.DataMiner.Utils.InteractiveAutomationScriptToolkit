namespace InteractiveAutomationToolkitTests
{
	using System;
	using System.Collections;
	using System.ComponentModel;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.InteractiveAutomationToolkit;

	using static FluentAssertions.FluentActions;

	[TestClass]
	public class TreeViewNodeTests
	{
		[TestMethod]
		public void DefaultNodeTest()
		{
			// Act
			var node = new TreeViewNode();

			// Assert
			node.Children.Should().BeEmpty("a newly instantiated node should not have children.");
			node.Descendants.Should().BeEmpty("there can't be descendants if there are no children.");
			node.Parent.Should().BeNull("a newly instantiated node should not have a parent");
			node.Ancestors.Should().BeEmpty("there can't be ancestors if there is no parent.");
			node.Siblings.Should().BeEmpty("there can't be siblings if there is no parent.");
			node.Depth.Should().Be(0, "because it is a root node.");
			node.Text.Should().Be("Node", "because this is the default when no text is specified.");
			node.Style.Should().Be(TreeViewNodeStyle.None, "because this is the default.");
		}

		[TestMethod]
		public void NodeWithTextTest()
		{
			// Act
			var node = new TreeViewNode("foo");

			// Assert
			node.Text.Should().Be("foo");
		}

		[TestMethod]
		public void NodeWithTextAndCheckTest()
		{
			// Act
			var node = new TreeViewNode("foo", true);

			// Assert
			node.Text.Should().Be("foo");
			node.IsChecked.Should().BeTrue();
		}

		[TestMethod]
		public void SetTextTest()
		{
			// Arrange
			var node = new TreeViewNode("foo");

			// Act
			node.Text = "bar";

			// Assert
			node.Text.Should().Be("bar");
		}

		[TestMethod]
		public void SetTextNullTest()
		{
			// Arrange
			var node = new TreeViewNode("foo");

			// Act
			node.Text = null;

			// Assert
			node.Text.Should().BeEmpty();
		}

		[TestMethod]
		public void SetStyleTest()
		{
			// Arrange
			var node = new TreeViewNode();

			// Act
			node.Style = TreeViewNodeStyle.Checkbox;

			// Assert
			node.Style.Should().Be(TreeViewNodeStyle.Checkbox);
		}

		[TestMethod]
		public void SetUnknownStyleTest()
		{
			// Arrange
			var node = new TreeViewNode();

			// Act & Assert
			Invoking(() => node.Style = (TreeViewNodeStyle)(-1)).Should().Throw<InvalidEnumArgumentException>();
		}

		[TestMethod]
		public void AddChildTest()
		{
			// Arrange
			var node = new TreeViewNode();
			var child = new TreeViewNode();

			// Act
			node.Children.Add(child);

			// Assert
			node.Children.Should().HaveCount(1, "a child was added.");
			child.Parent.Should().Be(node);
			child.Ancestors.Should().HaveCount(1, "the single parent counts as ancestor.");
			child.Depth.Should().Be(1, "it is the direct child of the root node with depth 0.");
			child.Siblings.Should().BeEmpty("the parent does not have other children.");
		}

		[TestMethod]
		public void AddNullChildTest()
		{
			// Arrange
			var node = new TreeViewNode();

			// Act & Assert
			Invoking(() => node.Children.Add(null)).Should().Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void AddSameChildTwiceTest()
		{
			// Arrange
			var node = new TreeViewNode();
			var child = new TreeViewNode();
			node.Children.Add(child);

			// Act & Assert
			Invoking(() => node.Children.Add(child)).Should().Throw<ArgumentException>();
		}

		[TestMethod]
		public void AddChildWithParentTest()
		{
			// Arrange
			var node = new TreeViewNode();
			var child = new TreeViewNode();
			node.Children.Add(child);

			// Act & Assert
			Invoking(() => new TreeViewNode().Children.Add(child)).Should().Throw<ArgumentException>();
		}

		[TestMethod]
		public void SetChildrenTest()
		{
			// Arrange
			var node = new TreeViewNode();
			var child1 = new TreeViewNode();
			var child2 = new TreeViewNode();

			// Act
			node.SetChildren(new[] { child1, child2 });

			// Assert
			node.Children.Should().HaveCount(2, "2 children got added.");
			child1.Siblings.Should().HaveCount(1).And.HaveElementAt(0, child2, "child1 has the same parent as child2.");
		}

		[TestMethod]
		public void SetChildrenNullTest()
		{
			// Arrange
			var node = new TreeViewNode();

			// Act & Assert
			Invoking(() => node.SetChildren(null)).Should().Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void RemoveChildTest()
		{
			// Arrange
			var node = new TreeViewNode();
			var child = new TreeViewNode();
			node.Children.Add(child);

			// Act
			node.Children.Remove(child);

			// Assert
			node.Children.Should().BeEmpty("it should no longer have a child.");
			child.Parent.Should().BeNull("it was removed from the parent.");
			child.Depth.Should().Be(0, "because it became a root node.");
		}

		[TestMethod]
		public void RemoveNullChildTest()
		{
			// Arrange
			var node = new TreeViewNode();
			var child = new TreeViewNode();
			node.Children.Add(child);

			// Act & Assert
			Invoking(() => node.Children.Remove(null)).Should().NotThrow();
		}

		[TestMethod]
		public void RemoveUnknownChildTest()
		{
			// Arrange
			var node = new TreeViewNode();
			var child = new TreeViewNode();
			node.Children.Add(child);

			// Act & Assert
			Invoking(() => node.Children.Remove(new TreeViewNode())).Should().NotThrow();
		}

		[TestMethod]
		public void ContainsChildTest()
		{
			// Arrange
			var node = new TreeViewNode();
			var child = new TreeViewNode();
			node.Children.Add(child);

			// Act
			bool containsChild = node.Children.Contains(child);

			// Assert
			containsChild.Should().BeTrue();
		}

		[TestMethod]
		public void ContainsNullChildTest()
		{
			// Arrange
			var node = new TreeViewNode();
			var child = new TreeViewNode();
			node.Children.Add(child);

			// Act & Assert
			Invoking(() => node.Children.Contains(null)).Should().Throw<ArgumentNullException>();
		}

		[TestMethod]
		public void AncestorTest()
		{
			// Arrange
			var nodeA = new TreeViewNode();
			var nodeB = new TreeViewNode();
			nodeA.Children.Add(nodeB);
			var nodeC = new TreeViewNode();
			nodeB.Children.Add(nodeC);

			// Act & Assert
			nodeC.Ancestors.Should().ContainInOrder(nodeB, nodeA);
		}

		[TestMethod]
		public void ClearChildrenTest()
		{
			// Arrange
			var nodeA = new TreeViewNode();
			var nodeB = new TreeViewNode();
			nodeA.Children.Add(nodeB);
			var nodeC = new TreeViewNode();
			nodeB.Children.Add(nodeC);

			// Act
			nodeA.Children.Clear();

			// Assert
			nodeA.Children.Should().BeEmpty("all children should have been removed.");
			nodeC.Depth.Should().Be(1, "parent nodeB became a root node.");
		}

		[TestMethod]
		public void CopyToTest()
		{
			// Arrange
			var node = new TreeViewNode();
			var child = new TreeViewNode();
			node.Children.Add(child);
			var array = new TreeViewNode[1];

			// Act
			node.Children.CopyTo(array, 0);

			// Assert
			array[0].Should().Be(child);
		}

		[TestMethod]
		public void RootNodeEnumeratorTest()
		{
			// Arrange
			var node = new TreeViewNode();
			var child = new TreeViewNode();
			node.Children.Add(child);

			// Act
			IEnumerator enumerator = ((IEnumerable)node.Children).GetEnumerator();

			// Assert
			enumerator.MoveNext().Should().BeTrue();
		}
	}
}