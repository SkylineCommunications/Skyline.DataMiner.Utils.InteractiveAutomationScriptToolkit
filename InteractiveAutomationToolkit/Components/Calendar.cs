namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Globalization;
	using System.Linq;
	using Microsoft.Extensions.Logging;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript.Extensions;

	/// <summary>
	///     Widget to show/edit a datetime.
	/// </summary>
	public class Calendar : InteractiveWidget, IValidationWidget, IIsReadonlyWidget
	{
		private bool changed;
		private bool focusLost;

		private DateTime dateTime;
		private DateTime previous;

		/// <summary>
		///     Initializes a new instance of the <see cref="Calendar" /> class.
		/// </summary>
		/// <param name="dateTime">Value displayed on the calendar.</param>
		public Calendar(DateTime dateTime)
		{
			Type = UIBlockType.Calendar;
			DateTime = dateTime;
			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
			IsReadOnly = false;
			BlockDefinition.ClientTimeInfo = UIClientTimeInfo.Return;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Calendar" /> class.
		/// </summary>
		public Calendar() : this(DateTime.Now)
		{
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public virtual bool IsReadOnly
		{
			get
			{
				return BlockDefinition.IsReadOnly;
			}

			set
			{
				BlockDefinition.IsReadOnly = value;
			}
		}

		/// <summary>
		///     Triggered when a different datetime is picked.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<CalendarChangedEventArgs> Changed
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
		///     Triggered when the user loses focus of the Calender.
		///     WantsOnFocusLost will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<CalendarFocusLostEventArgs> FocusLost
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

		private event EventHandler<CalendarChangedEventArgs> OnChanged;

		private event EventHandler<CalendarFocusLostEventArgs> OnFocusLost;

		/// <summary>
		///     Gets or sets the datetime displayed on the calendar.
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
		///		Gets the datetime as displayed in the client.
		///		If the client datetime is not available, the returned value will be <see cref="DateTimeOffset.MinValue"/>.
		/// </summary>
		public DateTimeOffset ClientDateTime { get; private set; } = DateTimeOffset.MinValue;

		/// <summary>
		///		Gets the time zone info of the client in which this <see cref="DateTimePicker"/> is displayed.
		///		If the client time zone info is not available, the returned value will be null.
		/// </summary>
		public TimeZoneInfo ClientTimeZoneInfo { get; private set; }

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
		/// 	Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
		/// 	This should be used by the client to add a visual marker on the input field.
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
		/// 	Gets or sets the text that is shown if the validation state is invalid.
		/// 	This should be used by the client to add a visual marker on the input field.
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

		/// <inheritdoc	/>
		protected internal override void LoadResult(IUIResults uiResults, ILogger logger = null)
		{
			DateTime result = uiResults.GetDateTime(DestVar);
			logger?.Debug(nameof(Calendar), nameof(LoadResult), result.ToString("O"));

			bool wasOnFocusLost = uiResults.WasOnFocusLost(this);

			ClientDateTime = uiResults.GetClientDateTime(this);

			try
			{
				ClientTimeZoneInfo = uiResults.GetClientTimeZoneInfo(this);
			}
			catch (System.Runtime.Serialization.SerializationException)
			{
				ClientTimeZoneInfo = null;
			}

			if (BlockDefinition.WantsOnChange && (result != DateTime))
			{
				changed = true;
				previous = DateTime;
			}

			if (BlockDefinition.WantsOnFocusLost)
			{
				focusLost = wasOnFocusLost;
			}

			DateTime = result;
		}

		/// <inheritdoc	/>
		protected internal override void RaiseResultEvents(ILogger logger = null)
		{
			if (changed && OnChanged != null)
			{
				logger?.Debug(nameof(Calendar), nameof(RaiseResultEvents), $"OnChange; Value changed from {previous.ToString("O")} to {DateTime.ToString("O")}");
				OnChanged?.Invoke(this, new CalendarChangedEventArgs(DateTime, previous));
			}

			if (focusLost)
			{
				logger?.Debug(nameof(Calendar), nameof(RaiseResultEvents), $"OnFocusLost; Lost focus with value {DateTime.ToString("O")}");
				OnFocusLost?.Invoke(this, new CalendarFocusLostEventArgs(DateTime));
			}

			changed = false;
			focusLost = false;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class CalendarChangedEventArgs : EventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="CalendarChangedEventArgs"/> class.
			/// </summary>
			/// <param name="dateTime">The new value.</param>
			/// <param name="previous">The previous value.</param>
			internal CalendarChangedEventArgs(DateTime dateTime, DateTime previous)
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

		/// <summary>
		///     Provides data for the <see cref="FocusLost" /> event.
		/// </summary>
		public class CalendarFocusLostEventArgs : EventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="CalendarFocusLostEventArgs"/> class.
			/// </summary>
			/// <param name="dateTime">The new value.</param>
			internal CalendarFocusLostEventArgs(DateTime dateTime)
			{
				DateTime = dateTime;
			}

			/// <summary>
			///     Gets the new datetime value.
			/// </summary>
			public DateTime DateTime { get; private set; }
		}
	}
}