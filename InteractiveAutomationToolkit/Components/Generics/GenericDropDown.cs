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
	public class DropDown<T> : DropDownBase, IDropDown<T>
	{
		private readonly OptionCollection<T> dropDownOptions = new OptionCollection<T>();
		private bool changed;
		private Option<T> previous;

		/// <summary>
		///     Initializes a new instance of the <see cref="DropDown" /> class.
		/// </summary>
		public DropDown() : this(Enumerable.Empty<Option<T>>())
		{
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="DropDown" /> class.
		/// </summary>
		/// <param name="options">Values that can be selected, every value is visualized in the dropdown by its <see cref="Object.ToString()"/> counterpart.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public DropDown(IEnumerable<T> options) : this(options, options.FirstOrDefault())
		{
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="DropDown" /> class.
		/// </summary>
		/// <param name="options">Values that can be selected, every value is visualized in the dropdown by its <see cref="Object.ToString()"/> counterpart.</param>
		/// <param name="selected">Default selected value.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public DropDown(IEnumerable<T> options, T selected)
			: this(options.Select(x => new Option<T>(x)), selected != null ? new Option<T>(selected) : null)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="DropDown" /> class.
		/// </summary>
		/// <param name="options">Options to be displayed in the list.</param>
		/// <param name="selected">The selected item in the list.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public DropDown(IEnumerable<Option<T>> options, Option<T> selected = null)
		{
			SetOptions(options);
			if (selected != null) SelectedOption = selected;
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
		public virtual IEnumerable<Option<T>> Options
		{
			get
			{
				return dropDownOptions;
			}

			set
			{
				SetOptions(value);
			}
		}

		/// <inheritdoc/>
		public virtual IEnumerable<T> Values
		{
			get
			{
				return dropDownOptions.Select(x => x.Value);
			}

			set
			{
				SetOptions(value);
			}
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentException">If the given option is not null and not defined as a possible option in the dropdown.</exception>
		public Option<T> SelectedOption
		{
			get
			{
				return dropDownOptions.FirstOrDefault(x => x.DisplayValue.Equals(BlockDefinition.InitialValue));
			}

			set
			{
				if (value == null)
				{
					BlockDefinition.InitialValue = null;
					return;
				}

				if (!dropDownOptions.Contains(value)) throw new ArgumentException($"Value is not defined as an option");
				BlockDefinition.InitialValue = value.DisplayValue;
			}
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentException">If no option is defined in the dropdown that represents the given value.</exception>
		public T Selected
		{
			get
			{
				if (SelectedOption == null) return default;
				return SelectedOption.Value;
			}

			set
			{
				var option = dropDownOptions.FirstOrDefault(x => Object.Equals(x.Value, value)) ?? throw new ArgumentException($"No option available where the value of the option matches the given value");
				SelectedOption = option;
			}
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		public void AddOption(Option<T> option)
		{
			if (option == null)
			{
				throw new ArgumentNullException("option");
			}

			if (!dropDownOptions.Contains(option))
			{
				dropDownOptions.Add(option);
				BlockDefinition.AddDropDownOption(dropDownOptions.GetRawValue(option), option.DisplayValue);
			}
		}

		/// <summary>
		///  <inheritdoc/>
		///  This value is represented in the dropdown by its <see cref="Object.ToString()"/> counterpart.
		/// </summary>
		/// <param name="value">Value to be added as an option to the dropdown.</param>
		public void AddOption(T value)
		{
			AddOption(new Option<T>(value));
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public void SetOptions(IEnumerable<Option<T>> options)
		{
			if (options == null) throw new ArgumentNullException(nameof(options));

			ClearOptions();
			foreach (var option in options)
			{
				AddOption(option);
			}

			if (SelectedOption == null || !options.Contains(SelectedOption))
			{
				SelectedOption = options.FirstOrDefault();
			}
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public virtual void SetOptions(IEnumerable<T> options)
		{
			if (options == null) throw new ArgumentNullException(nameof(options));
			SetOptions(options.Select(x => new Option<T>(x)));
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		public void RemoveOption(Option<T> option)
		{
			if (option == null)
			{
				throw new ArgumentNullException("option");
			}

			var currentSelectedOption = SelectedOption;
			if (dropDownOptions.Remove(option))
			{
				RecreateUiBlock();
				foreach (var optionToAdd in dropDownOptions)
				{
					BlockDefinition.AddDropDownOption(dropDownOptions.GetRawValue(optionToAdd), optionToAdd.DisplayValue);
				}

				if (currentSelectedOption == option)
				{
					SelectedOption = dropDownOptions.FirstOrDefault();
				}
			}
		}

		/// <inheritdoc/>
		public void RemoveOption(T value)
		{
			var options = dropDownOptions.Where(x => Object.Equals(x.Value, value)).ToList();
			foreach (var option in options)
			{
				RemoveOption(option);
			}
		}

		internal string GetRawValue(Option<T> option)
		{
			return dropDownOptions.GetRawValue(option);
		}

		/// <inheritdoc	/>
		protected internal override void LoadResult(IUIResults uiResults, ILogger logger = null)
		{
			base.LoadResult(uiResults, logger);

			var rawSelectedValue = uiResults.GetString(this);
			logger?.Debug(nameof(DropDown), nameof(LoadResult), $"Raw selected value: {rawSelectedValue}");

			if (!dropDownOptions.TryGetByRawValue(rawSelectedValue, out var selectedValue))
			{
				return;
			}

			logger?.Debug(nameof(DropDown), nameof(LoadResult), $"Selected value: {selectedValue}");

			if (BlockDefinition.WantsOnChange)
			{
				changed = selectedValue != SelectedOption;
			}

			previous = SelectedOption;
			SelectedOption = selectedValue;
		}

		/// <inheritdoc	/>
		protected internal override void RaiseResultEvents(ILogger logger = null)
		{
			base.RaiseResultEvents(logger);

			if (changed)
			{
				logger?.Debug(nameof(DropDown), nameof(RaiseResultEvents), $"OnChange; Selected changed from {previous?.DisplayValue} to {SelectedOption.DisplayValue}");
				OnChanged?.Invoke(this, new DropDownChangedEventArgs(SelectedOption, previous));
			}

			changed = false;
		}

		private void ClearOptions()
		{
			dropDownOptions.Clear();
			RecreateUiBlock();
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
			internal DropDownChangedEventArgs(Option<T> selected, Option<T> previous)
			{
				SelectedOption = selected;
				PreviousOption = previous;

				Selected = selected == null ? default : selected.Value;
				Previous = previous == null ? default : previous.Value;
			}

			/// <summary>
			///     Gets the previously selected option.
			/// </summary>
			public Option<T> PreviousOption { get; private set; }

			/// <summary>
			///     Gets the previously selected value.
			/// </summary>
			public T Previous { get; private set; }

			/// <summary>
			///     Gets the option that has been selected.
			/// </summary>
			public Option<T> SelectedOption { get; private set; }

			/// <summary>
			///     Gets the value that has been selected.
			/// </summary>
			public T Selected { get; private set; }
		}
	}
}