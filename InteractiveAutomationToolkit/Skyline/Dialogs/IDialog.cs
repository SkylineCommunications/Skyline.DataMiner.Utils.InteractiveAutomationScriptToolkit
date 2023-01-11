namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     A dialog represents a single window that can be shown.
	///     You can add widgets to the dialog by adding them to the <see cref="Panel" /> of the dialog.
	/// </summary>
	public interface IDialog
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
		/// Gets the root panel of the dialog. Widgets and other panels can be added to build a UI.
		/// </summary>
		IPanel Panel { get; }

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
		/// 	Shows the dialog window and returns immediately.
		/// </summary>
		/// <remarks>Users wont be able to interact with widgets.</remarks>
		/// <param name="disabled">When <c>true</c>, shows all widgets in a disabled state.</param>
		void ShowStatic(bool disabled);

		/// <summary>
		///     Shows the dialog window and returns only after a user has interacted with a widget that has at least one event handler registered.
		/// </summary>
		void ShowInteractive();
	}

	/// <inheritdoc />
	/// <typeparam name="TPanel">The type of the root panel used by the dialog.</typeparam>
	public interface IDialog<TPanel> : IDialog where TPanel : IPanel, new()
	{
		/// <summary>
		/// Gets the root panel of the dialog. Widgets and other panels can be added to build a UI.
		/// </summary>
		new TPanel Panel { get; }
	}
}