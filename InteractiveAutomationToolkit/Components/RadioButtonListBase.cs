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

		/// <inheritdoc/>
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
