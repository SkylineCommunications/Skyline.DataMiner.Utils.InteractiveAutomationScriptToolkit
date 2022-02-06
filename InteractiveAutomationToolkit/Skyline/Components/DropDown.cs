﻿namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	using Automation;

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
		public DropDown() : this(Enumerable.Empty<string>())
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
			if (options == null) throw new ArgumentNullException(nameof(options));

			Type = UIBlockType.DropDown;
			optionsCollection = new OptionsCollection(this);

			SetOptions(options);

			Selected = selected;

			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
		}

		/// <inheritdoc />
		public event EventHandler<DropDownChangedEventArgs> Changed
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

		private event EventHandler<DropDownChangedEventArgs> OnChanged;

		/// <inheritdoc />
		public ICollection<string> Options
		{
			get
			{
				return optionsCollection;
			}
		}

		/// <inheritdoc />
		public string Selected
		{
			get
			{
				return BlockDefinition.InitialValue;
			}

			set
			{
				// Prevent setting a value that is not part of the options
				if (Options.Contains(value))
				{
					BlockDefinition.InitialValue = value;
				}
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
		public bool IsDisplayFilterShown
		{
			get
			{
				return BlockDefinition.DisplayFilter;
			}

			set
			{
				BlockDefinition.DisplayFilter = value;
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
		public void AddOption(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException(nameof(option));
			}

			Options.Add(option);
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
				Options.Add(option);
			}

			Selected = copyOfSelected;
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
		public void ForceSelected(string selected)
		{
			if (selected == null) throw new ArgumentNullException(nameof(selected));

			BlockDefinition.InitialValue = selected;
		}

		internal override void LoadResult(UIResults uiResults)
		{
			string selectedValue = uiResults.GetString(this);
			if (WantsOnChange)
			{
				changed = selectedValue != Selected;
			}

			previous = Selected;
			Selected = selectedValue;
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			if (changed && OnChanged != null)
			{
				OnChanged(this, new DropDownChangedEventArgs(Selected, previous));
			}

			changed = false;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class DropDownChangedEventArgs : EventArgs
		{
			public DropDownChangedEventArgs(string selected, string previous)
			{
				Selected = selected;
				Previous = previous;
			}

			/// <summary>
			///     Gets the previously selected option.
			/// </summary>
			public string Previous { get; private set; }

			/// <summary>
			///     Gets the option that has been selected.
			/// </summary>
			public string Selected { get; private set; }
		}

		private class OptionsCollection : ICollection<string>, IReadOnlyCollection<string>
		{
			private readonly DropDown owner;
			private readonly ICollection<string> options;

			// At time of writing, the options collection is implemented as List.
			// Use a hashset to improve performance,
			// although by the time performance matters, the list will be impractically large.
			private readonly HashSet<string> optionsHashSet;

			public OptionsCollection(DropDown owner)
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
				optionsHashSet.Clear();
				owner.BlockDefinition.InitialValue = null;
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
				if (owner.Selected == item)
				{
					owner.Selected = this.FirstOrDefault();
				}

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
	}
}
