namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	///     A list of checkboxes.
	/// </summary>
	public class CheckBoxList<T> : CheckBoxListBase, ICheckBoxList<T>
	{
		private readonly Dictionary<Option<T>, bool> checkBoxListOptions = new Dictionary<Option<T>, bool>();
		private readonly List<ChangedOption> changedOptions = new List<ChangedOption>();

		/// <summary>
		///     Initializes a new instance of the <see cref="CheckBoxList" /> class.
		/// </summary>
		public CheckBoxList() : this(Enumerable.Empty<Option<T>>())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="CheckBoxList" /> class.
		/// </summary>
		/// <param name="options">Values that can be selected, every value is visualized in the checkboxlist by its <see cref="Object.ToString()"/> counterpart.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public CheckBoxList(IEnumerable<T> options) : this(options.Select(x => new Option<T>(x)).ToList())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="CheckBoxList" /> class.
		/// </summary>
		/// <param name="options">Options that can be selected.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public CheckBoxList(IEnumerable<Option<T>> options)
		{
			SetOptions(options);
		}

		/// <summary>
		///     Triggered when the state of a checkbox changes.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<CheckBoxListChangedEventArgs> Changed
		{
			add
			{
				OnChanged += value;
				BlockDefinition.WantsOnChange = true;
			}

			remove
			{
				OnChanged -= value;
				if (OnChanged == null || !OnChanged.GetInvocationList().Any())
				{
					BlockDefinition.WantsOnChange = false;
				}
			}
		}

		private event EventHandler<CheckBoxListChangedEventArgs> OnChanged;

		/// <inheritdoc/>
		public IEnumerable<Option<T>> CheckedOptions
		{
			get
			{
				return checkBoxListOptions.Where(option => option.Value).Select(option => option.Key);
			}
		}

		/// <inheritdoc/>
		public IEnumerable<Option<T>> UncheckedOptions
		{
			get
			{
				return checkBoxListOptions.Where(option => !option.Value).Select(option => option.Key);
			}
		}

		/// <inheritdoc/>
		public virtual IEnumerable<Option<T>> Options
		{
			get
			{
				return checkBoxListOptions.Keys;
			}

			set
			{
				SetOptions(value);
			}
		}

		/// <inheritdoc/>
		public IEnumerable<T> Checked => CheckedOptions.Select(x => x.Value);

		/// <inheritdoc/>
		public IEnumerable<T> Unchecked => UncheckedOptions.Select(x => x.Value);

		/// <summary>
		///		<inheritdoc/>
		///		Setting this property overrides all options and causes every value to be visually represented by their <see cref="Object.ToString()"/> counterpart.
		/// </summary>
		public virtual IEnumerable<T> Values
		{
			get
			{
				return checkBoxListOptions.Keys.Select(x => x.Value);
			}

			set
			{
				SetOptions(value);
			}
		}

		/// <inheritdoc/>
		/// <param name="option">Option to add.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public void AddOption(Option<T> option)
		{
			if (checkBoxListOptions.ContainsKey(option)) return;

			checkBoxListOptions.Add(option, false);
			BlockDefinition.AddCheckBoxListOption(option.DisplayValue);
		}

		/// <summary>
		///		<inheritdoc/>
		///		This value is represented in the checkboxlist by its <see cref="Object.ToString()"/> counterpart.
		/// </summary>
		/// <param name="value"></param>
		public void AddOption(T value)
		{
			AddOption(new Option<T>(value));
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		/// <exception cref="ArgumentException">When the option does not exist.</exception>
		public void Check(Option<T> option)
		{
			if (!checkBoxListOptions.TryGetValue(option, out var currentValue))
			{
				throw new ArgumentException($"Option is not defined as a valid option");
			}

			if (!currentValue)
			{
				checkBoxListOptions[option] = true;
				BlockDefinition.InitialValue = string.Join(";", BlockDefinition.InitialValue, option.DisplayValue);
			}
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentException">When the option does not exist.</exception>
		public void Check(T value)
		{
			var options = checkBoxListOptions.Keys.Where(x => Object.Equals(x.Value, value)).ToList();
			if (!options.Any()) throw new ArgumentException($"CheckboxList does not have value: {value}");

			foreach (var option in options)
			{
				Check(option);
			}
		}

		/// <inheritdoc/>
		public override void CheckAll()
		{
			foreach (var option in checkBoxListOptions.Keys.ToList())
			{
				checkBoxListOptions[option] = true;
			}

			BlockDefinition.InitialValue = string.Join(";", checkBoxListOptions.Keys.Select(x => x.DisplayValue));
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public void SetOptions(IEnumerable<Option<T>> options)
		{
			if (options == null) throw new ArgumentNullException(nameof(options));

			ClearOptions();
			foreach (var option in options)
			{
				AddOption(option);
			}
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public void SetOptions(IEnumerable<T> options)
		{
			if (options == null) throw new ArgumentNullException(nameof(options));
			SetOptions(options.Select(x => new Option<T>(x)));
		}

		/// <inheritdoc/>
		/// <exception cref="NullReferenceException">When option is null.</exception>
		public void RemoveOption(Option<T> option)
		{
			if (checkBoxListOptions.Remove(option))
			{
				RecreateUiBlock();
				foreach (var remainingOption in checkBoxListOptions.Keys)
				{
					BlockDefinition.AddCheckBoxListOption(remainingOption.DisplayValue);
				}
			}
		}

		/// <inheritdoc/>
		public void RemoveOption(T value)
		{
			var options = checkBoxListOptions.Keys.Where(x => Object.Equals(x.Value, value)).ToList();
			foreach (var option in options)
			{
				RemoveOption(option);
			}
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		/// <exception cref="ArgumentException">When the option does not exist.</exception>
		public void Uncheck(Option<T> option)
		{
			if (!checkBoxListOptions.TryGetValue(option, out var currentValue))
			{
				throw new ArgumentException("CheckboxList does not have option: " + option);
			}

			if (currentValue)
			{
				checkBoxListOptions[option] = false;
				BlockDefinition.InitialValue = string.Join(";", CheckedOptions.Select(x => x.DisplayValue));
			}
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentException">When the option does not exist.</exception>
		public void Uncheck(T value)
		{
			var options = checkBoxListOptions.Keys.Where(x => Object.Equals(x.Value, value)).ToList();
			if (!options.Any()) throw new ArgumentException($"CheckboxList does not have value: {value}");

			foreach (var option in options)
			{
				Uncheck(option);
			}
		}

		/// <inheritdoc/>
		public override void UncheckAll()
		{
			foreach (var option in checkBoxListOptions.Keys.ToList())
			{
				checkBoxListOptions[option] = false;
			}

			BlockDefinition.InitialValue = null;
		}

		/// <summary>
		///     Load any changes made through user interaction.
		/// </summary>
		/// <param name="uiResults">
		///     Represents the information a user has entered or selected in a dialog box of an interactive
		///     Automation script.
		/// </param>
		/// <remarks><see cref="InteractiveWidget.DestVar" /> should be used as key to get the changes for this widget.</remarks>
		protected internal override void LoadResult(IUIResults uiResults)
		{
			string results = uiResults.GetString(this);

			if (results == null)
			{
				// results can be null if the list of options is empty
				BlockDefinition.InitialValue = string.Empty;
				return;
			}

			var checkedOptions = new HashSet<string>(results.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
			foreach (var option in checkBoxListOptions.Keys.ToList())
			{
				bool isChecked = checkedOptions.Contains(option.DisplayValue);
				bool hasChanged = checkBoxListOptions[option] != isChecked;

				checkBoxListOptions[option] = isChecked;

				if (hasChanged && BlockDefinition.WantsOnChange)
				{
					changedOptions.Add(new ChangedOption(option, isChecked));
				}
			}

			BlockDefinition.InitialValue = string.Join(";", CheckedOptions.Select(x => x.DisplayValue));
		}

		/// <summary>
		///     Raises zero or more events of the widget.
		///     This method is called after <see cref="InteractiveWidget.LoadResult" /> was called on all widgets.
		/// </summary>
		/// <remarks>It is up to the implementer to determine if an event must be raised.</remarks>
		protected internal override void RaiseResultEvents()
		{
			foreach (var change in changedOptions)
			{
				OnChanged?.Invoke(this, new CheckBoxListChangedEventArgs(change.Option, change.IsChecked));
			}

			changedOptions.Clear();
		}

		private void ClearOptions()
		{
			checkBoxListOptions.Clear();
			RecreateUiBlock();
			BlockDefinition.InitialValue = null;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class CheckBoxListChangedEventArgs : EventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="CheckBoxListChangedEventArgs"/> class.
			/// </summary>
			/// <param name="option">The option that changed state.</param>
			/// <param name="isChecked">The new state of the option.</param>
			internal CheckBoxListChangedEventArgs(Option<T> option, bool isChecked)
			{
				Option = option;
				Value = option.Value;
				IsChecked = isChecked;
			}

			/// <summary>
			///     Gets a value indicating whether the checkbox has been selected.
			/// </summary>
			public bool IsChecked { get; private set; }

			/// <summary>
			///     Gets the option of which the state has changed.
			/// </summary>
			public Option<T> Option { get; private set; }

			/// <summary>
			///		Gets the value of which the state has changed.
			/// </summary>
			public T Value { get; private set; }
		}

		private sealed class ChangedOption
		{
			public ChangedOption(Option<T> option, bool isChecked)
			{
				Option = option;
				IsChecked = isChecked;
			}

			public Option<T> Option { get; private set; }

			public bool IsChecked { get; private set; }
		}
	}
}
