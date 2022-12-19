namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <inheritdoc cref="Skyline.DataMiner.InteractiveAutomationToolkit.ICheckBoxList{T}" />
	public class CheckBoxList<T> : InteractiveWidget, ICheckBoxList<T>
	{
		private readonly Validation validation;
		private readonly CheckedOptionCollection<T> checkedCollection;
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

			checkedCollection = new CheckedOptionCollection<T>(optionsCollection);
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
		public ICheckedOptionCollection<T> Checked => checkedCollection;

		/// <inheritdoc />
		public IOptionsList<T> Options => optionsCollection;

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
		public void CheckAll()
		{
			Checked.AddRange(Options);
		}

		/// <inheritdoc />
		public void UncheckAll()
		{
			Checked.Clear();
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
				bool isChecked = checkedOptions.Contains(option.Text);
				bool hasChanged = Checked.Contains(option) != isChecked;

				if (isChecked)
				{
					Checked.Add(option);
				}
				else
				{
					Checked.Remove(option);
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
	internal class CheckBoxListCollection<T> : OptionsList<T>
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
				checkBoxList.Checked.Remove(base[index]);
				base[index] = value;
			}
		}

		/// <inheritdoc/>
		public override void Clear()
		{
			checkBoxList.Checked.Clear();
			base.Clear();
		}

		/// <inheritdoc/>
		public override void RemoveAt(int index)
		{
			checkBoxList.Checked.Remove(this[index]);
			base.RemoveAt(index);
		}
	}

	/// <inheritdoc />
	internal class CheckedOptionCollection<T> : ICheckedOptionCollection<T>
	{
		private readonly HashSet<Option<T>> checkedOptions = new HashSet<Option<T>>();
		private readonly IOptionsList<T> optionsList;

		/// <summary>
		/// Initializes a new instance of the <see cref="CheckedOptionCollection{T}"/> class.
		/// </summary>
		/// <param name="optionsList">The options list of the checkbox list.</param>
		public CheckedOptionCollection(IOptionsList<T> optionsList) => this.optionsList = optionsList;

		/// <inheritdoc/>
		public int Count => checkedOptions.Count;

		/// <inheritdoc/>
		public bool IsReadOnly => false;

		/// <inheritdoc/>
		public IEnumerator<Option<T>> GetEnumerator()
		{
			return checkedOptions.GetEnumerator();
		}

		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)checkedOptions).GetEnumerator();
		}

		/// <inheritdoc/>
		public void Add(Option<T> item)
		{
			checkedOptions.Add(item);
		}

		/// <inheritdoc/>
		public void AddText(string text)
		{
			int index = optionsList.IndexOfText(text);
			if (index == -1)
			{
				return;
			}

			Add(optionsList[index]);
		}

		/// <inheritdoc/>
		public void AddValue(T value)
		{
			int index = optionsList.IndexOfValue(value);
			if (index == -1)
			{
				return;
			}

			Add(optionsList[index]);
		}

		/// <inheritdoc/>
		public void AddRange(IEnumerable<Option<T>> options)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			foreach (Option<T> option in options)
			{
				Add(option);
			}
		}

		/// <inheritdoc/>
		public void AddTextRange(IEnumerable<string> texts)
		{
			if (texts == null)
			{
				throw new ArgumentNullException(nameof(texts));
			}

			foreach (string text in texts)
			{
				AddText(text);
			}
		}

		/// <inheritdoc/>
		public void AddValueRange(IEnumerable<T> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException(nameof(values));
			}

			foreach (T value in values)
			{
				AddValue(value);
			}
		}

		/// <inheritdoc/>
		public void Clear()
		{
			checkedOptions.Clear();
		}

		/// <inheritdoc/>
		public bool Contains(Option<T> item)
		{
			return checkedOptions.Contains(item);
		}

		/// <inheritdoc/>
		public bool ContainsText(string text)
		{
			return optionsList.Any(option => option.Text == text);
		}

		/// <inheritdoc/>
		public bool ContainsValue(T value)
		{
			return optionsList.Any(option => Equals(option.Value, value));
		}

		/// <inheritdoc/>
		public void CopyTo(Option<T>[] array, int arrayIndex)
		{
			checkedOptions.CopyTo(array, arrayIndex);
		}

		/// <inheritdoc/>
		public bool Remove(Option<T> item)
		{
			return checkedOptions.Remove(item);
		}

		/// <inheritdoc/>
		public void RemoveText(string text)
		{
			int index = optionsList.IndexOfText(text);
			if (index == -1)
			{
				return;
			}

			Remove(optionsList[index]);
		}

		/// <inheritdoc/>
		public void RemoveValue(T value)
		{
			int index = optionsList.IndexOfValue(value);
			if (index == -1)
			{
				return;
			}

			Remove(optionsList[index]);
		}
	}
}