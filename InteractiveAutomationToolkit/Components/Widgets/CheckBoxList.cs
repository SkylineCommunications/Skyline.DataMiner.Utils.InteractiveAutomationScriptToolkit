namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     A list of checkboxes.
	/// </summary>
	public class CheckBoxList : InteractiveWidget, ICheckBoxList
	{
		private readonly CheckedOptionCollection checkedOptionCollection;
		private readonly CheckBoxOptionList checkBoxOptionList;

		private bool changed;
		private string changedOption;
		private bool changedValue;

		/// <summary>
		///     Initializes a new instance of the <see cref="CheckBoxList" /> class.
		/// </summary>
		public CheckBoxList()
			: this(Enumerable.Empty<string>())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="CheckBoxList" /> class.
		/// </summary>
		/// <param name="options">Name of options that can be selected.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public CheckBoxList(IEnumerable<string> options)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			Type = UIBlockType.CheckBoxList;
			checkBoxOptionList = new CheckBoxOptionList(this);
			checkedOptionCollection = new CheckedOptionCollection(this);

			SetOptions(options);
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
		[Obsolete("Use CheckedOptions instead.")]
		ICollection<string> ICheckBoxList.Checked => checkedOptionCollection;

		/// <summary>
		/// 	Gets all selected options.
		/// </summary>
		[Obsolete("Use CheckedOptions instead.")]
		public CheckedOptionCollection Checked => checkedOptionCollection;

		/// <inheritdoc />
		ICollection<string> ICheckBoxList.CheckedOptions => checkedOptionCollection;

		/// <summary>
		/// 	Gets all selected options.
		/// </summary>
		public CheckedOptionCollection CheckedOptions => checkedOptionCollection;

		/// <inheritdoc />
		IList<string> ICheckBoxList.Options => checkBoxOptionList;

		/// <summary>
		/// 	Gets all options.
		/// </summary>
		public OptionList Options => checkBoxOptionList;

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

		/// <inheritdoc />
		public void AddOption(string option)
		{
			Options.Add(option ?? String.Empty);
		}

		/// <inheritdoc />
		public void Check(string option)
		{
			CheckedOptions.Add(option ?? String.Empty);
		}

		/// <inheritdoc />
		public void CheckAll()
		{
			CheckedOptions.AddRange(Options);
		}

		/// <inheritdoc />
		public void RemoveOption(string option)
		{
			Options.Remove(option ?? String.Empty);
		}

		/// <inheritdoc />
		public void SetOptions(IEnumerable<string> options)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			Options.Clear();
			foreach (string option in options)
			{
				Options.Add(option ?? String.Empty);
			}
		}

		/// <inheritdoc />
		public void Uncheck(string option)
		{
			CheckedOptions.Remove(option);
		}

		/// <inheritdoc />
		public void UncheckAll()
		{
			CheckedOptions.Clear();
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
			foreach (string option in Options)
			{
				bool isChecked = checkedOptions.Contains(option);
				bool hasChanged = CheckedOptions.Contains(option) != isChecked;

				if (isChecked)
				{
					Check(option);
				}
				else
				{
					Uncheck(option);
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
			public ChangedEventArgs(string option, bool isChecked)
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
			public string Option { get; }
		}

		private class CheckBoxOptionList : OptionList
		{
			private readonly CheckBoxList checkBoxList;

			public CheckBoxOptionList(CheckBoxList checkBoxList)
				: base(checkBoxList.BlockDefinition.GetOptionsCollection())
			{
				this.checkBoxList = checkBoxList;
			}

			public override string this[int index]
			{
				get => base[index];

				set
				{
					string replacedOption = base[index];
					base[index] = value;
					checkBoxList.CheckedOptions.Remove(replacedOption);
				}
			}

			public override void Clear()
			{
				base.Clear();
				checkBoxList.CheckedOptions.Clear();
			}

			public override bool Remove(string item)
			{
				if (!base.Remove(item))
				{
					return false;
				}

				checkBoxList.CheckedOptions.Remove(item);
				return true;
			}

			public override void RemoveAt(int index)
			{
				checkBoxList.CheckedOptions.Remove(this[index]);
				base.RemoveAt(index);
			}
		}
	}

	/// <summary>
	///     Collection of checked options in a <see cref="CheckBoxList" />.
	/// </summary>
	public class CheckedOptionCollection : ICollection<string>, IReadOnlyCollection<string>
	{
		private readonly HashSet<string> @checked = new HashSet<string>();
		private readonly CheckBoxList checkBoxList;

		/// <summary>
		/// Initializes a new instance of the <see cref="CheckedOptionCollection"/> class.
		/// </summary>
		/// <param name="checkBoxList">The checkbox list widget for this collection.</param>
		public CheckedOptionCollection(CheckBoxList checkBoxList) => this.checkBoxList = checkBoxList;

		/// <inheritdoc cref="ICollection{T}.Count" />
		public int Count => @checked.Count;

		/// <inheritdoc/>
		public bool IsReadOnly => ((ICollection<string>)@checked).IsReadOnly;

		/// <inheritdoc/>
		public void Add(string item)
		{
			if (checkBoxList.Options.Contains(item) && @checked.Add(item))
			{
				checkBoxList.BlockDefinition.InitialValue = String.Join(";", @checked);
			}
		}

		/// <summary>
		/// 	Adds the elements of the specified collection to the end of the <see cref="CheckedOptionCollection"/>.
		/// </summary>
		/// <param name="items">The collection whose elements should be added to the end of the <see cref="CheckedOptionCollection"/>.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="items"/> is <c>null</c>.</exception>
		public void AddRange(IEnumerable<string> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException(nameof(items));
			}

			foreach (string item in items)
			{
				Add(item);
			}
		}

		/// <inheritdoc/>
		public void Clear()
		{
			@checked.Clear();
			checkBoxList.BlockDefinition.InitialValue = String.Empty;
		}

		/// <inheritdoc/>
		public bool Contains(string item)
		{
			return @checked.Contains(item);
		}

		/// <inheritdoc/>
		public void CopyTo(string[] array, int arrayIndex)
		{
			@checked.CopyTo(array, arrayIndex);
		}

		/// <inheritdoc/>
		public IEnumerator<string> GetEnumerator()
		{
			return @checked.GetEnumerator();
		}

		/// <inheritdoc/>
		public bool Remove(string item)
		{
			if (@checked.Remove(item))
			{
				checkBoxList.BlockDefinition.InitialValue = String.Join(";", @checked);
				return true;
			}

			return false;
		}

		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)@checked).GetEnumerator();
		}
	}
}