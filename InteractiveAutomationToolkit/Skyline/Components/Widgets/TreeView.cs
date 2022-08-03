namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Linq;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Net.AutomationUI.Objects;

	/// <summary>
	///     A tree structure with nodes that can be checked.
	/// </summary>
	/// <remarks>This component is only supported on scripts launched from a web UI (e.g. Dashboards).</remarks>
	public class TreeView : InteractiveWidget, ITreeView
	{
		private bool changed;
		private TreeViewNode changedNode;

		/// <summary>
		/// 	Initializes a new instance of the <see cref="TreeView" /> class.
		/// </summary>
		/// <remarks>This component is only supported on scripts launched from a web UI (e.g. Dashboards).</remarks>
		public TreeView() : this(Array.Empty<TreeViewNode>())
		{
		}

		/// <summary>
		/// 	Initializes a new instance of the <see cref="TreeView" /> class.
		/// </summary>
		/// <param name="rootNode">Root node of the tree view.</param>
		/// <remarks>This component is only supported on scripts launched from a web UI (e.g. Dashboards).</remarks>
		public TreeView(TreeViewNode rootNode) : this(new[] { rootNode })
		{
		}

		/// <summary>
		/// 	Initializes a new instance of the <see cref="TreeView" /> class.
		/// </summary>
		/// <param name="rootNodes">Root nodes of the tree view.</param>
		/// <remarks>This component is only supported on scripts launched from a web UI (e.g. Dashboards).</remarks>
		public TreeView(IEnumerable<TreeViewNode> rootNodes)
		{
			if (rootNodes == null)
			{
				throw new ArgumentNullException(nameof(rootNodes));
			}

			Type = UIBlockType.TreeView;
			RootNodes = new RootNodeCollection(BlockDefinition);
			SetRootNodes(rootNodes);
		}

		/// <inheritdoc />
		public event EventHandler<ChangedEventArgs> Changed
		{
			add
			{
				OnChanged += value;
				WantsOnChange = true;
			}

			remove
			{
				OnChanged -= value;
				if (OnChanged == null || !OnChanged.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<ChangedEventArgs> OnChanged;

		/// <inheritdoc />
		public ICollection<TreeViewNode> RootNodes { get; }

		/// <inheritdoc />
		public IEnumerable<TreeViewNode> Nodes
		{
			get
			{
				foreach (TreeViewNode rootNode in RootNodes)
				{
					yield return rootNode;

					foreach (TreeViewNode node in rootNode.Descendants)
					{
						yield return node;
					}
				}
			}
		}

		/// <inheritdoc />
		public IEnumerable<TreeViewNode> Leaves
		{
			get
			{
				return Nodes.Where(node => node.IsLeaf);
			}
		}

		/// <inheritdoc />
		public IEnumerable<TreeViewNode> InternalNodes
		{
			get
			{
				return Nodes.Where(node => node.IsInternalNode);
			}
		}

		/// <inheritdoc />
		public IEnumerable<TreeViewNode> CheckedNodes
		{
			get
			{
				return Nodes.Where(node => node.IsChecked);
			}
		}

		/// <inheritdoc />
		public IEnumerable<TreeViewNode> CheckedLeaves
		{
			get
			{
				return Nodes.Where(node => node.IsLeaf && node.IsChecked);
			}
		}

		/// <inheritdoc />
		public IEnumerable<TreeViewNode> CheckedInternalNodes
		{
			get
			{
				return Nodes.Where(node => node.IsInternalNode && node.IsChecked);
			}
		}

		/// <inheritdoc />
		public IEnumerable<TreeViewNode> UnCheckedNodes
		{
			get
			{
				return Nodes.Where(node => !node.IsChecked);
			}
		}

		/// <inheritdoc />
		public IEnumerable<TreeViewNode> UncheckedLeaves
		{
			get
			{
				return Nodes.Where(node => node.IsLeaf && !node.IsChecked);
			}
		}

		/// <inheritdoc />
		public IEnumerable<TreeViewNode> UnCheckedInternalNodes
		{
			get
			{
				return Nodes.Where(node => node.IsInternalNode && !node.IsChecked);
			}
		}

		/// <inheritdoc />
		public void SetRootNodes(IEnumerable<TreeViewNode> nodes)
		{
			RootNodes.Clear();
			foreach (TreeViewNode node in nodes)
			{
				RootNodes.Add(node);
			}
		}

		/// <inheritdoc />
		protected internal override void LoadResult(UIResults results)
		{
			string[] checkedNodes = results.GetCheckedItemKeys(this);
			string[] expandedNodes = results.GetExpandedItemKeys(this);

			var nodesWithChangedCheckState = new List<TreeViewNode>();
			foreach (TreeViewNode node in Nodes)
			{
				bool isChecked = checkedNodes.Contains(node.Key);
				if (node.IsChecked != isChecked)
				{
					nodesWithChangedCheckState.Add(node);
				}

				bool isCollapsed = !expandedNodes.Contains(node.Key);
				bool collapseChanged = node.IsCollapsed != isCollapsed;
				node.IsCollapsed = isCollapsed;

				if (WantsOnChange && collapseChanged)
				{
					changed = true;
					changedNode = node;
					break;
				}
			}

			foreach (TreeViewNode node in Nodes)
			{
				bool isChecked = checkedNodes.Contains(node.Key);
				bool checkChanged = node.IsChecked != isChecked;
				if (checkChanged)
				{
					// Optimization
					// Only update when changed because the change could apply recursively
					node.IsChecked = isChecked;
				}
			}

			if (WantsOnChange && nodesWithChangedCheckState.Count == 1)
			{
				changed = true;
				changedNode = nodesWithChangedCheckState.Single();
				return;
			}

			if (WantsOnChange && nodesWithChangedCheckState.Any())
			{
				changed = true;
				changedNode = FindNodeThatCausedChange(nodesWithChangedCheckState);
			}
		}

		/// <inheritdoc />
		protected internal override void RaiseResultEvents()
		{
			if (changed && OnChanged != null)
			{
				OnChanged(this, new ChangedEventArgs(changedNode));
			}

			changed = false;
		}

		private static TreeViewNode FindNodeThatCausedChange(List<TreeViewNode> nodesWithChangedCheckState)
		{
			TreeViewNode lowestCommonAncestor = nodesWithChangedCheckState.OrderBy(node => node.Depth).First();
			bool ancestorChanged = lowestCommonAncestor.Children
				.All(node => node.IsChecked == lowestCommonAncestor.IsChecked);

			if (ancestorChanged)
			{
				return lowestCommonAncestor;
			}

			TreeViewNode lowestChangedNode = nodesWithChangedCheckState.OrderByDescending(node => node.Depth).First();
			return lowestChangedNode;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class ChangedEventArgs : EventArgs
		{
			/// <summary>
			///     Initializes a new instance of the <see cref="ChangedEventArgs" /> class.
			/// </summary>
			/// <param name="node">The node that has been changed.</param>
			public ChangedEventArgs(TreeViewNode node) => Node = node;

			/// <summary>
			///     Gets the node that has been changed.
			/// </summary>
			public TreeViewNode Node { get; }
		}

		private class RootNodeCollection : ICollection<TreeViewNode>
		{
			private readonly HybridDictionary nodes = new HybridDictionary();
			private readonly UIBlockDefinition blockDefinition;

			public RootNodeCollection(UIBlockDefinition blockDefinition)
			{
				this.blockDefinition = blockDefinition;
				this.blockDefinition.TreeViewItems = new List<TreeViewItem>();
			}

			public int Count => nodes.Count;

			public bool IsReadOnly => ((ICollection<TreeViewNode>)nodes.Values).IsReadOnly;

			public IEnumerator<TreeViewNode> GetEnumerator()
			{
				return nodes.Values.Cast<TreeViewNode>().GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable)nodes).GetEnumerator();
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

				if (nodes.Contains(item.Key))
				{
					throw new ArgumentException("Root node is already added.", nameof(item));
				}

				nodes.Add(item.Key, item);
				blockDefinition.TreeViewItems.Add(item.TreeViewItem);
			}

			public void Clear()
			{
				nodes.Clear();
				blockDefinition.TreeViewItems.Clear();
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
				if (array == null)
				{
					throw new ArgumentNullException(nameof(array));
				}

				if (arrayIndex < 0)
				{
					throw new ArgumentOutOfRangeException(nameof(arrayIndex));
				}

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

				nodes.Remove(item.Key);
				blockDefinition.TreeViewItems.Remove(item.TreeViewItem);
				return true;
			}
		}
	}
}