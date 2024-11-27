namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     A drop-down list.
	/// </summary>
	public class DropDown<T> : InteractiveWidget
	{
		private readonly Dictionary<string, T> options = new Dictionary<string, T>();
		private bool changed;
		private T previous;

		/// <summary>
		///     Initializes a new instance of the <see cref="DropDown" /> class.
		/// </summary>
		public DropDown() : this(new Dictionary<string, T>())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="DropDown" /> class.
		/// </summary>
		/// <param name="options">Options to be displayed in the list.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public DropDown(IReadOnlyDictionary<string, T> options) : this(options, options.Values.FirstOrDefault())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="DropDown" /> class.
		/// </summary>
		/// <param name="options">Options to be displayed in the list.</param>
		/// <param name="selected">The selected item in the list.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public DropDown(IReadOnlyDictionary<string, T> options, T selected)
		{
			Type = UIBlockType.DropDown;
			SetOptions(options);

			Selected = selected;

			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
			IsReadOnly = false;
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

		/// <summary>
		///     Gets or sets the possible options.
		/// </summary>
		public virtual IReadOnlyDictionary<string, T> Options
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

		/// <summary>
		///     Gets or sets the selected option.
		/// </summary>
		public T Selected
		{
			get
			{
				return options[BlockDefinition.InitialValue];
			}

			set
			{
				var selectedOption = options.FirstOrDefault(x => x.Value.Equals(value));
				if (selectedOption.Equals(default(KeyValuePair<string, T>))) throw new InvalidOperationException($"Value is not defined as an option in the dropdown");
				BlockDefinition.InitialValue = selectedOption.Key;
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
		/// 	Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
		/// 	This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and 10.0.1.0 Main Release.</remarks>
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
		/// 	Gets or sets the text that is shown if the validation state is invalid.
		/// 	This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
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
		///     Gets or sets a value indicating whether a filter box is available for the drop-down list.
		/// </summary>
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

		/// <summary>
		///     Adds an option to the drop-down list.
		/// </summary>
		/// <param name="option">Option to add.</param>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		internal void AddOption(string option, T value)
		{
			if (option == null)
			{
				throw new ArgumentNullException("option");
			}

			if (options.ContainsKey(option)) return;

			options.Add(option, value);
			BlockDefinition.AddDropDownOption(option);
		}

		/// <summary>
		///     Sets the displayed options.
		///     Replaces existing options.
		/// </summary>
		/// <param name="optionsToSet">Options to set.</param>
		/// <exception cref="ArgumentNullException">When optionsToSet is null.</exception>
		public void SetOptions(IReadOnlyDictionary<string, T> optionsToSet)
		{
			if (optionsToSet == null)
			{
				throw new ArgumentNullException("optionsToSet");
			}

			ClearOptions();
			foreach (var option in optionsToSet)
			{
				AddOption(option.Key, option.Value);
			}

			if (!optionsToSet.Any())
			{
				Selected = default;
				return;
			}

			if (Selected.Equals(default(T)) || !optionsToSet.Values.Contains(Selected))
			{
				Selected = optionsToSet.First().Value;
			}
		}

		/// <summary>
		/// 	Removes an option from the drop-down list.
		/// </summary>
		/// <param name="option">Option to remove.</param>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		internal void RemoveOption(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException("option");
			}

			if (options.Remove(option))
			{
				RecreateUiBlock();
				foreach (string optionToAdd in options.Keys)
				{
					BlockDefinition.AddDropDownOption(optionToAdd);
				}

				if (Selected.Equals(default(T)) && options.Any())
				{
					Selected = options.First().Value;
				}
			}
		}

		/// <summary>
		///     Load any changes made through user interaction.
		/// </summary>
		/// <param name="uiResults">
		///     Represents the information a user has entered or selected in a dialog box of an interactive
		///     Automation script.
		/// </param>
		/// <remarks><see cref="InteractiveWidget.DestVar" /> should be used as key to get the changes for this widget.</remarks>
		protected internal override void LoadResult(UIResults uiResults)
		{
			T selectedValue = options[uiResults.GetString(this)];

			if (BlockDefinition.WantsOnChange)
			{
				changed = !selectedValue.Equals(Selected);
			}

			previous = Selected;
			Selected = selectedValue;
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
				OnChanged?.Invoke(this, new DropDownChangedEventArgs(Selected, previous));
			}

			changed = false;
		}

		private void ClearOptions()
		{
			options.Clear();
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
			internal DropDownChangedEventArgs(T selected, T previous)
			{
				Selected = selected;
				Previous = previous;
			}

			/// <summary>
			///     Gets the previously selected option.
			/// </summary>
			public T Previous { get; private set; }

			/// <summary>
			///     Gets the option that has been selected.
			/// </summary>
			public T Selected { get; private set; }
		}
	}
}
