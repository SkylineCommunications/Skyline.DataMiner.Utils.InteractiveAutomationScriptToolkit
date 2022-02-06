namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;

	using Automation;

	/// <summary>
	/// Base class for time-based widgets that rely on the <see cref="AutomationDateTimeUpDownOptions" />.
	/// </summary>
	public abstract class TimePickerBase : InteractiveWidget, ITimePickerBase
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

		/// <inheritdoc />
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

		/// <inheritdoc />
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

		/// <inheritdoc />
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

		/// <inheritdoc />
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

		/// <inheritdoc />
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

		/// <inheritdoc />
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

		/// <inheritdoc />
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
					throw new ArgumentNullException(nameof(value));
				}

				dateTimeUpDownOptions = value;
				BlockDefinition.ConfigOptions = value;
			}
		}
	}
}
