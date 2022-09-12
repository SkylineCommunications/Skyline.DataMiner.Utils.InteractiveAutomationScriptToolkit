namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.ComponentModel;

	using Skyline.DataMiner.Automation;

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
		string Tooltip { get; set; }
	}
}