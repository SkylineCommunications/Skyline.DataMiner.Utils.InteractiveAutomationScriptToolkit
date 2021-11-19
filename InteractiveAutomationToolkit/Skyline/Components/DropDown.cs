namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Skyline.DataMiner.Automation;

	/// <summary>
	///     A drop-down list.
	/// </summary>
	public class DropDown : InteractiveWidget
	{
		private readonly HashSet<string> options = new HashSet<string>();
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
			Type = UIBlockType.DropDown;
			SetOptions(options);
			if (selected != null) Selected = selected;
			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
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
				WantsOnChange = true;
			}

			remove
			{
				OnChanged -= value;
				if (OnChanged == null || !OnChanged.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<DropDownChangedEventArgs> OnChanged;

		/// <summary>
		///     Gets or sets the possible options.
		/// </summary>
		public IEnumerable<string> Options
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
		public string Selected
		{
			get
			{
				return BlockDefinition.InitialValue;
			}

			set
			{
				BlockDefinition.InitialValue = value;
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
		///		Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
		///		This should be used by the client to add a visual marker on the input field.
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
		///		Gets or sets the text that is shown if the validation state is invalid.
		///		This should be used by the client to add a visual marker on the input field.
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
		///     Adds an option to the drop-down list.
		/// </summary>
		/// <param name="option">Option to add.</param>
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
				BlockDefinition.AddDropDownOption(option);
			}
		}

		/// <summary>
		///     Sets the displayed options.
		///     Replaces existing options.
		/// </summary>
		/// <param name="optionsToSet">Options to set.</param>
		/// <exception cref="ArgumentNullException">When optionsToSet is null.</exception>
		public void SetOptions(IEnumerable<string> optionsToSet)
		{
			if (optionsToSet == null)
			{
				throw new ArgumentNullException("optionsToSet");
			}

			ClearOptions();
			foreach (string option in optionsToSet)
			{
				AddOption(option);
			}

			if (Selected == null || !optionsToSet.Contains(Selected))
			{
				Selected = optionsToSet.FirstOrDefault();
			}
		}

		/// <summary>
		/// 	Removes an option from the drop-down list.
		/// </summary>
		/// <param name="option">Option to remove.</param>
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
					BlockDefinition.AddDropDownOption(optionToAdd);
				}

				if (Selected == option)
				{
					Selected = options.FirstOrDefault();
				}
			}
		}

		internal override void LoadResult(UIResults uiResults)
		{
			string selectedValue = uiResults.GetString(this);
			if (WantsOnChange)
			{
				changed = selectedValue != Selected;
			}

			previous = Selected;
			Selected = selectedValue;
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			if (changed && (OnChanged != null))
			{
				OnChanged(this, new DropDownChangedEventArgs(Selected, previous));
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
