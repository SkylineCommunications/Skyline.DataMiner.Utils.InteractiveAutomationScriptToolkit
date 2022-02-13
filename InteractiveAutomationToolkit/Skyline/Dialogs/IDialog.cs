namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;

	using Automation;

	public interface IDialog
	{
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
		///     Gets the number of columns of the grid layout.
		/// </summary>
		int ColumnCount { get; }

		/// <summary>
		///     Gets the link to the SLAutomation process.
		/// </summary>
		IEngine Engine { get; }

		/// <summary>
		///     Gets the number of rows in the grid layout.
		/// </summary>
		int RowCount { get; }

		/// <summary>
		///     Gets or sets the title at the top of the window.
		/// </summary>
		/// <remarks>Available from DataMiner 9.6.6 onwards.</remarks>
		string Title { get; set; }

		/// <summary>
		///     Gets widgets that are added to the dialog.
		/// </summary>
		IReadOnlyCollection<IWidget> Widgets { get; }

		/// <summary>
		///     Triggered when the back button of the dialog is pressed.
		/// </summary>
		event EventHandler<EventArgs> Back;

		/// <summary>
		///     Triggered when the forward button of the dialog is pressed.
		/// </summary>
		event EventHandler<EventArgs> Forward;

		/// <summary>
		///     Triggered when there is any user interaction.
		/// </summary>
		event EventHandler<EventArgs> Interacted;

		/// <summary>
		///     Adds a widget to the dialog.
		/// </summary>
		/// <param name="widget">Widget to add to the dialog.</param>
		/// <param name="widgetLayout">Location of the widget on the grid layout.</param>
		/// <returns>The dialog.</returns>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to the dialog.</exception>
		IDialog AddWidget(IWidget widget, IWidgetLayout widgetLayout);

		/// <summary>
		///     Adds a widget to the dialog.
		/// </summary>
		/// <param name="widget">Widget to add to the dialog.</param>
		/// <param name="row">Row location of widget on the grid.</param>
		/// <param name="column">Column location of the widget on the grid.</param>
		/// <param name="horizontalAlignment">Horizontal alignment of the widget.</param>
		/// <param name="verticalAlignment">Vertical alignment of the widget.</param>
		/// <returns>The dialog.</returns>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to the dialog.</exception>
		IDialog AddWidget(
			IWidget widget,
			int row,
			int column,
			HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment verticalAlignment = VerticalAlignment.Center);

		/// <summary>
		///     Adds a widget to the dialog.
		/// </summary>
		/// <param name="widget">Widget to add to the dialog.</param>
		/// <param name="fromRow">Row location of widget on the grid.</param>
		/// <param name="fromColumn">Column location of the widget on the grid.</param>
		/// <param name="rowSpan">Number of rows the widget will use.</param>
		/// <param name="colSpan">Number of columns the widget will use.</param>
		/// <param name="horizontalAlignment">Horizontal alignment of the widget.</param>
		/// <param name="verticalAlignment">Vertical alignment of the widget.</param>
		/// <returns>The dialog.</returns>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to the dialog.</exception>
		IDialog AddWidget(
			IWidget widget,
			int fromRow,
			int fromColumn,
			int rowSpan,
			int colSpan,
			HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment verticalAlignment = VerticalAlignment.Center);

		/// <summary>
		///     Gets the layout of the widget in the dialog.
		/// </summary>
		/// <param name="widget">A widget that is part of the dialog.</param>
		/// <returns>The widget layout in the dialog.</returns>
		/// <exception cref="NullReferenceException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of the dialog.</exception>
		IWidgetLayout GetWidgetLayout(IWidget widget);

		/// <summary>
		///     Removes a widget from the dialog.
		/// </summary>
		/// <param name="widget">Widget to remove.</param>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		void RemoveWidget(IWidget widget);

		/// <summary>
		/// Adds the widgets from the section to the dialog.
		/// </summary>
		/// <param name="section">Section to be added to the dialog.</param>
		/// <param name="layout">Left top position of the section within the dialog.</param>
		/// <returns>Updated dialog.</returns>
		IDialog AddSection(Section section, SectionLayout layout);

		/// <summary>
		/// Adds the widgets from the section to the dialog.
		/// </summary>
		/// <param name="section">Section to be added to the dialog.</param>
		/// <param name="fromRow">Row in the dialog where the section should be added.</param>
		/// <param name="fromColumn">Column in the dialog where the section should be added.</param>
		/// <returns>Updated dialog.</returns>
		IDialog AddSection(Section section, int fromRow, int fromColumn);

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
		///     Sets the layout of the widget in the dialog.
		/// </summary>
		/// <param name="widget">A widget that is part of the dialog.</param>
		/// <param name="widgetLayout">The layout to apply to the widget.</param>
		/// <exception cref="NullReferenceException">When widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of the dialog.</exception>
		void SetWidgetLayout(IWidget widget, IWidgetLayout widgetLayout);

		/// <summary>
		///     Shows the dialog window.
		///     Also loads changes and triggers events when <paramref name="requireResponse" /> is <c>true</c>.
		/// </summary>
		/// <param name="requireResponse">If the dialog expects user interaction.</param>
		/// <remarks>Should only be used when you create your own event loop.</remarks>
		void Show(bool requireResponse = true);

		/// <summary>
		/// Removes all widgets from the dialog.
		/// </summary>
		void Clear();

		/// <summary>
		/// Enables all widgets added to this dialog.
		/// Sets the <see cref="IInteractiveWidget.IsEnabled"/> property of all <see cref="IInteractiveWidget"/> to <c>true</c>.
		/// </summary>
		void EnableAllWidgets();

		/// <summary>
		/// Disables all widgets added to this dialog.
		/// Sets the <see cref="IInteractiveWidget.IsEnabled"/> property of all <see cref="IInteractiveWidget"/> to <c>false</c>.
		/// </summary>
		void DisableAllWidgets();

		/// <summary>
		/// Removes a section from the dialog.
		/// </summary>
		/// <param name="section">Section to remove</param>
		/// <exception cref="ArgumentNullException">When <paramref name="section"/> is <c>null</c>.</exception>
		void RemoveSection(Section section);
	}
}