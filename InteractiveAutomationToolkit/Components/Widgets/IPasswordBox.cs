namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;

	/// <summary>
	///     Represents a text box for passwords.
	/// </summary>
	public interface IPasswordBox : IInteractiveWidget, IValidate
	{
		/// <summary>
		///     Triggered when the password changes.
		/// </summary>
		event EventHandler<PasswordBox.ChangedEventArgs> Changed;

		/// <summary>
		///     Gets or sets a value indicating whether the peek icon to reveal the password is shown.
		///     Default: <c>false</c>.
		/// </summary>
		bool HasPeekIcon { get; set; }

		/// <summary>
		/// 	Gets or sets a value indicating whether it is mandatory to provide a value.
		/// 	If the <see cref="IPasswordBox"/> is empty, the it will have a red border and will display that the field cannot be empty.
		/// 	This is only a visual indicator to the user, so take care to always validate user input.
		/// </summary>
		/// <remarks>This property only works for web compliant scripts or when launched from a DataMiner web app.</remarks>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
		bool IsRequired { get; set; }

		/// <summary>
		///     Gets or sets the password set in the password box.
		/// </summary>
		string Password { get; set; }

		/// <summary>
		///     Gets or sets the text that should be displayed as a placeholder.
		/// </summary>
		/// <remarks>This property only works for web compliant scripts or when launched from a DataMiner web app.</remarks>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
		string PlaceHolder { get; set; }

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		/// <remarks>This property only works for web compliant scripts or when launched from a DataMiner web app.</remarks>
		/// <remarks>Available from DataMiner Feature Release 10.0.8 and Main Release 10.1.0 onwards.</remarks>
		string Tooltip { get; set; }
	}
}