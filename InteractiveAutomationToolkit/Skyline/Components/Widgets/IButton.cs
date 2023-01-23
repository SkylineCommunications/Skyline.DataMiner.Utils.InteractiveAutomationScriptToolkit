namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
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
		/// <remarks>This property only works for web compliant scripts or when launched from a DataMiner web app.</remarks>
		/// <remarks>Available from DataMiner Feature Release 10.0.8 and Main Release 10.1.0 onwards.</remarks>
		string Tooltip { get; set; }
	}
}