namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Represents a widget to show/edit a datetime.
	/// </summary>
	public interface ICalendar : IInteractiveWidget
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
		/// <exception cref="ArgumentNullException">When the value is <c>null</c>.</exception>
		string Tooltip { get; set; }

		/// <summary>
		///     Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
		///     This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner 10.0.5 onwards.</remarks>
		UIValidationState ValidationState { get; set; }

		/// <summary>
		///     Gets or sets the text that is shown if the validation state is invalid.
		///     This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner 10.0.5 onwards.</remarks>
		string ValidationText { get; set; }
	}
}