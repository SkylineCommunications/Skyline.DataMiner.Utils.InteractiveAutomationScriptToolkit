namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <inheritdoc cref="Skyline.DataMiner.InteractiveAutomationToolkit.IRadioButtonList{T}" />
	public class RadioButtonList<T> : InteractiveWidget, IRadioButtonList<T>
	{
		private readonly Validation validation;
		private readonly RadioButtonOptionsList<T> optionsCollection;
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
		/// <param name="options">Name of options that can be selected.</param>
		public RadioButtonList(IEnumerable<Option<T>> options)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			Type = UIBlockType.RadioButtonList;
			validation = new Validation(this);
			optionsCollection = new RadioButtonOptionsList<T>(this);

			optionsCollection.AddRange(options);
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
		public IOptionsList<T> Options => optionsCollection;

		/// <inheritdoc />
		public bool IsSorted
		{
			get => BlockDefinition.IsSorted;
			set => BlockDefinition.IsSorted = value;
		}

		/// <inheritdoc/>
		public Option<T> Selected
		{
			get => SelectedText == null ? default : optionsCollection[optionsCollection.IndexOfText(SelectedText)];

			set
			{
				if (value == default)
				{
					SelectedText = null;
					return;
				}

				if (!optionsCollection.Contains(value))
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
				if (value == null)
				{
					BlockDefinition.InitialValue = null;
					return;
				}

				if (Options.ContainsText(value))
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
				int index = optionsCollection.IndexOfText(SelectedText);
				return index == -1 ? default : optionsCollection[index].Value;
			}

			set
			{
				if (EqualityComparer<T>.Default.Equals(value, default))
				{
					SelectedText = null;
					return;
				}

				int index = optionsCollection.IndexOfValue(value);
				if (index == -1)
				{
					return;
				}

				SelectedText = optionsCollection[index].Text;
			}
		}

		/// <inheritdoc />
		public string Tooltip
		{
			get => BlockDefinition.TooltipText;
			set => BlockDefinition.TooltipText = value ?? String.Empty;
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
				if (!String.IsNullOrEmpty(checkedOption) && checkedOption != SelectedText)
				{
					previous = SelectedText;
					SelectedText = checkedOption;
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
				int index = optionsCollection.IndexOfText(previous);
				Option<T> previousOption = default;
				if (index != -1)
				{
					previousOption = optionsCollection[index];
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
	internal class RadioButtonOptionsList<T> : OptionsList<T>
	{
		private readonly IRadioButtonList<T> radioButtonList;

		/// <summary>
		/// Initializes a new instance of the <see cref="RadioButtonOptionsList{T}"/> class.
		/// </summary>
		/// <param name="radioButtonList">The radio button list widget for this collection.</param>
		public RadioButtonOptionsList(IRadioButtonList<T> radioButtonList) :
			base(radioButtonList.BlockDefinition.GetOptionsCollection()) => this.radioButtonList = radioButtonList;

		/// <inheritdoc/>
		public override Option<T> this[int index]
		{
			get => base[index];

			set
			{
				Option<T> replacedOption = base[index];
				base[index] = value;

				if (radioButtonList.SelectedText == replacedOption.Text)
				{
					radioButtonList.SelectedText = null;
				}
			}
		}

		/// <inheritdoc/>
		public override void Clear()
		{
			base.Clear();
			radioButtonList.SelectedText = null;
		}

		/// <inheritdoc/>
		public override void RemoveAt(int index)
		{
			Option<T> removedOption = this[index];
			base.RemoveAt(index);
			if (radioButtonList.SelectedText == removedOption.Text)
			{
				radioButtonList.SelectedText = null;
			}
		}
	}
}