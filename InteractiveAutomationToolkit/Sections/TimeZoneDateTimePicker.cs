namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;

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
		private readonly Button _showHideTimeZoneSelectionButton = new Button("🌍") { Width = 44 };
		private readonly DropDown<TimeZoneInfo> _timeZoneDropDown = new DropDown<TimeZoneInfo>(TimeZoneInfo.GetSystemTimeZones()) { IsVisible = false, IsDisplayFilterShown = true, IsSorted = true };

		private bool wasButtonPressed = false;
		private bool useSelectedTimeZone = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="TimeZoneDateTimePicker"/> class.
		/// </summary>
		public TimeZoneDateTimePicker()
		{
			_showHideTimeZoneSelectionButton.Pressed += ShowHideTimeZoneSelectionButton_Pressed;

			BuildUi();
		}

		/// <summary>
		/// Sets or gets the DateTime value, optionally taking the selected timezone into account.
		/// </summary>
		public DateTime DateTime
		{
			get
			{
				if (!useSelectedTimeZone) return _dateTimePicker.DateTime;

				var offset = TimeZoneInfo.Local.BaseUtcOffset - _timeZoneDropDown.Selected.BaseUtcOffset;
				return _dateTimePicker.DateTime.Add(offset);
			}

			set
			{
				if (!useSelectedTimeZone)
				{
					_dateTimePicker.DateTime = value;
				}
				else
				{
					var offset = _timeZoneDropDown.Selected.BaseUtcOffset - TimeZoneInfo.Local.BaseUtcOffset;
					_dateTimePicker.DateTime = value.Add(offset);
				}
			}
		}

		/// <summary>
		/// Gets the selected TimeZone from the dropdown.
		/// </summary>
		public TimeZoneInfo TimeZone => _timeZoneDropDown.Selected;

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

		private void ShowHideTimeZoneSelectionButton_Pressed(object sender, EventArgs e)
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
	}
}
