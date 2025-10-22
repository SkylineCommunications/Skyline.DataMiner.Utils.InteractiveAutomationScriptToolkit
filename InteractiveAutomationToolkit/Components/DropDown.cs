namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	///     A drop-down list.
	/// </summary>
	public class DropDown : DropDownBase, IDropDown
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
				return BlockDefinition.InitialValue;
			}

			set
			{
				BlockDefinition.InitialValue = value;
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
				BlockDefinition.AddDropDownOption(option);
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

			Clear();
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
					BlockDefinition.AddDropDownOption(optionToAdd);
				}

				if (Selected == option)
				{
					Selected = options.FirstOrDefault();
				}
			}
		}

		/// <inheritdoc/>
		public void Clear()
		{
			options.Clear();
			RecreateUiBlock();
			Selected = null;
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
			string selectedValue = uiResults.GetString(this);

			if (BlockDefinition.WantsOnChange)
			{
				changed = selectedValue != Selected;
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
