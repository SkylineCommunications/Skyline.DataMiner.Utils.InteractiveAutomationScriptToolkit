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

			CheckedOptions = new CheckedOptionCollection<T>(optionsCollection);
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
		public CheckedOptionCollection<T> CheckedOptions { get; }

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
					CheckedOptions.Check(option);
				}
				else
				{
					CheckedOptions.Uncheck(option);
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

		/// <summary>
		/// Collection of checked options in a <see cref="ICheckBoxList{T}"/>.
		/// </summary>
		/// <typeparam name="TValue">The type of the value of the options.</typeparam>
		public class CheckedOptionCollection<TValue> : ICollection<Option<TValue>>, IReadOnlyCollection<Option<TValue>>
		{
			private readonly HashSet<Option<TValue>> checkedOptions = new HashSet<Option<TValue>>();
			private readonly IOptionsList<TValue> optionsList;

			/// <summary>
			/// Initializes a new instance of the <see cref="CheckedOptionCollection{T}"/> class.
			/// </summary>
			/// <param name="optionsList">The options list of the checkbox list.</param>
			public CheckedOptionCollection(IOptionsList<TValue> optionsList) => this.optionsList = optionsList;

			/// <inheritdoc cref="ICollection{T}.Count" />
			public virtual int Count => checkedOptions.Count;

			/// <inheritdoc/>
			bool ICollection<Option<TValue>>.IsReadOnly => false;

			/// <inheritdoc/>
			public virtual IEnumerator<Option<TValue>> GetEnumerator()
			{
				return checkedOptions.GetEnumerator();
			}

			/// <inheritdoc/>
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable)checkedOptions).GetEnumerator();
			}

			/// <inheritdoc/>
			void ICollection<Option<TValue>>.Add(Option<TValue> item)
			{
				Check(item);
			}

			/// <summary>
			/// Checks the specified option.
			/// </summary>
			/// <param name="item">The option to be checked.</param>
			public virtual void Check(Option<TValue> item)
			{
				if (optionsList.Contains(item))
				{
					checkedOptions.Add(item);
				}
			}

			/// <summary>
			/// Checks the option with specified name.
			/// </summary>
			/// <param name="name">The option with specified name to be checked.</param>
			public virtual void CheckName(string name)
			{
				int index = optionsList.IndexOfName(name);
				if (index == -1)
				{
					return;
				}

				checkedOptions.Add(optionsList[index]);
			}

			/// <summary>
			/// Checks the option with specified value.
			/// </summary>
			/// <param name="value">The options with specified value to be checked.</param>
			public virtual void CheckValue(TValue value)
			{
				int index = optionsList.IndexOfValue(value);
				if (index == -1)
				{
					return;
				}

				checkedOptions.Add(optionsList[index]);
			}

			/// <summary>
			/// Checks all options of the specified collection.
			/// </summary>
			/// <param name="options">The collections of options to be checked.</param>
			public virtual void CheckRange(IEnumerable<Option<TValue>> options)
			{
				if (options == null)
				{
					throw new ArgumentNullException(nameof(options));
				}

				foreach (Option<TValue> option in options)
				{
					Check(option);
				}
			}

			/// <summary>
			/// Checks all options with the specified names.
			/// </summary>
			/// <param name="names">The collections of names of options to be checked.</param>
			public virtual void CheckNameRange(IEnumerable<string> names)
			{
				if (names == null)
				{
					throw new ArgumentNullException(nameof(names));
				}

				foreach (string text in names)
				{
					CheckName(text);
				}
			}

			/// <summary>
			/// Checks all options with the specified values.
			/// </summary>
			/// <param name="values">The collections of values of options to be checked.</param>
			public virtual void CheckValueRange(IEnumerable<TValue> values)
			{
				if (values == null)
				{
					throw new ArgumentNullException(nameof(values));
				}

				foreach (TValue value in values)
				{
					CheckValue(value);
				}
			}

			/// <summary>
			/// Checks all options.
			/// </summary>
			public virtual void CheckAll()
			{
				CheckRange(optionsList);
			}

			/// <inheritdoc/>
			void ICollection<Option<TValue>>.Clear()
			{
				checkedOptions.Clear();
			}

			/// <summary>
			/// Unchecks all options.
			/// </summary>
			public virtual void UncheckAll()
			{
				checkedOptions.Clear();
			}

			/// <inheritdoc/>
			bool ICollection<Option<TValue>>.Contains(Option<TValue> item)
			{
				return checkedOptions.Contains(item);
			}

			/// <summary>
			/// Determines whether an option is checked.
			/// </summary>
			/// <param name="item">The option to be determined if checked.</param>
			/// <returns><c>true</c> if the option is checked; otherwise <c>false</c>.</returns>
			public virtual bool IsChecked(Option<TValue> item)
			{
				return checkedOptions.Contains(item);
			}

			/// <summary>
			/// Determines whether an option with specified name is checked.
			/// </summary>
			/// <param name="name">The name of the option to be determined if checked.</param>
			/// <returns><c>true</c> if the option is checked; otherwise <c>false</c>.</returns>
			public virtual bool IsNameChecked(string name)
			{
				return optionsList.Any(option => option.Name == name);
			}

			/// <summary>
			/// Determines whether an option with specified value is checked.
			/// </summary>
			/// <param name="value">The value of the option to be determined if checked.</param>
			/// <returns><c>true</c> if the option is checked; otherwise <c>false</c>.</returns>
			public virtual bool IsValueChecked(T value)
			{
				return optionsList.Any(option => Equals(option.Value, value));
			}

			/// <inheritdoc/>
			void ICollection<Option<TValue>>.CopyTo(Option<TValue>[] array, int arrayIndex)
			{
				checkedOptions.CopyTo(array, arrayIndex);
			}

			/// <summary>
			/// Unchecks the specified option.
			/// </summary>
			/// <param name="item">The option to be unchecked.</param>
			public virtual void Uncheck(Option<TValue> item)
			{
				checkedOptions.Remove(item);
			}

			/// <inheritdoc/>
			bool ICollection<Option<TValue>>.Remove(Option<TValue> item)
			{
				return checkedOptions.Remove(item);
			}

			/// <summary>
			/// Unchecks the option with specified name.
			/// </summary>
			/// <param name="name">The option with specified name to be checked.</param>
			public void UncheckName(string name)
			{
				int index = optionsList.IndexOfName(name);
				if (index == -1)
				{
					return;
				}

				Uncheck(optionsList[index]);
			}

			/// <summary>
			/// Unchecks the option with specified value.
			/// </summary>
			/// <param name="value">The option with specified value to be checked.</param>
			public void UncheckValue(TValue value)
			{
				int index = optionsList.IndexOfValue(value);
				if (index == -1)
				{
					return;
				}

				Uncheck(optionsList[index]);
			}
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
				Option<T> replaced = base[index];
				base[index] = value;
				checkBoxList.CheckedOptions.Uncheck(replaced);
			}
		}

		/// <inheritdoc/>
		public override void Clear()
		{
			checkBoxList.CheckedOptions.UncheckAll();
			base.Clear();
		}

		/// <inheritdoc/>
		public override void RemoveAt(int index)
		{
			checkBoxList.CheckedOptions.Uncheck(this[index]);
			base.RemoveAt(index);
		}
	}
}