namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
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
		/// <remarks>This property only works for web compliant scripts or when launched from a DataMiner web app.</remarks>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
		string PlaceHolder { get; set; }

		/// <summary>
		///     Gets or sets the text displayed in the text box.
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