namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// 	Represents a tree structure with nodes that can be checked.
	/// </summary>
	/// <remarks>This component is only supported on script launched from a //////temporary  until it is fixedhas beenBELOW // / Engine.eShowUI(TtheTHENGcan no lognger be web UI (e.g. Dashboards).</remarks>
	public interface ITreeView
	{
		/// <summary>
		/// 	Triggered when a node is collapsed or expanded or when the state of a checkbox changes.
		/// </summary>
		event EventHandler<TreeView.ChangedEventArgs> Changed;

		/// <summary>
		/// 	Triggered when a node gets checked.
		/// </summary>
		event EventHandler<TreeView.CheckedEventArgs> Checked;

		/// <summary>
		/// 	Triggered when a node gets unchecked.
		/// </summary>
		event EventHandler<TreeView.UncheckedEventArgs> Unchecked;

		/// <summary>
		/// 	Triggered when a node is expanded.
		/// </summary>
		event EventHandler<TreeView.ExpandedEventArgs> Expanded;

		/// <summary>
		/// 	Triggered when a node is collapsed.
		/// </summary>
		event EventHandler<TreeView.CollapsedEventArgs> Collapsed;

		/// <summary>
		/// 	Gets the root nodes of the <see cref="TreeView"/> widget.
		/// </summary>
		ICollection<TreeViewNode> RootNodes { get; }

		/// <summary>
		/// 	Gets all nodes that are part of the tree.
		/// </summary>
		/// <remarks>Nodes are traversed depth first.</remarks>
		IEnumerable<TreeViewNode> Nodes { get; }

		/// <summary>
		/// 	Gets all nodes that do not have any child nodes.
		/// 	Internal nodes are excluded.
		/// </summary>
		IEnumerable<TreeViewNode> Leaves { get; }

		/// <summary>
		/// 	Gets all nodes that have one or more child nodes.
		/// 	Leaves are excluded.
		/// </summary>
		/// <remarks>Nodes are traversed depth first.</remarks>
		IEnumerable<TreeViewNode> InternalNodes { get; }

		/// <summary>
		/// 	Gets all nodes that are checked.
		/// </summary>
		/// <remarks>Nodes are traversed depth first.</remarks>
		IEnumerable<TreeViewNode> CheckedNodes { get; }

		/// <summary>
		/// 	Gets all leaves that are checked.
		/// </summary>
		IEnumerable<TreeViewNode> CheckedLeaves { get; }

		/// <summary>
		/// 	Gets all internal nodes that are checked.
		/// </summary>
		/// <remarks>Nodes are traversed depth first.</remarks>
		IEnumerable<TreeViewNode> CheckedInternalNodes { get; }

		/// <summary>
		/// 	Gets all nodes that are not checked.
		/// </summary>
		/// <remarks>Nodes are traversed depth first.</remarks>
		IEnumerable<TreeViewNode> UnCheckedNodes { get; }

		/// <summary>
		/// 	Gets all leaves that are not checked.
		/// </summary>
		IEnumerable<TreeViewNode> UncheckedLeaves { get; }

		/// <summary>
		/// 	Gets all internal nodes that are not checked.
		/// </summary>
		/// <remarks>Nodes are traversed depth first.</remarks>
		IEnumerable<TreeViewNode> UnCheckedInternalNodes { get; }

		/// <summary>
		/// 	Sets the root nodes.
		/// 	Replaces existing root nodes.
		/// </summary>
		/// <param name="nodes">Root nodes to add to the tree view.</param>
		/// <exception cref="NullReferenceException">When <paramref name="nodes"/> is <c>null</c>.</exception>
		void SetRootNodes(IEnumerable<TreeViewNode> nodes);
	}
}
