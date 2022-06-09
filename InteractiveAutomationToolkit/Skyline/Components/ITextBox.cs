namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;

	using Automation;

	/// <summary>
	///     Represents a widget that is used to edit and display text.
	/// </summary>
	public interface ITextBox : IInteractiveWidget
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
		///     Gets or sets the text displayed in the text box.
		/// </summary>
		string Text { get; set; }

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		/// <exception cref="ArgumentNullException">When the value is <c>null</c>.</exception>
		string Tooltip { get; set; }

		/// <summary>
		///		Gets or sets the text that should be displayed as a placeholder.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
		string PlaceHolder { get; set; }

		/// <summary>
		///		Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
		///		This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
		UIValidationState ValidationState { get; set; }

		/// <summary>
		///		Gets or sets the text that is shown if the validation state is invalid.
		///		This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
		string ValidationText { get; set; }
	}
}