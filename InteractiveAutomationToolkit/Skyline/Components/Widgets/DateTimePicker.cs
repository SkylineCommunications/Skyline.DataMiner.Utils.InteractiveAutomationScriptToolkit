namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.ComponentModel;
	using System.Globalization;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Widget to show/edit a datetime.
	/// </summary>
	public class DateTimePicker : TimePickerBase, IDateTimePicker
	{
		private readonly AutomationDateTimePickerOptions dateTimePickerOptions;

		private bool changed;
		private DateTime dateTime;
		private bool displayServerTime;
		private DateTime previous;
		private readonly Validation validation;

		/// <summary>
		///     Initializes a new instance of the <see cref="DateTimePicker" /> class.
		/// </summary>
		/// <param name="dateTime">Value displayed in the datetime picker.</param>
		public DateTimePicker(DateTime dateTime)
			: base(new AutomationDateTimePickerOptions())
		{
			Type = UIBlockType.Time;
			validation = new Validation(this);
			DateTime = dateTime;
			dateTimePickerOptions = (AutomationDateTimePickerOptions)DateTimeUpDownOptions;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="DateTimePicker" /> class.
		/// </summary>
		public DateTimePicker()
			: this(DateTime.Now)
		{
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
		public bool AutoCloseCalendar
		{
			get => dateTimePickerOptions.AutoCloseCalendar;
			set => dateTimePickerOptions.AutoCloseCalendar = value;
		}

		/// <inheritdoc />
		public CalendarMode CalendarDisplayMode
		{
			get => dateTimePickerOptions.CalendarDisplayMode;
			set => dateTimePickerOptions.CalendarDisplayMode = value;
		}

		/// <inheritdoc />
		public string CustomTimeFormat
		{
			get => dateTimePickerOptions.TimeFormatString;

			set
			{
				TimeFormat = DateTimeFormat.Custom;
				dateTimePickerOptions.TimeFormatString = value;
			}
		}

		/// <inheritdoc />
		public DateTime DateTime
		{
			get => dateTime;

			set
			{
				dateTime = value;
				if (DisplayServerTime)
				{
					BlockDefinition.InitialValue = value.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
				}
				else
				{
					BlockDefinition.InitialValue = value.ToString(
						AutomationConfigOptions.GlobalDateTimeFormat,
						CultureInfo.InvariantCulture);
				}
			}
		}

		/// <inheritdoc />
		public bool DisplayServerTime
		{
			get => displayServerTime;

			set
			{
				displayServerTime = value;
				DateTime = dateTime;
			}
		}

		/// <inheritdoc />
		public bool HasDropDownButton
		{
			get => dateTimePickerOptions.ShowDropDownButton;
			set => dateTimePickerOptions.ShowDropDownButton = value;
		}

		/// <inheritdoc />
		public bool HasTimePickerSpinnerButton
		{
			get => dateTimePickerOptions.TimePickerShowButtonSpinner;
			set => dateTimePickerOptions.TimePickerShowButtonSpinner = value;
		}

		/// <inheritdoc />
		public bool IsTimePickerSpinnerButtonEnabled
		{
			get => dateTimePickerOptions.TimePickerAllowSpin;
			set => dateTimePickerOptions.TimePickerAllowSpin = value;
		}

		/// <inheritdoc />
		public bool IsTimePickerVisible
		{
			get => dateTimePickerOptions.TimePickerVisible;
			set => dateTimePickerOptions.TimePickerVisible = value;
		}

		/// <inheritdoc />
		public DateTime Maximum
		{
			get => DateTimeUpDownOptions.Maximum ?? DateTime.MaxValue;
			set => DateTimeUpDownOptions.Maximum = value;
		}

		/// <inheritdoc />
		public DateTime Minimum
		{
			get => DateTimeUpDownOptions.Minimum ?? DateTime.MinValue;
			set => DateTimeUpDownOptions.Minimum = value;
		}

		/// <inheritdoc />
		public DateTimeFormat TimeFormat
		{
			get => dateTimePickerOptions.TimeFormat;
			set => dateTimePickerOptions.TimeFormat = value;
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
			get => validation.ValidationState;
			set => validation.ValidationState = value;
		}

		/// <inheritdoc />
		public string ValidationText
		{
			get => validation.ValidationText;
			set => validation.ValidationText = value;
		}

		/// <inheritdoc />
		protected internal override void LoadResult(UIResults results)
		{
			string isoString = results.GetString(DestVar);
			DateTime result = DateTime.Parse(isoString);

			if (WantsOnChange && result != DateTime)
			{
				changed = true;
				previous = DateTime;
			}

			DateTime = result;
		}

		/// <inheritdoc />
		protected internal override void RaiseResultEvents()
		{
			if (changed && OnChanged != null)
			{
				OnChanged(this, new ChangedEventArgs(DateTime, previous));
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
			/// <param name="dateTime">The new datetime value.</param>
			/// <param name="previous">The previous datetime value.</param>
			public ChangedEventArgs(DateTime dateTime, DateTime previous)
			{
				DateTime = dateTime;
				Previous = previous;
			}

			/// <summary>
			///     Gets the new datetime value.
			/// </summary>
			public DateTime DateTime { get; }

			/// <summary>
			///     Gets the previous datetime value.
			/// </summary>
			public DateTime Previous { get; }
		}
	}
}