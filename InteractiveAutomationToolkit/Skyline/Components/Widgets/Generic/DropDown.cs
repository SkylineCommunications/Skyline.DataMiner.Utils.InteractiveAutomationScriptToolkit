namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <inheritdoc cref="IDropDown{T}" />
	public class DropDown<T> : InteractiveWidget, IDropDown<T>
	{
		private readonly Validation validation;
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
			validation = new Validation(this);
			dropdownOptionsList = new DropdownOptionsList<T>(this);

			Options.AddRange(options);
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
			get => SelectedName == null ? default : new Option<T>(SelectedName, SelectedValue);

			set
			{
				if (!dropdownOptionsList.Contains(value))
				{
					return;
				}

				SelectedName = value.Name;
			}
		}

		/// <inheritdoc />
		public string SelectedName
		{
			get => BlockDefinition.InitialValue;

			set
			{
				if (value == null)
				{
					if (Options.Any())
					{
						// Unlike RadioButtonList, setting null as initial value will cause cube to display the first option
						// however, the UIResult will report null instead of the first option.
						// This feels weird, so we have decided to not allow null for simplicity.
						throw new ArgumentNullException(nameof(value));
					}

					BlockDefinition.InitialValue = null;
				}

				// Prevent setting a value that is not part of the options
				if (Options.ContainsName(value))
				{
					BlockDefinition.InitialValue = value;
				}
			}
		}

		/// <inheritdoc />
		public T SelectedValue
		{
			get
			{
				int index = dropdownOptionsList.IndexOfName(SelectedName);
				return index == -1 ? default : dropdownOptionsList[index].Value;
			}

			set
			{
				int index = dropdownOptionsList.IndexOfValue(value);
				if (index == -1)
				{
					return;
				}

				SelectedName = dropdownOptionsList[index].Name;
			}
		}

		/// <inheritdoc />
		public string Tooltip
		{
			get => BlockDefinition.TooltipText;
			set => BlockDefinition.TooltipText = value;
		}

		/// <inheritdoc/>
		public UIValidationState ValidationState
		{
			get => validation.ValidationState;

			set => validation.ValidationState = value;
		}

		/// <inheritdoc/>
		public string ValidationText
		{
			get => validation.ValidationText;

			set => validation.ValidationText = value;
		}

		/// <inheritdoc />
		public void ForceSelected(string selected)
		{
			BlockDefinition.InitialValue = selected;
		}

		/// <inheritdoc />
		protected internal override void LoadResult(UIResults results)
		{
			string selectedValue = results.GetString(this);
			if (WantsOnChange)
			{
				changed = selectedValue != SelectedName;
			}

			previous = SelectedName;

			// Write to BlockDefinition instead of Selected property so a force selected option does not reset after every interaction
			BlockDefinition.InitialValue = selectedValue;
		}

		/// <inheritdoc />
		protected internal override void RaiseResultEvents()
		{
			if (changed && OnChanged != null)
			{
				int index = dropdownOptionsList.IndexOfName(previous);
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
				Option<T> replacedOption = base[index];

				base[index] = value;

				if (dropDown.SelectedName == replacedOption.Name)
				{
					dropDown.BlockDefinition.InitialValue = value.Name;
				}

				// Cube will select the first option even if UIBlockDefinition.InitialValue is null.
				// But I believe this behavior should be reflected by the Selected property.
				// Selected should never be null if options is not empty.
				if (dropDown.SelectedName == null)
				{
					dropDown.SelectedName = value.Name;
				}
			}
		}

		/// <inheritdoc/>
		public override void Add(string name, T value)
		{
			base.Add(name, value);

			// Cube will select the first option even if UIBlockDefinition.InitialValue is null.
			// But I believe this behavior should be reflected by the Selected property.
			// Selected should never be null if options is not empty.
			if (dropDown.SelectedName == null)
			{
				dropDown.BlockDefinition.InitialValue = name;
			}
		}

		/// <inheritdoc/>
		public override void Clear()
		{
			base.Clear();
			dropDown.BlockDefinition.InitialValue = null;
		}

		/// <inheritdoc/>
		public override void Insert(int index, string name, T value)
		{
			base.Insert(index, name, value);

			// Cube will select the first option even if UIBlockDefinition.InitialValue is null.
			// But I believe this behavior should be reflected by the Selected property.
			// Selected should never be null if options is not empty.
			if (dropDown.SelectedName == null)
			{
				dropDown.BlockDefinition.InitialValue = name;
			}
		}

		/// <inheritdoc/>
		public override void RemoveAt(int index)
		{
			string option = this[index].Name;
			base.RemoveAt(index);
			if (dropDown.SelectedName == option)
			{
				dropDown.BlockDefinition.InitialValue = this.FirstOrDefault()?.Name;
			}
		}
	}
}