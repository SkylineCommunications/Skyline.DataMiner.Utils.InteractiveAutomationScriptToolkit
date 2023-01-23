namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System.ComponentModel;

	/// <summary>
	///     Represents a label is used to display text in different styles.
	/// </summary>
	public interface ILabel : IWidget
	{
		/// <summary>
		///     Gets or sets the text style of the label.
		/// </summary>
		/// <exception cref="InvalidEnumArgumentException">When <paramref name="value"/> does not specify a valid member of <see cref="TextStyle"/>.</exception>
		TextStyle Style { get; set; }

		/// <summary>
		///     Gets or sets the displayed text.
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