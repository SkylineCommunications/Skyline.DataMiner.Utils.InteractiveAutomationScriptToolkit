namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     A group of radio buttons.
	/// </summary>
	public class RadioButtonList : InteractiveWidget, IRadioButtonList
	{
		private readonly OptionsCollection optionsCollection;
		private bool changed;
		private string previous;

		/// <summary>
		///     Initializes a new instance of the <see cref="RadioButtonList" /> class.
		/// </summary>
		public RadioButtonList()
			: this(Enumerable.Empty<string>())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="RadioButtonList" /> class.
		/// </summary>
		/// <param name="options">Name of options that can be selected.</param>
		/// <param name="selected">Selected option.</param>
		public RadioButtonList(IEnumerable<string> options, string selected = null)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			Type = UIBlockType.RadioButtonList;
			optionsCollection = new OptionsCollection(this);

			SetOptions(options);

			Selected = selected;
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
		public ICollection<string> Options => optionsCollection;

		/// <inheritdoc />
		public bool IsSorted
		{
			get => BlockDefinition.IsSorted;
			set => BlockDefinition.IsSorted = value;
		}

		/// <inheritdoc />
		public string Selected
		{
			get => BlockDefinition.InitialValue;

			set
			{
				if (value == null)
				{
					BlockDefinition.InitialValue = null;
					return;
				}

				if (Options.Contains(value))
				{
					BlockDefinition.InitialValue = value;
				}
			}
		}

		/// <inheritdoc />
		public string Tooltip
		{
			get => BlockDefinition.TooltipText;

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
		public void AddOption(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException(nameof(option));
			}

			Options.Add(option);
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
		public void SetOptions(IEnumerable<string> optionsToSet)
		{
			if (optionsToSet == null)
			{
				throw new ArgumentNullException(nameof(optionsToSet));
			}

			string previousSelected = Selected;

			Options.Clear();
			foreach (string option in optionsToSet)
			{
				Options.Add(option);
			}

			Selected = previousSelected;
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
				if (!String.IsNullOrEmpty(checkedOption) && checkedOption != Selected)
				{
					previous = Selected;
					Selected = checkedOption;
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
				OnChanged(this, new ChangedEventArgs(Selected, previous));
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
			public ChangedEventArgs(string selectedValue, string previous)
			{
				SelectedValue = selectedValue;
				Previous = previous;
			}

			/// <summary>
			///     Gets the previously selected option.
			/// </summary>
			public string Previous { get; }

			/// <summary>
			///     Gets the option that has been selected.
			/// </summary>
			public string SelectedValue { get; }
		}

		private class OptionsCollection : ICollection<string>, IReadOnlyCollection<string>
		{
			private readonly ICollection<string> options;

			// At time of writing, the options collection is implemented as List.
			// Use a hashset to improve performance,
			// although by the time performance matters, the list will be impractically large.
			private readonly HashSet<string> optionsHashSet;
			private readonly RadioButtonList owner;

			public OptionsCollection(RadioButtonList owner)
			{
				this.owner = owner;
				options = owner.BlockDefinition.GetOptionsCollection();
				optionsHashSet = new HashSet<string>(options);
			}

			public int Count => options.Count;

			public bool IsReadOnly => options.IsReadOnly;

			public void Add(string item)
			{
				if (item == null)
				{
					throw new ArgumentNullException(nameof(item));
				}

				if (!optionsHashSet.Add(item))
				{
					return;
				}

				options.Add(item);
			}

			public void Clear()
			{
				optionsHashSet.Clear();
				options.Clear();
				owner.Selected = null;
			}

			public bool Contains(string item)
			{
				return optionsHashSet.Contains(item);
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
				if (!optionsHashSet.Remove(item))
				{
					return false;
				}

				options.Remove(item);
				if (owner.Selected == item)
				{
					owner.Selected = null;
				}

				return true;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable)options).GetEnumerator();
			}
		}
	}
}