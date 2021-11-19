namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using Skyline.DataMiner.Automation;

	/// <summary>
	/// Base class for time-based widgets that rely on the <see cref="AutomationDateTimeUpDownOptions" />.
	/// </summary>
	public abstract class TimePickerBase : InteractiveWidget
	{
		private AutomationDateTimeUpDownOptions dateTimeUpDownOptions;

		/// <summary>
		/// Initializes a new instance of the <see cref="TimePickerBase" />
		/// </summary>
		/// <param name="dateTimeUpDownOptions">Configuration for the new TimePickerBase instance.</param>
		protected TimePickerBase(AutomationDateTimeUpDownOptions dateTimeUpDownOptions)
		{
			DateTimeUpDownOptions = dateTimeUpDownOptions;
			UpdateOnEnter = false;
			Kind = DateTimeKind.Local;
		}

		/// <summary>
		///     Gets or sets a value indicating whether the spinner button is enabled.
		///     Default <c>true</c>.
		/// </summary>
		public bool IsSpinnerButtonEnabled
		{
			get
			{
				return DateTimeUpDownOptions.AllowSpin;
			}

			set
			{
				DateTimeUpDownOptions.AllowSpin = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the widget has a spinner button.
		///     Default <c>true</c>.
		/// </summary>
		public bool HasSpinnerButton
		{
			get
			{
				return DateTimeUpDownOptions.ShowButtonSpinner;
			}

			set
			{
				DateTimeUpDownOptions.ShowButtonSpinner = value;
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
				return DateTimeUpDownOptions.UpdateValueOnEnterKey;
			}

			set
			{
				DateTimeUpDownOptions.UpdateValueOnEnterKey = value;
			}
		}

		/// <summary>
		///     Gets or sets the date and time format used by the up-down control.
		///     Default: FullDateTime in DataMiner 9.5.3, general dateTime from DataMiner 9.5.4 onwards (Format = Custom, CustomFormat = "G").
		/// </summary>
		public DateTimeFormat DateTimeFormat
		{
			get
			{
				return DateTimeUpDownOptions.Format;
			}

			set
			{
				DateTimeUpDownOptions.Format = value;
			}
		}

		/// <summary>
		///     Gets or sets the date-time format to be used by the control when DateTimeFormat is set to
		///     <c>DateTimeFormat.Custom</c>.
		///     Default: G (from DataMiner 9.5.4 onwards; previously the default value was null).
		/// </summary>
		/// <remarks>Sets <see cref="DateTimeFormat" /> to <c>DateTimeFormat.Custom</c></remarks>
		public string CustomDateTimeFormat
		{
			get
			{
				return DateTimeUpDownOptions.FormatString;
			}

			set
			{
				DateTimeUpDownOptions.FormatString = value;
				DateTimeFormat = DateTimeFormat.Custom;
			}
		}

		/// <summary>
		///     Gets or sets the DateTimeKind (.NET) used by the datetime up-down control.
		///     Default: <c>DateTimeKind.Unspecified</c>
		/// </summary>
		public DateTimeKind Kind
		{
			get
			{
				return DateTimeUpDownOptions.Kind;
			}

			set
			{
				DateTimeUpDownOptions.Kind = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the value is clipped to the range.
		///     Default: <c>false</c>
		/// </summary>
		public bool ClipValueToRange
		{
			get
			{
				return DateTimeUpDownOptions.ClipValueToMinMax;
			}

			set
			{
				DateTimeUpDownOptions.ClipValueToMinMax = value;
			}
		}

		/// <summary>
		/// Configuration of this <see cref="TimePickerBase" /> instance.
		/// </summary>
		protected AutomationDateTimeUpDownOptions DateTimeUpDownOptions
		{
			get
			{
				return dateTimeUpDownOptions;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				dateTimeUpDownOptions = value;
				BlockDefinition.ConfigOptions = value;
			}
		}
	}
}
