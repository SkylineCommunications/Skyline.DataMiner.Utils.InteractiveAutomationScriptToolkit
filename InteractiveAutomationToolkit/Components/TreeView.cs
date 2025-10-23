namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Net.AutomationUI.Objects;

	/// <summary>
	///  A tree view structure.
	/// </summary>
	public class TreeView : TreeViewBase, ITreeView
	{
		private readonly List<TreeViewItem> rootItems = new List<TreeViewItem>();
		private Dictionary<string, bool> checkedItemCache;
		private Dictionary<string, bool> collapsedItemCache; // TODO: should only contain Items with LazyLoading set to true
		private Dictionary<string, TreeViewItem> lookupTable;

		private bool itemsChanged = false;
		private List<TreeViewItem> changedItems = new List<TreeViewItem>();

		private bool itemsChecked = false;
		private List<TreeViewItem> checkedItems = new List<TreeViewItem>();

		private bool itemsUnchecked = false;
		private List<TreeViewItem> uncheckedItems = new List<TreeViewItem>();

		private bool itemsExpanded = false;
		private List<TreeViewItem> expandedItems = new List<TreeViewItem>();

		private bool itemsCollapsed = false;
		private List<TreeViewItem> collapsedItems = new List<TreeViewItem>();

		/// <summary>
		/// 	Initializes a new instance of the <see cref="TreeView" /> class.
		/// </summary>
		public TreeView() : this(Enumerable.Empty<TreeViewItem>())
		{
		}

		/// <summary>
		/// 	Initializes a new instance of the <see cref="TreeView" /> class.
		/// </summary>
		/// <param name="treeViewItems">Root nodes of the tree view.</param>
		public TreeView(IEnumerable<TreeViewItem> treeViewItems)
		{
			Items = treeViewItems;
		}

		/// <summary>
		///     Triggered when a different item is selected or no longer selected.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<IEnumerable<TreeViewItem>> Changed
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
		public event EventHandler<IEnumerable<TreeViewItem>> Checked
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
		public event EventHandler<IEnumerable<TreeViewItem>> Unchecked
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
		public event EventHandler<IEnumerable<TreeViewItem>> Expanded
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
		public event EventHandler<IEnumerable<TreeViewItem>> Collapsed
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

		private event EventHandler<IEnumerable<TreeViewItem>> OnChanged;

		private event EventHandler<IEnumerable<TreeViewItem>> OnChecked;

		private event EventHandler<IEnumerable<TreeViewItem>> OnUnchecked;

		private event EventHandler<IEnumerable<TreeViewItem>> OnExpanded;

		private event EventHandler<IEnumerable<TreeViewItem>> OnCollapsed;

		/// <inheritdoc/>
		public IEnumerable<TreeViewItem> Items
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

				BlockDefinition.TreeViewItems = rootItems;

				UpdateItemCache();
			}
		}

		/// <inheritdoc/>
		public IEnumerable<TreeViewItem> CheckedItems
		{
			get
			{
				return GetCheckedItems();
			}
		}

		/// <inheritdoc/>
		public IEnumerable<TreeViewItem> CheckedLeaves
		{
			get
			{
				return GetCheckedItems().Where(x => !x.ChildItems.Any());
			}
		}

		/// <inheritdoc/>
		public IEnumerable<TreeViewItem> CheckedNodes
		{
			get
			{
				return GetCheckedItems().Where(x => x.ChildItems.Any());
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
		public bool TryFindTreeViewItem(string key, out TreeViewItem item)
		{
			return lookupTable.TryGetValue(key, out item);
		}

		/// <inheritdoc/>
		public override void UpdateItemCache()
		{
			checkedItemCache = new Dictionary<string, bool>();
			collapsedItemCache = new Dictionary<string, bool>();
			lookupTable = new Dictionary<string, TreeViewItem>();

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
		public IEnumerable<TreeViewItem> GetAllItems()
		{
			return lookupTable.Values;
		}

		/// <inheritdoc/>
		public IEnumerable<TreeViewItem> GetItems(int depth)
		{
			return GetItems(Items, depth, 0);
		}

		/// <inheritdoc/>
		protected internal override void LoadResult(IUIResults uiResults)
		{
			var checkedItemKeys = uiResults.GetCheckedItemKeys(this); // this includes all checked items
			var expandedItemKeys = uiResults.GetExpandedItemKeys(this); // this includes all expanded items with LazyLoading set to true

			// Check for changes
			// Expanded Items
			RegisterExpandedItems(expandedItemKeys);

			// Collapsed Items
			RegisterCollapsedItems(expandedItemKeys);

			// Checked Items
			List<string> newlyCheckedItemKeys = RegisterCheckedItems(checkedItemKeys);

			// Unchecked Items
			List<string> newlyUncheckedItemKeys = RegisterUncheckedItems(checkedItemKeys);

			// Changed Items
			List<string> changedItemKeys = new List<string>();
			changedItemKeys.AddRange(newlyCheckedItemKeys);
			changedItemKeys.AddRange(newlyUncheckedItemKeys);
			if (changedItemKeys.Any() && OnChanged != null)
			{
				itemsChanged = true;
				changedItems = new List<TreeViewItem>();

				foreach (string changedItemKey in changedItemKeys)
				{
					changedItems.Add(lookupTable[changedItemKey]);
				}
			}

			// Persist states
			foreach (TreeViewItem item in lookupTable.Values)
			{
				item.IsChecked = checkedItemKeys.Contains(item.KeyValue);
				item.IsCollapsed = !expandedItemKeys.Contains(item.KeyValue);
			}

			UpdateItemCache();
		}

		/// <inheritdoc/>
		protected internal override void RaiseResultEvents()
		{
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
				OnChecked(this, checkedItems);
			}

			// Unchecked items
			if (itemsUnchecked && OnUnchecked != null)
			{
				OnUnchecked(this, uncheckedItems);
			}

			// Changed items
			if (itemsChanged && OnChanged != null)
			{
				OnChanged(this, changedItems);
			}

			itemsExpanded = false;
			itemsCollapsed = false;
			itemsChecked = false;
			itemsUnchecked = false;
			itemsChanged = false;

			UpdateItemCache();
		}

		/// <summary>
		/// Returns all items in the TreeView that are checked.
		/// </summary>
		/// <returns>All checked TreeViewItems in the TreeView.</returns>
		private IEnumerable<TreeViewItem> GetCheckedItems()
		{
			return lookupTable.Values.Where(x => x.ItemType == TreeViewItem.TreeViewItemType.CheckBox && x.IsChecked);
		}

		/// <summary>
		/// This method is used to recursively go through all the items in the TreeView.
		/// </summary>
		/// <param name="children">List of TreeViewItems to be visited.</param>
		/// <returns>Flat collection containing every item in the provided children collection and all underlying items.</returns>
		private IEnumerable<TreeViewItem> GetAllItems(IEnumerable<TreeViewItem> children)
		{
			if (children == null)
				yield break;

			var queue = new Queue<TreeViewItem>(children);

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
		private IEnumerable<TreeViewItem> GetItems(IEnumerable<TreeViewItem> children, int requestedDepth, int currentDepth)
		{
			List<TreeViewItem> requestedItems = new List<TreeViewItem>();
			bool depthReached = requestedDepth == currentDepth;
			foreach (TreeViewItem item in children)
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

		private void RegisterExpandedItems(IEnumerable<string> expandedItemKeys)
		{
			List<string> newlyExpandedItems = collapsedItemCache.Where(x => expandedItemKeys.Contains(x.Key) && x.Value).Select(x => x.Key).ToList();
			if (newlyExpandedItems.Any() && OnExpanded != null)
			{
				itemsExpanded = true;
				expandedItems = new List<TreeViewItem>();

				foreach (string newlyExpandedItemKey in newlyExpandedItems)
				{
					expandedItems.Add(lookupTable[newlyExpandedItemKey]);
				}
			}
		}

		private void RegisterCollapsedItems(IEnumerable<string> expandedItemKeys)
		{
			List<string> newlyCollapsedItems = collapsedItemCache.Where(x => !expandedItemKeys.Contains(x.Key) && !x.Value).Select(x => x.Key).ToList();
			if (newlyCollapsedItems.Any() && OnCollapsed != null)
			{
				itemsCollapsed = true;
				collapsedItems = new List<TreeViewItem>();

				foreach (string newlyCollapsedItemKey in newlyCollapsedItems)
				{
					collapsedItems.Add(lookupTable[newlyCollapsedItemKey]);
				}
			}
		}

		private List<string> RegisterCheckedItems(IEnumerable<string> checkedItemKeys)
		{
			List<string> newlyCheckedItemKeys = checkedItemCache.Where(x => checkedItemKeys.Contains(x.Key) && !x.Value).Select(x => x.Key).ToList();
			if (newlyCheckedItemKeys.Any() && OnChecked != null)
			{
				itemsChecked = true;
				checkedItems = new List<TreeViewItem>();

				foreach (string newlyCheckedItemKey in newlyCheckedItemKeys)
				{
					checkedItems.Add(lookupTable[newlyCheckedItemKey]);
				}
			}

			return newlyCheckedItemKeys;
		}

		private List<string> RegisterUncheckedItems(IEnumerable<string> checkedItemKeys)
		{
			List<string> newlyUncheckedItemKeys = checkedItemCache.Where(x => !checkedItemKeys.Contains(x.Key) && x.Value).Select(x => x.Key).ToList();
			if (newlyUncheckedItemKeys.Any() && OnUnchecked != null)
			{
				itemsUnchecked = true;
				uncheckedItems = new List<TreeViewItem>();

				foreach (string newlyUncheckedItemKey in newlyUncheckedItemKeys)
				{
					uncheckedItems.Add(lookupTable[newlyUncheckedItemKey]);
				}
			}

			return newlyUncheckedItemKeys;
		}
	}
}
