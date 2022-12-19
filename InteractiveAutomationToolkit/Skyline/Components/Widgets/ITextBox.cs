namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;

	/// <summary>
	///     Represents a widget that is used to edit and display text.
	/// </summary>
	public interface ITextBox : IInteractiveWidget, IValidate
	{
		/// <summary>
		///     Triggered when the text in the text box changes.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		event EventHandler<TextBox.ChangedEventArgs> Changed;

		/// <summary>
		///     Gets or sets a value indicating whether users are able to enter multiple lines of text.
		/// </summary>
		bool IsMultiline { get; set; }

		/// <summary>
		///     Gets or sets the text that should be displayed as a placeholder.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
		string PlaceHolder { get; set; }

		/// <summary>
		///     Gets or sets the text displayed in the text box.
		/// </summary>
		string Text { get; set; }

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		string Tooltip { get; set; }
	}
}