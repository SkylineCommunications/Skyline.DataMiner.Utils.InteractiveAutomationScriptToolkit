namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Net.AutomationUI.Objects;

	/// <summary>
	///  A generic tree view structure that allows attaching custom metadata to each item.
	/// </summary>
	/// <typeparam name="T">The type of the value associated with each tree view item.</typeparam>
	public class TreeView<T> : TreeViewBase, ITreeView<T>
	{
		private readonly List<TreeViewItem<T>> rootItems = new List<TreeViewItem<T>>();

		private Dictionary<string, bool> checkedItemCache;
		private Dictionary<string, bool> collapsedItemCache;
		private Dictionary<string, TreeViewItem<T>> lookupTable;

		private bool itemsChecked = false;
		private List<TreeViewItem<T>> checkedItems = new List<TreeViewItem<T>>();

		private bool itemsUnchecked = false;
		private List<TreeViewItem<T>> uncheckedItems = new List<TreeViewItem<T>>();

		private bool itemsExpanded = false;
		private List<TreeViewItem<T>> expandedItems = new List<TreeViewItem<T>>();

		private bool itemsCollapsed = false;
		private List<TreeViewItem<T>> collapsedItems = new List<TreeViewItem<T>>();

		/// <summary>
		/// 	Initializes a new instance of the <see cref="TreeView{T}" /> class.
		/// </summary>
		public TreeView() : this(Enumerable.Empty<TreeViewItem<T>>())
		{
		}

		/// <summary>
		/// 	Initializes a new instance of the <see cref="TreeView{T}" /> class.
		/// </summary>
		/// <param name="treeViewItems">Root nodes of the tree view.</param>
		public TreeView(IEnumerable<TreeViewItem<T>> treeViewItems)
		{
			Items = treeViewItems;
		}

		/// <summary>
		///     Triggered when a different item is selected or no longer selected.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<IEnumerable<TreeViewItem<T>>> Changed
		{
			add
			{
				OnChanged += value;
				BlockDefinition.WantsOnChange = true;
			}

			remove
			{
				OnChanged -= value;
				if (OnChanged == null || !OnChanged.GetInvocationList().Any())
				{
					BlockDefinition.WantsOnChange = false;
				}
			}
		}

		/// <summary>
		///  Triggered whenever an item is selected.
		///  WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<IEnumerable<TreeViewItem<T>>> Checked
		{
			add
			{
				OnChecked += value;
				BlockDefinition.WantsOnChange = true;
			}

			remove
			{
				OnChecked -= value;
				if (OnChecked == null || !OnChecked.GetInvocationList().Any())
				{
					BlockDefinition.WantsOnChange = false;
				}
			}
		}

		/// <summary>
		///  Triggered whenever an item is no longer selected.
		///  WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<IEnumerable<TreeViewItem<T>>> Unchecked
		{
			add
			{
				OnUnchecked += value;
				BlockDefinition.WantsOnChange = true;
			}

			remove
			{
				OnUnchecked -= value;
				if (OnUnchecked == null || !OnUnchecked.GetInvocationList().Any())
				{
					BlockDefinition.WantsOnChange = false;
				}
			}
		}

		/// <summary>
		///  Triggered whenever an item is expanded.
		///  Can be used for lazy loading.
		///  Will be triggered whenever a node with SupportsLazyLoading set to true is expanded.
		/// </summary>
		public event EventHandler<IEnumerable<TreeViewItem<T>>> Expanded
		{
			add
			{
				OnExpanded += value;
			}

			remove
			{
				OnExpanded -= value;
			}
		}

		/// <summary>
		///  Triggered whenever an item is collapsed.
		///  Will be triggered whenever a node with SupportsLazyLoading set to true is collapsed.
		/// </summary>
		public event EventHandler<IEnumerable<TreeViewItem<T>>> Collapsed
		{
			add
			{
				OnCollapsed += value;
			}

			remove
			{
				OnCollapsed -= value;
			}
		}

		private event EventHandler<IEnumerable<TreeViewItem<T>>> OnChanged;

		private event EventHandler<IEnumerable<TreeViewItem<T>>> OnChecked;

		private event EventHandler<IEnumerable<TreeViewItem<T>>> OnUnchecked;

		private event EventHandler<IEnumerable<TreeViewItem<T>>> OnExpanded;

		private event EventHandler<IEnumerable<TreeViewItem<T>>> OnCollapsed;

		/// <inheritdoc/>
		public IEnumerable<TreeViewItem<T>> Items
		{
			get
			{
				return rootItems;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value));
				}

				rootItems.Clear();
				rootItems.AddRange(value);

				BlockDefinition.TreeViewItems = rootItems.Select(x => x.Item).ToList();

				UpdateItemCache();
			}
		}

		/// <inheritdoc/>
		public IEnumerable<TreeViewItem<T>> CheckedItems
		{
			get
			{
				return GetCheckedItems();
			}
		}

		/// <inheritdoc/>
		public IEnumerable<TreeViewItem<T>> CheckedLeaves
		{
			get
			{
				return GetCheckedItems().Where(x => !x.ChildItems.Any());
			}
		}

		/// <inheritdoc/>
		public IEnumerable<TreeViewItem<T>> CheckedNodes
		{
			get
			{
				return GetCheckedItems().Where(x => x.ChildItems.Any());
			}
		}

		/// <inheritdoc/>
		public IEnumerable<T> CheckedValues
		{
			get
			{
				return GetCheckedItems().Select(x => x.Value);
			}
		}

		/// <inheritdoc/>
		public IEnumerable<T> CheckedLeafValues
		{
			get
			{
				return CheckedLeaves.Select(x => x.Value);
			}
		}

		/// <inheritdoc/>
		public IEnumerable<T> CheckedNodeValues
		{
			get
			{
				return CheckedNodes.Select(x => x.Value);
			}
		}

		/// <inheritdoc/>
		public override void Collapse()
		{
			foreach (var item in GetAllItems())
			{
				item.IsCollapsed = true;
			}
		}

		/// <inheritdoc/>
		public override void Expand()
		{
			foreach (var item in GetAllItems())
			{
				item.IsCollapsed = false;
			}
		}

		/// <inheritdoc/>
		public bool TryFindTreeViewItem(string key, out TreeViewItem<T> item)
		{
			return lookupTable.TryGetValue(key, out item);
		}

		/// <inheritdoc/>
		public override void UpdateItemCache()
		{
			checkedItemCache = new Dictionary<string, bool>();
			collapsedItemCache = new Dictionary<string, bool>();
			lookupTable = new Dictionary<string, TreeViewItem<T>>();

			foreach (var item in GetAllItems(rootItems))
			{
				try
				{
					checkedItemCache.Add(item.KeyValue, item.IsChecked);
					if (item.SupportsLazyLoading)
					{
						collapsedItemCache.Add(item.KeyValue, item.IsCollapsed);
					}

					lookupTable.Add(item.KeyValue, item);
				}
				catch (Exception e)
				{
					throw new TreeViewDuplicateItemsException(item.KeyValue, e);
				}
			}
		}

		/// <inheritdoc/>
		public IEnumerable<TreeViewItem<T>> GetAllItems()
		{
			return lookupTable.Values;
		}

		/// <inheritdoc/>
		public IEnumerable<TreeViewItem<T>> GetItems(int depth)
		{
			return GetItems(Items, depth, 0);
		}

		/// <inheritdoc/>
		protected internal override void LoadResult(IUIResults uiResults)
		{
			var checkedItemKeys = uiResults.GetCheckedItemKeys(this).ToHashSet(); // this includes all checked items
			var expandedItemKeys = uiResults.GetExpandedItemKeys(this).ToHashSet(); // this includes all expanded items with LazyLoading set to true

			// Check for changes
			// Expanded Items
			RegisterExpandedItems(expandedItemKeys);

			// Collapsed Items
			RegisterCollapsedItems(expandedItemKeys);

			// Checked Items
			RegisterCheckedItems(checkedItemKeys);

			// Unchecked Items
			RegisterUncheckedItems(checkedItemKeys);

			// Persist states
			foreach (TreeViewItem<T> item in lookupTable.Values)
			{
				item.Item.IsChecked = checkedItemKeys.Contains(item.KeyValue);
				item.Item.IsCollapsed = !expandedItemKeys.Contains(item.KeyValue);
			}

			UpdateItemCache();
		}

		/// <inheritdoc/>
		protected internal override void RaiseResultEvents()
		{
			var changedItems = new List<TreeViewItem<T>>();

			// Expanded items
			if (itemsExpanded && OnExpanded != null)
			{
				OnExpanded(this, expandedItems);
			}

			// Collapsed items
			if (itemsCollapsed && OnCollapsed != null)
			{
				OnCollapsed(this, collapsedItems);
			}

			// Checked items
			if (itemsChecked && OnChecked != null)
			{
				changedItems.AddRange(checkedItems);
				OnChecked(this, checkedItems);
			}

			// Unchecked items
			if (itemsUnchecked && OnUnchecked != null)
			{
				changedItems.AddRange(uncheckedItems);
				OnUnchecked(this, uncheckedItems);
			}

			// Changed items
			if (changedItems.Any() && OnChanged != null)
			{
				OnChanged(this, changedItems);
			}

			itemsExpanded = false;
			itemsCollapsed = false;
			itemsChecked = false;
			itemsUnchecked = false;

			UpdateItemCache();
		}

		/// <summary>
		/// Returns all items in the TreeView that are checked.
		/// </summary>
		/// <returns>All checked TreeViewItems in the TreeView.</returns>
		private IEnumerable<TreeViewItem<T>> GetCheckedItems()
		{
			return lookupTable.Values.Where(x => x.ItemType == TreeViewItem.TreeViewItemType.CheckBox && x.IsChecked);
		}

		/// <summary>
		/// This method is used to recursively go through all the items in the TreeView.
		/// </summary>
		/// <param name="children">List of TreeViewItems to be visited.</param>
		/// <returns>Flat collection containing every item in the provided children collection and all underlying items.</returns>
		private IEnumerable<TreeViewItem<T>> GetAllItems(IEnumerable<TreeViewItem<T>> children)
		{
			if (children == null)
				yield break;

			var queue = new Queue<TreeViewItem<T>>(children);

			while (queue.Count > 0)
			{
				var item = queue.Dequeue();
				yield return item;

				foreach (var child in item.ChildItems)
				{
					queue.Enqueue(child);
				}
			}
		}

		/// <summary>
		/// Returns all TreeViewItems in the TreeView that are located on the provided depth.
		/// </summary>
		/// <param name="children">Items to be checked.</param>
		/// <param name="requestedDepth">Depth that was requested.</param>
		/// <param name="currentDepth">Current depth in the tree.</param>
		/// <returns>All TreeViewItems in the TreeView that are located on the provided depth.</returns>
		private IEnumerable<TreeViewItem<T>> GetItems(IEnumerable<TreeViewItem<T>> children, int requestedDepth, int currentDepth)
		{
			List<TreeViewItem<T>> requestedItems = new List<TreeViewItem<T>>();
			bool depthReached = requestedDepth == currentDepth;
			foreach (TreeViewItem<T> item in children)
			{
				if (depthReached)
				{
					requestedItems.Add(item);
				}
				else
				{
					int newDepth = currentDepth + 1;
					requestedItems.AddRange(GetItems(item.ChildItems, requestedDepth, newDepth));
				}
			}

			return requestedItems;
		}

		private void RegisterExpandedItems(ISet<string> expandedItemKeys)
		{
			var newlyExpandedItems = collapsedItemCache
				.Where(x => expandedItemKeys.Contains(x.Key) && x.Value)
				.Select(x => x.Key)
				.ToList();

			if (newlyExpandedItems.Any() && OnExpanded != null)
			{
				itemsExpanded = true;
				expandedItems = newlyExpandedItems.Select(x => lookupTable[x]).ToList();
			}
		}

		private void RegisterCollapsedItems(ISet<string> expandedItemKeys)
		{
			var newlyCollapsedItems = collapsedItemCache
				.Where(x => !expandedItemKeys.Contains(x.Key) && !x.Value)
				.Select(x => x.Key)
				.ToList();

			if (newlyCollapsedItems.Any() && OnCollapsed != null)
			{
				itemsCollapsed = true;
				collapsedItems = newlyCollapsedItems.Select(x => lookupTable[x]).ToList();
			}
		}

		private void RegisterCheckedItems(ISet<string> checkedItemKeys)
		{
			var newlyCheckedItemKeys = checkedItemCache
				.Where(x => checkedItemKeys.Contains(x.Key) && !x.Value)
				.Select(x => x.Key)
				.ToList();

			if (newlyCheckedItemKeys.Any() && OnChecked != null)
			{
				itemsChecked = true;
				checkedItems = newlyCheckedItemKeys.Select(x => lookupTable[x]).ToList();
			}
		}

		private void RegisterUncheckedItems(ISet<string> checkedItemKeys)
		{
			var newlyUncheckedItemKeys = checkedItemCache
				.Where(x => !checkedItemKeys.Contains(x.Key) && x.Value)
				.Select(x => x.Key)
				.ToList();

			if (newlyUncheckedItemKeys.Any() && OnUnchecked != null)
			{
				itemsUnchecked = true;
				uncheckedItems = newlyUncheckedItemKeys.Select(x => lookupTable[x]).ToList();
			}
		}
	}
}
