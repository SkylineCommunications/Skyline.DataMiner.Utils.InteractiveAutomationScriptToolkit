namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     A drop-down list.
	/// </summary>
	public class DropDown : InteractiveWidget, IDropDown
	{
		private readonly Validation validation;
		private readonly DropDownOptionCollection optionCollection;
		private bool changed;
		private string previous;

		/// <summary>
		///     Initializes a new instance of the <see cref="DropDown" /> class.
		/// </summary>
		public DropDown()
			: this(Enumerable.Empty<string>())
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
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			Type = UIBlockType.DropDown;
			validation = new Validation(this);
			optionCollection = new DropDownOptionCollection(this);

			SetOptions(options);

			Selected = selected;
		}

		/// <inheritdoc />
		public event EventHandler<ChangedEventArgs> Changed
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

		private event EventHandler<ChangedEventArgs> OnChanged;

		/// <inheritdoc />
		IList<string> IDropDown.Options => optionCollection;

		/// <summary>
		///     Gets  the possible options.
		/// </summary>
		public OptionList Options => optionCollection;

		/// <inheritdoc />
		public bool IsDisplayFilterShown
		{
			get => BlockDefinition.DisplayFilter;
			set => BlockDefinition.DisplayFilter = value;
		}

		/// <inheritdoc />
		public bool IsSorted
		{
			get => BlockDefinition.IsSorted;
			set => BlockDefinition.IsSorted = value;
		}

		/// <inheritdoc />
		public string Selected
		{
			get => BlockDefinition.InitialValue;

			set
			{
				// Prevent setting a value that is not part of the options
				if (Options.Contains(value))
				{
					BlockDefinition.InitialValue = value;
				}
			}
		}

		/// <inheritdoc />
		public string Tooltip
		{
			get => BlockDefinition.TooltipText;
			set => BlockDefinition.TooltipText = value ?? String.Empty;
		}

		/// <inheritdoc />
		public UIValidationState ValidationState
		{
			get => validation.ValidationState;
			set => validation.ValidationState = value;
		}

		/// <inheritdoc />
		public string ValidationText
		{
			get => validation.ValidationText;
			set => validation.ValidationText = value;
		}

		/// <inheritdoc />
		[Obsolete("Call Options.Add instead.")]
		public void AddOption(string option)
		{
			Options.Add(option ?? String.Empty);
		}

		/// <inheritdoc />
		public void ForceSelected(string selected)
		{
			BlockDefinition.InitialValue = selected;
		}

		/// <inheritdoc />
		[Obsolete("Call Options.Remove instead.")]
		public void RemoveOption(string option)
		{
			Options.Remove(option);
		}

		/// <inheritdoc />
		public void SetOptions(IEnumerable<string> optionsToSet)
		{
			if (optionsToSet == null)
			{
				throw new ArgumentNullException(nameof(optionsToSet));
			}

			string copyOfSelected = Selected;

			Options.Clear();
			foreach (string option in optionsToSet)
			{
				Options.Add(option);
			}

			Selected = copyOfSelected;
		}

		/// <inheritdoc />
		protected internal override void LoadResult(UIResults results)
		{
			string selectedValue = results.GetString(this);
			if (WantsOnChange)
			{
				changed = selectedValue != Selected;
			}

			previous = Selected;

			// Write to BlockDefinition instead of Selected property so a force selected option does not reset after every interaction
			BlockDefinition.InitialValue = selectedValue;
		}

		/// <inheritdoc />
		protected internal override void RaiseResultEvents()
		{
			if (changed && OnChanged != null)
			{
				OnChanged(this, new ChangedEventArgs(Selected, previous));
			}

			changed = false;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class ChangedEventArgs : EventArgs
		{
			/// <summary>
			///     Initializes a new instance of the <see cref="ChangedEventArgs" /> class.
			/// </summary>
			/// <param name="selected">The option that has been selected.</param>
			/// <param name="previous">The previously selected option.</param>
			public ChangedEventArgs(string selected, string previous)
			{
				Selected = selected;
				Previous = previous;
			}

			/// <summary>
			///     Gets the previously selected option.
			/// </summary>
			public string Previous { get; }

			/// <summary>
			///     Gets the option that has been selected.
			/// </summary>
			public string Selected { get; }
		}

		private class DropDownOptionCollection : OptionList
		{
			private readonly DropDown dropDown;

			public DropDownOptionCollection(DropDown dropDown) : base(dropDown.BlockDefinition.GetOptionsCollection())
			{
				this.dropDown = dropDown;
			}

			public override string this[int index]
			{
				get => base[index];

				set
				{
					string replacedOption = base[index];
					base[index] = value;

					if (dropDown.Selected == replacedOption)
					{
						dropDown.BlockDefinition.InitialValue = value;
					}

					// Cube will select the first option even if UIBlockDefinition.InitialValue is null.
					// But I believe this behavior should be reflected by the Selected property.
					// Selected should never be null if options is not empty.
					if (dropDown.Selected == null)
					{
						dropDown.Selected = value;
					}
				}
			}

			public override void Add(string item)
			{
				base.Add(item);

				// Cube will select the first option even if UIBlockDefinition.InitialValue is null.
				// But I believe this behavior should be reflected by the Selected property.
				// Selected should never be null if options is not empty.
				if (dropDown.Selected == null)
				{
					dropDown.Selected = item;
				}
			}

			public override void Clear()
			{
				base.Clear();
				dropDown.BlockDefinition.InitialValue = null;
			}

			public override void Insert(int index, string item)
			{
				base.Insert(index, item);

				// Cube will select the first option even if UIBlockDefinition.InitialValue is null.
				// But I believe this behavior should be reflected by the Selected property.
				// Selected should never be null if options is not empty.
				if (dropDown.Selected == null)
				{
					dropDown.Selected = item;
				}
			}

			public override bool Remove(string item)
			{
				if (!base.Remove(item))
				{
					return false;
				}

				if (dropDown.Selected == item)
				{
					dropDown.BlockDefinition.InitialValue = this.FirstOrDefault();
				}

				return true;
			}

			public override void RemoveAt(int index)
			{
				string item = this[index];
				base.RemoveAt(index);
				if (dropDown.Selected == item)
				{
					dropDown.BlockDefinition.InitialValue = this.FirstOrDefault();
				}
			}
		}
	}
}