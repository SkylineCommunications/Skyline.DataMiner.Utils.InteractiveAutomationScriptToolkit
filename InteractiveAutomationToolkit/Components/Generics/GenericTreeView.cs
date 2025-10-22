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
	public class TreeView<T> : InteractiveWidget, IIsReadonlyWidget
	{
		private Dictionary<string, bool> checkedItemCache;
		private Dictionary<string, bool> collapsedItemCache;
		private Dictionary<string, TreeViewItemOption<T>> lookupTable;

		private bool itemsChanged = false;
		private List<TreeViewItemOption<T>> changedItems = new List<TreeViewItemOption<T>>();

		private bool itemsChecked = false;
		private List<TreeViewItemOption<T>> checkedItems = new List<TreeViewItemOption<T>>();

		private bool itemsUnchecked = false;
		private List<TreeViewItemOption<T>> uncheckedItems = new List<TreeViewItemOption<T>>();

		private bool itemsExpanded = false;
		private List<TreeViewItemOption<T>> expandedItems = new List<TreeViewItemOption<T>>();

		private bool itemsCollapsed = false;
		private List<TreeViewItemOption<T>> collapsedItems = new List<TreeViewItemOption<T>>();

		/// <summary>
		///     Initializes a new instance of the <see cref="TreeView{T}" /> class.
		/// </summary>
		public TreeView() : this(Enumerable.Empty<TreeViewItemOption<T>>())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="TreeView{T}" /> class.
		/// </summary>
		/// <param name="treeViewItems">Root nodes of the tree view.</param>
		public TreeView(IEnumerable<TreeViewItemOption<T>> treeViewItems)
		{
			Type = UIBlockType.TreeView;
			Items = treeViewItems;
			IsReadOnly = false;
		}

		/// <summary>
		///     Triggered when a different item is selected or no longer selected.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<IEnumerable<TreeViewItemOption<T>>> Changed
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
		public event EventHandler<IEnumerable<TreeViewItemOption<T>>> Checked
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
		public event EventHandler<IEnumerable<TreeViewItemOption<T>>> Unchecked
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
		public event EventHandler<IEnumerable<TreeViewItemOption<T>>> Expanded
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
		public event EventHandler<IEnumerable<TreeViewItemOption<T>>> Collapsed
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

		private event EventHandler<IEnumerable<TreeViewItemOption<T>>> OnChanged;

		private event EventHandler<IEnumerable<TreeViewItemOption<T>>> OnChecked;

		private event EventHandler<IEnumerable<TreeViewItemOption<T>>> OnUnchecked;

		private event EventHandler<IEnumerable<TreeViewItemOption<T>>> OnExpanded;

		private event EventHandler<IEnumerable<TreeViewItemOption<T>>> OnCollapsed;

		/// <summary>
		///     Gets or sets the top-level items in the tree view.
		///     The TreeViewItemOption.Item.ChildItems property can be used to navigate further down the tree.
		/// </summary>
		public IEnumerable<TreeViewItemOption<T>> Items
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
		public IEnumerable<TreeViewItemOption<T>> CheckedItems
		{
			get
			{
				return GetCheckedItems();
			}
		}

		/// <summary>
		///     Gets all leaves (= items without children) in the tree view that are selected.
		/// </summary>
		public IEnumerable<TreeViewItemOption<T>> CheckedLeaves
		{
			get
			{
				return GetCheckedItems().Where(x => !x.ChildItems.Any());
			}
		}

		/// <summary>
		///     Gets all nodes (= items with children) in the tree view that are selected.
		/// </summary>
		public IEnumerable<TreeViewItemOption<T>> CheckedNodes
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

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		/// <exception cref="ArgumentNullException">When the value is <c>null</c>.</exception>
		public string Tooltip
		{
			get
			{
				return BlockDefinition.TooltipText;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value));
				}

				BlockDefinition.TooltipText = value;
			}
		}

		/// <inheritdoc/>
		public virtual bool IsReadOnly
		{
			get
			{
				return BlockDefinition.IsReadOnly;
			}

			set
			{
				BlockDefinition.IsReadOnly = value;
			}
		}

		/// <summary>
		///     Sets the IsCollapsed state for all items in the tree view to true, causing the entire tree view to be collapsed.
		/// </summary>
		public void Collapse()
		{
			foreach (var item in GetAllItems())
			{
				item.IsCollapsed = true;
			}
		}

		/// <summary>
		///     Sets the IsCollapsed state for all items in the tree view to false, causing the entire tree view to be expanded.
		/// </summary>
		public void Expand()
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
		public bool TryFindTreeViewItem(string key, out TreeViewItemOption<T> item)
		{
			return lookupTable.TryGetValue(key, out item);
		}

		/// <summary>
		///     This method is used to update the cached TreeViewItems and lookup table.
		///     This is done after loading the results from the UI Block, after handling the Events or when setting the Items.
		///     This method should only be called from outside the TreeView if you are checking or collapsing items from outside of the TreeView and need to access the CheckedItems or CollapsedItems.
		/// </summary>
		public void UpdateItemCache()
		{
			UpdateItemCache(Items);
		}

		/// <summary>
		///     Iterates over all items in the tree and returns them in a flat collection.
		/// </summary>
		/// <returns>A flat collection containing all items in the tree view.</returns>
		public IEnumerable<TreeViewItemOption<T>> GetAllItems()
		{
			return lookupTable.Values;
		}

		/// <summary>
		///     Returns all items in the tree view that are located at the provided depth.
		///     Whenever the requested depth is greater than the longest branch in the tree, an empty collection will be returned.
		/// </summary>
		/// <param name="depth">Depth of the requested items.</param>
		/// <returns>All items in the tree view that are located at the provided depth.</returns>
		public IEnumerable<TreeViewItemOption<T>> GetItems(int depth)
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
				changedItems = new List<TreeViewItemOption<T>>();

				foreach (string changedItemKey in changedItemKeys)
				{
					if (lookupTable.TryGetValue(changedItemKey, out var item))
					{
						changedItems.Add(item);
					}
				}
			}

			// Persist states
			foreach (TreeViewItemOption<T> item in lookupTable.Values)
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
		private IEnumerable<TreeViewItemOption<T>> GetCheckedItems()
		{
			return lookupTable.Values.Where(x => x.ItemType == TreeViewItem.TreeViewItemType.CheckBox && x.IsChecked);
		}

		/// <summary>
		///     This method is used to recursively go through all the items in the TreeView.
		/// </summary>
		/// <param name="children">List of TreeViewItems to be visited.</param>
		/// <param name="parentOptions">Parent TreeViewItemOption wrappers.</param>
		/// <returns>Flat collection containing every item in the provided children collection and all underlying items.</returns>
		private IEnumerable<TreeViewItemOption<T>> GetAllItemsRecursive(IEnumerable<TreeViewItem> children, IEnumerable<TreeViewItemOption<T>> parentOptions)
		{
			List<TreeViewItemOption<T>> allItems = new List<TreeViewItemOption<T>>();
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
		private IEnumerable<TreeViewItemOption<T>> GetItems(IEnumerable<TreeViewItemOption<T>> children, int requestedDepth, int currentDepth)
		{
			List<TreeViewItemOption<T>> requestedItems = new List<TreeViewItemOption<T>>();
			bool depthReached = requestedDepth == currentDepth;
			foreach (TreeViewItemOption<T> item in children)
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
				expandedItems = new List<TreeViewItemOption<T>>();

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
				collapsedItems = new List<TreeViewItemOption<T>>();

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
				checkedItems = new List<TreeViewItemOption<T>>();

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
				uncheckedItems = new List<TreeViewItemOption<T>>();

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

		private void UpdateItemCache(IEnumerable<TreeViewItemOption<T>> items)
		{
			checkedItemCache = new Dictionary<string, bool>();
			collapsedItemCache = new Dictionary<string, bool>();
			lookupTable = new Dictionary<string, TreeViewItemOption<T>>();

			BuildLookupTable(items);

			foreach (var item in lookupTable.Values)
			{
				try
				{
					checkedItemCache.Add(item.KeyValue, item.IsChecked);
					if (item.SupportsLazyLoading)
					{
						collapsedItemCache.Add(item.KeyValue, item.IsCollapsed);
					}
				}
				catch (Exception e)
				{
					throw new TreeViewDuplicateItemsException(item.KeyValue, e);
				}
			}
		}

		private void BuildLookupTable(IEnumerable<TreeViewItemOption<T>> items)
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
					var wrapper = new TreeViewItemOption<T>(item, default(T));
					lookupTable[item.KeyValue] = wrapper;
				}

				BuildLookupTableFromTreeViewItems(item.ChildItems);
			}
		}
	}
}
