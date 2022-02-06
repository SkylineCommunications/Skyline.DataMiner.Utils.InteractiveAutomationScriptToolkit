namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Globalization;
	using System.Linq;

	using Automation;

	/// <summary>
	///     Widget to show/edit a time of day.
	/// </summary>
	public class TimePicker : TimePickerBase, ITimePicker
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

		/// <inheritdoc />
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
				if (OnChanged == null || !OnChanged.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<TimePickerChangedEventArgs> OnChanged;

		/// <inheritdoc />
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

		/// <inheritdoc />
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

		/// <inheritdoc />
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

		/// <inheritdoc />
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

		/// <inheritdoc />
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

		/// <inheritdoc />
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

		/// <inheritdoc />
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
					throw new ArgumentNullException(nameof(value));
				}

				timePickerOptions = value;
				BlockDefinition.ConfigOptions = value;
			}
		}

		/// <inheritdoc />
		internal override void LoadResult(UIResults uiResults)
		{
			TimeSpan result = uiResults.GetTime(this);
			if (result != Time && WantsOnChange)
			{
				changed = true;
				previous = Time;
			}

			Time = result;
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			if (changed && OnChanged != null)
			{
				OnChanged(this, new TimePickerChangedEventArgs(Time, previous));
			}

			changed = false;
		}

		private static void CheckTimeOfDay(TimeSpan value)
		{
			if (value.Ticks < 0 && value.Days >= 1)
			{
				throw new ArgumentOutOfRangeException(nameof(value), "TimeSpan must represent time of day");
			}
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class TimePickerChangedEventArgs : EventArgs
		{
			public TimePickerChangedEventArgs(TimeSpan timeSpan, TimeSpan previous)
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