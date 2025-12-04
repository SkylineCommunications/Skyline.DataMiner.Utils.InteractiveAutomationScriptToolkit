namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Microsoft.Extensions.Logging;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript.Extensions;

	/// <summary>
	///     A group of radio buttons.
	/// </summary>
	public class RadioButtonList : RadioButtonListBase, IRadioButtonList
	{
		private readonly HashSet<string> options = new HashSet<string>();
		private readonly RawValueMapping<string> rawValueMapping = new RawValueMapping<string>();

		private bool changed;
		private string previous;

		/// <summary>
		///     Initializes a new instance of the <see cref="RadioButtonList" /> class.
		/// </summary>
		public RadioButtonList() : this(Enumerable.Empty<string>())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="RadioButtonList" /> class.
		/// </summary>
		/// <param name="options">Name of options that can be selected.</param>
		/// <param name="selected">Selected option.</param>
		public RadioButtonList(IEnumerable<string> options, string selected = null)
		{
			SetOptions(options);
			Selected = selected;
		}

		/// <summary>
		///     Triggered when a different option is selected.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<RadioButtonChangedEventArgs> Changed
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

		private event EventHandler<RadioButtonChangedEventArgs> OnChanged;

		/// <inheritdoc/>
		public IEnumerable<string> Options
		{
			get
			{
				return options;
			}

			set
			{
				SetOptions(value);
			}
		}

		/// <inheritdoc/>
		public string Selected
		{
			get
			{
				return BlockDefinition.InitialValue;
			}

			set
			{
				BlockDefinition.InitialValue = value;
			}
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		public void AddOption(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException("option");
			}

			if (!options.Contains(option))
			{
				options.Add(option);
				rawValueMapping.Add(option);

				BlockDefinition.AddRadioButtonListOption(rawValueMapping.GetRawValue(option), option);
			}
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		public void RemoveOption(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException("option");
			}

			if (options.Remove(option))
			{
				rawValueMapping.Remove(option);

				RecreateUiBlock();
				foreach (string optionToAdd in options)
				{
					BlockDefinition.AddRadioButtonListOption(rawValueMapping.GetRawValue(optionToAdd), optionToAdd);
				}

				if (Selected == option)
				{
					Selected = options.FirstOrDefault();
				}
			}
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">When optionsToSet is null.</exception>
		public void SetOptions(IEnumerable<string> options)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			ClearOptions();
			foreach (string option in options)
			{
				AddOption(option);
			}

			if (Selected == null || !options.Contains(Selected))
			{
				Selected = null;
			}
		}

		/// <inheritdoc	/>
		protected internal override void LoadResult(IUIResults uiResults, ILogger logger = null)
		{
			string result = uiResults.GetString(this);
			logger?.Debug(nameof(RadioButtonList), nameof(LoadResult), $"Raw result: {result}");

			if (String.IsNullOrWhiteSpace(result))
			{
				return;
			}

			string[] rawCheckedOptions = result.Split(';');
			foreach (string rawCheckedOption in rawCheckedOptions)
			{
				if (String.IsNullOrEmpty(rawCheckedOption))
				{
					continue;
				}

				if (!rawValueMapping.TryGetByRawValue(rawCheckedOption, out var checkedOption))
				{
					continue;
				}

				if (checkedOption != Selected)
				{
					previous = Selected;
					Selected = checkedOption;
					changed = true;
					break;
				}
			}
		}

		/// <inheritdoc	/>
		protected internal override void RaiseResultEvents(ILogger logger = null)
		{
			if (changed)
			{
				logger?.Debug(nameof(RadioButtonList), nameof(RaiseResultEvents), $"OnChanged; Selected option changed from {previous} to {Selected}");
				OnChanged?.Invoke(this, new RadioButtonChangedEventArgs(Selected, previous));
			}

			changed = false;
		}

		private void ClearOptions()
		{
			options.Clear();
			rawValueMapping.Clear();
			RecreateUiBlock();
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class RadioButtonChangedEventArgs : EventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="RadioButtonChangedEventArgs"/> class.
			/// </summary>
			/// <param name="selectedValue">The new value.</param>
			/// <param name="previous">The previous value.</param>
			internal RadioButtonChangedEventArgs(string selectedValue, string previous)
			{
				SelectedValue = selectedValue;
				Previous = previous;
			}

			/// <summary>
			///     Gets the previously selected option.
			/// </summary>
			public string Previous { get; private set; }

			/// <summary>
			///     Gets the option that has been selected.
			/// </summary>
			public string SelectedValue { get; private set; }
		}
	}
}
