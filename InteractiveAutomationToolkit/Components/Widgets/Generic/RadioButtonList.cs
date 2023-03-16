namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <inheritdoc cref="IRadioButtonList{T}" />
	public class RadioButtonList<T> : InteractiveWidget, IRadioButtonList<T>
	{
		private readonly Validation validation;
		private readonly RadioButtonOptionList<T> optionCollection;
		private bool changed;
		private string previous;

		/// <summary>
		///     Initializes a new instance of the <see cref="RadioButtonList{T}"/> class.
		/// </summary>
		public RadioButtonList()
			: this(Enumerable.Empty<Option<T>>())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="RadioButtonList{T}"/> class.
		/// </summary>
		/// <param name="options">Options to be displayed in the list.</param>
		public RadioButtonList(IEnumerable<Option<T>> options)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			Type = UIBlockType.RadioButtonList;
			validation = new Validation(this);
			optionCollection = new RadioButtonOptionList<T>(this);

			optionCollection.AddRange(options);
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
		IList<Option<T>> IRadioButtonList<T>.Options => optionCollection;

		/// <summary>
		/// 	Gets all options.
		/// </summary>
		public OptionList<T> Options => optionCollection;

		/// <inheritdoc />
		public bool IsSorted
		{
			get => BlockDefinition.IsSorted;
			set => BlockDefinition.IsSorted = value;
		}

		/// <inheritdoc/>
		public Option<T> Selected
		{
			get => SelectedName == null ? default : optionCollection[optionCollection.IndexOfName(SelectedName)];

			set
			{
				if (value == default)
				{
					SelectedName = null;
					return;
				}

				if (optionCollection.Contains(value))
				{
					SelectedName = value.Name;
				}
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
					BlockDefinition.InitialValue = null;
					return;
				}

				if (Options.ContainsName(value))
				{
					BlockDefinition.InitialValue = value;
				}
			}
		}

		/// <inheritdoc/>
		public T SelectedValue
		{
			get
			{
				int index = optionCollection.IndexOfName(SelectedName);
				return index == -1 ? default : optionCollection[index].Value;
			}

			set
			{
				int index = optionCollection.IndexOfValue(value);
				if (index != -1)
				{
					SelectedName = optionCollection[index].Name;
					return;
				}

				if (EqualityComparer<T>.Default.Equals(value, default))
				{
					SelectedName = null;
				}
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
		protected internal override void LoadResult(UIResults results)
		{
			string result = results.GetString(this);
			if (String.IsNullOrWhiteSpace(result))
			{
				return;
			}

			string[] checkedOptions = result.Split(';');
			foreach (string checkedOption in checkedOptions)
			{
				if (!String.IsNullOrEmpty(checkedOption) && checkedOption != SelectedName)
				{
					previous = SelectedName;
					SelectedName = checkedOption;
					changed = true;
					break;
				}
			}
		}

		/// <inheritdoc />
		protected internal override void RaiseResultEvents()
		{
			if (changed && OnChanged != null)
			{
				int index = optionCollection.IndexOfName(previous);
				Option<T> previousOption = default;
				if (index != -1)
				{
					previousOption = optionCollection[index];
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
			/// <param name="selectedValue">The option that has been selected.</param>
			/// <param name="previous">The previously selected option.</param>
			public ChangedEventArgs(Option<T> selectedValue, Option<T> previous)
			{
				SelectedValue = selectedValue;
				Previous = previous;
			}

			/// <summary>
			///     Gets the previously selected option.
			/// </summary>
			public Option<T> Previous { get; }

			/// <summary>
			///     Gets the option that has been selected.
			/// </summary>
			public Option<T> SelectedValue { get; }
		}
	}

	/// <inheritdoc />
	internal class RadioButtonOptionList<T> : OptionList<T>
	{
		private readonly IRadioButtonList<T> radioButtonList;

		/// <summary>
		/// Initializes a new instance of the <see cref="RadioButtonOptionList{T}"/> class.
		/// </summary>
		/// <param name="radioButtonList">The radio button list widget for this collection.</param>
		public RadioButtonOptionList(IRadioButtonList<T> radioButtonList) :
			base(radioButtonList.BlockDefinition.GetOptionsCollection()) => this.radioButtonList = radioButtonList;

		/// <inheritdoc/>
		public override Option<T> this[int index]
		{
			get => base[index];

			set
			{
				Option<T> replacedOption = base[index];
				base[index] = value;

				if (radioButtonList.SelectedName == replacedOption.Name)
				{
					radioButtonList.SelectedName = value.Name;
				}
			}
		}

		/// <inheritdoc/>
		public override void Clear()
		{
			base.Clear();
			radioButtonList.SelectedName = null;
		}

		/// <inheritdoc/>
		public override void RemoveAt(int index)
		{
			Option<T> removedOption = this[index];
			base.RemoveAt(index);
			if (radioButtonList.SelectedName == removedOption.Name)
			{
				radioButtonList.SelectedName = null;
			}
		}
	}
}