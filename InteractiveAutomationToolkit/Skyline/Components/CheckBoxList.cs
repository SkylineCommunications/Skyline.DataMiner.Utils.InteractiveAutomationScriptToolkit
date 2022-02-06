namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	using Automation;

	/// <summary>
	///     A list of checkboxes.
	/// </summary>
	public class CheckBoxList : InteractiveWidget, ICheckBoxList
	{
		private readonly OptionsCollection optionsCollection;
		private readonly CheckedCollection checkedCollection;
		private readonly UnCheckedCollection unCheckedCollection;

		private bool changed;
		private string changedOption;
		private bool changedValue;

		/// <summary>
		///     Initializes a new instance of the <see cref="CheckBoxList" /> class.
		/// </summary>
		public CheckBoxList() : this(Enumerable.Empty<string>())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="CheckBoxList" /> class.
		/// </summary>
		/// <param name="options">Name of options that can be selected.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public CheckBoxList(IEnumerable<string> options)
		{
			if (options == null) throw new ArgumentNullException(nameof(options));

			Type = UIBlockType.CheckBoxList;
			optionsCollection = new OptionsCollection(this);
			checkedCollection = new CheckedCollection(this);
			unCheckedCollection = new UnCheckedCollection(this);

			SetOptions(options);
			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
		}

		/// <inheritdoc />
		public event EventHandler<CheckBoxListChangedEventArgs> Changed
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

		private event EventHandler<CheckBoxListChangedEventArgs> OnChanged;

		/// <inheritdoc />
		public ICollection<string> Checked
		{
			get
			{
				return checkedCollection;
			}
		}

		/// <inheritdoc />
		public bool IsSorted
		{
			get
			{
				return BlockDefinition.IsSorted;
			}

			set
			{
				BlockDefinition.IsSorted = value;
			}
		}

		/// <inheritdoc />
		public string Tooltip
		{
			get
			{
				return BlockDefinition.TooltipText;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value));
				}

				BlockDefinition.TooltipText = value;
			}
		}

		/// <inheritdoc />
		public ICollection<string> Options
		{
			get
			{
				return optionsCollection;
			}
		}

		/// <inheritdoc />
		public ICollection<string> Unchecked
		{
			get
			{
				return unCheckedCollection;
			}
		}

		/// <inheritdoc />
		public UIValidationState ValidationState
		{
			get
			{
				return BlockDefinition.ValidationState;
			}

			set
			{
				BlockDefinition.ValidationState = value;
			}
		}

		/// <inheritdoc />
		public string ValidationText
		{
			get
			{
				return BlockDefinition.ValidationText;
			}

			set
			{
				BlockDefinition.ValidationText = value;
			}
		}

		/// <inheritdoc />
		public void AddOption(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException(nameof(option));
			}

			Options.Add(option);
		}

		/// <inheritdoc />
		public void Check(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException(nameof(option));
			}

			Checked.Add(option);
		}

		/// <inheritdoc />
		public void CheckAll()
		{
			Unchecked.Clear();
		}

		/// <inheritdoc />
		public void SetOptions(IEnumerable<string> options)
		{
			if (options == null) throw new ArgumentNullException(nameof(options));

			Options.Clear();
			foreach (string option in options)
			{
				Options.Add(option);
			}
		}

		/// <inheritdoc />
		public void RemoveOption(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException(nameof(option));
			}

			Options.Remove(option);
		}

		/// <inheritdoc />
		public void Uncheck(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException(nameof(option));
			}

			Unchecked.Add(option);
		}

		/// <inheritdoc />
		public void UncheckAll()
		{
			Checked.Clear();
		}

		internal override void LoadResult(UIResults uiResults)
		{
			string results = uiResults.GetString(this);
			if (results == null)
			{
				// results can be null if the list of options is empty
				BlockDefinition.InitialValue = String.Empty;
				return;
			}

			var checkedOptions = new HashSet<string>(results.Split(';'));
			foreach (string option in Options)
			{
				bool isChecked = checkedOptions.Contains(option);
				bool hasChanged = Checked.Contains(option) != isChecked;

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
		internal override void RaiseResultEvents()
		{
			if (changed && OnChanged != null)
			{
				OnChanged(this, new CheckBoxListChangedEventArgs(changedOption, changedValue));
			}

			changed = false;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class CheckBoxListChangedEventArgs : EventArgs
		{
			public CheckBoxListChangedEventArgs(string option, bool isChecked)
			{
				Option = option;
				IsChecked = isChecked;
			}

			/// <summary>
			///     Gets a value indicating whether the checkbox has been selected.
			/// </summary>
			public bool IsChecked { get; private set; }

			/// <summary>
			///     Gets the option of which the state has changed.
			/// </summary>
			public string Option { get; private set; }
		}

		private class OptionsCollection : ICollection<string>, IReadOnlyCollection<string>
		{
			private readonly CheckBoxList owner;
			private readonly ICollection<string> options;

			// At time of writing, the options collection is implemented as List.
			// Use a hashset to improve performance,
			// although by the time performance matters, the list will be impractically large.
			private readonly HashSet<string> optionsHashSet;

			public OptionsCollection(CheckBoxList owner)
			{
				this.owner = owner;
				options = owner.BlockDefinition.GetOptionsCollection();
				optionsHashSet = new HashSet<string>(options);
			}

			public IEnumerator<string> GetEnumerator()
			{
				return options.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable)options).GetEnumerator();
			}

			public void Add(string item)
			{
				if (item == null) throw new ArgumentNullException(nameof(item));

				if (!optionsHashSet.Add(item)) return;

				options.Add(item);
				owner.Unchecked.Add(item);
			}

			public void Clear()
			{
				options.Clear();
				optionsHashSet.Clear();
				owner.Checked.Clear();
				owner.Unchecked.Clear();
			}

			public bool Contains(string item)
			{
				return optionsHashSet.Contains(item);
			}

			public void CopyTo(string[] array, int arrayIndex)
			{
				options.CopyTo(array, arrayIndex);
			}

			public bool Remove(string item)
			{
				if (!optionsHashSet.Remove(item)) return false;

				options.Remove(item);
				owner.Checked.Remove(item);
				owner.Unchecked.Remove(item);

				return true;
			}

			public int Count
			{
				get
				{
					return options.Count;
				}
			}

			public bool IsReadOnly
			{
				get
				{
					return options.IsReadOnly;
				}
			}
		}

		private class CheckedCollection : ICollection<string>, IReadOnlyCollection<string>
		{
			private readonly CheckBoxList owner;
			private readonly HashSet<string> @checked = new HashSet<string>();

			public CheckedCollection(CheckBoxList owner)
			{
				this.owner = owner;
			}

			public IEnumerator<string> GetEnumerator()
			{
				return @checked.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable)@checked).GetEnumerator();
			}

			public void Add(string item)
			{
				if (!owner.Options.Contains(item)) return;

				if (!@checked.Add(item)) return;

				owner.Unchecked.Remove(item);
				owner.BlockDefinition.InitialValue = String.Join(";", @checked);
			}

			public void Clear()
			{
				@checked.Clear();
				foreach (string option in owner.Options)
				{
					owner.Unchecked.Add(option);
				}

				owner.BlockDefinition.InitialValue = String.Empty;
			}

			public bool Contains(string item)
			{
				return @checked.Contains(item);
			}

			public void CopyTo(string[] array, int arrayIndex)
			{
				@checked.CopyTo(array, arrayIndex);
			}

			public bool Remove(string item)
			{
				if (!@checked.Remove(item)) return false;

				owner.Unchecked.Add(item);
				owner.BlockDefinition.InitialValue = String.Join(";", item);

				return true;
			}

			public int Count
			{
				get
				{
					return @checked.Count;
				}
			}

			public bool IsReadOnly
			{
				get
				{
					return ((ICollection<string>)@checked).IsReadOnly;
				}
			}
		}

		private class UnCheckedCollection : ICollection<string>, IReadOnlyCollection<string>
		{
			private readonly CheckBoxList owner;
			private readonly HashSet<string> @unchecked = new HashSet<string>();

			public UnCheckedCollection(CheckBoxList owner)
			{
				this.owner = owner;
			}

			public IEnumerator<string> GetEnumerator()
			{
				return @unchecked.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable)@unchecked).GetEnumerator();
			}

			public void Add(string item)
			{
				if (!owner.Options.Contains(item)) return;

				if (!@unchecked.Add(item)) return;

				owner.Checked.Remove(item);
			}

			public void Clear()
			{
				@unchecked.Clear();
				foreach (string option in owner.Options)
				{
					owner.Checked.Add(option);
				}
			}

			public bool Contains(string item)
			{
				return @unchecked.Contains(item);
			}

			public void CopyTo(string[] array, int arrayIndex)
			{
				@unchecked.CopyTo(array, arrayIndex);
			}

			public bool Remove(string item)
			{
				if (!@unchecked.Remove(item)) return false;

				owner.Checked.Add(item);

				return true;
			}

			public int Count
			{
				get
				{
					return @unchecked.Count;
				}
			}

			public bool IsReadOnly
			{
				get
				{
					return ((ICollection<string>)@unchecked).IsReadOnly;
				}
			}
		}
	}
}
