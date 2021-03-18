namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;

	public interface IWidgetLayout : ILayout
	{
		/// <summary>
		///     Gets how many columns the widget is spanning in the grid.
		/// </summary>
		/// <remarks>The widget will start at <see cref="Column" /></remarks>
		int ColumnSpan { get; }

		/// <summary>
		///     Gets or sets the horizontal alignment of the widget.
		/// </summary>
		HorizontalAlignment HorizontalAlignment { get; set; }

		/// <summary>
		///     Gets or sets the margin around the widget.
		/// </summary>
		/// <exception cref="ArgumentNullException">When value is null.</exception>
		Margin Margin { get; set; }

		/// <summary>
		///     Gets how many rows the widget is spanning in the grid.
		/// </summary>
		/// <remarks>The widget will start at <see cref="Row" /></remarks>
		int RowSpan { get; }

		/// <summary>
		///     Gets or sets the vertical alignment of the widget.
		/// </summary>
		VerticalAlignment VerticalAlignment { get; set; }
	}
}
