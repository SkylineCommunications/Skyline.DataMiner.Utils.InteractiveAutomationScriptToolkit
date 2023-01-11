namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	/// <summary>
	///     Represents a widget that requires user input.
	/// </summary>
	public interface IInteractiveWidget : IWidget
	{
		/// <summary>
		///     Gets or sets a value indicating whether the control is enabled in the UI.
		///     Disabling causes the widgets to be grayed out and disables user interaction.
		/// </summary>
		/// <remarks>Available from DataMiner 9.5.3 onwards.</remarks>
		bool IsEnabled { get; set; }
	}
}