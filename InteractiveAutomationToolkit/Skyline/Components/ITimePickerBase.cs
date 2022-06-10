namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Interface for the base class for time-based widgets that rely on the <see cref="AutomationDateTimeUpDownOptions" />.
	/// </summary>
	public interface ITimePickerBase : IInteractiveWidget
	{
		/// <summary>
		///     Gets or sets a value indicating whether the value is clipped to the range.
		///     Default: <c>false</c>.
		/// </summary>
		bool ClipValueToRange { get; set; }

		/// <summary>
		///     Gets or sets the date-time format to be used by the control when DateTimeFormat is set to
		///     <c>DateTimeFormat.Custom</c>.
		///     Default: G (from DataMiner 9.5.4 onwards; previously the default value was null).
		/// </summary>
		/// <remarks>Sets <see cref="DateTimeFormat" /> to <c>DateTimeFormat.Custom</c>.</remarks>
		string CustomDateTimeFormat { get; set; }

		/// <summary>
		///     Gets or sets the date and time format used by the up-down control.
		///     Default: FullDateTime in DataMiner 9.5.3, general dateTime from DataMiner 9.5.4 onwards (Format = Custom,
		///     CustomFormat = "G").
		/// </summary>
		DateTimeFormat DateTimeFormat { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether the widget has a spinner button.
		///     Default <c>true</c>.
		/// </summary>
		bool HasSpinnerButton { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether the spinner button is enabled.
		///     Default <c>true</c>.
		/// </summary>
		bool IsSpinnerButtonEnabled { get; set; }

		/// <summary>
		///     Gets or sets the DateTimeKind (.NET) used by the datetime up-down control.
		///     Default: <c>DateTimeKind.Unspecified</c>.
		/// </summary>
		DateTimeKind Kind { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether the widget will only trigger an event when the enter key is pressed.
		///     Default: <c>false</c>.
		/// </summary>
		bool UpdateOnEnter { get; set; }
	}
}