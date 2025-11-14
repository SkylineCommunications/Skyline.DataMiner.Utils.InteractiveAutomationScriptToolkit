namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Microsoft.Extensions.Logging;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript.Extensions;

	/// <summary>
	///     A list of checkboxes.
	/// </summary>
	public class CheckBoxList : CheckBoxListBase, ICheckBoxList
	{
		private readonly IDictionary<string, bool> options = new Dictionary<string, bool>();
		private readonly List<ChangedOption> changedOptions = new List<ChangedOption>();

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
		public IEnumerable<string> Checked
		{
			get
			{
				return options.Where(option => option.Value).Select(option => option.Key);
			}
		}

		/// <inheritdoc/>
		public virtual IEnumerable<string> Options
		{
			get
			{
				return options.Keys;
			}
		}

		/// <inheritdoc/>
		public IEnumerable<string> Unchecked
		{
			get
			{
				return options.Where(option => !option.Value).Select(option => option.Key);
			}
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public void AddOption(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException("option");
			}

			if (!options.ContainsKey(option))
			{
				options.Add(option, false);
				BlockDefinition.AddCheckBoxListOption(option);
			}
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		/// <exception cref="ArgumentException">When the option does not exist.</exception>
		public void Check(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException("option");
			}

			if (!options.ContainsKey(option))
			{
				throw new ArgumentException("CheckboxList does not have option: " + option, option);
			}

			if (!options[option])
			{
				options[option] = true;
				BlockDefinition.InitialValue = String.Join(";", BlockDefinition.InitialValue, option);
			}
		}

		/// <inheritdoc/>
		public override void CheckAll()
		{
			foreach (string option in options.Keys.ToList())
			{
				options[option] = true;
			}

			BlockDefinition.InitialValue = String.Join(";", options.Keys);
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public void SetOptions(IEnumerable<string> options)
		{
			ClearOptions();
			foreach (string option in options)
			{
				AddOption(option);
			}
		}

		/// <inheritdoc/>
		/// <exception cref="NullReferenceException">When option is null.</exception>
		public void RemoveOption(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException("option");
			}

			if (options.Remove(option))
			{
				RecreateUiBlock();
				foreach (string optionsKey in options.Keys)
				{
					BlockDefinition.AddCheckBoxListOption(optionsKey);
				}
			}
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		/// <exception cref="ArgumentException">When the option does not exist.</exception>
		public void Uncheck(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException("option");
			}

			if (!options.ContainsKey(option))
			{
				throw new ArgumentException("CheckboxList does not have option: " + option, option);
			}

			if (options[option])
			{
				options[option] = false;
				BlockDefinition.InitialValue = String.Join(";", Checked);
			}
		}

		/// <inheritdoc/>
		public override void UncheckAll()
		{
			foreach (string option in options.Keys.ToList())
			{
				options[option] = false;
			}

			BlockDefinition.InitialValue = null;
		}

		/// <inheritdoc	/>
		protected internal override void LoadResult(IUIResults uiResults, ILogger logger = null)
		{
			string results = uiResults.GetString(this);
			logger?.Debug(nameof(CheckBoxList), nameof(LoadResult), $"Checked Items: {results}");

			if (results == null)
			{
				// results can be null if the list of options is empty
				BlockDefinition.InitialValue = String.Empty;
				return;
			}

			var checkedOptions = new HashSet<string>(results.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
			foreach (string option in options.Keys.ToList())
			{
				bool isChecked = checkedOptions.Contains(option);
				bool hasChanged = options[option] != isChecked;

				options[option] = isChecked;

				if (hasChanged && BlockDefinition.WantsOnChange)
				{
					changedOptions.Add(new ChangedOption(option, isChecked));
				}
			}

			BlockDefinition.InitialValue = String.Join(";", Checked);
		}

		/// <inheritdoc	/>
		protected internal override void RaiseResultEvents(ILogger logger = null)
		{
			foreach (var change in changedOptions)
			{
				logger?.Debug(nameof(CheckBoxList), nameof(RaiseResultEvents), $"OnChanged; option: {change.Option}, IsChecked: {change.IsChecked}");
				OnChanged?.Invoke(this, new CheckBoxListChangedEventArgs(change.Option, change.IsChecked));
			}

			changedOptions.Clear();
		}

		private void ClearOptions()
		{
			options.Clear();
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
			internal CheckBoxListChangedEventArgs(string option, bool isChecked)
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

		private sealed class ChangedOption
		{
			public ChangedOption(string option, bool isChecked)
			{
				Option = option;
				IsChecked = isChecked;
			}

			public string Option { get; private set; }

			public bool IsChecked { get; private set; }
		}
	}
}
