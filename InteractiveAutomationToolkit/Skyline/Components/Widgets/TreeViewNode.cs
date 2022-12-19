namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Linq;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Net.AutomationUI.Objects;

	/// <summary>
	/// 	Implements a selectable item in a <see cref="ITreeView"/> control.
	/// </summary>
	public class TreeViewNode
	{
		private int depth;

		/// <summary>
		/// 	Initializes a new instance of the <see cref="TreeViewNode" /> class.
		/// </summary>
		public TreeViewNode() : this("Node", false)
		{
		}

		/// <summary>
		/// 	Initializes a new instance of the <see cref="TreeViewNode" /> class.
		/// </summary>
		/// <param name="text">The text displayed next to the node.</param>
		public TreeViewNode(string text) : this(text, false)
		{
		}

		/// <summary>
		/// 	Initializes a new instance of the <see cref="TreeViewNode" /> class.
		/// </summary>
		/// <param name="text">The text displayed next to the node.</param>
		/// <param name="isChecked">Value indicating whether the node should be checked.</param>
		public TreeViewNode(string text, bool isChecked)
		{
			TreeViewItem = new TreeViewItem(text ?? String.Empty, Guid.NewGuid().ToString(), new List<TreeViewItem>())
			{
				SupportsLazyLoading = true,
			};

			Children = new NodeCollection(this);
			Style = TreeViewNodeStyle.None;
			CheckRecursively = true;
			IsChecked = isChecked;
			IsCollapsed = true;
		}

		/// <summary>
		/// 	Gets the parent node.
		/// </summary>
		public TreeViewNode Parent { get; private set; }

		/// <summary>
		/// 	Gets child nodes.
		/// </summary>
		public ICollection<TreeViewNode> Children { get; }

		/// <summary>
		/// Gets or sets the style of the node.
		/// </summary>
		/// <exception cref="InvalidEnumArgumentException">When <paramref name="value"/> does not specify a valid member of <see cref="TreeViewNodeStyle"/>.</exception>
		public TreeViewNodeStyle Style
		{
			get
			{
				switch (TreeViewItem.ItemType)
				{
					case TreeViewItem.TreeViewItemType.Empty:
						return TreeViewNodeStyle.None;

					case TreeViewItem.TreeViewItemType.CheckBox:
						return TreeViewNodeStyle.Checkbox;

					default:
						Debug.Assert(true, "Missing type should be implemented");
						return TreeViewNodeStyle.None;
				}
			}

			set
			{
				switch (value)
				{
					case TreeViewNodeStyle.None:
						TreeViewItem.ItemType = TreeViewItem.TreeViewItemType.Empty;
						return;

					case TreeViewNodeStyle.Checkbox:
						TreeViewItem.ItemType = TreeViewItem.TreeViewItemType.CheckBox;
						return;

					default:
						throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(TreeViewNodeStyle));
				}
			}
		}

		/// <summary>
		/// 	Gets or sets a value indicating whether checking this node will also check the child nodes.
		/// </summary>
		public bool CheckRecursively
		{
			get => TreeViewItem.CheckingBehavior == TreeViewItem.TreeViewItemCheckingBehavior.FullRecursion;

			set => TreeViewItem.CheckingBehavior =
				value
					? TreeViewItem.TreeViewItemCheckingBehavior.FullRecursion
					: TreeViewItem.TreeViewItemCheckingBehavior.None;
		}

		/// <summary>
		/// 	Gets or sets a value indicating whether this node is checked.
		/// </summary>
		public bool IsChecked
		{
			get => TreeViewItem.IsChecked;

			set
			{
				TreeViewItem.IsChecked = value;
				if (CheckRecursively && value)
				{
					foreach (TreeViewNode child in Children)
					{
						child.IsChecked = true;
					}
				}

				if (Parent != null && Parent.CheckRecursively && !value)
				{
					Parent.IsChecked = false;
				}
			}
		}

		/// <summary>
		/// 	Gets or sets a value indicating whether this node is displayed in a collapsed state.
		/// </summary>
		public bool IsCollapsed
		{
			get => TreeViewItem.IsCollapsed;

			set => TreeViewItem.IsCollapsed = value;
		}

		/// <summary>
		/// 	Gets or sets the text displayed next to the node.
		/// </summary>
		public string Text
		{
			get => TreeViewItem.DisplayValue;
			set => TreeViewItem.DisplayValue = value ?? String.Empty;
		}

		/// <summary>
		/// 	Gets a value indicating whether the node is a leaf.
		/// <c>true</c> when the node has no children.
		/// <c>false</c> when the node has one or more children.
		/// </summary>
		public bool IsLeaf => !IsInternalNode;

		/// <summary>
		/// 	Gets a value indicating whether the node is a internal node.
		/// <c>true</c> when the node has one or more children.
		/// <c>false</c> when the node has no children.
		/// </summary>
		public bool IsInternalNode => Children.Any();

		/// <summary>
		/// 	Gets the length of the path to the root node.
		/// 	The root nodes have a depth of 0.
		/// </summary>
		public int Depth
		{
			get => depth;

			private set
			{
				depth = value;
				foreach (TreeViewNode child in Children)
				{
					child.depth = value + 1;
				}
			}
		}

		/// <summary>
		/// 	Gets all nodes reachable by repeated proceeding from parent to child.
		/// </summary>
		/// <remarks>This node is not included. Nodes are traversed depth first.</remarks>
		public IEnumerable<TreeViewNode> Descendants
		{
			get
			{
				foreach (TreeViewNode child in Children)
				{
					yield return child;

					foreach (TreeViewNode node in child.Descendants)
					{
						yield return node;
					}
				}
			}
		}

		/// <summary>
		/// 	Gets all nodes reachable by repeated proceeding from child to parent.
		/// </summary>
		/// <remarks>This node is not included.</remarks>
		public IEnumerable<TreeViewNode> Ancestors
		{
			get
			{
				if (Parent == null)
				{
					yield break;
				}

				yield return Parent;

				foreach (TreeViewNode node in Parent.Ancestors)
				{
					yield return node;
				}
			}
		}

		/// <summary>
		/// 	Gets all nodes that have the same parent is this node.
		/// </summary>
		/// <remarks>This node is not included.</remarks>
		public IEnumerable<TreeViewNode> Siblings
		{
			get
			{
				return Parent == null
					? Enumerable.Empty<TreeViewNode>()
					: Parent.Children.Where(sibling => sibling != this);
			}
		}

		/// <summary>
		/// 	Gets the string value that is used as a key to retrieve the selected state of the item from
		/// 	<see cref="UIResults"/>.
		/// </summary>
		protected internal string Key => TreeViewItem.KeyValue;

		/// <summary>
		/// 	Gets the internal DataMiner representation of the node.
		/// </summary>
		protected internal TreeViewItem TreeViewItem { get; }

		/// <summary>
		/// 	Sets the child nodes.
		/// 	Replaces existing child nodes.
		/// </summary>
		/// <param name="nodes">Child nodes to add to the node.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="nodes"/> is <c>null</c>.</exception>
		public void SetChildren(IEnumerable<TreeViewNode> nodes)
		{
			if (nodes == null)
			{
				throw new ArgumentNullException(nameof(nodes));
			}

			Children.Clear();
			foreach (TreeViewNode node in nodes)
			{
				Children.Add(node);
			}
		}

		private class NodeCollection : ICollection<TreeViewNode>
		{
			private readonly TreeViewNode parent;
			private readonly HybridDictionary nodes = new HybridDictionary();

			public NodeCollection(TreeViewNode parent) => this.parent = parent;

			public int Count => nodes.Count;

			public bool IsReadOnly => nodes.IsReadOnly;

			public IEnumerator<TreeViewNode> GetEnumerator()
			{
				return nodes.Values.Cast<TreeViewNode>().GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return nodes.Keys.GetEnumerator();
			}

			public void Add(TreeViewNode item)
			{
				if (item == null)
				{
					throw new ArgumentNullException(nameof(item));
				}

				if (item.Parent != null)
				{
					throw new ArgumentException("Node already has a parent.", nameof(item));
				}

				item.Parent = parent;
				item.Depth = parent.Depth + 1;
				parent.TreeViewItem.ChildItems.Add(item.TreeViewItem);
				nodes.Add(item.Key, item);
				if (parent.CheckRecursively && parent.IsChecked)
				{
					item.IsChecked = true;
				}
			}

			public void Clear()
			{
				foreach (TreeViewNode node in nodes.Values)
				{
					node.Parent = null;
					node.Depth = 0;
				}

				parent.TreeViewItem.ChildItems.Clear();
				nodes.Clear();
			}

			public bool Contains(TreeViewNode item)
			{
				if (item == null)
				{
					throw new ArgumentNullException(nameof(item));
				}

				return nodes.Contains(item.Key);
			}

			public void CopyTo(TreeViewNode[] array, int arrayIndex)
			{
				nodes.Values.CopyTo(array, arrayIndex);
			}

			public bool Remove(TreeViewNode item)
			{
				if (item == null)
				{
					return false;
				}

				if (!nodes.Contains(item.Key))
				{
					return false;
				}

				item.Parent = null;
				item.Depth = 0;
				nodes.Remove(item.Key);
				parent.TreeViewItem.ChildItems.Remove(item.TreeViewItem);
				return true;
			}
		}
	}
}