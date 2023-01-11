namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Base class for time-based widgets that rely on the <see cref="AutomationDateTimeUpDownOptions" />.
	/// </summary>
	public abstract class TimePickerBase : InteractiveWidget, ITimePickerBase
	{
		private AutomationDateTimeUpDownOptions dateTimeUpDownOptions;

		/// <summary>
		///     Initializes a new instance of the <see cref="TimePickerBase" /> class.
		/// </summary>
		/// <param name="dateTimeUpDownOptions">Configuration for the new TimePickerBase instance.</param>
		protected TimePickerBase(AutomationDateTimeUpDownOptions dateTimeUpDownOptions)
		{
			DateTimeUpDownOptions = dateTimeUpDownOptions;
			UpdateOnEnter = false;
			Kind = DateTimeKind.Local;
		}

		/// <inheritdoc />
		public bool ClipValueToRange
		{
			get => DateTimeUpDownOptions.ClipValueToMinMax;
			set => DateTimeUpDownOptions.ClipValueToMinMax = value;
		}

		/// <inheritdoc />
		public string CustomDateTimeFormat
		{
			get => DateTimeUpDownOptions.FormatString;

			set
			{
				DateTimeUpDownOptions.FormatString = value;
				DateTimeFormat = DateTimeFormat.Custom;
			}
		}

		/// <inheritdoc />
		public DateTimeFormat DateTimeFormat
		{
			get => DateTimeUpDownOptions.Format;
			set => DateTimeUpDownOptions.Format = value;
		}

		/// <inheritdoc />
		public bool HasSpinnerButton
		{
			get => DateTimeUpDownOptions.ShowButtonSpinner;
			set => DateTimeUpDownOptions.ShowButtonSpinner = value;
		}

		/// <inheritdoc />
		public bool IsSpinnerButtonEnabled
		{
			get => DateTimeUpDownOptions.AllowSpin;
			set => DateTimeUpDownOptions.AllowSpin = value;
		}

		/// <inheritdoc />
		public DateTimeKind Kind
		{
			get => DateTimeUpDownOptions.Kind;
			set => DateTimeUpDownOptions.Kind = value;
		}

		/// <inheritdoc />
		public bool UpdateOnEnter
		{
			get => DateTimeUpDownOptions.UpdateValueOnEnterKey;
			set => DateTimeUpDownOptions.UpdateValueOnEnterKey = value;
		}

		/// <summary>
		///     Gets or sets the configuration of this <see cref="TimePickerBase" /> instance.
		/// </summary>
		protected AutomationDateTimeUpDownOptions DateTimeUpDownOptions
		{
			get => dateTimeUpDownOptions;

			set
			{
				dateTimeUpDownOptions = value ?? throw new ArgumentNullException(nameof(value));
				BlockDefinition.ConfigOptions = value;
			}
		}
	}
}