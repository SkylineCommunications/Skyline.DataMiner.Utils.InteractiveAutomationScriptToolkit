namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Globalization;
	using System.Linq;

	using Automation;

	/// <summary>
	///     Widget to show/edit a datetime.
	/// </summary>
	public class DateTimePicker : TimePickerBase, IDateTimePicker
	{
		private readonly AutomationDateTimePickerOptions dateTimePickerOptions;

		private bool changed;
		private DateTime dateTime;
		private DateTime previous;
		private bool displayServerTime;

		/// <summary>
		///     Initializes a new instance of the <see cref="DateTimePicker" /> class.
		/// </summary>
		/// <param name="dateTime">Value displayed in the datetime picker.</param>
		public DateTimePicker(DateTime dateTime) : base(new AutomationDateTimePickerOptions())
		{
			Type = UIBlockType.Time;
			DateTime = dateTime;
			dateTimePickerOptions = (AutomationDateTimePickerOptions)DateTimeUpDownOptions;
			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="DateTimePicker" /> class.
		/// </summary>
		public DateTimePicker() : this(DateTime.Now)
		{
		}

		/// <inheritdoc />
		public event EventHandler<DateTimePickerChangedEventArgs> Changed
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

		private event EventHandler<DateTimePickerChangedEventArgs> OnChanged;

		/// <inheritdoc />
		public bool DisplayServerTime
		{
			get
			{
				return displayServerTime;
			}

			set
			{
				displayServerTime = value;
				DateTime = dateTime;
			}
		}

		/// <inheritdoc />
		public DateTime DateTime
		{
			get
			{
				return dateTime;
			}

			set
			{
				dateTime = value;
				if (DisplayServerTime)
				{
					BlockDefinition.InitialValue = value.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
				}
				else
				{
					BlockDefinition.InitialValue = value.ToString(AutomationConfigOptions.GlobalDateTimeFormat, CultureInfo.InvariantCulture);
				}
			}
		}

		/// <inheritdoc />
		public bool AutoCloseCalendar
		{
			get
			{
				return dateTimePickerOptions.AutoCloseCalendar;
			}

			set
			{
				dateTimePickerOptions.AutoCloseCalendar = value;
			}
		}

		/// <inheritdoc />
		public DateTime Maximum
		{
			get
			{
				return DateTimeUpDownOptions.Maximum ?? DateTime.MaxValue;
			}

			set
			{
				DateTimeUpDownOptions.Maximum = value;
			}
		}

		/// <inheritdoc />
		public DateTime Minimum
		{
			get
			{
				return DateTimeUpDownOptions.Minimum ?? DateTime.MinValue;
			}

			set
			{
				DateTimeUpDownOptions.Minimum = value;
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
		public CalendarMode CalendarDisplayMode
		{
			get
			{
				return dateTimePickerOptions.CalendarDisplayMode;
			}

			set
			{
				dateTimePickerOptions.CalendarDisplayMode = value;
			}
		}

		/// <inheritdoc />
		public bool HasDropDownButton
		{
			get
			{
				return dateTimePickerOptions.ShowDropDownButton;
			}

			set
			{
				dateTimePickerOptions.ShowDropDownButton = value;
			}
		}

		/// <inheritdoc />
		public bool IsTimePickerVisible
		{
			get
			{
				return dateTimePickerOptions.TimePickerVisible;
			}

			set
			{
				dateTimePickerOptions.TimePickerVisible = value;
			}
		}

		/// <inheritdoc />
		public bool HasTimePickerSpinnerButton
		{
			get
			{
				return dateTimePickerOptions.TimePickerShowButtonSpinner;
			}

			set
			{
				dateTimePickerOptions.TimePickerShowButtonSpinner = value;
			}
		}

		/// <inheritdoc />
		public bool IsTimePickerSpinnerButtonEnabled
		{
			get
			{
				return dateTimePickerOptions.TimePickerAllowSpin;
			}

			set
			{
				dateTimePickerOptions.TimePickerAllowSpin = value;
			}
		}

		/// <inheritdoc />
		public DateTimeFormat TimeFormat
		{
			get
			{
				return dateTimePickerOptions.TimeFormat;
			}

			set
			{
				dateTimePickerOptions.TimeFormat = value;
			}
		}

		/// <inheritdoc />
		public string CustomTimeFormat
		{
			get
			{
				return dateTimePickerOptions.TimeFormatString;
			}

			set
			{
				TimeFormat = DateTimeFormat.Custom;
				dateTimePickerOptions.TimeFormatString = value;
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
		protected internal override void LoadResult(UIResults uiResults)
		{
			string isoString = uiResults.GetString(DestVar);
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
				OnChanged(this, new DateTimePickerChangedEventArgs(DateTime, previous));
			}

			changed = false;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class DateTimePickerChangedEventArgs : EventArgs
		{
			public DateTimePickerChangedEventArgs(DateTime dateTime, DateTime previous)
			{
				DateTime = dateTime;
				Previous = previous;
			}

			/// <summary>
			///     Gets the new datetime value.
			/// </summary>
			public DateTime DateTime { get; private set; }

			/// <summary>
			///     Gets the previous datetime value.
			/// </summary>
			public DateTime Previous { get; private set; }
		}
	}
}