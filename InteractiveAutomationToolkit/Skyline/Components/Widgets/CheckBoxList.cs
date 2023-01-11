namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     A list of checkboxes.
	/// </summary>
	public class CheckBoxList : InteractiveWidget, ICheckBoxList
	{
		private readonly CheckedCollection checkedCollection;
		private readonly OptionsCollection optionsCollection;

		[SuppressMessage(
			"StyleCop.CSharp.NamingRules",
			"SA1305:Field names should not use Hungarian notation",
			Justification = "false positive")]
		private readonly UnCheckedCollection unCheckedCollection;

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
			optionsCollection = new OptionsCollection(this);
			checkedCollection = new CheckedCollection(this);
			unCheckedCollection = new UnCheckedCollection(this);

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
		public ICollection<string> Checked => checkedCollection;

		/// <inheritdoc />
		public IList<string> Options => optionsCollection;

		/// <inheritdoc />
		public ICollection<string> Unchecked => unCheckedCollection;

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
			Checked.Add(option ?? String.Empty);
		}

		/// <inheritdoc />
		public void CheckAll()
		{
			Unchecked.Clear();
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
			Unchecked.Add(option ?? String.Empty);
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

		private class OptionsCollection : IList<string>, IReadOnlyList<string>
		{
			private readonly IList<string> options;
			private readonly CheckBoxList owner;

			public OptionsCollection(CheckBoxList owner)
			{
				this.owner = owner;
				options = owner.BlockDefinition.GetOptionsCollection();
			}

			public int Count => options.Count;

			public bool IsReadOnly => options.IsReadOnly;

			public string this[int index]
			{
				get => options[index];

				set
				{
					value = value ?? String.Empty;
					if (options.Contains(value))
					{
						throw new InvalidOperationException($"{nameof(CheckBoxList)} already contains option: {value}");
					}

					string option = options[index];
					options[index] = value;

					owner.Checked.Remove(option);
					owner.Unchecked.Remove(option);
				}
			}

			public void Add(string item)
			{
				item = item ?? String.Empty;
				if (Contains(item))
				{
					throw new InvalidOperationException($"{nameof(CheckBoxList)} already contains option: {item}");
				}

				options.Add(item);
				owner.Unchecked.Add(item);
			}

			public void Clear()
			{
				options.Clear();
				owner.Checked.Clear();
				owner.Unchecked.Clear();
			}

			public bool Contains(string item)
			{
				return options.Contains(item ?? String.Empty);
			}

			public void CopyTo(string[] array, int arrayIndex)
			{
				options.CopyTo(array, arrayIndex);
			}

			public IEnumerator<string> GetEnumerator()
			{
				return options.GetEnumerator();
			}

			public bool Remove(string item)
			{
				item = item ?? String.Empty;
				if (!options.Remove(item))
				{
					return false;
				}

				owner.Checked.Remove(item);
				owner.Unchecked.Remove(item);

				return true;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable)options).GetEnumerator();
			}

			public int IndexOf(string item)
			{
				return options.IndexOf(item ?? String.Empty);
			}

			public void Insert(int index, string item)
			{
				item = item ?? String.Empty;

				if (!options.Contains(item))
				{
					throw new InvalidOperationException($"{nameof(CheckBoxList)} already contains option: {item}");
				}

				options.Insert(index, item);
				owner.Unchecked.Add(item);
			}

			public void RemoveAt(int index)
			{
				string option = options[index];
				options.RemoveAt(index);
				owner.Checked.Remove(option);
				owner.Unchecked.Remove(option);
			}
		}

		private class CheckedCollection : ICollection<string>, IReadOnlyCollection<string>
		{
			private readonly HashSet<string> @checked = new HashSet<string>();
			private readonly CheckBoxList owner;

			public CheckedCollection(CheckBoxList owner) => this.owner = owner;

			public int Count => @checked.Count;

			public bool IsReadOnly => ((ICollection<string>)@checked).IsReadOnly;

			public void Add(string item)
			{
				item = item ?? String.Empty;
				if (!owner.Options.Contains(item))
				{
					return;
				}

				if (!@checked.Add(item))
				{
					return;
				}

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
				return @checked.Contains(item ?? String.Empty);
			}

			public void CopyTo(string[] array, int arrayIndex)
			{
				@checked.CopyTo(array, arrayIndex);
			}

			public IEnumerator<string> GetEnumerator()
			{
				return @checked.GetEnumerator();
			}

			public bool Remove(string item)
			{
				item = item ?? String.Empty;
				if (!@checked.Remove(item))
				{
					return false;
				}

				owner.BlockDefinition.InitialValue = String.Join(";", @checked);

				return true;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable)@checked).GetEnumerator();
			}
		}

		private class UnCheckedCollection : ICollection<string>, IReadOnlyCollection<string>
		{
			private readonly CheckBoxList owner;
			private readonly HashSet<string> @unchecked = new HashSet<string>();

			public UnCheckedCollection(CheckBoxList owner) => this.owner = owner;

			public int Count => @unchecked.Count;

			public bool IsReadOnly => ((ICollection<string>)@unchecked).IsReadOnly;

			public void Add(string item)
			{
				item = item ?? String.Empty;
				if (!owner.Options.Contains(item))
				{
					return;
				}

				if (!@unchecked.Add(item))
				{
					return;
				}

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
				return @unchecked.Contains(item ?? String.Empty);
			}

			public void CopyTo(string[] array, int arrayIndex)
			{
				@unchecked.CopyTo(array, arrayIndex);
			}

			public IEnumerator<string> GetEnumerator()
			{
				return @unchecked.GetEnumerator();
			}

			public bool Remove(string item)
			{
				item = item ?? String.Empty;
				if (!@unchecked.Remove(item))
				{
					return false;
				}

				owner.Checked.Add(item);

				return true;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable)@unchecked).GetEnumerator();
			}
		}
	}
}