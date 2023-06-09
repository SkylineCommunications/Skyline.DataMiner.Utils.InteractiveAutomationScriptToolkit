﻿namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     A group of radio buttons.
	/// </summary>
	public class RadioButtonList : InteractiveWidget, IRadioButtonList
	{
		private readonly RadioButtonOptionList options;
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
			this.options = new RadioButtonOptionList(this);

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
		IList<string> IRadioButtonList.Options => options;

		/// <summary>
		/// Gets all options.
		/// </summary>
		public OptionList Options => options;

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
			set => BlockDefinition.TooltipText = value ?? String.Empty;
		}

		/// <inheritdoc />
		[Obsolete("Call Options.Add instead.")]
		public void AddOption(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException(nameof(option));
			}

			Options.Add(option);
		}

		/// <inheritdoc />
		[Obsolete("Call Options.Remove instead.")]
		public void RemoveOption(string option)
		{
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

		private class RadioButtonOptionList : OptionList
		{
			private readonly RadioButtonList radioButtonList;

			public RadioButtonOptionList(RadioButtonList radioButtonList)
				: base(radioButtonList.BlockDefinition.GetOptionsCollection())
			{
				this.radioButtonList = radioButtonList;
			}

			public override string this[int index]
			{
				get => base[index];

				set
				{
					string replacedOption = base[index];
					base[index] = value;

					if (radioButtonList.Selected == replacedOption)
					{
						radioButtonList.Selected = value;
					}
				}
			}

			public override void Clear()
			{
				base.Clear();
				radioButtonList.Selected = null;
			}

			public override bool Remove(string item)
			{
				if (!base.Remove(item))
				{
					return false;
				}

				if (radioButtonList.Selected == item)
				{
					radioButtonList.Selected = null;
				}

				return true;
			}

			public override void RemoveAt(int index)
			{
				string removedOption = this[index];
				base.RemoveAt(index);
				if (radioButtonList.Selected == removedOption)
				{
					radioButtonList.Selected = null;
				}
			}
		}
	}
}