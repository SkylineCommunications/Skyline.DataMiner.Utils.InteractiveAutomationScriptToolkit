namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <inheritdoc cref="Skyline.DataMiner.InteractiveAutomationToolkit.IDropDown{T}" />
	public class DropDown<T> : InteractiveWidget, IDropDown<T>
	{
		private readonly DropdownOptionsList<T> dropdownOptionsList;
		private bool changed;
		private string previous;

		/// <summary>
		///     Initializes a new instance of the <see cref="DropDown{T}" /> class.
		/// </summary>
		public DropDown()
			: this(Enumerable.Empty<Option<T>>())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="DropDown{T}" /> class.
		/// </summary>
		/// <param name="options">Options to be displayed in the list.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public DropDown(IEnumerable<Option<T>> options)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			Type = UIBlockType.DropDown;
			dropdownOptionsList = new DropdownOptionsList<T>(this);

			Options.AddRange(options);

			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
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
		public IOptionsList<T> Options => dropdownOptionsList;

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

		/// <inheritdoc/>
		public Option<T> Selected
		{
			get => SelectedText == null ? default : new Option<T>(SelectedText, SelectedValue);

			set
			{
				if (!dropdownOptionsList.Contains(value))
				{
					return;
				}

				SelectedText = value.Text;
			}
		}

		/// <inheritdoc />
		public string SelectedText
		{
			get => BlockDefinition.InitialValue;

			set
			{
				// Prevent setting a value that is not part of the options
				if (Options.ContainsText(value ?? String.Empty))
				{
					BlockDefinition.InitialValue = value ?? String.Empty;
				}
			}
		}

		/// <inheritdoc />
		public T SelectedValue
		{
			get
			{
				int index = dropdownOptionsList.IndexOfText(SelectedText);
				return index == -1 ? default : dropdownOptionsList[index].Value;
			}

			set
			{
				int index = dropdownOptionsList.IndexOfValue(value);
				if (index == -1)
				{
					return;
				}

				SelectedText = dropdownOptionsList[index].Text;
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
			get => BlockDefinition.ValidationState;

			set
			{
				if (!Enum.IsDefined(typeof(UIValidationState), value))
				{
					throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(UIValidationState));
				}

				BlockDefinition.ValidationState = value;
			}
		}

		/// <inheritdoc />
		public string ValidationText
		{
			get => BlockDefinition.ValidationText;
			set => BlockDefinition.ValidationText = value ?? String.Empty;
		}

		/// <inheritdoc />
		public void ForceSelected(string selected)
		{
			BlockDefinition.InitialValue = selected ?? String.Empty;
		}

		/// <inheritdoc />
		protected internal override void LoadResult(UIResults results)
		{
			string selectedValue = results.GetString(this);
			if (WantsOnChange)
			{
				changed = selectedValue != SelectedText;
			}

			previous = SelectedText;

			// Write to BlockDefinition instead of Selected property so a force selected option does not reset after every interaction
			BlockDefinition.InitialValue = selectedValue;
		}

		/// <inheritdoc />
		protected internal override void RaiseResultEvents()
		{
			if (changed && OnChanged != null)
			{
				int index = dropdownOptionsList.IndexOfText(previous);
				Option<T> previousOption = default;
				if (index != -1)
				{
					previousOption = dropdownOptionsList[index];
				}

				OnChanged(this, new ChangedEventArgs(Selected, previousOption));
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
			/// <param name="selectedOption">The option that has been selected.</param>
			/// <param name="previousOption">the option that was previously selected.</param>
			public ChangedEventArgs(Option<T> selectedOption, Option<T> previousOption)
			{
				SelectedOption = selectedOption;
				PreviousOption = previousOption;
			}

			/// <summary>
			///     Gets the option that has been selected.
			/// </summary>
			public Option<T> SelectedOption { get; }

			/// <summary>
			///     Gets the previously selected option.
			/// </summary>
			public Option<T> PreviousOption { get; }
		}
	}

	/// <inheritdoc />
	internal class DropdownOptionsList<T> : OptionsList<T>
	{
		private readonly IDropDown<T> dropDown;

		/// <summary>
		/// Initializes a new instance of the <see cref="DropdownOptionsList{T}"/> class.
		/// </summary>
		/// <param name="dropDown">The dropdown widget for this collection.</param>
		public DropdownOptionsList(IDropDown<T> dropDown) :
			base(dropDown.BlockDefinition.GetOptionsCollection()) => this.dropDown = dropDown;

		/// <inheritdoc/>
		public override Option<T> this[int index]
		{
			get => base[index];

			set
			{
				base[index] = value;

				if (dropDown.SelectedText == (value.Text ?? String.Empty))
				{
					dropDown.SelectedText = this.FirstOrDefault().Text;
				}

				// Cube will select the first option even if UIBlockDefinition.InitialValue is null.
				// But I believe this behavior should be reflected by the Selected property.
				// Selected should never be null if options is not empty.
				if (dropDown.SelectedText == null)
				{
					dropDown.SelectedText = value.Text ?? String.Empty;
				}
			}
		}

		/// <inheritdoc/>
		public override void Add(string text, T value)
		{
			base.Add(text, value);

			// Cube will select the first option even if UIBlockDefinition.InitialValue is null.
			// But I believe this behavior should be reflected by the Selected property.
			// Selected should never be null if options is not empty.
			if (dropDown.SelectedText == null)
			{
				dropDown.SelectedText = text ?? String.Empty;
			}
		}

		/// <inheritdoc/>
		public override void Clear()
		{
			base.Clear();
			dropDown.BlockDefinition.InitialValue = null;
		}

		/// <inheritdoc/>
		public override void Insert(int index, string text, T value)
		{
			base.Insert(index, text, value);

			// Cube will select the first option even if UIBlockDefinition.InitialValue is null.
			// But I believe this behavior should be reflected by the Selected property.
			// Selected should never be null if options is not empty.
			if (dropDown.SelectedText == null)
			{
				dropDown.SelectedText = text;
			}
		}

		/// <inheritdoc/>
		public override void RemoveAt(int index)
		{
			string option = this[index].Text;
			base.RemoveAt(index);
			if (dropDown.SelectedText == option)
			{
				dropDown.BlockDefinition.InitialValue = this.FirstOrDefault().Text;
			}
		}
	}
}