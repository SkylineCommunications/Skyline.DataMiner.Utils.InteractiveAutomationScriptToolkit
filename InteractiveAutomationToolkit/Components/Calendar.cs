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
			IsReadOnly = false;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Calendar" /> class.
		/// </summary>
		public Calendar() : this(DateTime.Now)
		{
		}

		/// <summary>
		///		Gets or sets a value indicating whether the control is displayed in read-only mode.
		///		Read-only mode causes the widgets to appear read-write but the user won't be able to change their value.
		///		This only affects interactive scripts running in a web environment.
		/// </summary>
		/// <remarks>Available from DataMiner 10.4.1 onwards.</remarks>
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

		/// <summary>
		///     Load any changes made through user interaction.
		/// </summary>
		/// <param name="uiResults">
		///     Represents the information a user has entered or selected in a dialog box of an interactive
		///     Automation script.
		/// </param>
		/// <remarks><see cref="InteractiveWidget.DestVar" /> should be used as key to get the changes for this widget.</remarks>
		protected internal override void LoadResult(UIResults uiResults)
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

		/// <summary>
		///     Raises zero or more events of the widget.
		///     This method is called after <see cref="InteractiveWidget.LoadResult" /> was called on all widgets.
		/// </summary>
		/// <remarks>It is up to the implementer to determine if an event must be raised.</remarks>
		protected internal override void RaiseResultEvents()
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