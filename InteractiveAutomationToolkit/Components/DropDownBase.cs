namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using Skyline.DataMiner.Automation;

	public abstract class DropDownBase : InteractiveWidget, IDropDownBase
	{
		protected DropDownBase()
		{
			Type = UIBlockType.DropDown;
			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
			IsReadOnly = false;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
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
		/// <inheritdoc/>
		/// </summary>
		public UIValidationState ValidationState
		{
			get
			{
				return BlockDefinition.ValidationState;
			}

			set
			{
				BlockDefinition.ValidationState = value;
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public string ValidationText
		{
			get
			{
				return BlockDefinition.ValidationText;
			}

			set
			{
				BlockDefinition.ValidationText = value;
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public bool IsDisplayFilterShown
		{
			get
			{
				return BlockDefinition.DisplayFilter;
			}

			set
			{
				BlockDefinition.DisplayFilter = value;
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
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
		/// <inheritdoc/>
		/// </summary>
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
