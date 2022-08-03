namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     A drop-down list.
	/// </summary>
	public class DropDown : InteractiveWidget, IDropDown
	{
		private readonly OptionsCollection optionsCollection;
		private bool changed;
		private string previous;

		/// <summary>
		///     Initializes a new instance of the <see cref="DropDown" /> class.
		/// </summary>
		public DropDown()
			: this(Enumerable.Empty<string>())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="DropDown" /> class.
		/// </summary>
		/// <param name="options">Options to be displayed in the list.</param>
		/// <param name="selected">The selected item in the list.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public DropDown(IEnumerable<string> options, string selected = null)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			Type = UIBlockType.DropDown;
			optionsCollection = new OptionsCollection(this);

			SetOptions(options);

			Selected = selected;

			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
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
		public IList<string> Options => optionsCollection;

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

		/// <inheritdoc />
		public string Selected
		{
			get => BlockDefinition.InitialValue;

			set
			{
				// Prevent setting a value that is not part of the options
				if (Options.Contains(value ?? String.Empty))
				{
					BlockDefinition.InitialValue = value ?? String.Empty;
				}
			}
		}

		/// <inheritdoc />
		public string Tooltip
		{
			get => BlockDefinition.TooltipText;
			set => BlockDefinition.TooltipText = value ?? String.Empty;
		}

		/// <inheritdoc />
		public UIValidationState ValidationState
		{
			get => BlockDefinition.ValidationState;

			set
			{
				if (!Enum.IsDefined(typeof(UIValidationState), value))
				{
					throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(UIValidationState));
				}

				BlockDefinition.ValidationState = value;
			}
		}

		/// <inheritdoc />
		public string ValidationText
		{
			get => BlockDefinition.ValidationText;
			set => BlockDefinition.ValidationText = value ?? String.Empty;
		}

		/// <inheritdoc />
		public void AddOption(string option)
		{
			Options.Add(option ?? String.Empty);
		}

		/// <inheritdoc />
		public void ForceSelected(string selected)
		{
			BlockDefinition.InitialValue = selected ?? String.Empty;
		}

		/// <inheritdoc />
		public void RemoveOption(string option)
		{
			Options.Remove(option ?? String.Empty);
		}

		/// <inheritdoc />
		public void SetOptions(IEnumerable<string> optionsToSet)
		{
			if (optionsToSet == null)
			{
				throw new ArgumentNullException(nameof(optionsToSet));
			}

			string copyOfSelected = Selected;

			Options.Clear();
			foreach (string option in optionsToSet)
			{
				Options.Add(option ?? String.Empty);
			}

			Selected = copyOfSelected;
		}

		/// <inheritdoc />
		protected internal override void LoadResult(UIResults results)
		{
			string selectedValue = results.GetString(this);
			if (WantsOnChange)
			{
				changed = selectedValue != Selected;
			}

			previous = Selected;

			// Write to BlockDefinition instead of Selected property so a force selected option does not reset after every interaction
			BlockDefinition.InitialValue = selectedValue;
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
			/// <param name="selected">The option that has been selected.</param>
			/// <param name="previous">The previously selected option.</param>
			public ChangedEventArgs(string selected, string previous)
			{
				Selected = selected;
				Previous = previous;
			}

			/// <summary>
			///     Gets the previously selected option.
			/// </summary>
			public string Previous { get; }

			/// <summary>
			///     Gets the option that has been selected.
			/// </summary>
			public string Selected { get; }
		}

		private class OptionsCollection : IList<string>, IReadOnlyList<string>
		{
			private readonly IList<string> options;
			private readonly DropDown owner;

			public OptionsCollection(DropDown owner)
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
						throw new InvalidOperationException($"{nameof(DropDown)} already contains option: {value}");
					}

					string option = options[index];
					options[index] = value;

					if (owner.Selected == option)
					{
						owner.Selected = this.FirstOrDefault();
					}

					// Cube will select the first option even if UIBlockDefinition.InitialValue is null.
					// But I believe this behavior should be reflected by the Selected property.
					// Selected should never be null if options is not empty.
					if (owner.Selected == null)
					{
						owner.Selected = value;
					}
				}
			}

			public void Add(string item)
			{
				item = item ?? String.Empty;

				if (options.Contains(item))
				{
					throw new InvalidOperationException($"{nameof(DropDown)} already contains option: {item}");
				}

				options.Add(item);

				// Cube will select the first option even if UIBlockDefinition.InitialValue is null.
				// But I believe this behavior should be reflected by the Selected property.
				// Selected should never be null if options is not empty.
				if (owner.Selected == null)
				{
					owner.Selected = item;
				}
			}

			public void Clear()
			{
				options.Clear();
				owner.BlockDefinition.InitialValue = null;
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

				if (owner.Selected == item)
				{
					owner.Selected = this.FirstOrDefault();
				}

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

				if (options.Contains(item))
				{
					throw new InvalidOperationException($"{nameof(DropDown)} already contains option: {item}");
				}

				options.Insert(index, item);

				// Cube will select the first option even if UIBlockDefinition.InitialValue is null.
				// But I believe this behavior should be reflected by the Selected property.
				// Selected should never be null if options is not empty.
				if (owner.Selected == null)
				{
					owner.Selected = item;
				}
			}

			public void RemoveAt(int index)
			{
				string option = options[index];
				options.RemoveAt(index);
				if (owner.Selected == option)
				{
					owner.Selected = this.FirstOrDefault();
				}
			}
		}
	}
}