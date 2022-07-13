namespace Skyline.DataMiner.InteractiveAutomationToolkit
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
		string Tooltip { get; set; }
	}
}