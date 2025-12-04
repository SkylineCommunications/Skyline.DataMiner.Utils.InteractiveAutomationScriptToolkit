namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Linq;

	/// <summary>
	/// Defines a section that allows the user to define a DateTime in a specific time zone.
	/// </summary>
	/// <example>
	///	<code>
	/// public class ScheduleAppointmentDialog : Dialog
	/// {
	/// 	private readonly TimeZoneDateTimePicker _timeZoneDateTimePicker = new TimeZoneDateTimePicker();
	/// 	private readonly TextBox _whatTextBox = new TextBox();
	/// 	private readonly TextBox _whereTextBox = new TextBox();
	/// 
	/// 	private readonly Label _whatLabel = new Label("What?");
	/// 	private readonly Label _whenLabel = new Label("When?");
	/// 	private readonly Label _whereLabel = new Label("Where?");
	/// 
	/// 	public ScheduleAppointmentDialog(IEngine engine) : base(engine)
	/// 	{
	/// 		ContinueButton.Pressed += (s, e) =>
	/// 		{
	/// 			Engine.GenerateInformation($"{Description} on {DateTime.ToString("O")} at {Location}");
	/// 			Engine.ExitSuccess("Exit");
	/// 		};
	/// 
	/// 		BuildUi();
	/// 	}
	/// 
	/// 	public string Description => _whatTextBox.Text;
	/// 
	/// 	public DateTime DateTime => _timeZoneDateTimePicker.DateTime;
	/// 
	/// 	public string Location => _whereTextBox.Text;
	/// 
	/// 	public void BuildUi()
	/// 	{
	/// 		Clear();
	/// 
	/// 		int row = -1;
	/// 
	/// 		AddWidget(_whatLabel, ++row, 0);
	/// 		AddWidget(_whatTextBox, row, 1, 1, 3);
	/// 
	/// 		AddWidget(_whenLabel, ++row, 0);
	/// 		AddSection(_timeZoneDateTimePicker, row, 1);
	/// 
	/// 		AddWidget(_whereLabel, ++row, 0);
	/// 		AddWidget(_whereTextBox, row, 1, 1, 3);
	/// 
	/// 		AddWidget(new WhiteSpace(), ++row, 0);
	/// 
	/// 		AddWidget(ContinueButton, ++row, 0, 1, 4, HorizontalAlignment.Right);
	/// 	}
	/// 
	/// 	public Button ContinueButton { get; } = new Button("Continue") { Style = ButtonStyle.CallToAction };
	/// }
	///	</code>
	/// </example>
	public class TimeZoneDateTimePicker : Section
	{
		private readonly DateTimePicker _dateTimePicker = new DateTimePicker();
		private readonly Button _showHideTimeZoneSelectionButton = new Button("🌍") { Width = 50 };
		private readonly DropDown<TimeZoneInfo> _timeZoneDropDown = new DropDown<TimeZoneInfo>(TimeZoneInfo.GetSystemTimeZones().OrderBy(x => x.BaseUtcOffset)) { IsVisible = false, IsDisplayFilterShown = true };

		private bool wasButtonPressed = false;
		private bool useSelectedTimeZone = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="TimeZoneDateTimePicker"/> class.
		/// </summary>
		public TimeZoneDateTimePicker()
		{
			_showHideTimeZoneSelectionButton.Pressed += (s, e) =>
			{
				ShowHideTimeZoneSelectionButtonPressed();
				Changed?.Invoke(this, new TimeZoneDateTimePickerEventArgs(DateTime, TimeZone));
			};

			_dateTimePicker.Changed += (s, e) => Changed?.Invoke(this, new TimeZoneDateTimePickerEventArgs(DateTime, TimeZone));
			_timeZoneDropDown.Changed += (s, e) => Changed?.Invoke(this, new TimeZoneDateTimePickerEventArgs(DateTime, TimeZone));

			BuildUi();
		}

		/// <summary>
		///     Triggered when a different datetime is picked or when timezone selection changes.
		/// </summary>
		public event EventHandler<TimeZoneDateTimePickerEventArgs> Changed;

		/// <summary>
		/// Sets or gets the DateTime value, optionally taking the selected timezone into account.
		/// </summary>
		public DateTime DateTime
		{
			get
			{
				if (!useSelectedTimeZone) return _dateTimePicker.DateTime;

				var dateTime = new DateTime(_dateTimePicker.ClientDateTime.Year, _dateTimePicker.ClientDateTime.Month, _dateTimePicker.ClientDateTime.Day, _dateTimePicker.ClientDateTime.Hour, _dateTimePicker.ClientDateTime.Minute, _dateTimePicker.ClientDateTime.Second, DateTimeKind.Utc);
				return dateTime.Add(_timeZoneDropDown.Selected.BaseUtcOffset);
			}
		}

		/// <summary>
		/// Gets the selected TimeZone from the dropdown.
		/// </summary>
		public TimeZoneInfo TimeZone
		{
			get
			{
				if (!useSelectedTimeZone) return null;
				return _timeZoneDropDown.Selected;
			}
		}

		/// <summary>
		/// Rebuilds the section.
		/// </summary>
		public void BuildUi()
		{
			Clear();

			AddWidget(_dateTimePicker, 0, 0);
			AddWidget(_showHideTimeZoneSelectionButton, 0, 1);
			AddWidget(_timeZoneDropDown, 0, 2);
		}

		private void ShowHideTimeZoneSelectionButtonPressed()
		{
			if (!wasButtonPressed)
			{
				var defaultTimeZone = _dateTimePicker.ClientTimeZoneInfo;
				if (defaultTimeZone == null)
				{
					defaultTimeZone = TimeZoneInfo.Local;
				}

				_timeZoneDropDown.Selected = defaultTimeZone;
			}

			wasButtonPressed = true;
			useSelectedTimeZone = !useSelectedTimeZone;
			_timeZoneDropDown.IsVisible = useSelectedTimeZone;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class TimeZoneDateTimePickerEventArgs : EventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="TimeZoneDateTimePickerEventArgs"/> class.
			/// </summary>
			/// <param name="dateTime">New datetime value.</param>
			/// <param name="timeZone">New timezone value.</param>
			internal TimeZoneDateTimePickerEventArgs(DateTime dateTime, TimeZoneInfo timeZone)
			{
				DateTime = dateTime;
				TimeZone = timeZone;
			}

			/// <summary>
			///     Gets the new datetime value.
			/// </summary>
			public DateTime DateTime { get; }

			/// <summary>
			///     Gets the new timezone value.
			/// </summary>
			public TimeZoneInfo TimeZone { get; }
		}
	}
}
