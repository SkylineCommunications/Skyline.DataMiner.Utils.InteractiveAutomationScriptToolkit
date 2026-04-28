namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Linq;

	using Microsoft.Extensions.Logging;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript.Extensions;

	public abstract class DropDownBase : InteractiveWidget, IDropDownBase
	{
		private string previousFilterValue = String.Empty;
		private string filterValue = String.Empty;
		private bool filterValueChanged;

		protected DropDownBase()
		{
			Type = UIBlockType.DropDown;
			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
			IsDisplayFilterShown = true;
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
		/// <remarks>Available from DataMiner 10.0.5 onwards.</remarks>
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

		/// <inheritdoc/>
		/// <remarks>Available from DataMiner 10.0.5 onwards.</remarks>
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

		/// <inheritdoc/>
		/// <remarks>Available from DataMiner 9.5.6 onwards.</remarks>
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

		/// <summary>
		///     Triggered when the filter value changes.
		///     WantsOnFilter will be set to true when this event is subscribed to.
		/// </summary>
		/// <remarks>
		///		Don't update the <see cref="DropDown.Selected"/> property from this event, this event should only be used to filter the available options.
		///		This requires the use of WebUI Components V2 (default from 10.6.1).
		///		Add useNewIASInputComponents=true to the LCA URL (available from DM 10.4.0).
		///		Set IEngine.WebUIVersion = WebUIVersion.V2 (available from DM 10.5.12).
		/// </remarks>
		public event EventHandler<DropDownFilterChangedEventArgs> FilterChanged
		{
			add
			{
				OnFilterChanged += value;
				BlockDefinition.WantsOnFilter = true;
			}

			remove
			{
				OnFilterChanged -= value;
				if (OnFilterChanged == null || !OnFilterChanged.GetInvocationList().Any())
				{
					BlockDefinition.WantsOnFilter = false;
				}
			}
		}

		private event EventHandler<DropDownFilterChangedEventArgs> OnFilterChanged;

		/// <inheritdoc	/>
		protected internal override void LoadResult(IUIResults uiResults, ILogger logger = null)
		{
			string newFilterValue = uiResults.GetFilterString(this) ?? String.Empty;

			logger?.Debug(nameof(DropDown), nameof(LoadResult), $"Filter value: {newFilterValue}");

			if (BlockDefinition.WantsOnFilter && uiResults.WasOnFilter(this))
			{
				filterValueChanged = newFilterValue != filterValue;
			}

			previousFilterValue = filterValue;
			filterValue = newFilterValue;
		}

		/// <inheritdoc	/>
		protected internal override void RaiseResultEvents(ILogger logger = null)
		{
			if (filterValueChanged)
			{
				CacheSelectedValue(logger);

				logger?.Debug(nameof(DropDown), nameof(RaiseResultEvents), $"OnFilterChange; Filter Value changed from {previousFilterValue} to {filterValue}");
				OnFilterChanged?.Invoke(this, new DropDownFilterChangedEventArgs(filterValue, previousFilterValue));

				// Add the selected items again to the options in case the user updated the options based on the provided filter.
				RestoreCachedSelectedValue(logger);
			}

			filterValueChanged = false;
		}

		protected internal abstract void CacheSelectedValue(ILogger logger = null);

		protected internal abstract void RestoreCachedSelectedValue(ILogger logger = null);

		/// <summary>
		///     Provides data for the <see cref="FilterChanged" /> event.
		/// </summary>
		public class DropDownFilterChangedEventArgs : EventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="DropDownFilterChangedEventArgs"/> class.
			/// </summary>
			/// <param name="filterValue">The new filter value.</param>
			/// <param name="previousFilterValue">The previous filter value.</param>
			internal DropDownFilterChangedEventArgs(string filterValue, string previousFilterValue)
			{
				FilterValue = filterValue;
				PreviousFilterValue = previousFilterValue;
			}

			/// <summary>
			///     Gets the previously value of the filter.
			/// </summary>
			public string PreviousFilterValue { get; private set; }

			/// <summary>
			///     Gets the value of the filter.
			/// </summary>
			public string FilterValue { get; private set; }
		}
	}
}
