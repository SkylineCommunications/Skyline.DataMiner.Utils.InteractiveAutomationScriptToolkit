namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Skyline.DataMiner.Analytics.GenericInterface;
	using Skyline.DataMiner.Automation;

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

		public RadioButtonList(IEnumerable<T> options) : this(options.Select(x => new Option<T>(x)))
		{
		}

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
		public IEnumerable<Option<T>> Options
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

		public virtual IEnumerable<T> Values
		{
			get
			{
				return radioButtonListOptions.Select(x => x.Value);
			}

			set
			{
				if (value == null) throw new InvalidOperationException();
				SetOptions(value.Select(x => new Option<T>(x)));
			}
		}

		public Option<T> SelectedOption
		{
			get
			{
				return radioButtonListOptions.FirstOrDefault(x => x.DisplayValue.Equals(BlockDefinition.InitialValue));
			}

			set
			{
				if (!radioButtonListOptions.Contains(value)) throw new InvalidOperationException($"Value is not defined as an option");
				BlockDefinition.InitialValue = value.DisplayValue;
			}
		}

		/// <summary>
		///     Gets or sets the selected option.
		/// </summary>
		public T Selected
		{
			get
			{
				return SelectedOption.Value;
			}

			set
			{
				var option = radioButtonListOptions.FirstOrDefault(x => x.Value.Equals(value)) ?? throw new InvalidOperationException($"No option available where the value of the option matches the given value");
				SelectedOption = option;
			}
		}

		/// <summary>
		///     Adds a radio button to the group.
		/// </summary>
		/// <param name="option">Option to add.</param>
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

		public void AddOption(T value)
		{
			AddOption(new Option<T>(value));
		}

		/// <summary>
		/// 	Removes an option from the radio button list.
		/// </summary>
		/// <param name="option">Option to remove.</param>
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

		public void RemoveOption(T value)
		{
			RemoveOption(new Option<T>(value));
		}

		/// <summary>
		///     Sets the displayed options.
		///     Replaces existing options.
		/// </summary>
		/// <param name="options">Options to set.</param>
		/// <exception cref="ArgumentNullException">When optionsToSet is null.</exception>
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

			if (SelectedOption == null || !options.Contains(SelectedOption))
			{
				SelectedOption = options.FirstOrDefault();
			}
		}

		public void SetOptions(IEnumerable<T> options)
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

				Selected = selectedValue.Value;
				Previous = previous.Value;
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
