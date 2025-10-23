namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;

	using Skyline.DataMiner.Automation;

	public abstract class TreeViewBase : InteractiveWidget, ITreeViewBase
	{
		protected TreeViewBase()
		{
			Type = UIBlockType.TreeView;
			IsReadOnly = false;
		}

		/// <inheritdoc/>
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

		/// <inheritdoc/>
		public abstract void Collapse();

		/// <inheritdoc/>
		public abstract void Expand();

		/// <inheritdoc/>
		public abstract void UpdateItemCache();
	}
}
