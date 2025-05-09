namespace Skyline.DataMiner.Utils.InteractiveAutomationScript.Sections
{
	using System;

	/// <summary>
	/// Defines a section that allows the user to define a DateTime in a specific time zone.
	/// </summary>
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

			Build();
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
		/// Rebuilds the section.
		/// </summary>
		public void Build()
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
