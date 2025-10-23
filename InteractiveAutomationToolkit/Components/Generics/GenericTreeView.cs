namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Net.AutomationUI.Objects;

	/// <summary>
	///     A generic tree view structure that allows attaching custom metadata to each item.
	/// </summary>
	/// <typeparam name="T">The type of the value associated with each tree view item.</typeparam>
	public class TreeView<T> : TreeViewBase, ITreeView<T>
	{
		private Dictionary<string, bool> checkedItemCache;
		private Dictionary<string, bool> collapsedItemCache;
		private Dictionary<string, TreeViewItem<T>> lookupTable;

		private bool itemsChanged = false;
		private List<TreeViewItem<T>> changedItems = new List<TreeViewItem<T>>();

		private bool itemsChecked = false;
		private List<TreeViewItem<T>> checkedItems = new List<TreeViewItem<T>>();

		private bool itemsUnchecked = false;
		private List<TreeViewItem<T>> uncheckedItems = new List<TreeViewItem<T>>();

		private bool itemsExpanded = false;
		private List<TreeViewItem<T>> expandedItems = new List<TreeViewItem<T>>();

		private bool itemsCollapsed = false;
		private List<TreeViewItem<T>> collapsedItems = new List<TreeViewItem<T>>();

		/// <summary>
		///     Initializes a new instance of the <see cref="TreeView{T}" /> class.
		/// </summary>
		public TreeView() : this(Enumerable.Empty<TreeViewItem<T>>())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="TreeView{T}" /> class.
		/// </summary>
		/// <param name="treeViewItems">Root nodes of the tree view.</param>
		public TreeView(IEnumerable<TreeViewItem<T>> treeViewItems)
		{
			Type = UIBlockType.TreeView;
			Items = treeViewItems;
			IsReadOnly = false;
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
		///     Triggered whenever an item is selected.
		///     WantsOnChange will be set to true when this event is subscribed to.
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
		///     Triggered whenever an item is no longer selected.
		///     WantsOnChange will be set to true when this event is subscribed to.
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
		///     Triggered whenever an item is expanded.
		///     Can be used for lazy loading.
		///     Will be triggered whenever a node with SupportsLazyLoading set to true is expanded.
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
		///     Triggered whenever an item is collapsed.
		///     Will be triggered whenever a node with SupportsLazyLoading set to true is collapsed.
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

		/// <summary>
		///     Gets or sets the top-level items in the tree view.
		///     The TreeViewItemOption.Item.ChildItems property can be used to navigate further down the tree.
		/// </summary>
		public IEnumerable<TreeViewItem<T>> Items
		{
			get
			{
				// Return the TreeViewItemOption wrappers based on the underlying TreeViewItems
				return BlockDefinition.TreeViewItems.Select(item => lookupTable.Values.FirstOrDefault(x => x.Item == item))
					.Where(x => x != null);
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value));
				}

				BlockDefinition.TreeViewItems = new List<TreeViewItem>(value.Select(x => x.Item));
				UpdateItemCache(value);
			}
		}

		/// <summary>
		///     Gets all items in the tree view that are selected.
		/// </summary>
		public IEnumerable<TreeViewItem<T>> CheckedItems
		{
			get
			{
				return GetCheckedItems();
			}
		}

		/// <summary>
		///     Gets all leaves (= items without children) in the tree view that are selected.
		/// </summary>
		public IEnumerable<TreeViewItem<T>> CheckedLeaves
		{
			get
			{
				return GetCheckedItems().Where(x => !x.ChildItems.Any());
			}
		}

		/// <summary>
		///     Gets all nodes (= items with children) in the tree view that are selected.
		/// </summary>
		public IEnumerable<TreeViewItem<T>> CheckedNodes
		{
			get
			{
				return GetCheckedItems().Where(x => x.ChildItems.Any());
			}
		}

		/// <summary>
		///     Gets the values of all items in the tree view that are selected.
		/// </summary>
		public IEnumerable<T> CheckedValues
		{
			get
			{
				return GetCheckedItems().Select(x => x.Value);
			}
		}

		/// <summary>
		///     Gets the values of all leaves in the tree view that are selected.
		/// </summary>
		public IEnumerable<T> CheckedLeafValues
		{
			get
			{
				return CheckedLeaves.Select(x => x.Value);
			}
		}

		/// <summary>
		///     Gets the values of all nodes in the tree view that are selected.
		/// </summary>
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

		/// <summary>
		///     Can be used to retrieve an item from the tree view based on its key value.
		/// </summary>
		/// <param name="key">Key used to search for the item.</param>
		/// <param name="item">Item in the tree that matches the provided key.</param>
		/// <returns>True if the item was found, otherwise false.</returns>
		public bool TryFindTreeViewItem(string key, out TreeViewItem<T> item)
		{
			return lookupTable.TryGetValue(key, out item);
		}

		/// <summary>
		///     This method is used to update the cached TreeViewItems and lookup table.
		///     This is done after loading the results from the UI Block, after handling the Events or when setting the Items.
		///     This method should only be called from outside the TreeView if you are checking or collapsing items from outside of the TreeView and need to access the CheckedItems or CollapsedItems.
		/// </summary>
		public override void UpdateItemCache()
		{
			// Don't pass Items here as it would cause recursion - just update the existing cache
			var newCheckedItemCache = new Dictionary<string, bool>();
			var newCollapsedItemCache = new Dictionary<string, bool>();
			
			// Ensure lookupTable exists
			if (lookupTable == null)
			{
				lookupTable = new Dictionary<string, TreeViewItem<T>>();
			}

			// Update caches based on current lookup table
			foreach (var item in lookupTable.Values)
			{
				try
				{
					newCheckedItemCache.Add(item.KeyValue, item.IsChecked);
					if (item.SupportsLazyLoading)
					{
						newCollapsedItemCache.Add(item.KeyValue, item.IsCollapsed);
					}
				}
				catch (Exception e)
				{
					throw new TreeViewDuplicateItemsException(item.KeyValue, e);
				}
			}

			// Replace caches atomically
			checkedItemCache = newCheckedItemCache;
			collapsedItemCache = newCollapsedItemCache;
		}

		/// <summary>
		///     Iterates over all items in the tree and returns them in a flat collection.
		/// </summary>
		/// <returns>A flat collection containing all items in the tree view.</returns>
		public IEnumerable<TreeViewItem<T>> GetAllItems()
		{
			return lookupTable.Values;
		}

		/// <summary>
		///     Returns all items in the tree view that are located at the provided depth.
		///     Whenever the requested depth is greater than the longest branch in the tree, an empty collection will be returned.
		/// </summary>
		/// <param name="depth">Depth of the requested items.</param>
		/// <returns>All items in the tree view that are located at the provided depth.</returns>
		public IEnumerable<TreeViewItem<T>> GetItems(int depth)
		{
			return GetItems(Items, depth, 0);
		}

		/// <summary>
		///     Load any changes made through user interaction.
		/// </summary>
		/// <param name="uiResults">
		///     Represents the information a user has entered or selected in a dialog box of an interactive
		///     Automation script.
		/// </param>
		/// <remarks><see cref="InteractiveWidget.DestVar" /> should be used as key to get the changes for this widget.</remarks>
		protected internal override void LoadResult(IUIResults uiResults)
		{
			var checkedItemKeys = uiResults.GetCheckedItemKeys(this);
			var expandedItemKeys = uiResults.GetExpandedItemKeys(this);

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
				changedItems = new List<TreeViewItem<T>>();

				foreach (string changedItemKey in changedItemKeys)
				{
					if (lookupTable.TryGetValue(changedItemKey, out var item))
					{
						changedItems.Add(item);
					}
				}
			}

			// Persist states
			foreach (TreeViewItem<T> item in lookupTable.Values)
			{
				item.Item.IsChecked = checkedItemKeys.Contains(item.KeyValue);
				item.Item.IsCollapsed = !expandedItemKeys.Contains(item.KeyValue);
			}

			UpdateItemCache();
		}

		/// <summary>
		///     Raises zero or more events of the widget.
		///     This method is called after <see cref="InteractiveWidget.LoadResult" /> was called on all widgets.
		/// </summary>
		/// <remarks>It is up to the implementer to determine if an event must be raised.</remarks>
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
		///     Returns all items in the TreeView that are checked.
		/// </summary>
		/// <returns>All checked TreeViewItems in the TreeView.</returns>
		private IEnumerable<TreeViewItem<T>> GetCheckedItems()
		{
			return lookupTable.Values.Where(x => x.ItemType == TreeViewItem.TreeViewItemType.CheckBox && x.IsChecked);
		}

		/// <summary>
		///     This method is used to recursively go through all the items in the TreeView.
		/// </summary>
		/// <param name="children">List of TreeViewItems to be visited.</param>
		/// <param name="parentOptions">Parent TreeViewItemOption wrappers.</param>
		/// <returns>Flat collection containing every item in the provided children collection and all underlying items.</returns>
		private IEnumerable<TreeViewItem<T>> GetAllItemsRecursive(IEnumerable<TreeViewItem> children, IEnumerable<TreeViewItem<T>> parentOptions)
		{
			List<TreeViewItem<T>> allItems = new List<TreeViewItem<T>>();
			foreach (var item in children)
			{
				var option = parentOptions.FirstOrDefault(x => x.Item == item);
				if (option != null)
				{
					allItems.Add(option);
					// For child items, we need to search in the lookup table
					var childOptions = option.ChildItems.Select(child => lookupTable.Values.FirstOrDefault(x => x.Item == child)).Where(x => x != null);
					allItems.AddRange(GetAllItemsRecursive(option.ChildItems, childOptions));
				}
			}

			return allItems;
		}

		/// <summary>
		///     Returns all TreeViewItems in the TreeView that are located on the provided depth.
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
					var childOptions = item.ChildItems.Select(child => lookupTable.Values.FirstOrDefault(x => x.Item == child)).Where(x => x != null);
					requestedItems.AddRange(GetItems(childOptions, requestedDepth, newDepth));
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
				expandedItems = new List<TreeViewItem<T>>();

				foreach (string newlyExpandedItemKey in newlyExpandedItems)
				{
					if (lookupTable.TryGetValue(newlyExpandedItemKey, out var item))
					{
						expandedItems.Add(item);
					}
				}
			}
		}

		private void RegisterCollapsedItems(IEnumerable<string> expandedItemKeys)
		{
			List<string> newlyCollapsedItems = collapsedItemCache.Where(x => !expandedItemKeys.Contains(x.Key) && !x.Value).Select(x => x.Key).ToList();
			if (newlyCollapsedItems.Any() && OnCollapsed != null)
			{
				itemsCollapsed = true;
				collapsedItems = new List<TreeViewItem<T>>();

				foreach (string newlyCollapsedItemKey in newlyCollapsedItems)
				{
					if (lookupTable.TryGetValue(newlyCollapsedItemKey, out var item))
					{
						collapsedItems.Add(item);
					}
				}
			}
		}

		private List<string> RegisterCheckedItems(IEnumerable<string> checkedItemKeys)
		{
			List<string> newlyCheckedItemKeys = checkedItemCache.Where(x => checkedItemKeys.Contains(x.Key) && !x.Value).Select(x => x.Key).ToList();
			if (newlyCheckedItemKeys.Any() && OnChecked != null)
			{
				itemsChecked = true;
				checkedItems = new List<TreeViewItem<T>>();

				foreach (string newlyCheckedItemKey in newlyCheckedItemKeys)
				{
					if (lookupTable.TryGetValue(newlyCheckedItemKey, out var item))
					{
						checkedItems.Add(item);
					}
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
				uncheckedItems = new List<TreeViewItem<T>>();

				foreach (string newlyUncheckedItemKey in newlyUncheckedItemKeys)
				{
					if (lookupTable.TryGetValue(newlyUncheckedItemKey, out var item))
					{
						uncheckedItems.Add(item);
					}
				}
			}

			return newlyUncheckedItemKeys;
		}

		private void UpdateItemCache(IEnumerable<TreeViewItem<T>> items)
		{
			// Initialize new caches but preserve the existing lookup table if items is null or empty
			var newCheckedItemCache = new Dictionary<string, bool>();
			var newCollapsedItemCache = new Dictionary<string, bool>();
			
			// Only rebuild lookup table if we have new items to process
			if (items != null && items.Any())
			{
				lookupTable = new Dictionary<string, TreeViewItem<T>>();
				BuildLookupTable(items);
			}
			else if (lookupTable == null)
			{
				// Initialize empty lookup table only if it doesn't exist
				lookupTable = new Dictionary<string, TreeViewItem<T>>();
			}

			// Update caches based on current lookup table
			foreach (var item in lookupTable.Values)
			{
				try
				{
					newCheckedItemCache.Add(item.KeyValue, item.IsChecked);
					if (item.SupportsLazyLoading)
					{
						newCollapsedItemCache.Add(item.KeyValue, item.IsCollapsed);
					}
				}
				catch (Exception e)
				{
					throw new TreeViewDuplicateItemsException(item.KeyValue, e);
				}
			}

			// Replace caches atomically
			checkedItemCache = newCheckedItemCache;
			collapsedItemCache = newCollapsedItemCache;
		}

		private void BuildLookupTable(IEnumerable<TreeViewItem<T>> items)
		{
			foreach (var item in items)
			{
				lookupTable[item.KeyValue] = item;
				
				// Recursively process children
				var childOptions = item.ChildItems.Select(child =>
				{
					// Try to find existing wrapper or create a temporary one
					var existing = lookupTable.Values.FirstOrDefault(x => x.Item == child);
					return existing;
				}).Where(x => x != null);

				if (item.ChildItems.Any())
				{
					// For children that don't have wrappers yet, we need to handle them
					// This is a limitation - child items added directly to TreeViewItem won't have metadata
					BuildLookupTableFromTreeViewItems(item.ChildItems);
				}
			}
		}

		private void BuildLookupTableFromTreeViewItems(IEnumerable<TreeViewItem> items)
		{
			foreach (var item in items)
			{
				if (!lookupTable.ContainsKey(item.KeyValue))
				{
					// Create a wrapper with default value for items without explicit metadata
					var wrapper = new TreeViewItem<T>(item, default(T));
					lookupTable[item.KeyValue] = wrapper;
				}

				BuildLookupTableFromTreeViewItems(item.ChildItems);
			}
		}
	}
}
