namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Globalization;
	using System.Linq;
	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Widget to show/edit a time of day.
	/// </summary>
	public class TimePicker : TimePickerBase
	{
		private bool changed;
		private int maxDropDownHeight;
		private TimeSpan maximum;
		private TimeSpan minimum;
		private TimeSpan previous;
		private TimeSpan time;
		private AutomationTimePickerOptions timePickerOptions;

		/// <summary>
		///     Initializes a new instance of the <see cref="TimePicker" /> class.
		/// </summary>
		/// <param name="time">Time displayed in the time picker.</param>
		public TimePicker(TimeSpan time) : base(new AutomationTimePickerOptions())
		{
			Type = UIBlockType.Time;
			Time = time;
			TimePickerOptions = (AutomationTimePickerOptions)DateTimeUpDownOptions;
			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="TimePicker" /> class.
		/// </summary>
		public TimePicker() : this(DateTime.Now.TimeOfDay)
		{
		}

		/// <summary>
		///     Triggered when a different time is picked.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<TimePickerChangedEventArgs> Changed
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

		private event EventHandler<TimePickerChangedEventArgs> OnChanged;

		/// <summary>
		///     Gets or sets the last time listed in the time picker control.
		///     Default: <c>TimeSpan.FromMinutes(1439)</c> (1 day - 1 minute).
		/// </summary>
		public TimeSpan EndTime
		{
			get
			{
				return TimePickerOptions.EndTime;
			}

			set
			{
				CheckTimeOfDay(value);
				TimePickerOptions.EndTime = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the drop-down button of the time picker control is shown.
		///     Default: <c>true</c>
		/// </summary>
		public bool HasDropDownButton
		{
			get
			{
				return TimePickerOptions.ShowDropDownButton;
			}

			set
			{
				TimePickerOptions.ShowDropDownButton = value;
			}
		}

		/// <summary>
		///     Gets or sets the height of the time picker control.
		///     Default: 130.
		/// </summary>
		public int MaxDropDownHeight
		{
			get
			{
				return maxDropDownHeight;
			}

			set
			{
				maxDropDownHeight = value;
				TimePickerOptions.MaxDropDownHeight = value;
			}
		}

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		/// <exception cref="ArgumentNullException">When the value is <c>null</c>.</exception>
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
					throw new ArgumentNullException("value");
				}

				BlockDefinition.TooltipText = value;
			}
		}

		/// <summary>
		///     Gets or sets the maximum time of day.
		/// </summary>
		public TimeSpan Maximum
		{
			get
			{
				return maximum;
			}

			set
			{
				CheckTimeOfDay(value);
				maximum = value;
				DateTimeUpDownOptions.Maximum = new DateTime() + value;
			}
		}

		/// <summary>
		///     Gets or sets the minimum time of day.
		/// </summary>
		public TimeSpan Minimum
		{
			get
			{
				return minimum;
			}

			set
			{
				CheckTimeOfDay(value);
				minimum = value;
				DateTimeUpDownOptions.Minimum = new DateTime() + value;
			}
		}

		/// <summary>
		///     Gets or sets the earliest time listed in the time picker control.
		///     Default: <c>TimeSpan.Zero</c>
		/// </summary>
		public TimeSpan StartTime
		{
			get
			{
				return TimePickerOptions.StartTime;
			}

			set
			{
				CheckTimeOfDay(value);
				TimePickerOptions.StartTime = value;
			}
		}

		/// <summary>
		///     Gets or sets the time of day displayed in the time picker.
		/// </summary>
		public TimeSpan Time
		{
			get
			{
				return time;
			}

			set
			{
				CheckTimeOfDay(value);
				time = value;
				BlockDefinition.InitialValue = value.ToString(
					AutomationConfigOptions.GlobalTimeSpanFormat,
					CultureInfo.InvariantCulture);
			}
		}

		/// <summary>
		///     Gets or sets the time interval between two time items in the time picker control.
		///     Default: <c>TimeSpan.FromHours(1)</c>
		/// </summary>
		public TimeSpan TimeInterval
		{
			get
			{
				return TimePickerOptions.TimeInterval;
			}

			set
			{
				CheckTimeOfDay(value);
				TimePickerOptions.TimeInterval = value;
			}
		}

		/// <summary>
		///		Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
		///		This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
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
		///		Gets or sets the text that is shown if the validation state is invalid.
		///		This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
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

		private AutomationTimePickerOptions TimePickerOptions
		{
			get
			{
				return timePickerOptions;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				timePickerOptions = value;
				BlockDefinition.ConfigOptions = value;
			}
		}

		/// <inheritdoc />
		internal override void LoadResult(UIResults uiResults)
		{
			TimeSpan result = uiResults.GetTime(this);
			if ((result != Time) && WantsOnChange)
			{
				changed = true;
				previous = Time;
			}

			Time = result;
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			if (changed && (OnChanged != null))
			{
				OnChanged(this, new TimePickerChangedEventArgs(Time, previous));
			}

			changed = false;
		}

		private static void CheckTimeOfDay(TimeSpan value)
		{
			if ((value.Ticks < 0) && (value.Days >= 1))
			{
				throw new ArgumentOutOfRangeException("value", "TimeSpan must represent time of day");
			}
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class TimePickerChangedEventArgs : EventArgs
		{
			internal TimePickerChangedEventArgs(TimeSpan timeSpan, TimeSpan previous)
			{
				TimeSpan = timeSpan;
				Previous = previous;
			}

			/// <summary>
			///     Gets the previous time of day.
			/// </summary>
			public TimeSpan Previous { get; private set; }

			/// <summary>
			///     Gets the new time of day.
			/// </summary>
			public TimeSpan TimeSpan { get; private set; }
		}
	}
}