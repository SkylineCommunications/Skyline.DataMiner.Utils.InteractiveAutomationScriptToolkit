namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;

	/// <summary>
	/// Used to define the position of a widget in a grid layout.
	/// </summary>
	public interface IWidgetLayout : ILayout
	{
		/// <summary>
		///     Gets how many columns the widget spans on the grid.
		/// </summary>
		int ColumnSpan { get; }

		/// <summary>
		///     Gets or sets the horizontal alignment of the widget.
		/// </summary>
		HorizontalAlignment HorizontalAlignment { get; set; }

		/// <summary>
		///     Gets or sets the margin around the widget.
		/// </summary>
		/// <exception cref="ArgumentNullException">When the value is null.</exception>
		Margin Margin { get; set; }

		/// <summary>
		///     Gets how many rows the widget spans on the grid.
		/// </summary>
		int RowSpan { get; }

		/// <summary>
		///     Gets or sets the vertical alignment of the widget.
		/// </summary>
		VerticalAlignment VerticalAlignment { get; set; }
	}
}
