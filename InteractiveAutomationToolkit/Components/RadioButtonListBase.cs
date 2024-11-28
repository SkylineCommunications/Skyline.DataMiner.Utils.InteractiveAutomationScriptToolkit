namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using Skyline.DataMiner.Automation;

	public abstract class RadioButtonListBase : InteractiveWidget, IRadioButtonListBase
	{
		protected RadioButtonListBase()
		{
			Type = UIBlockType.RadioButtonList;
			IsReadOnly = false;
		}

		/// <summary>
		///     Gets or sets a value indicating whether the options are sorted naturally.
		/// </summary>
		/// <remarks>Available from DataMiner 9.5.6 onwards.</remarks>
		public bool IsSorted
		{
			get
			{
				return BlockDefinition.IsSorted;
			}

			set
			{
				BlockDefinition.IsSorted = value;
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
					throw new ArgumentNullException("value");
				}

				BlockDefinition.TooltipText = value;
			}
		}

		/// <summary>
		///		Gets or sets a value indicating whether the control is displayed in read-only mode.
		///		Read-only mode causes the widgets to appear read-write but the user won't be able to change their value.
		///		This only affects interactive scripts running in a web environment.
		/// </summary>
		/// <remarks>Available from DataMiner 10.4.1 onwards.</remarks>
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
	}
}
