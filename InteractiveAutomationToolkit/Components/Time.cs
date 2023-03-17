namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Globalization;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Widget to show/edit a time duration.
	/// </summary>
	public class Time : InteractiveWidget
	{
		private bool changed;
		private bool focusLost;

		private TimeSpan previous;
		private TimeSpan timeSpan;
		private AutomationTimeUpDownOptions timeUpDownOptions;

		/// <summary>
		///     Initializes a new instance of the <see cref="Time" /> class.
		/// </summary>
		/// <param name="timeSpan">The timespan displayed in the time widget.</param>
		public Time(TimeSpan timeSpan)
		{
			Type = UIBlockType.Time;
			TimeUpDownOptions = new AutomationTimeUpDownOptions { UpdateValueOnEnterKey = false };
			TimeSpan = timeSpan;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Time" /> class.
		/// </summary>
		public Time() : this(default)
		{
		}

		/// <summary>
		///     Triggered when the timespan changes.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<TimeChangedEventArgs> Changed
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

		/// <summary>
		///     Triggered when the user loses focus of the Time.
		///     WantsOnFocusLost will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<TimeFocusLostEventArgs> FocusLost
		{
			add
			{
				OnFocusLost += value;
				BlockDefinition.WantsOnFocusLost = true;
			}

			remove
			{
				OnFocusLost -= value;
				if (OnFocusLost == null || !OnFocusLost.GetInvocationList().Any())
				{
					BlockDefinition.WantsOnFocusLost = false;
				}
			}
		}

		private event EventHandler<TimeChangedEventArgs> OnChanged;

		private event EventHandler<TimeFocusLostEventArgs> OnFocusLost;

		/// <summary>
		///     Gets or sets a value indicating whether the value is clipped to the range.
		///     Default: <c>false</c>.
		/// </summary>
		public bool ClipValueToRange
		{
			get
			{
				return TimeUpDownOptions.ClipValueToMinMax;
			}

			set
			{
				TimeUpDownOptions.ClipValueToMinMax = value;
			}
		}

		/// <summary>
		///     Gets or sets the number of digits to be used in order to represent the fractions of seconds.
		///     Default: <c>0</c>.
		/// </summary>
		public int Decimals
		{
			get
			{
				return TimeUpDownOptions.FractionalSecondsDigitsCount;
			}

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				TimeUpDownOptions.FractionalSecondsDigitsCount = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether seconds are displayed in the time widget.
		///     Default: <c>true</c>.
		/// </summary>
		public bool HasSeconds
		{
			get
			{
				return TimeUpDownOptions.ShowSeconds;
			}

			set
			{
				TimeUpDownOptions.ShowSeconds = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether a spinner button is shown.
		///     Default: <c>true</c>.
		/// </summary>
		public bool HasSpinnerButton
		{
			get
			{
				return TimeUpDownOptions.ShowButtonSpinner;
			}

			set
			{
				TimeUpDownOptions.ShowButtonSpinner = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the spinner button is enabled.
		///     Default: <c>true</c>.
		/// </summary>
		public bool IsSpinnerButtonEnabled
		{
			get
			{
				return TimeUpDownOptions.AllowSpin;
			}

			set
			{
				TimeUpDownOptions.AllowSpin = value;
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
		///     Gets or sets the maximum timespan.
		///     Default: <c>TimeSpan.MaxValue</c>.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the maximum is smaller than the minimum.</exception>
		public TimeSpan Maximum
		{
			get
			{
				return TimeUpDownOptions.Maximum ?? TimeSpan.MaxValue;
			}

			set
			{
				if (value < Minimum)
				{
					throw new ArgumentOutOfRangeException("value", "Maximum can't be smaller than Minimum");
				}

				TimeUpDownOptions.Maximum = value;
			}
		}

		/// <summary>
		///     Gets or sets the minimum timespan.
		///     Default: <c>TimeSpan.MinValue</c>.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the minimum is larger than the maximum.</exception>
		public TimeSpan Minimum
		{
			get
			{
				return TimeUpDownOptions.Minimum ?? TimeSpan.MinValue;
			}

			set
			{
				if (value > Maximum)
				{
					throw new ArgumentOutOfRangeException("value", "Minimum can't be larger than Maximum");
				}

				TimeUpDownOptions.Minimum = value;
			}
		}

		/// <summary>
		///     Gets or sets the timespan displayed in the time widget.
		/// </summary>
		public TimeSpan TimeSpan
		{
			get
			{
				return timeSpan;
			}

			set
			{
				timeSpan = value;
				BlockDefinition.InitialValue = timeSpan.ToString(
					AutomationConfigOptions.GlobalTimeSpanFormat,
					CultureInfo.InvariantCulture);
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the widget will only trigger an event when the enter key is pressed.
		///     Default: <c>false</c>.
		/// </summary>
		public bool UpdateOnEnter
		{
			get
			{
				return TimeUpDownOptions.UpdateValueOnEnterKey;
			}

			set
			{
				TimeUpDownOptions.UpdateValueOnEnterKey = value;
			}
		}

		/// <summary>
		/// 	Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
		/// 	This should be used by the client to add a visual marker on the input field.
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
		/// 	Gets or sets the text that is shown if the validation state is invalid.
		/// 	This should be used by the client to add a visual marker on the input field.
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

		private AutomationTimeUpDownOptions TimeUpDownOptions
		{
			get
			{
				return timeUpDownOptions;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				timeUpDownOptions = value;
				BlockDefinition.ConfigOptions = value;
			}
		}

		/// <inheritdoc />
		internal override void LoadResult(UIResults uiResults)
		{
			TimeSpan result = uiResults.GetTime(this);
			bool wasOnFocusLost = uiResults.WasOnFocusLost(this);

			if ((result != TimeSpan) && BlockDefinition.WantsOnChange)
			{
				changed = true;
				previous = TimeSpan;
			}

			if (BlockDefinition.WantsOnFocusLost)
			{
				focusLost = wasOnFocusLost;
			}

			TimeSpan = result;
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			if (changed)
			{
				OnChanged?.Invoke(this, new TimeChangedEventArgs(TimeSpan, previous));
			}

			if (focusLost)
			{
				OnFocusLost?.Invoke(this, new TimeFocusLostEventArgs(TimeSpan));
			}

			changed = false;
			focusLost = false;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class TimeChangedEventArgs : EventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="TimeChangedEventArgs"/> class.
			/// </summary>
			/// <param name="timeSpan"></param>
			/// <param name="previous"></param>
			internal TimeChangedEventArgs(TimeSpan timeSpan, TimeSpan previous)
			{
				TimeSpan = timeSpan;
				Previous = previous;
			}

			/// <summary>
			///     Gets the previous timespan.
			/// </summary>
			public TimeSpan Previous { get; private set; }

			/// <summary>
			///     Gets the new timespan.
			/// </summary>
			public TimeSpan TimeSpan { get; private set; }
		}

		/// <summary>
		///     Provides data for the <see cref="FocusLost" /> event.
		/// </summary>
		public class TimeFocusLostEventArgs : EventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="TimeFocusLostEventArgs"/> class.
			/// </summary>
			/// <param name="timeSpan"></param>
			internal TimeFocusLostEventArgs(TimeSpan timeSpan)
			{
				TimeSpan = timeSpan;
			}

			/// <summary>
			///     Gets the new timespan.
			/// </summary>
			public TimeSpan TimeSpan { get; private set; }
		}
	}
}
