namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.ComponentModel;
	using System.Globalization;
	using System.Linq;

	using Skyline.DataMiner.Automation;

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
		public TimePicker(TimeSpan time)
			: base(new AutomationTimePickerOptions())
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
		public TimePicker()
			: this(DateTime.Now.TimeOfDay)
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
		public TimeSpan EndTime
		{
			get => TimePickerOptions.EndTime;

			set
			{
				CheckTimeOfDay(value);
				TimePickerOptions.EndTime = value;
			}
		}

		/// <inheritdoc />
		public bool HasDropDownButton
		{
			get => TimePickerOptions.ShowDropDownButton;
			set => TimePickerOptions.ShowDropDownButton = value;
		}

		/// <inheritdoc />
		public int MaxDropDownHeight
		{
			get => maxDropDownHeight;

			set
			{
				maxDropDownHeight = value;
				TimePickerOptions.MaxDropDownHeight = value;
			}
		}

		/// <inheritdoc />
		public TimeSpan Maximum
		{
			get => maximum;

			set
			{
				CheckTimeOfDay(value);
				maximum = value;
				DateTimeUpDownOptions.Maximum = default(DateTime) + value;
			}
		}

		/// <inheritdoc />
		public TimeSpan Minimum
		{
			get => minimum;

			set
			{
				CheckTimeOfDay(value);
				minimum = value;
				DateTimeUpDownOptions.Minimum = default(DateTime) + value;
			}
		}

		/// <inheritdoc />
		public TimeSpan StartTime
		{
			get => TimePickerOptions.StartTime;

			set
			{
				CheckTimeOfDay(value);
				TimePickerOptions.StartTime = value;
			}
		}

		/// <inheritdoc />
		public TimeSpan Time
		{
			get => time;

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
			get => TimePickerOptions.TimeInterval;

			set
			{
				CheckTimeOfDay(value);
				TimePickerOptions.TimeInterval = value;
			}
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
			get => BlockDefinition.ValidationState;

			set
			{
				if (!Enum.IsDefined(typeof(UIValidationState), value))
				{
					throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(UIValidationState));
				}

				BlockDefinition.ValidationState = value;
			}
		}

		/// <inheritdoc />
		public string ValidationText
		{
			get => BlockDefinition.ValidationText;
			set => BlockDefinition.ValidationText = value ?? String.Empty;
		}

		private AutomationTimePickerOptions TimePickerOptions
		{
			get => timePickerOptions;

			set
			{
				timePickerOptions = value ?? throw new ArgumentNullException(nameof(value));
				BlockDefinition.ConfigOptions = value;
			}
		}

		/// <inheritdoc />
		protected internal override void LoadResult(UIResults results)
		{
			TimeSpan result = results.GetTime(this);
			if (result != Time && WantsOnChange)
			{
				changed = true;
				previous = Time;
			}

			Time = result;
		}

		/// <inheritdoc />
		protected internal override void RaiseResultEvents()
		{
			if (changed && OnChanged != null)
			{
				OnChanged(this, new ChangedEventArgs(Time, previous));
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
		public class ChangedEventArgs : EventArgs
		{
			/// <summary>
			///     Initializes a new instance of the <see cref="ChangedEventArgs" /> class.
			/// </summary>
			/// <param name="timeSpan">The new time of day.</param>
			/// <param name="previous">The previous time of day.</param>
			public ChangedEventArgs(TimeSpan timeSpan, TimeSpan previous)
			{
				TimeSpan = timeSpan;
				Previous = previous;
			}

			/// <summary>
			///     Gets the previous time of day.
			/// </summary>
			public TimeSpan Previous { get; }

			/// <summary>
			///     Gets the new time of day.
			/// </summary>
			public TimeSpan TimeSpan { get; }
		}
	}
}