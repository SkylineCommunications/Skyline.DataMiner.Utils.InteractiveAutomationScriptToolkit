namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System;

	/// <summary>
	///     Represents a checkbox that can be selected or cleared.
	/// </summary>
	public interface ICheckBox : IInteractiveWidget
	{
		/// <summary>
		///     Triggered when the state of the checkbox changes.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		event EventHandler<CheckBox.ChangedEventArgs> Changed;

		/// <summary>
		///     Triggered when the checkbox is selected.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		event EventHandler<EventArgs> Checked;

		/// <summary>
		///     Triggered when the checkbox is cleared.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		event EventHandler<EventArgs> UnChecked;

		/// <summary>
		///     Gets or sets a value indicating whether the checkbox is selected.
		/// </summary>
		bool IsChecked { get; set; }

		/// <summary>
		///     Gets or sets the displayed text next to the checkbox.
		/// </summary>
		string Text { get; set; }

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		string Tooltip { get; set; }
	}
}