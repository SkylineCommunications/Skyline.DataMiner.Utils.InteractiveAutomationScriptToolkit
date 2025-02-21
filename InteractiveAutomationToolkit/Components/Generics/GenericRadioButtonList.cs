namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	///     A group of radio buttons.
	/// </summary>
	public class RadioButtonList<T> : RadioButtonListBase, IRadioButtonList<T>
	{
		private readonly OptionCollection<T> radioButtonListOptions = new OptionCollection<T>();
		private bool changed;
		private Option<T> previous;

		/// <summary>
		///     Initializes a new instance of the <see cref="RadioButtonList" /> class.
		/// </summary>
		public RadioButtonList() : this(Enumerable.Empty<Option<T>>())
		{
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="RadioButtonList" /> class.
		/// </summary>
		/// <param name="options">Values that can be selected, every value is visualized in the radiobuttonlist by its <see cref="Object.ToString()"/> counterpart.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public RadioButtonList(IEnumerable<T> options) : this(options.Select(x => new Option<T>(x)))
		{
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="RadioButtonList" /> class.
		/// </summary>
		/// <param name="options">Values that can be selected, every value is visualized in the radiobuttonlist by its <see cref="Object.ToString()"/> counterpart.</param>
		/// <param name="selected">Default selected value.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public RadioButtonList(IEnumerable<T> options, T selected) : this(options.Select(x => new Option<T>(x)), new Option<T>(selected))
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="RadioButtonList" /> class.
		/// </summary>
		/// <param name="options">Name of options that can be selected.</param>
		/// <param name="selected">Selected option.</param>
		public RadioButtonList(IEnumerable<Option<T>> options, Option<T> selected = null)
		{
			SetOptions(options);
			SelectedOption = selected;
		}

		/// <summary>
		///     Triggered when a different option is selected.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<RadioButtonChangedEventArgs> Changed
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

		private event EventHandler<RadioButtonChangedEventArgs> OnChanged;

		/// <summary>
		///     Gets or sets all options.
		/// </summary>
		public virtual IEnumerable<Option<T>> Options
		{
			get
			{
				return radioButtonListOptions;
			}

			set
			{
				SetOptions(value);
			}
		}

		/// <summary>
		///		<inheritdoc/>
		///		Setting this property overrides all options and causes every value to be visually represented by their <see cref="Object.ToString()"/> counterpart.
		/// </summary>
		public virtual IEnumerable<T> Values
		{
			get
			{
				return radioButtonListOptions.Select(x => x.Value);
			}

			set
			{
				if (value == null) throw new ArgumentNullException(nameof(value));
				SetOptions(value.Select(x => new Option<T>(x)));
			}
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentException">If the given option is not null and not defined as a possible option in the radiobuttonlist.</exception>
		public Option<T> SelectedOption
		{
			get
			{
				return radioButtonListOptions.FirstOrDefault(x => x.DisplayValue.Equals(BlockDefinition.InitialValue));
			}

			set
			{
				if (value == null)
				{
					BlockDefinition.InitialValue = null;
					return;
				}

				if (!radioButtonListOptions.Contains(value)) throw new ArgumentException($"Value is not defined as an option");
				BlockDefinition.InitialValue = value.DisplayValue;
			}
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentException">If no option is defined in the radiobuttonlist that represents the given value.</exception>
		public T Selected
		{
			get
			{
				if (SelectedOption == null) return default;
				return SelectedOption.Value;
			}

			set
			{
				var option = radioButtonListOptions.FirstOrDefault(x => Object.Equals(x.Value, value)) ?? throw new ArgumentException($"No option available where the value of the option matches the given value");
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

			if (!radioButtonListOptions.Contains(option))
			{
				radioButtonListOptions.Add(option);
				BlockDefinition.AddCheckBoxListOption(option.DisplayValue);
			}
		}

		/// <summary>
		///		<inheritdoc/>
		///		This value is represented in the radiobuttonlist by its <see cref="Object.ToString()"/> counterpart.
		/// </summary>
		/// <param name="value">Value to be added as an option to the radiobuttonlist.</param>
		public void AddOption(T value)
		{
			AddOption(new Option<T>(value));
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
			if (radioButtonListOptions.Remove(option))
			{
				RecreateUiBlock();
				foreach (var optionToAdd in radioButtonListOptions)
				{
					BlockDefinition.AddCheckBoxListOption(optionToAdd.DisplayValue);
				}

				if (currentSelectedOption == option)
				{
					SelectedOption = radioButtonListOptions.FirstOrDefault();
				}
			}
		}

		/// <inheritdoc/>
		public void RemoveOption(T value)
		{
			var options = radioButtonListOptions.Where(x => Object.Equals(x.Value, value)).ToList();
			foreach (var option in options)
			{
				RemoveOption(option);
			}
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public void SetOptions(IEnumerable<Option<T>> options)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			ClearOptions();
			foreach (var option in options)
			{
				AddOption(option);
			}

			if (SelectedOption != null && !options.Contains(SelectedOption))
			{
				SelectedOption = null;
			}
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public virtual void SetOptions(IEnumerable<T> options)
		{
			if (options == null) throw new ArgumentNullException(nameof(options));
			SetOptions(options.Select(x => new Option<T>(x)));
		}

		/// <summary>
		///     Load any changes made through user interaction.
		/// </summary>
		/// <param name="uiResults">
		///     Represents the information a user has entered or selected in a dialog box of an interactive
		///     Automation script.
		/// </param>
		/// <remarks><see cref="InteractiveWidget.DestVar" /> should be used as key to get the changes for this widget.</remarks>
		protected internal override void LoadResult(IUIResults uiResults)
		{
			string result = uiResults.GetString(this);

			if (String.IsNullOrWhiteSpace(result))
			{
				return;
			}

			string[] checkedOptions = result.Split(';');
			foreach (string checkedOption in checkedOptions)
			{
				if (String.IsNullOrEmpty(checkedOption)) continue;
				if (String.Equals(checkedOption, SelectedOption?.DisplayValue)) continue;

				var selectedOption = radioButtonListOptions.FirstOrDefault(x => x.DisplayValue.Equals(checkedOption));

				previous = SelectedOption;
				SelectedOption = selectedOption;
				changed = true;
				break;
			}
		}

		/// <summary>
		///     Raises zero or more events of the widget.
		///     This method is called after <see cref="InteractiveWidget.LoadResult" /> was called on all widgets.
		/// </summary>
		/// <remarks>It is up to the implementer to determine if an event must be raised.</remarks>
		protected internal override void RaiseResultEvents()
		{
			if (changed)
			{
				OnChanged?.Invoke(this, new RadioButtonChangedEventArgs(SelectedOption, previous));
			}

			changed = false;
		}

		private void ClearOptions()
		{
			radioButtonListOptions.Clear();
			RecreateUiBlock();
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class RadioButtonChangedEventArgs : EventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="RadioButtonChangedEventArgs"/> class.
			/// </summary>
			/// <param name="selectedValue">The new value.</param>
			/// <param name="previous">The previous value.</param>
			internal RadioButtonChangedEventArgs(Option<T> selectedValue, Option<T> previous)
			{
				SelectedOption = selectedValue;
				PreviousOption = previous;

				Selected = selectedValue == null ? default : selectedValue.Value;
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
