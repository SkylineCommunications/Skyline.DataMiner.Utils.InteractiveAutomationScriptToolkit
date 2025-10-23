namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	/// <summary>
	/// Defines the base functionality for a treeview widget.
	/// </summary>
	public interface ITreeViewBase : IIsReadonlyWidget
	{
		/// <summary>
		/// Gets or sets the tooltip text associated with the treeview.
		/// </summary>
		string Tooltip { get; set; }

		/// <summary>
		/// Sets the IsCollapsed state for all items in the tree view to false, causing the entire tree view to be expanded.
		/// </summary>
		void Expand();

		/// <summary>
		/// Sets the IsCollapsed state for all items in the tree view to true, causing the entire tree view to be collapsed.
		/// </summary>
		void Collapse();

		/// <summary>
		/// This method is used to update the cached TreeViewItems and lookup table.
		/// This is done after loading the results from the UI Block, after handling the Events or when setting the Items.
		/// This method should only be called from outside the TreeView if you are checking or collapsing items from outside of the TreeView and need to access the CheckedItems or CollapsedItems.
		/// </summary>
		void UpdateItemCache();
	}
}