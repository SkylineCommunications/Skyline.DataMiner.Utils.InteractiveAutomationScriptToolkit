namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System.Collections.Generic;

	using Skyline.DataMiner.Net.AutomationUI.Objects;

	/// <summary>
	/// Defines a tree view widget with basic operations.
	/// </summary>
	public interface ITreeView : ITreeViewBase
	{
		/// <summary>
		/// Gets or sets the top-level items in the tree view.
		/// The TreeViewItem.ChildItems property can be used to navigate further down the tree.
		/// </summary>
		IEnumerable<TreeViewItem> Items { get; set; }

		/// <summary>
		/// Gets all items in the tree view that are selected.
		/// </summary>
		IEnumerable<TreeViewItem> CheckedItems { get; }

		/// <summary>
		/// Gets all leaves (= items without children) in the tree view that are selected.
		/// </summary>
		IEnumerable<TreeViewItem> CheckedLeaves { get; }

		/// <summary>
		/// Gets all nodes (= items with children) in the tree view that are selected.
		/// </summary>
		IEnumerable<TreeViewItem> CheckedNodes { get; }

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

		/// <summary>
		/// Can be used to retrieve an item from the tree view based on its key value.
		/// </summary>
		/// <param name="key">Key used to search for the item.</param>
		/// <param name="item">Item in the tree that matches the provided key.</param>
		/// <returns>True if the item was found, otherwise false.</returns>
		bool TryFindTreeViewItem(string key, out TreeViewItem item);
	}

	/// <summary>
	/// Defines a generic tree view widget with support for typed treeview items.
	/// </summary>
	/// <typeparam name="T">The type of the value associated with each treeview item.</typeparam>
	public interface ITreeView<T> : ITreeViewBase
	{
		/// <summary>
		/// Gets or sets the top-level items in the tree view.
		/// The TreeViewItem.ChildItems property can be used to navigate further down the tree.
		/// </summary>
		IEnumerable<TreeViewItem<T>> Items { get; set; }

		/// <summary>
		/// Gets all items in the tree view that are selected.
		/// </summary>
		IEnumerable<TreeViewItem<T>> CheckedItems { get; }

		/// <summary>
		/// Gets all leaves (= items without children) in the tree view that are selected.
		/// </summary>
		IEnumerable<TreeViewItem<T>> CheckedLeaves { get; }

		/// <summary>
		/// Gets all nodes (= items with children) in the tree view that are selected.
		/// </summary>
		IEnumerable<TreeViewItem<T>> CheckedNodes { get; }

		/// <summary>
		/// Iterates over all items in the tree and returns them in a flat collection.
		/// </summary>
		/// <returns>A flat collection containing all items in the tree view.</returns>
		IEnumerable<TreeViewItem<T>> GetAllItems();

		/// <summary>
		/// Returns all items in the tree view that are located at the provided depth.
		/// Whenever the requested depth is greater than the longest branch in the tree, an empty collection will be returned.
		/// </summary>
		/// <param name="depth">Depth of the requested items.</param>
		/// <returns>All items in the tree view that are located at the provided depth.</returns>
		IEnumerable<TreeViewItem<T>> GetItems(int depth);

		/// <summary>
		/// Can be used to retrieve an item from the tree view based on its key value.
		/// </summary>
		/// <param name="key">Key used to search for the item.</param>
		/// <param name="item">Item in the tree that matches the provided key.</param>
		/// <returns>True if the item was found, otherwise false.</returns>
		bool TryFindTreeViewItem(string key, out TreeViewItem<T> item);
	}
}