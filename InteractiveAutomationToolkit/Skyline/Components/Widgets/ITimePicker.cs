namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System;

	/// <summary>
	///     Represents a widget to show/edit a time of day.
	/// </summary>
	public interface ITimePicker : ITimePickerBase, IValidate
	{
		/// <summary>
		///     Triggered when a different time is picked.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		event EventHandler<TimePicker.ChangedEventArgs> Changed;

		/// <summary>
		///     Gets or sets the last time listed in the time picker control.
		///     Default: <c>TimeSpan.FromMinutes(1439)</c> (1 day - 1 minute).
		/// </summary>
		TimeSpan EndTime { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether the drop-down button of the time picker control is shown.
		///     Default: <c>true</c>.
		/// </summary>
		bool HasDropDownButton { get; set; }

		/// <summary>
		///     Gets or sets the height of the time picker control.
		///     Default: 130.
		/// </summary>
		int MaxDropDownHeight { get; set; }

		/// <summary>
		///     Gets or sets the maximum time of day.
		/// </summary>
		TimeSpan Maximum { get; set; }

		/// <summary>
		///     Gets or sets the minimum time of day.
		/// </summary>
		TimeSpan Minimum { get; set; }

		/// <summary>
		///     Gets or sets the earliest time listed in the time picker control.
		///     Default: <c>TimeSpan.Zero</c>.
		/// </summary>
		TimeSpan StartTime { get; set; }

		/// <summary>
		///     Gets or sets the time of day displayed in the time picker.
		/// </summary>
		TimeSpan Time { get; set; }

		/// <summary>
		///     Gets or sets the time interval between two time items in the time picker control.
		///     Default: <c>TimeSpan.FromHours(1)</c>.
		/// </summary>
		TimeSpan TimeInterval { get; set; }

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		string Tooltip { get; set; }
	}
}