namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;

	/// <summary>
	///     Represents a label is used to display text in different styles.
	/// </summary>
	public interface ILabel : IWidget
	{
		/// <summary>
		///     Gets or sets the text style of the label.
		/// </summary>
		TextStyle Style { get; set; }

		/// <summary>
		///     Gets or sets the displayed text.
		/// </summary>
		string Text { get; set; }

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		/// <exception cref="ArgumentNullException">When the value is <c>null</c>.</exception>
		string Tooltip { get; set; }
	}
}