namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Collections.Specialized;
	using System.Linq;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Net.AutomationUI.Objects;

	/// <summary>
	///     A tree structure with nodes that can be checked.
	/// </summary>
	/// <remarks>This widget only works for web compliant scripts or when launched from a DataMiner web app.</remarks>
	public class TreeView : InteractiveWidget, ITreeView
	{
		private bool changed;
		private TreeViewNode changedNode;
		private TreeViewNode.Change change;
		private List<TreeViewNode> changedNodes;

		/// <summary>
		/// 	Initializes a new instance of the <see cref="TreeView" /> class.
		/// </summary>
		/// <remarks>This widget only works for web compliant scripts or when launched from a DataMiner web app.</remarks>
		public TreeView() : this(Array.Empty<TreeViewNode>())
		{
		}

		/// <summary>
		/// 	Initializes a new instance of the <see cref="TreeView" /> class.
		/// </summary>
		/// <param name="rootNode">Root node of the tree view.</param>
		/// <remarks>This widget only works for web compliant scripts or when launched from a DataMiner web app.</remarks>
		public TreeView(TreeViewNode rootNode) : this(new[] { rootNode })
		{
		}

		/// <summary>
		/// 	Initializes a new instance of the <see cref="TreeView" /> class.
		/// </summary>
		/// <param name="rootNodes">Root nodes of the tree view.</param>
		/// <remarks>This widget only works for web compliant scripts or when launched from a DataMiner web app.</remarks>
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
				if (!IsSubscribedTo())
				{
					WantsOnChange = false;
				}
			}
		}

		/// <inheritdoc />
		public event EventHandler<CheckedEventArgs> Checked
		{
			add
			{
				OnChecked += value;
				WantsOnChange = true;
			}

			remove
			{
				OnChecked -= value;
				if (!IsSubscribedTo())
				{
					WantsOnChange = false;
				}
			}
		}

		/// <inheritdoc />
		public event EventHandler<UncheckedEventArgs> Unchecked
		{
			add
			{
				OnUnchecked += value;
				WantsOnChange = true;
			}

			remove
			{
				OnUnchecked -= value;
				if (!IsSubscribedTo())
				{
					WantsOnChange = false;
				}
			}
		}

		/// <inheritdoc />
		public event EventHandler<ExpandedEventArgs> Expanded
		{
			add
			{
				OnExpanded += value;
				WantsOnChange = true;
			}

			remove
			{
				OnExpanded -= value;
				if (!IsSubscribedTo())
				{
					WantsOnChange = false;
				}
			}
		}

		/// <inheritdoc />
		public event EventHandler<CollapsedEventArgs> Collapsed
		{
			add
			{
				OnCollapsed += value;
				WantsOnChange = true;
			}

			remove
			{
				OnCollapsed -= value;
				if (!IsSubscribedTo())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<ChangedEventArgs> OnChanged;

		private event EventHandler<CheckedEventArgs> OnChecked;

		private event EventHandler<UncheckedEventArgs> OnUnchecked;

		private event EventHandler<ExpandedEventArgs> OnExpanded;

		private event EventHandler<CollapsedEventArgs> OnCollapsed;

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
					change = isCollapsed ? TreeViewNode.Change.Collapsed : TreeViewNode.Change.Expanded;
					changedNodes = new List<TreeViewNode> { node };
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
				change = changedNode.IsChecked ? TreeViewNode.Change.Checked : TreeViewNode.Change.Unchecked;
				changedNodes = nodesWithChangedCheckState;
				return;
			}

			if (WantsOnChange && nodesWithChangedCheckState.Any())
			{
				changed = true;
				changedNode = FindNodeThatCausedChange(nodesWithChangedCheckState);
				change = changedNode.IsChecked ? TreeViewNode.Change.Checked : TreeViewNode.Change.Unchecked;
				changedNodes = nodesWithChangedCheckState;
			}
		}

		/// <inheritdoc />
		protected internal override void RaiseResultEvents()
		{
			if (changed)
			{
				OnChanged?.Invoke(this, new ChangedEventArgs(changedNode, change, changedNodes));
			}

			if (changed && change == TreeViewNode.Change.Checked)
			{
				OnChecked?.Invoke(this, new CheckedEventArgs(changedNodes));
			}

			if (changed && change == TreeViewNode.Change.Unchecked)
			{
				OnUnchecked?.Invoke(this, new UncheckedEventArgs(changedNodes));
			}

			if (changed && change == TreeViewNode.Change.Expanded)
			{
				OnExpanded?.Invoke(this, new ExpandedEventArgs(changedNode));
			}

			if (changed && change == TreeViewNode.Change.Collapsed)
			{
				OnCollapsed?.Invoke(this, new CollapsedEventArgs(changedNode));
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

		private bool IsSubscribedTo()
		{
			return OnChanged?.GetInvocationList().Any() == true ||
				OnChecked?.GetInvocationList().Any() == true ||
				OnUnchecked?.GetInvocationList().Any() == true ||
				OnExpanded?.GetInvocationList().Any() == true ||
				OnCollapsed?.GetInvocationList().Any() == true;
		}

		/// <summary>
		///     Provides data for the <see cref="TreeView.Changed" /> event.
		/// </summary>
		public class ChangedEventArgs : EventArgs
		{
			/// <summary>
			///     Initializes a new instance of the <see cref="ChangedEventArgs" /> class.
			/// </summary>
			/// <param name="interactedNode">The node that has been changed.</param>
			/// <param name="change">The type of change that has occured.</param>
			/// <param name="changedNodes">Collection of all nodes that were changed.</param>
			public ChangedEventArgs(
				TreeViewNode interactedNode,
				TreeViewNode.Change change,
				IList<TreeViewNode> changedNodes)
			{
				InteractedNode = interactedNode;
				Change = change;
				ChangedNodes = new ReadOnlyCollection<TreeViewNode>(changedNodes);
			}

			/// <summary>
			///     Gets the node that caused the change.
			/// </summary>
			public TreeViewNode InteractedNode { get; }

			/// <summary>
			/// 	Gets the type of change that has occured.
			/// </summary>
			public TreeViewNode.Change Change { get; }

			/// <summary>
			/// 	Gets a collection of all nodes that were changed.
			/// </summary>
			/// <remarks>Only contains the interacted node when no other nodes were affected.</remarks>
			public ICollection<TreeViewNode> ChangedNodes { get; }
		}

		/// <summary>
		///     Provides data for the <see cref="TreeView.Checked" /> event.
		/// </summary>
		public class CheckedEventArgs : EventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="CheckedEventArgs"/> class.
			/// </summary>
			/// <param name="checkedNodes">Collection of <see cref="TreeViewNode"/> objects that got checked during interaction.</param>
			public CheckedEventArgs(IEnumerable<TreeViewNode> checkedNodes) => CheckedNodes = checkedNodes.ToArray();

			/// <summary>
			/// 	Gets a collection of <see cref="TreeViewNode"/> objects that got checked during interaction.
			/// </summary>
			/// <remarks>The collection might contain more than one node when <see cref="TreeViewNode.CheckRecursively"/> is <c>true</c>.</remarks>
			public ICollection<TreeViewNode> CheckedNodes { get; }
		}

		/// <summary>
		///     Provides data for the <see cref="TreeView.Unchecked" /> event.
		/// </summary>
		public class UncheckedEventArgs : EventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="UncheckedEventArgs"/> class.
			/// </summary>
			/// <param name="uncheckedNodes">Collection of <see cref="TreeViewNode"/> objects that got unchecked during interaction.</param>
			public UncheckedEventArgs(IEnumerable<TreeViewNode> uncheckedNodes) => UncheckedNodes = uncheckedNodes.ToArray();

			/// <summary>
			/// 	Gets a collection of <see cref="TreeViewNode"/> objects that got unchecked during interaction.
			/// </summary>
			/// <remarks>The collection might contain more than one node when <see cref="TreeViewNode.CheckRecursively"/> is <c>true</c>.</remarks>
			public ICollection<TreeViewNode> UncheckedNodes { get; }
		}

		/// <summary>
		///     Provides data for the <see cref="TreeView.Expanded" /> event.
		/// </summary>
		public class ExpandedEventArgs : EventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="ExpandedEventArgs"/> class.
			/// </summary>
			/// <param name="expandedNode">The Node that got expanded.</param>
			public ExpandedEventArgs(TreeViewNode expandedNode) => ExpandedNode = expandedNode;

			/// <summary>
			/// 	Gets the node that got expanded.
			/// </summary>
			public TreeViewNode ExpandedNode { get; }
		}

		/// <summary>
		///     Provides data for the <see cref="TreeView.Collapsed" /> event.
		/// </summary>
		public class CollapsedEventArgs : EventArgs
		{
			/// <summary>
			/// 	Initializes a new instance of the <see cref="CollapsedEventArgs"/> class.
			/// </summary>
			/// <param name="collapsedNode">The node that got collapsed.</param>
			public CollapsedEventArgs(TreeViewNode collapsedNode) => CollapsedNode = collapsedNode;

			/// <summary>
			/// 	Gets the node that got collapsed.
			/// </summary>
			public TreeViewNode CollapsedNode { get; }
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