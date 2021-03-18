namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Globalization;
	using System.Linq;
	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Widget to show/edit a datetime.
	/// </summary>
	public class DateTimePicker : TimePickerBase
	{
		private bool changed;
		private DateTime dateTime;
		private DateTime previous;

		/// <summary>
		///     Initializes a new instance of the <see cref="DateTimePicker" /> class.
		/// </summary>
		/// <param name="dateTime">Value displayed in the datetime picker.</param>
		public DateTimePicker(DateTime dateTime) : base(new AutomationDateTimePickerOptions())
		{
			Type = UIBlockType.Calendar;
			DateTime = dateTime;
			DateTimePickerOptions = (AutomationDateTimePickerOptions)DateTimeUpDownOptions;
			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="DateTimePicker" /> class.
		/// </summary>
		public DateTimePicker() : this(DateTime.Now)
		{
		}

		/// <summary>
		///     Events triggers when a different datetime is picked.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
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
				if(OnChanged == null || !OnChanged.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<DateTimePickerChangedEventArgs> OnChanged;

		/// <summary>
		///     Gets or sets the datetime displayed in the datetime picker.
		/// </summary>
		public DateTime DateTime
		{
			get
			{
				return dateTime;
			}

			set
			{
				dateTime = value;
				BlockDefinition.InitialValue = value.ToString(AutomationConfigOptions.GlobalDateTimeFormat, CultureInfo.InvariantCulture);
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the calendar pop-up will close when the user clicks a new date.
		/// </summary>
		public bool AutoCloseCalendar
		{
			get
			{
				return DateTimePickerOptions.AutoCloseCalendar;
			}

			set
			{
				DateTimePickerOptions.AutoCloseCalendar = value;
			}
		}

		/// <summary>
		///     Gets or sets the maximum timestamp.
		/// </summary>
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

		/// <summary>
		///     Gets or sets the minimum timestamp.
		/// </summary>
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

		/// <summary>
		///     Gets or sets the display mode of the calendar inside the date-time picker control.
		///     Default: <c>CalendarMode.Month</c>
		/// </summary>
		public CalendarMode CalendarDisplayMode
		{
			get
			{
				return DateTimePickerOptions.CalendarDisplayMode;
			}

			set
			{
				DateTimePickerOptions.CalendarDisplayMode = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the calendar control drop-down button is shown.
		///     Default: <c>true</c>
		/// </summary>
		public bool HasDropDownButton
		{
			get
			{
				return DateTimePickerOptions.ShowDropDownButton;
			}

			set
			{
				DateTimePickerOptions.ShowDropDownButton = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the time picker is shown within the calender control.
		///     Default: <c>true</c>
		/// </summary>
		public bool IsTimePickerVisible
		{
			get
			{
				return DateTimePickerOptions.TimePickerVisible;
			}

			set
			{
				DateTimePickerOptions.TimePickerVisible = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the spin box of the calender control is shown.
		///     Default: <c>true</c>
		/// </summary>
		public bool HasTimePickerSpinnerButton
		{
			get
			{
				return DateTimePickerOptions.TimePickerShowButtonSpinner;
			}

			set
			{
				DateTimePickerOptions.TimePickerShowButtonSpinner = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the spin box of the calender is enabled.
		///     Default: <c>true</c>
		/// </summary>
		public bool IsTimePickerSpinnerButtonEnabled
		{
			get
			{
				return DateTimePickerOptions.TimePickerAllowSpin;
			}

			set
			{
				DateTimePickerOptions.TimePickerAllowSpin = value;
			}
		}

		/// <summary>
		///     Gets or sets the time format of the time picker.
		///     Default: <c>DateTimeFormat.ShortTime</c>
		/// </summary>
		public DateTimeFormat TimeFormat
		{
			get
			{
				return DateTimePickerOptions.TimeFormat;
			}

			set
			{
				DateTimePickerOptions.TimeFormat = value;
			}
		}

		/// <summary>
		///     Gets or sets the time format string used when TimeFormat is set to <c>DateTimeFormat.Custom</c>.
		/// </summary>
		/// <remarks>Sets <see cref="TimeFormat" /> to <c>DateTimeFormat.Custom</c></remarks>
		public string CustomTimeFormat
		{
			get
			{
				return DateTimePickerOptions.TimeFormatString;
			}

			set
			{
				TimeFormat = DateTimeFormat.Custom;
				DateTimePickerOptions.TimeFormatString = value;
			}
		}

		/// <summary>
		///		Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
		///		This should be used by client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner 10.0.5 onwards.</remarks>
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

		/// <summary>
		///		Gets or sets the text that is shown if the ValidationState is Invalid.
		///		This should be used by client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner 10.0.5 onwards.</remarks>
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

		private AutomationDateTimePickerOptions DateTimePickerOptions { get; set; }

		/// <inheritdoc />
		internal override void LoadResult(UIResults uiResults)
		{
			// TODO check this
			DateTime result;
			try
			{
				result = uiResults.GetDateTime(this);
			}
			catch (Exception)
			{
				string isoString = uiResults.GetString(DestVar);
				result = DateTime.Parse(isoString);
			}

			if (WantsOnChange && (result != DateTime))
			{
				changed = true;
				previous = DateTime;
			}

			DateTime = result;
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
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
			internal DateTimePickerChangedEventArgs(DateTime dateTime, DateTime previous)
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
