namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <inheritdoc cref="ICheckBoxList{T}" />
	public class CheckBoxList<T> : InteractiveWidget, ICheckBoxList<T>
	{
		private readonly Validation validation;
		private readonly CheckBoxListCollection<T> optionsCollection;

		private bool changed;
		private Option<T> changedOption;
		private bool changedValue;

		/// <summary>
		///     Initializes a new instance of the <see cref="CheckBoxList{T}"/> class.
		/// </summary>
		public CheckBoxList()
			: this(Enumerable.Empty<Option<T>>())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="CheckBoxList{T}"/> class.
		/// </summary>
		/// <param name="options">Name of options that can be selected.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public CheckBoxList(IEnumerable<Option<T>> options)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			Type = UIBlockType.CheckBoxList;
			validation = new Validation(this);
			optionsCollection = new CheckBoxListCollection<T>(this);
			optionsCollection.AddRange(options);

			CheckedOptions = new CheckedOptionCollection<T>(this);
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
		ICollection<Option<T>> ICheckBoxList<T>.CheckedOptions => CheckedOptions;

		/// <summary>
		/// 	Gets all checked options.
		/// </summary>
		public CheckedOptionCollection<T> CheckedOptions { get; }

		/// <inheritdoc />
		IList<Option<T>> ICheckBoxList<T>.Options => optionsCollection;

		/// <summary>
		/// 	Gets all options.
		/// </summary>
		public OptionList<T> Options => optionsCollection;

		/// <inheritdoc />
		public bool IsSorted
		{
			get => BlockDefinition.IsSorted;
			set => BlockDefinition.IsSorted = value;
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
		protected internal override void LoadResult(UIResults results)
		{
			string result = results.GetString(this);
			if (result == null)
			{
				// results can be null if the list of options is empty
				BlockDefinition.InitialValue = String.Empty;
				return;
			}

			var checkedOptions = new HashSet<string>(result.Split(';'));
			foreach (Option<T> option in Options)
			{
				bool isChecked = checkedOptions.Contains(option.Name);
				bool hasChanged = CheckedOptions.Contains(option) != isChecked;

				if (isChecked)
				{
					CheckedOptions.Add(option);
				}
				else
				{
					CheckedOptions.Remove(option);
				}

				if (hasChanged && WantsOnChange)
				{
					changed = true;
					changedOption = option;
					changedValue = isChecked;

					// only a single checkbox can be toggled when WantsOnChange is true
					break;
				}
			}
		}

		/// <inheritdoc />
		protected internal override void RaiseResultEvents()
		{
			if (changed && OnChanged != null)
			{
				OnChanged(this, new ChangedEventArgs(changedOption, changedValue));
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
			/// <param name="option">The option of which the state has changed.</param>
			/// <param name="isChecked">The new checked state of that option.</param>
			public ChangedEventArgs(Option<T> option, bool isChecked)
			{
				Option = option;
				IsChecked = isChecked;
			}

			/// <summary>
			///     Gets a value indicating whether the checkbox has been selected.
			/// </summary>
			public bool IsChecked { get; }

			/// <summary>
			///     Gets the option of which the state has changed.
			/// </summary>
			public Option<T> Option { get; }
		}
	}

	/// <inheritdoc />
	internal class CheckBoxListCollection<T> : OptionList<T>
	{
		private readonly ICheckBoxList<T> checkBoxList;

		/// <summary>
		/// Initializes a new instance of the <see cref="CheckBoxListCollection{T}"/> class.
		/// </summary>
		/// <param name="checkBoxList">The checkbox list widget for this collection.</param>
		public CheckBoxListCollection(ICheckBoxList<T> checkBoxList) :
			base(checkBoxList.BlockDefinition.GetOptionsCollection()) => this.checkBoxList = checkBoxList;

		/// <inheritdoc/>
		public override Option<T> this[int index]
		{
			get => base[index];

			set
			{
				Option<T> replaced = base[index];
				base[index] = value;
				checkBoxList.CheckedOptions.Remove(replaced);
			}
		}

		/// <inheritdoc/>
		public override void Clear()
		{
			checkBoxList.CheckedOptions.Clear();
			base.Clear();
		}

		/// <inheritdoc/>
		public override void RemoveAt(int index)
		{
			checkBoxList.CheckedOptions.Remove(this[index]);
			base.RemoveAt(index);
		}
	}
}