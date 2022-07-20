namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;

	public interface IGridPanel : IPanel
	{
		/// <summary>
		///     Adds a panel to this component.
		/// </summary>
		/// <param name="panel">Panel to be added to this component.</param>
		/// <param name="location">Starting location of the panel within this component.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="panel" /> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">When trying to add a panel to itself.</exception>
		/// <exception cref="ArgumentException">When the panel is already added.</exception>
		/// <exception cref="ArgumentException">When the panel contains a widget that is already added.</exception>
		void Add(IPanel panel, PanelLocation location);

		/// <summary>
		///     Adds a panel to this component.
		/// </summary>
		/// <param name="panel">Panel to be added to this component.</param>
		/// <param name="fromRow">Row in the component where the panel should be added.</param>
		/// <param name="fromColumn">Column in the component where the panel should be added.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="panel" /> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">When trying to add a panel to itself.</exception>
		/// <exception cref="ArgumentException">When the panel is already added.</exception>
		/// <exception cref="ArgumentException">When the panel contains a widget that is already added.</exception>
		void Add(IPanel panel, int fromRow, int fromColumn);

		/// <summary>
		///     Adds a widget to this component.
		/// </summary>
		/// <param name="widget">Widget to add to this component.</param>
		/// <param name="location">Location of the widget on the grid.</param>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to this component.</exception>
		void Add(IWidget widget, WidgetLocation location);

		/// <summary>
		///     Adds a widget to this component.
		/// </summary>
		/// <param name="widget">Widget to add to this component.</param>
		/// <param name="row">Row location of widget on the grid.</param>
		/// <param name="column">Column location of the widget on the grid.</param>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to this component.</exception>
		void Add(IWidget widget, int row, int column);

		/// <summary>
		///     Adds a widget to this component.
		/// </summary>
		/// <param name="widget">Widget to add to this component.</param>
		/// <param name="fromRow">Row location of widget on the grid.</param>
		/// <param name="fromColumn">Column location of the widget on the grid.</param>
		/// <param name="rowSpan">Number of rows the widget will use.</param>
		/// <param name="colSpan">Number of columns the widget will use.</param>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to this component.</exception>
		void Add(IWidget widget, int fromRow, int fromColumn, int rowSpan, int colSpan);

		/// <summary>
		///     Changes the relative location of the panel within this component.
		/// </summary>
		/// <param name="panel">A panel that is part of this component.</param>
		/// <param name="location">The new location of the panel.</param>
		/// <exception cref="NullReferenceException">When panel is null.</exception>
		/// <exception cref="ArgumentException">When the panel is not part of this component.</exception>
		void Move(IPanel panel, PanelLocation location);

		/// <summary>
		///     Changes the relative location of the panel within this component.
		/// </summary>
		/// <param name="panel">A panel that is part of this component.</param>
		/// <param name="fromRow">New row where the panel should be moved.</param>
		/// <param name="fromColumn">New column where the panel should be moved.</param>
		/// <exception cref="NullReferenceException">When panel is null.</exception>
		/// <exception cref="ArgumentException">When the panel is not part of this component.</exception>
		void Move(IPanel panel, int fromRow, int fromColumn);

		/// <summary>
		///     Moves the widget within this component.
		/// </summary>
		/// <param name="widget">A widget that is part of this component.</param>
		/// <param name="widgetLocation">The new location of the widget.</param>
		/// <exception cref="NullReferenceException">When widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of this component.</exception>
		void Move(IWidget widget, WidgetLocation widgetLocation);

		/// <summary>
		///     Moves the widget within this component.
		/// </summary>
		/// <param name="widget">A widget that is part of this component.</param>
		/// <param name="row">New row location of widget on the grid.</param>
		/// <param name="column">New column location of the widget on the grid.</param>
		/// <exception cref="NullReferenceException">When widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of this component.</exception>
		void Move(IWidget widget, int row, int column);

		/// <summary>
		///     Moves the widget within this component.
		/// </summary>
		/// <param name="widget">A widget that is part of this component.</param>
		/// <param name="fromRow">New row location of widget on the grid.</param>
		/// <param name="fromColumn">New column location of the widget on the grid.</param>
		/// <param name="rowSpan">New number of rows the widget will use.</param>
		/// <param name="colSpan">New number of columns the widget will use.</param>
		/// <exception cref="NullReferenceException">When widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of this component.</exception>
		void Move(IWidget widget, int fromRow, int fromColumn, int rowSpan, int colSpan);

		/// <summary>
		///     Gets the location of the panel in this component.
		/// </summary>
		/// <param name="panel">A panel that is part of this component.</param>
		/// <returns>The panel location in this component.</returns>
		/// <exception cref="NullReferenceException">When the panel is null.</exception>
		/// <exception cref="ArgumentException">When the panel is not part of this component.</exception>
		PanelLocation GetLocation(IPanel panel);

		/// <summary>
		///     Gets the location of the widget in this component.
		/// </summary>
		/// <param name="widget">A widget that is part of this component.</param>
		/// <returns>The widget location in this component.</returns>
		/// <exception cref="NullReferenceException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of this component.</exception>
		WidgetLocation GetLocation(IWidget widget);

		/// <summary>
		///     Removes a panel from the dialog.
		/// </summary>
		/// <param name="panel">Panel to remove.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="panel" /> is <c>null</c>.</exception>
		void Remove(IPanel panel);

		/// <summary>
		///     Removes a widget from this component.
		/// </summary>
		/// <param name="widget">Widget to remove.</param>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		void Remove(IWidget widget);
	}
}