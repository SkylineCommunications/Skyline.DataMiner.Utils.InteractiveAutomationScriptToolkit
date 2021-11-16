namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
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
		public Time() : this(new TimeSpan())
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

		private event EventHandler<TimeChangedEventArgs> OnChanged;

		/// <summary>
		///     Gets or sets a value indicating whether the value is clipped to the range.
		///     Default: <c>false</c>
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
		///     Default: <c>0</c>
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
		///     Default: <c>true</c>
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
		///     Default: <c>true</c>
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
		///     Default: <c>true</c>
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
		///     Default: <c>TimeSpan.MaxValue</c>
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
		///     Default: <c>TimeSpan.MinValue</c>
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
		///     Default: <c>false</c>
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
			if ((result != TimeSpan) && WantsOnChange)
			{
				changed = true;
				previous = TimeSpan;
			}

			TimeSpan = result;
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			if (changed && (OnChanged != null))
			{
				OnChanged(this, new TimeChangedEventArgs(TimeSpan, previous));
			}

			changed = false;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class TimeChangedEventArgs : EventArgs
		{
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
	}
}
