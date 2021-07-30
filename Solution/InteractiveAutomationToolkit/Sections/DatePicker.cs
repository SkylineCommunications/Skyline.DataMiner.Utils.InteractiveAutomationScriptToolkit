namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// Custom control used to display a date. Make sure that <see cref="Dialog.AllowOverlappingWidgets"/> is set to true when you use this control.
	/// </summary>
	[Obsolete("Use a DateTimePicker with the LongDate or ShortDate format.")]
	public class DatePicker : Section
	{
		private readonly Dictionary<int, string> months = new Dictionary<int, string>()
		                                                  {
			                                                  { 1, "Jan" },
			                                                  { 2, "Feb" },
			                                                  { 3, "Mar" },
			                                                  { 4, "Apr" },
			                                                  { 5, "May" },
			                                                  { 6, "Jun" },
			                                                  { 7, "Jul" },
			                                                  { 8, "Aug" },
			                                                  { 9, "Sep" },
			                                                  { 10, "Oct" },
			                                                  { 11, "Nov" },
			                                                  { 12, "Dec" },
		                                                  };

		private readonly Numeric dayNumeric;

		private readonly DropDown monthDropDown;

		private readonly Numeric yearNumeric;

		private DateTime previous;

		/// <summary>
		/// Initializes a new instance of the DatePicker class.
		/// </summary>
		public DatePicker() : this(DateTime.Now)
		{
		}

		/// <summary>
		/// Initializes a new instance of the DatePicker class.
		/// </summary>
		/// <param name="dateTime"></param>
		public DatePicker(DateTime dateTime)
		{
			previous = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);

			dayNumeric = new Numeric(dateTime.Day)
			             {
				             Decimals = 0,
				             Minimum = 1,
				             Maximum = DateTime.DaysInMonth(dateTime.Year, dateTime.Month),
				             Width = 70,
				             Margin = new Margin(3,3,3,3),
			             };

			monthDropDown = new DropDown(months.Select(x => x.Value), months[dateTime.Month])
			                {
				                Width = 80,
				                Margin = new Margin(3,3,3,3)
			                };

			yearNumeric = new Numeric(dateTime.Year)
			              {
				              Decimals = 0,
				              Minimum = 1800,
				              Maximum = 9999,
				              Width = 80,
				              Margin = new Margin(3,3,3,3)
			              };

			dayNumeric.Changed += DayNumeric_OnChanged;
			monthDropDown.Changed += MonthDropDown_OnChanged;
			yearNumeric.Changed += YearNumeric_OnChanged;

			AddWidget(dayNumeric, 0, 0);
			AddWidget(monthDropDown, 0, 0);
			AddWidget(yearNumeric, 0, 0);
		}

		/// <summary>
		///     Triggered when a different date is picked.
		/// </summary>
		public event EventHandler<DatePickerChangedEventArgs> Changed
		{
			add
			{
				OnChanged += value;
			}

			remove
			{
				OnChanged -= value;
			}
		}

		private event EventHandler<DatePickerChangedEventArgs> OnChanged;

		/// <summary>
		/// Date that is displayed by the control.
		/// </summary>
		public DateTime Date
		{
			get
			{
				return new DateTime((int)yearNumeric.Value, months.First(x => x.Value == monthDropDown.Selected).Key, (int)dayNumeric.Value);
			}

			set
			{
				dayNumeric.Value = value.Day;
				dayNumeric.Maximum = DateTime.DaysInMonth(value.Year, value.Month);
				monthDropDown.Selected = months[value.Month];
				yearNumeric.Value = value.Year;

				previous = new DateTime(value.Year, value.Month, value.Day);
			}
		}

		/// <summary>
		/// Day that is displayed by the control.
		/// </summary>
		public int Day
		{
			get
			{
				return (int)dayNumeric.Value;
			}
		}

		/// <summary>
		/// Month that is displayed by the control.
		/// </summary>
		public int Month
		{
			get
			{
				return months.First(x => x.Value == monthDropDown.Selected).Key;
			}
		}

		/// <summary>
		/// Year that is displayed by the control.
		/// </summary>
		public int Year
		{
			get
			{
				return (int)yearNumeric.Value;
			}
		}

		/// <summary>
		/// Numeric widget that holds the day in this control.
		/// </summary>
		public Numeric DayNumeric
		{
			get
			{
				return dayNumeric;
			}
		}

		/// <summary>
		/// Drop down widget that holds the month in this control.
		/// </summary>
		public DropDown MonthDropDown
		{
			get
			{
				return monthDropDown;
			}
		}

		/// <summary>
		/// Numeric widget that holds the year in this control.
		/// </summary>
		public Numeric YearNumeric
		{
			get
			{
				return yearNumeric;
			}
		}

		private void DayNumeric_OnChanged(object sender, Numeric.NumericChangedEventArgs e)
		{
			RaiseEventResults();
		}

		private void MonthDropDown_OnChanged(object sender, DropDown.DropDownChangedEventArgs e)
		{
			dayNumeric.Maximum = DateTime.DaysInMonth(Year, Month);
			RaiseEventResults();
		}

		private void YearNumeric_OnChanged(object sender, Numeric.NumericChangedEventArgs e)
		{
			dayNumeric.Maximum = DateTime.DaysInMonth(Year, Month);
			RaiseEventResults();
		}

		private void RaiseEventResults()
		{
			if (Date == previous) return;

			if (OnChanged != null)
			{
				OnChanged(this, new DatePickerChangedEventArgs(Date, previous));
			}

			previous = Date;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class DatePickerChangedEventArgs : EventArgs
		{
			internal DatePickerChangedEventArgs(DateTime dateTime, DateTime previous)
			{
				DateTime = dateTime;
				Previous = previous;
			}

			/// <summary>
			///     Gets the new date value.
			/// </summary>
			public DateTime DateTime { get; private set; }

			/// <summary>
			///     Gets the previous date value.
			/// </summary>
			public DateTime Previous { get; private set; }
		}
	}
}
