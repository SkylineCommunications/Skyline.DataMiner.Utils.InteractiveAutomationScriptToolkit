namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Globalization;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Widget to show/edit a datetime.
	/// </summary>
	public class Calendar : InteractiveWidget
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
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Calendar" /> class.
		/// </summary>
		public Calendar() : this(DateTime.Now)
		{
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

		/// <inheritdoc />
		internal override void LoadResult(UIResults uiResults)
		{
			DateTime result = uiResults.GetDateTime(DestVar);
			bool wasOnFocusLost = uiResults.WasOnFocusLost(this);

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

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			if (changed && OnChanged != null)
			{
				OnChanged?.Invoke(this, new CalendarChangedEventArgs(DateTime, previous));
			}

			if (focusLost)
			{
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
			/// <param name="dateTime"></param>
			/// <param name="previous"></param>
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
			/// <param name="dateTime"></param>
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