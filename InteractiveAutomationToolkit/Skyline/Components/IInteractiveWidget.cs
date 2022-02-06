namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System.ComponentModel;

	public interface IInteractiveWidget : IWidget
	{
		/// <summary>
		///     Gets or sets a value indicating whether the control is enabled in the UI.
		///     Disabling causes the widgets to be grayed out and disables user interaction.
		/// </summary>
		/// <remarks>Available from DataMiner 9.5.3 onwards.</remarks>
		bool IsEnabled { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether an update of the current value of the dialog box item will trigger an event.
		/// </summary>
		/// <remarks>Should not be used as this value is set automatically when subscribing to events!</remarks>
		[EditorBrowsable(EditorBrowsableState.Never)]
		bool WantsOnChange { get; set; }
	}
}