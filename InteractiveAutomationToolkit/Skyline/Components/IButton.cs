namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;

	/// <summary>
	///     Represents a button that can be pressed.
	/// </summary>
	public interface IButton : IInteractiveWidget
	{
		/// <summary>
		///     Triggered when the button is pressed.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		event EventHandler<EventArgs> Pressed;

		/// <summary>
		///     Gets or sets the text displayed in the button.
		/// </summary>
		string Text { get; set; }

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		/// <exception cref="ArgumentNullException">When the value is <c>null</c>.</exception>
		string Tooltip { get; set; }
	}
}