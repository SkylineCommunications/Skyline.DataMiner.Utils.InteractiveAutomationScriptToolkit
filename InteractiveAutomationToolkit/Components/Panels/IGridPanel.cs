namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;

	/// <summary>
	/// Defines a flexible grid area that consists of columns and rows.
	/// </summary>
	public interface IGridPanel : IPanel
	{
		/// <summary>
		///     Adds a panel to the grid.
		/// </summary>
		/// <param name="panel">Panel to be added to the grid.</param>
		/// <param name="location">Starting location of the panel within the grid.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="panel" /> is <c>null</c>.</exception>
		/// <exception cref="InvalidOperationException">When trying to add a panel to itself.</exception>
		/// <exception cref="InvalidOperationException">When the panel is already added.</exception>
		/// <remarks>Unlike <see cref="StackPanel"/> and <see cref="FormPanel"/>, surrounding widgets will not be automatically moved to avoid overlap.</remarks>
		void Add(IPanel panel, PanelLocation location);

		/// <summary>
		///     Adds a panel to the grid.
		/// </summary>
		/// <param name="panel">Panel to be added to the grid.</param>
		/// <param name="fromRow">Row location of widget on the grid.</param>
		/// <param name="fromColumn">Column location of widget on the grid.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="panel" /> is <c>null</c>.</exception>
		/// <exception cref="InvalidOperationException">When trying to add a panel to itself.</exception>
		/// <exception cref="InvalidOperationException">When the panel is already added.</exception>
		/// <exception cref="ArgumentOutOfRangeException">When <paramref name="fromRow"/> or <paramref name="fromColumn"/> is smaller than 0.</exception>
		/// <remarks>Unlike <see cref="StackPanel"/> and <see cref="FormPanel"/>, surrounding widgets will not be automatically moved to avoid overlap.</remarks>
		void Add(IPanel panel, int fromRow, int fromColumn);

		/// <summary>
		///     Adds a widget to the grid.
		/// </summary>
		/// <param name="widget">Widget to be added to the grid.</param>
		/// <param name="location">Location of the widget on the grid.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="widget" /> is <c>null</c>.</exception>
		/// <exception cref="InvalidOperationException">When trying to add a panel to itself.</exception>
		/// <exception cref="InvalidOperationException">When the panel is already added.</exception>
		void Add(IWidget widget, WidgetLocation location);

		/// <summary>
		///     Adds a widget to the grid.
		/// </summary>
		/// <param name="widget">Widget to be added to the grid.</param>
		/// <param name="row">Row location of widget on the grid.</param>
		/// <param name="column">Column location of the widget on the grid.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="widget" /> is <c>null</c>.</exception>
		/// <exception cref="InvalidOperationException">When trying to add a panel to itself.</exception>
		/// <exception cref="InvalidOperationException">When the panel is already added.</exception>
		/// <exception cref="ArgumentOutOfRangeException">When <paramref name="row"/> or <paramref name="column"/> is smaller than 0.</exception>
		void Add(IWidget widget, int row, int column);

		/// <summary>
		///     Adds a widget to the grid.
		/// </summary>
		/// <param name="widget">Widget to be added to the grid.</param>
		/// <param name="fromRow">Row location of widget on the grid.</param>
		/// <param name="fromColumn">Column location of the widget on the grid.</param>
		/// <param name="rowSpan">Number of rows the widget will use.</param>
		/// <param name="colSpan">Number of columns the widget will use.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="widget" /> is <c>null</c>.</exception>
		/// <exception cref="InvalidOperationException">When trying to add a panel to itself.</exception>
		/// <exception cref="InvalidOperationException">When the panel is already added.</exception>
		/// <exception cref="ArgumentOutOfRangeException">When <paramref name="fromRow"/> or <paramref name="fromColumn"/> is smaller than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException">When <paramref name="rowSpan"/> or <paramref name="colSpan"/> is smaller than 1.</exception>
		void Add(IWidget widget, int fromRow, int fromColumn, int rowSpan, int colSpan);

		/// <summary>
		///     Changes the relative location of the panel within the grid.
		/// </summary>
		/// <param name="panel">A panel that is part of the grid.</param>
		/// <param name="location">The new location of the panel.</param>
		/// <exception cref="NullReferenceException">When panel is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">When the panel is not part of the grid.</exception>
		void Move(IPanel panel, PanelLocation location);

		/// <summary>
		///     Changes the relative location of the panel within the grid.
		/// </summary>
		/// <param name="panel">A panel that is part of the grid.</param>
		/// <param name="fromRow">New row where the panel should be moved.</param>
		/// <param name="fromColumn">New column where the panel should be moved.</param>
		/// <exception cref="NullReferenceException">When panel is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">When the panel is not part of the grid.</exception>
		/// <exception cref="ArgumentOutOfRangeException">When <paramref name="fromRow"/> or <paramref name="fromColumn"/> is smaller than 0.</exception>
		void Move(IPanel panel, int fromRow, int fromColumn);

		/// <summary>
		///     Moves the widget within the grid.
		/// </summary>
		/// <param name="widget">A widget that is part of the grid.</param>
		/// <param name="widgetLocation">The new location of the widget.</param>
		/// <exception cref="NullReferenceException">When widget is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of the grid.</exception>
		void Move(IWidget widget, WidgetLocation widgetLocation);

		/// <summary>
		///     Moves the widget within the grid.
		/// </summary>
		/// <param name="widget">A widget that is part of the grid.</param>
		/// <param name="row">New row location of widget on the grid.</param>
		/// <param name="column">New column location of the widget on the grid.</param>
		/// <exception cref="NullReferenceException">When widget is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of the grid.</exception>
		/// <exception cref="ArgumentOutOfRangeException">When <paramref name="row"/> or <paramref name="column"/> is smaller than 0.</exception>
		void Move(IWidget widget, int row, int column);

		/// <summary>
		///     Moves the widget within the grid.
		/// </summary>
		/// <param name="widget">A widget that is part of the grid.</param>
		/// <param name="fromRow">New row location of widget on the grid.</param>
		/// <param name="fromColumn">New column location of the widget on the grid.</param>
		/// <param name="rowSpan">New number of rows the widget will use.</param>
		/// <param name="colSpan">New number of columns the widget will use.</param>
		/// <exception cref="NullReferenceException">When widget is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of the grid.</exception>
		/// <exception cref="ArgumentOutOfRangeException">When <paramref name="fromRow"/> or <paramref name="fromColumn"/> is smaller than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException">When <paramref name="rowSpan"/> or <paramref name="colSpan"/> is smaller than 1.</exception>
		void Move(IWidget widget, int fromRow, int fromColumn, int rowSpan, int colSpan);

		/// <summary>
		///     Gets the location of the panel in the grid.
		/// </summary>
		/// <param name="panel">A panel that is part of the grid.</param>
		/// <returns>The panel location in the grid.</returns>
		/// <exception cref="NullReferenceException">When the panel is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">When the panel is not part of the grid.</exception>
		PanelLocation GetLocation(IPanel panel);

		/// <summary>
		///     Gets the location of the widget in the grid.
		/// </summary>
		/// <param name="widget">A widget that is part of the grid.</param>
		/// <returns>The widget location in the grid.</returns>
		/// <exception cref="NullReferenceException">When the widget is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of the grid.</exception>
		WidgetLocation GetLocation(IWidget widget);

		/// <summary>
		///     Removes a panel from the grid.
		/// </summary>
		/// <param name="panel">Panel to remove.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="panel" /> is <c>null</c>.</exception>
		void Remove(IPanel panel);

		/// <summary>
		///     Removes a widget from the grid.
		/// </summary>
		/// <param name="widget">Widget to remove.</param>`
		/// <exception cref="ArgumentNullException">When the widget is <c>null</c>.</exception>
		void Remove(IWidget widget);
	}
}