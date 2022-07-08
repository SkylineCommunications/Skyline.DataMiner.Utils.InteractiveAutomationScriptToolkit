namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     A dialog represents a single window that can be shown.
	/// </summary>
	public interface IDialog : ISection
	{
		/// <summary>
		///     Triggered when the back button of the dialog is pressed.
		/// </summary>
		event EventHandler<EventArgs> Back;

		/// <summary>
		///     Triggered when the forward button of the dialog is pressed.
		/// </summary>
		event EventHandler<EventArgs> Forward;

		/// <summary>
		///     Triggered when there is any user interaction before any other widget event.
		/// </summary>
		event EventHandler<EventArgs> Interacted;

		/// <summary>
		///     Gets the link to the SLAutomation process.
		/// </summary>
		IEngine Engine { get; }

		/// <summary>
		///     Gets or sets a value indicating whether overlapping widgets are allowed or not.
		///     Can be used in case you want to add multiple widgets to the same cell in the dialog.
		///     You can use the Margin property on the widgets to place them apart.
		/// </summary>
		bool AllowOverlappingWidgets { get; set; }

		/// <summary>
		///     Gets or sets the fixed height (in pixels) of the dialog.
		/// </summary>
		/// <remarks>
		///     The user will still be able to resize the window,
		///     but scrollbars will appear immediately.
		///     <see cref="MinHeight" /> should be used instead as it has a more desired effect.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		int Height { get; set; }

		/// <summary>
		///     Gets or sets the maximum height (in pixels) of the dialog.
		/// </summary>
		/// <remarks>
		///     The user will still be able to resize the window past this limit.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		int MaxHeight { get; set; }

		/// <summary>
		///     Gets or sets the maximum width (in pixels) of the dialog.
		/// </summary>
		/// <remarks>
		///     The user will still be able to resize the window past this limit.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		int MaxWidth { get; set; }

		/// <summary>
		///     Gets or sets the minimum height (in pixels) of the dialog.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		int MinHeight { get; set; }

		/// <summary>
		///     Gets or sets the minimum width (in pixels) of the dialog.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		int MinWidth { get; set; }

		/// <summary>
		///     Gets or sets the title at the top of the window.
		/// </summary>
		/// <remarks>Available from DataMiner 9.6.6 onwards.</remarks>
		string Title { get; set; }

		/// <summary>
		///     Gets or sets the fixed width (in pixels) of the dialog.
		/// </summary>
		/// <remarks>
		///     The user will still be able to resize the window,
		///     but scrollbars will appear immediately.
		///     <see cref="MinWidth" /> should be used instead as it has a more desired effect.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		int Width { get; set; }

		/// <summary>
		///     Applies a fixed width (in pixels) to a column.
		/// </summary>
		/// <param name="column">The index of the column on the grid.</param>
		/// <param name="columnWidth">The width of the column.</param>
		/// <exception cref="ArgumentOutOfRangeException">When the column index does not exist.</exception>
		/// <exception cref="ArgumentOutOfRangeException">When the column width is smaller than 0.</exception>
		void SetColumnWidth(int column, int columnWidth);

		/// <summary>
		///     The width of the column will be automatically adapted to the widest widget in that column.
		/// </summary>
		/// <param name="column">The index of the column on the grid.</param>
		/// <exception cref="ArgumentOutOfRangeException">When the column index does not exist.</exception>
		void SetColumnWidthAuto(int column);

		/// <summary>
		///     The column will have the largest possible width, depending on the width of the other columns.
		/// </summary>
		/// <param name="column">The index of the column on the grid.</param>
		/// <exception cref="ArgumentOutOfRangeException">When the column index does not exist.</exception>
		void SetColumnWidthStretch(int column);

		/// <summary>
		///     Applies a fixed height (in pixels) to a row.
		/// </summary>
		/// <param name="row">The index of the row on the grid.</param>
		/// <param name="rowHeight">The height of the column.</param>
		/// <exception cref="ArgumentOutOfRangeException">When the row index is smaller than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException">When the row height is smaller than 0.</exception>
		void SetRowHeight(int row, int rowHeight);

		/// <summary>
		///     The height of the row will be automatically adapted to the highest widget in that row.
		/// </summary>
		/// <param name="row">The index of the row on the grid.</param>
		/// <exception cref="ArgumentOutOfRangeException">When the row index is smaller than 0.</exception>
		void SetRowHeightAuto(int row);

		/// <summary>
		///     The row will have the largest possible height, depending on the height of the other rows.
		/// </summary>
		/// <param name="row">The index of the row on the grid.</param>
		/// <exception cref="ArgumentOutOfRangeException">When the row index is smaller than 0.</exception>
		void SetRowHeightStretch(int row);

		/// <summary>
		///     Shows the dialog window.
		///     Also loads changes and triggers events when <paramref name="requireResponse" /> is <c>true</c>.
		/// </summary>
		/// <param name="requireResponse">If the dialog expects user interaction.</param>
		/// <remarks>Should only be used when you create your own event loop.</remarks>
		void Show(bool requireResponse = true);
	}
}