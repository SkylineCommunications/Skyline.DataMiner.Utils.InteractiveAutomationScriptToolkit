namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Net.AutomationUI.Objects;

	/// <summary>
	///  A Tree view structure.
	/// </summary>
	public class TreeView : InteractiveWidget
	{
		private Dictionary<string, bool> checkedItemCache;
		private Dictionary<string, bool> collapsedItemCache;
		private Dictionary<string, TreeViewItem> lookupTable;

		private bool itemsChanged = false;
		private List<TreeViewItem> changedItems = new List<TreeViewItem>();

		private bool itemsChecked = false;
		private List<TreeViewItem> checkedItems = new List<TreeViewItem>();

		private bool itemsUnchecked = false;
		private List<TreeViewItem> uncheckedItems = new List<TreeViewItem>();

		private bool itemsExpanded = false;
		private List<TreeViewItem> expandedItems = new List<TreeViewItem>();

		/// <summary>
		///		Initializes a new instance of the <see cref="TreeView" /> class.
		/// </summary>
		/// <param name="treeViewItems"></param>
		public TreeView(IEnumerable<TreeViewItem> treeViewItems)
		{
			Type = UIBlockType.TreeView;
			Items = treeViewItems;
		}

		/// <summary>
		///     Event triggers when a different items are checked or unchecked.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<IEnumerable<TreeViewItem>> Changed
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

		private event EventHandler<IEnumerable<TreeViewItem>> OnChanged;

		/// <summary>
		///  Event triggers whenever an item is checked.
		///  WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<IEnumerable<TreeViewItem>> Checked
		{
			add
			{
				OnChecked += value;
				WantsOnChange = true;
			}

			remove
			{
				OnChecked -= value;
				if (OnChecked == null || !OnChecked.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<IEnumerable<TreeViewItem>> OnChecked;

		/// <summary>
		///  Event triggers whenever an item is unchecked.
		///  WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<IEnumerable<TreeViewItem>> Unchecked
		{
			add
			{
				OnUnchecked += value;
				WantsOnChange = true;
			}

			remove
			{
				OnUnchecked -= value;
				if (OnUnchecked == null || !OnUnchecked.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<IEnumerable<TreeViewItem>> OnUnchecked;

		/// <summary>
		///  Event triggers whenever an Item is expanded.
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

		private event EventHandler<IEnumerable<TreeViewItem>> OnExpanded;

		/// <summary>
		/// This will set the IsCollapsed state for all items in the TreeView to true, causing the entire TreeView to be collapsed.
		/// </summary>
		public void Collapse()
		{
			foreach(var item in GetAllItems())
			{
				item.IsCollapsed = true;
			}
		}

		/// <summary>
		/// This will set the IsCollapsed state for all items in the TreeView to false, causing the entire TreeView to be expanded.
		/// </summary>
		public void Expand()
		{
			foreach(var item in GetAllItems())
			{
				item.IsCollapsed = false;
			}
		}

		/// <summary>
		/// Returns the top level items in the TreeView.
		/// The TreeViewItem.ChildItems property can be used to navigate further down the tree.
		/// </summary>
		public IEnumerable<TreeViewItem> Items
		{
			get
			{
				return BlockDefinition.TreeViewItems;
			}

			set
			{
				if (value == null) throw new ArgumentNullException("value");
				BlockDefinition.TreeViewItems = new List<TreeViewItem>(value);
			}
		}

		/// <summary>
		/// Returns all items in the TreeView that are checked.
		/// </summary>
		public IEnumerable<TreeViewItem> CheckedItems
		{
			get
			{
				return GetCheckedItems();
			}
		}

		/// <summary>
		/// Returns all leaves (= items without children) in the TreeView that are checked.
		/// </summary>
		public IEnumerable<TreeViewItem> CheckedLeaves
		{
			get
			{
				return GetCheckedItems().Where(x => !x.ChildItems.Any());
			}
		}

		/// <summary>
		/// Returns all nodes (= items with children) in the TreeView that are checked.
		/// </summary>
		public IEnumerable<TreeViewItem> CheckedNodes
		{
			get
			{
				return GetCheckedItems().Where(x => x.ChildItems.Any());
			}
		}

		/// <summary>
		/// This method can be used to retrieve a TreeViewItem from the TreeView based on its KeyValue.
		/// </summary>
		/// <param name="key">Key used to search for the item.</param>
		/// <param name="item">Item in the Tree that matches the provided key.</param>
		/// <returns>True if the item was found, else false.</returns>
		public bool TryFindTreeViewItem(string key, out TreeViewItem item)
		{
			item = GetAllItems().FirstOrDefault(x => x.KeyValue.Equals(key));
			return item != null;
		}

		/// <summary>
		/// This method is used to update the cached TreeViewItems and lookup table.
		/// </summary>
		internal void UpdateItemCache()
		{
			checkedItemCache = new Dictionary<string, bool>();
			collapsedItemCache = new Dictionary<string, bool>();
			lookupTable = new Dictionary<string, TreeViewItem>();

			foreach (var item in GetAllItems())
			{
				try
				{
					checkedItemCache.Add(item.KeyValue, item.IsChecked);
					collapsedItemCache.Add(item.KeyValue, item.IsCollapsed);
					lookupTable.Add(item.KeyValue, item);
				}
				catch(Exception e)
				{
					throw new TreeViewDuplicateItemsException(item.KeyValue, e);
				}
			}
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
		/// This method will iterate over all items in the tree and return them in a flat collection.
		/// </summary>
		/// <returns>A flat collection containing all TreeViewItems in the TreeView.</returns>
		public IEnumerable<TreeViewItem> GetAllItems()
		{
			List<TreeViewItem> allItems = new List<TreeViewItem>();
			foreach(var item in Items)
			{
				allItems.Add(item);
				allItems.AddRange(GetAllItems(item.ChildItems));
			}

			return allItems;
		}

		/// <summary>
		/// This method is used to recursively go through all the items in the TreeView.
		/// </summary>
		/// <param name="children">List of TreeViewItems to be visited.</param>
		/// <returns>Flat collection containing every item in the provided children collection and all underlying items.</returns>
		private IEnumerable<TreeViewItem> GetAllItems(IEnumerable<TreeViewItem> children)
		{
			List<TreeViewItem> allItems = new List<TreeViewItem>();
			foreach(var item in children)
			{
				allItems.Add(item);
				allItems.AddRange(GetAllItems(item.ChildItems));
			}

			return allItems;
		}

		/// <summary>
		/// Returns all TreeViewItems in the TreeView that are located on the provided depth.
		/// Whenever the requested depth is greater than the longest branch in the tree, an empty collection will be returned.
		/// </summary>
		/// <param name="depth">Depth of the requested items.</param>
		/// <returns>All TreeViewItems in the TreeView that are located on the provided depth.</returns>
		public IEnumerable<TreeViewItem> GetItems(int depth)
		{
			return GetItems(Items, depth, 0);
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

		/// <summary>
		///     Gets or sets the Tooltip.
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
					throw new ArgumentNullException("value");
				}

				BlockDefinition.TooltipText = value;
			}
		}

		internal override void LoadResult(UIResults uiResults)
		{
			var checkedItemKeys = uiResults.GetCheckedItemKeys(this); // this includes all checked items
			var expandedItemKeys = uiResults.GetExpandedItemKeys(this); // this SHOULD include all expanded items

			// Check for changes
			// Expanded Items
			if (expandedItemKeys.Any() && OnExpanded != null)
			{
				itemsExpanded = true;
				expandedItems = new List<TreeViewItem>();

				foreach (string expandedItemKey in expandedItemKeys)
				{
					expandedItems.Add(lookupTable[expandedItemKey]);
				}
			}

			// Checked Items
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
			
			// Unchecked Items
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

			// Changed Items
			List<string> changedItemKeys = new List<string>();
			changedItemKeys.AddRange(newlyCheckedItemKeys);
			changedItemKeys.AddRange(newlyUncheckedItemKeys);
			if(changedItemKeys.Any() && OnChanged != null)
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
				item.IsCollapsed = !expandedItemKeys.Contains(item.KeyValue) && collapsedItemCache[item.KeyValue];
			}

			UpdateItemCache();
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			// Expanded items
			if (itemsExpanded && OnExpanded != null) OnExpanded(this, expandedItems);

			// Checked items
			if (itemsChecked && OnChecked != null) OnChecked(this, checkedItems);

			// Unchecked items
			if (itemsUnchecked && OnUnchecked != null) OnUnchecked(this, uncheckedItems);

			// Changed items
			if (itemsChanged && OnChanged != null) OnChanged(this, changedItems);

			itemsExpanded = false;
			itemsChecked = false;
			itemsUnchecked = false;
			itemsChanged = false;

			UpdateItemCache();
		}
	}
}
