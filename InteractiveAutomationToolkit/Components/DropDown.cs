namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Microsoft.Extensions.Logging;

	using Skyline.DataMiner.Utils.InteractiveAutomationScript.Extensions;

	/// <summary>
	///     A drop-down list.
	/// </summary>
	public class DropDown : DropDownBase, IDropDown
	{
		private readonly HashSet<string> options = new HashSet<string>();
		private readonly RawValueMapping<string> rawValueMapping = new RawValueMapping<string>();
		private string cachedSelectedValue;

		private bool changed;
		private string previous;

		/// <summary>
		///     Initializes a new instance of the <see cref="DropDown" /> class.
		/// </summary>
		public DropDown() : this(Enumerable.Empty<string>())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="DropDown" /> class.
		/// </summary>
		/// <param name="options">Options to be displayed in the list.</param>
		/// <param name="selected">The selected item in the list.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public DropDown(IEnumerable<string> options, string selected = null)
		{
			SetOptions(options);
			if (selected != null)
			{
				Selected = selected;
			}
		}

		/// <summary>
		///     Triggered when a different option is selected.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<DropDownChangedEventArgs> Changed
		{
			add
			{
				OnChanged += value;
				BlockDefinition.WantsOnChange = true;
			}

			remove
			{
				OnChanged -= value;
				if (OnChanged == null || !OnChanged.GetInvocationList().Any())
				{
					BlockDefinition.WantsOnChange = false;
				}
			}
		}

		private event EventHandler<DropDownChangedEventArgs> OnChanged;

		/// <inheritdoc/>
		public virtual IEnumerable<string> Options
		{
			get
			{
				return options;
			}

			set
			{
				SetOptions(value);
			}
		}

		/// <inheritdoc/>
		public string Selected
		{
			get
			{
				return rawValueMapping.TryGetByRawValue(BlockDefinition.InitialValue, out var value) ? value : null;
			}

			set
			{
				if (value == null)
				{
					BlockDefinition.InitialValue = null;
				}
				else if (rawValueMapping.TryGetRawValue(value, out var rawValue))
				{
					BlockDefinition.InitialValue = rawValue;
				}
				else
				{
					throw new ArgumentException("The selected value is not defined as an option.", "value");
				}
			}
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		public void AddOption(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException("option");
			}

			if (!options.Contains(option))
			{
				options.Add(option);
				rawValueMapping.Add(option, option);

				BlockDefinition.AddDropDownOption(rawValueMapping.GetRawValue(option), option);
			}
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public void SetOptions(IEnumerable<string> options)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			ClearOptions();
			foreach (string option in options)
			{
				AddOption(option);
			}

			if (Selected == null || !options.Contains(Selected))
			{
				Selected = options.FirstOrDefault();
			}
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		public void RemoveOption(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException("option");
			}

			if (options.Remove(option))
			{
				RecreateUiBlock();
				foreach (string optionToAdd in options)
				{
					BlockDefinition.AddDropDownOption(rawValueMapping.GetRawValue(optionToAdd), optionToAdd);
				}

				if (Selected == option)
				{
					Selected = options.FirstOrDefault();
				}

				rawValueMapping.Remove(option);
			}
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		public bool ContainsOption(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException(nameof(option));
			}

			return options.Contains(option);
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		public bool TrySelectOption(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException(nameof(option));
			}

			if (ContainsOption(option))
			{
				Selected = option;
				return true;
			}

			return false;
		}

		/// <inheritdoc	/>
		protected internal override void LoadResult(IUIResults uiResults, ILogger logger = null)
		{
			base.LoadResult(uiResults, logger);

			var rawSelectedValue = uiResults.GetString(this);
			logger?.Debug(nameof(DropDown), nameof(LoadResult), $"Raw selected value: {rawSelectedValue}");

			if (!rawValueMapping.TryGetByRawValue(rawSelectedValue, out var selectedValue))
			{
				return;
			}

			logger?.Debug(nameof(DropDown), nameof(LoadResult), $"Selected value: {selectedValue}");

			if (BlockDefinition.WantsOnChange)
			{
				changed = selectedValue != Selected;
			}

			previous = Selected;
			Selected = selectedValue;
		}

		/// <inheritdoc	/>
		protected internal override void RaiseResultEvents(ILogger logger = null)
		{
			base.RaiseResultEvents(logger);

			if (changed)
			{
				logger?.Debug(nameof(DropDown), nameof(RaiseResultEvents), $"OnChange; Selected changed from {previous} to {Selected}");
				OnChanged?.Invoke(this, new DropDownChangedEventArgs(Selected, previous));
			}

			changed = false;
		}

		private void ClearOptions()
		{
			options.Clear();
			rawValueMapping.Clear();
			RecreateUiBlock();
		}

		protected internal override void CacheSelectedValue(ILogger logger = null)
		{
			logger?.Debug(nameof(DropDown), nameof(CacheSelectedValue), $"Caching selected value '{Selected}'.");
			cachedSelectedValue = Selected;
		}

		protected internal override void RestoreCachedSelectedValue(ILogger logger = null)
		{
			if (cachedSelectedValue == null)
				return;

			logger?.Debug(nameof(DropDown), nameof(RestoreCachedSelectedValue), $"Restoring cached selected option '{cachedSelectedValue}'.");

			AddOption(cachedSelectedValue);
			Selected = cachedSelectedValue;

			cachedSelectedValue = null;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class DropDownChangedEventArgs : EventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="DropDownChangedEventArgs"/> class.
			/// </summary>
			/// <param name="selected">The new value.</param>
			/// <param name="previous">The previous value.</param>
			internal DropDownChangedEventArgs(string selected, string previous)
			{
				Selected = selected;
				Previous = previous;
			}

			/// <summary>
			///     Gets the previously selected option.
			/// </summary>
			public string Previous { get; private set; }

			/// <summary>
			///     Gets the option that has been selected.
			/// </summary>
			public string Selected { get; private set; }
		}
	}
}