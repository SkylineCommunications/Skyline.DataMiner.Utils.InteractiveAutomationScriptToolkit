namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Globalization;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Widget to show/edit a time duration.
	/// </summary>
	public class Time : InteractiveWidget, ITime
	{
		private readonly Validation validation;
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
			validation = new Validation(this);
			TimeUpDownOptions = new AutomationTimeUpDownOptions { UpdateValueOnEnterKey = false };
			TimeSpan = timeSpan;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Time" /> class.
		/// </summary>
		public Time()
			: this(default)
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
		public bool ClipValueToRange
		{
			get => TimeUpDownOptions.ClipValueToMinMax;
			set => TimeUpDownOptions.ClipValueToMinMax = value;
		}

		/// <inheritdoc />
		public int Decimals
		{
			get => TimeUpDownOptions.FractionalSecondsDigitsCount;

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException(nameof(value));
				}

				TimeUpDownOptions.FractionalSecondsDigitsCount = value;
			}
		}

		/// <inheritdoc />
		public bool HasSeconds
		{
			get => TimeUpDownOptions.ShowSeconds;
			set => TimeUpDownOptions.ShowSeconds = value;
		}

		/// <inheritdoc />
		public bool HasSpinnerButton
		{
			get => TimeUpDownOptions.ShowButtonSpinner;
			set => TimeUpDownOptions.ShowButtonSpinner = value;
		}

		/// <inheritdoc />
		public bool IsSpinnerButtonEnabled
		{
			get => TimeUpDownOptions.AllowSpin;
			set => TimeUpDownOptions.AllowSpin = value;
		}

		/// <inheritdoc />
		public TimeSpan Maximum
		{
			get => TimeUpDownOptions.Maximum ?? TimeSpan.MaxValue;

			set
			{
				if (value < Minimum)
				{
					throw new ArgumentOutOfRangeException(nameof(value), "Maximum can't be smaller than Minimum");
				}

				TimeUpDownOptions.Maximum = value;
			}
		}

		/// <inheritdoc />
		public TimeSpan Minimum
		{
			get => TimeUpDownOptions.Minimum ?? TimeSpan.MinValue;

			set
			{
				if (value > Maximum)
				{
					throw new ArgumentOutOfRangeException(nameof(value), "Minimum can't be larger than Maximum");
				}

				TimeUpDownOptions.Minimum = value;
			}
		}

		/// <inheritdoc />
		public TimeSpan TimeSpan
		{
			get => timeSpan;

			set
			{
				timeSpan = value;
				BlockDefinition.InitialValue = timeSpan.ToString(
					AutomationConfigOptions.GlobalTimeSpanFormat,
					CultureInfo.InvariantCulture);
			}
		}

		/// <inheritdoc />
		public string Tooltip
		{
			get => BlockDefinition.TooltipText;
			set => BlockDefinition.TooltipText = value ?? String.Empty;
		}

		/// <inheritdoc />
		public bool UpdateOnEnter
		{
			get => TimeUpDownOptions.UpdateValueOnEnterKey;
			set => TimeUpDownOptions.UpdateValueOnEnterKey = value;
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

		private AutomationTimeUpDownOptions TimeUpDownOptions
		{
			get => timeUpDownOptions;

			set
			{
				timeUpDownOptions = value ?? throw new ArgumentNullException(nameof(value));
				BlockDefinition.ConfigOptions = value;
			}
		}

		/// <inheritdoc />
		protected internal override void LoadResult(UIResults results)
		{
			TimeSpan result = results.GetTime(this);
			if (result != TimeSpan && WantsOnChange)
			{
				changed = true;
				previous = TimeSpan;
			}

			TimeSpan = result;
		}

		/// <inheritdoc />
		protected internal override void RaiseResultEvents()
		{
			if (changed && OnChanged != null)
			{
				OnChanged(this, new ChangedEventArgs(TimeSpan, previous));
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
			/// <param name="timeSpan">The new timespan.</param>
			/// <param name="previous">The previous timespan.</param>
			public ChangedEventArgs(TimeSpan timeSpan, TimeSpan previous)
			{
				TimeSpan = timeSpan;
				Previous = previous;
			}

			/// <summary>
			///     Gets the previous timespan.
			/// </summary>
			public TimeSpan Previous { get; }

			/// <summary>
			///     Gets the new timespan.
			/// </summary>
			public TimeSpan TimeSpan { get; }
		}
	}
}