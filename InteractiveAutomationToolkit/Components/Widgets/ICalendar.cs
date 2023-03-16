namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;

	/// <summary>
	///     Represents a widget to show/edit a datetime.
	/// </summary>
	public interface ICalendar : IInteractiveWidget, IValidate
	{
		/// <summary>
		///     Triggered when a different datetime is picked.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		event EventHandler<Calendar.ChangedEventArgs> Changed;

		/// <summary>
		///     Gets or sets the datetime displayed on the calendar.
		/// </summary>
		DateTime DateTime { get; set; }

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		/// <remarks>This property only works for web compliant scripts or when launched from a DataMiner web app.</remarks>
		/// <remarks>Available from DataMiner Feature Release 10.0.8 and Main Release 10.1.0 onwards.</remarks>
		string Tooltip { get; set; }
	}
}