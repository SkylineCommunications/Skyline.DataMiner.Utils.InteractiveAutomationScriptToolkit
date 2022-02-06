namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;

	using Net.AutomationUI.Objects;

	public interface ITreeView : IInteractiveWidget
	{
		/// <summary>
		/// Returns all items in the tree view that are selected.
		/// </summary>
		IEnumerable<TreeViewItem> CheckedItems { get; }

		/// <summary>
		/// Returns the top-level items in the tree view.
		/// The TreeViewItem.ChildItems property can be used to navigate further down the tree.
		/// </summary>
		IEnumerable<TreeViewItem> Items { get; set; }

		/// <summary>
		/// Returns all leaves (= items without children) in the tree view that are selected.
		/// </summary>
		IEnumerable<TreeViewItem> CheckedLeaves { get; }

		/// <summary>
		/// Returns all nodes (= items with children) in the tree view that are selected.
		/// </summary>
		IEnumerable<TreeViewItem> CheckedNodes { get; }

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		/// <exception cref="ArgumentNullException">When the value is <c>null</c>.</exception>
		string Tooltip { get; set; }

		/// <summary>
		///     Triggered when a different item is selected or no longer selected.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		event EventHandler<IEnumerable<TreeViewItem>> Changed;

		/// <summary>
		///  Triggered whenever an item is selected.
		///  WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		event EventHandler<IEnumerable<TreeViewItem>> Checked;

		/// <summary>
		///  Triggered whenever an item is no longer selected.
		///  WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		event EventHandler<IEnumerable<TreeViewItem>> Unchecked;

		/// <summary>
		///  Triggered whenever an item is expanded.
		///  Can be used for lazy loading.
		///  Will be triggered whenever a node with SupportsLazyLoading set to true is expanded.
		/// </summary>
		event EventHandler<IEnumerable<TreeViewItem>> Expanded;

		/// <summary>
		///  Triggered whenever an item is collapsed.
		///  Will be triggered whenever a node with SupportsLazyLoading set to true is collapsed.
		/// </summary>
		event EventHandler<IEnumerable<TreeViewItem>> Collapsed;

		/// <summary>
		/// Sets the IsCollapsed state for all items in the tree view to true, causing the entire tree view to be collapsed.
		/// </summary>
		void Collapse();

		/// <summary>
		/// Sets the IsCollapsed state for all items in the tree view to false, causing the entire tree view to be expanded.
		/// </summary>
		void Expand();

		/// <summary>
		/// Can be used to retrieve an item from the tree view based on its key value.
		/// </summary>
		/// <param name="key">Key used to search for the item.</param>
		/// <param name="item">Item in the tree that matches the provided key.</param>
		/// <returns>True if the item was found, otherwise false.</returns>
		bool TryFindTreeViewItem(string key, out TreeViewItem item);

		/// <summary>
		/// Iterates over all items in the tree and returns them in a flat collection.
		/// </summary>
		/// <returns>A flat collection containing all items in the tree view.</returns>
		IEnumerable<TreeViewItem> GetAllItems();

		/// <summary>
		/// Returns all items in the tree view that are located at the provided depth.
		/// Whenever the requested depth is greater than the longest branch in the tree, an empty collection will be returned.
		/// </summary>
		/// <param name="depth">Depth of the requested items.</param>
		/// <returns>All items in the tree view that are located at the provided depth.</returns>
		IEnumerable<TreeViewItem> GetItems(int depth);
	}
}