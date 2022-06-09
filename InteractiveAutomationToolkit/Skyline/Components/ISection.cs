namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Represents a special component that can group widgets together.
	/// </summary>
	public interface ISection
	{
		/// <summary>
		///     Gets the current number of columns allocated in the grid.
		/// </summary>
		int ColumnCount { get; }

		/// <summary>
		///     Gets the current number of rows allocated in the grid.
		/// </summary>
		int RowCount { get; }

		/// <summary>
		/// Gets all widgets added to this component.
		/// </summary>
		/// <returns>Widgets added to this component.</returns>
		/// <remarks>Also returns widgets from nested sections.</remarks>
		IEnumerable<IWidget> GetWidgets();

		/// <summary>
		/// Gets the widgets added to this component.
		/// </summary>
		/// <param name="includeNested">Include widgets from nested sections.</param>
		/// <returns>Widgets added to this component.</returns>
		IEnumerable<IWidget> GetWidgets(bool includeNested);

		/// <summary>
		///     Adds a widget to this component.
		/// </summary>
		/// <param name="widget">Widget to add to this component.</param>
		/// <param name="location">Location of the widget on the grid.</param>
		/// <returns>A reference to this instance.</returns>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to this component.</exception>
		ISection AddWidget(IWidget widget, WidgetLocation location);

		/// <summary>
		///     Adds a widget to this component.
		/// </summary>
		/// <param name="widget">Widget to add to this component.</param>
		/// <param name="row">Row location of widget on the grid.</param>
		/// <param name="column">Column location of the widget on the grid.</param>
		/// <returns>A reference to this instance.</returns>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to this component.</exception>
		ISection AddWidget(IWidget widget, int row, int column);

		/// <summary>
		///     Adds a widget to this component.
		/// </summary>
		/// <param name="widget">Widget to add to this component.</param>
		/// <param name="fromRow">Row location of widget on the grid.</param>
		/// <param name="fromColumn">Column location of the widget on the grid.</param>
		/// <param name="rowSpan">Number of rows the widget will use.</param>
		/// <param name="colSpan">Number of columns the widget will use.</param>
		/// <returns>A reference to this instance.</returns>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to this component.</exception>
		ISection AddWidget(IWidget widget, int fromRow, int fromColumn, int rowSpan, int colSpan);

		/// <summary>
		///     Removes a widget from this component.
		/// </summary>
		/// <param name="widget">Widget to remove.</param>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		void RemoveWidget(IWidget widget);

		/// <summary>
		///     Moves the widget within this component.
		/// </summary>
		/// <param name="widget">A widget that is part of this component.</param>
		/// <param name="widgetLocation">The new location of the widget.</param>
		/// <exception cref="NullReferenceException">When widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of this component.</exception>
		void MoveWidget(IWidget widget, WidgetLocation widgetLocation);

		/// <summary>
		///     Moves the widget within this component.
		/// </summary>
		/// <param name="widget">A widget that is part of this component.</param>
		/// <param name="row">New row location of widget on the grid.</param>
		/// <param name="column">New column location of the widget on the grid.</param>
		/// <exception cref="NullReferenceException">When widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of this component.</exception>
		void MoveWidget(IWidget widget, int row, int column);

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
		void MoveWidget(IWidget widget, int fromRow, int fromColumn, int rowSpan, int colSpan);

		/// <summary>
		///     Gets the location of the widget in this component.
		/// </summary>
		/// <param name="widget">A widget that is part of this component.</param>
		/// <returns>The widget location in this component.</returns>
		/// <exception cref="NullReferenceException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of this component.</exception>
		WidgetLocation GetWidgetLocation(IWidget widget);

		/// <summary>
		/// Gets all sections added to this component.
		/// </summary>
		/// <returns>Sections added to this component.</returns>
		/// <remarks>Also returns nested sections.</remarks>
		IEnumerable<ISection> GetSections();

		/// <summary>
		/// Gets sections added to this component.
		/// </summary>
		/// <param name="includeNested">Include nested sections.</param>
		/// <returns>Sections added to this component.</returns>
		IEnumerable<ISection> GetSections(bool includeNested);

		/// <summary>
		/// Adds a section to this component.
		/// </summary>
		/// <param name="section">Section to be added to this component.</param>
		/// <param name="location">Starting location of the section within this component.</param>
		/// <returns>A reference to this instance.</returns>
		/// <exception cref="ArgumentNullException">When <paramref name="section"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">When trying to add a section to itself.</exception>
		/// <exception cref="ArgumentException">When the section is already added.</exception>
		/// <exception cref="ArgumentException">When the section contains a widget that is already added.</exception>
		ISection AddSection(ISection section, SectionLocation location);

		/// <summary>
		/// Adds a section to this component.
		/// </summary>
		/// <param name="section">Section to be added to this component.</param>
		/// <param name="fromRow">Row in the component where the section should be added.</param>
		/// <param name="fromColumn">Column in the component where the section should be added.</param>
		/// <returns>A reference to this instance.</returns>
		/// <exception cref="ArgumentNullException">When <paramref name="section"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">When trying to add a section to itself.</exception>
		/// <exception cref="ArgumentException">When the section is already added.</exception>
		/// <exception cref="ArgumentException">When the section contains a widget that is already added.</exception>
		ISection AddSection(ISection section, int fromRow, int fromColumn);

		/// <summary>
		/// Removes a section from the dialog.
		/// </summary>
		/// <param name="section">Section to remove</param>
		/// <exception cref="ArgumentNullException">When <paramref name="section"/> is <c>null</c>.</exception>
		void RemoveSection(ISection section);

		/// <summary>
		///     Changes the relative location of the section within this component.
		/// </summary>
		/// <param name="section">A section that is part of this component.</param>
		/// <param name="location">The new location of the section.</param>
		/// <exception cref="NullReferenceException">When section is null.</exception>
		/// <exception cref="ArgumentException">When the section is not part of this component.</exception>
		void MoveSection(ISection section, SectionLocation location);

		/// <summary>
		///     Changes the relative location of the section within this component.
		/// </summary>
		/// <param name="section">A section that is part of this component.</param>
		/// <param name="fromRow">New row where the section should be moved.</param>
		/// <param name="fromColumn">New column where the section should be moved.</param>
		/// <exception cref="NullReferenceException">When section is null.</exception>
		/// <exception cref="ArgumentException">When the section is not part of this component.</exception>
		void MoveSection(ISection section, int fromRow, int fromColumn);

		/// <summary>
		///     Gets the location of the section in this component.
		/// </summary>
		/// <param name="section">A section that is part of this component.</param>
		/// <returns>The section location in this component.</returns>
		/// <exception cref="NullReferenceException">When the section is null.</exception>
		/// <exception cref="ArgumentException">When the section is not part of this component.</exception>
		SectionLocation GetSectionLocation(ISection section);

		/// <summary>
		/// Enables all widgets added to this component.
		/// Sets the <see cref="IInteractiveWidget.IsEnabled"/> property of all <see cref="IInteractiveWidget"/> to <c>true</c>.
		/// </summary>
		/// <remarks>Also changes nested widgets.</remarks>
		void EnableWidgets();

		/// <summary>
		/// Enables widgets added to this component.
		/// Sets the <see cref="IInteractiveWidget.IsEnabled"/> property of all <see cref="IInteractiveWidget"/> to <c>true</c>.
		/// <param name="includeNested"><c>true</c> if nested widgets should be included. <c>false</c> otherwise.</param>
		/// </summary>
		void EnableWidgets(bool includeNested);

		/// <summary>
		/// Disables all widgets added to this component.
		/// Sets the <see cref="IInteractiveWidget.IsEnabled"/> property of all <see cref="IInteractiveWidget"/> to <c>false</c>.
		/// </summary>
		/// <remarks>Also changes nested widgets.</remarks>
		void DisableWidgets();

		/// <summary>
		/// Disables widgets added to this component.
		/// Sets the <see cref="IInteractiveWidget.IsEnabled"/> property of all <see cref="IInteractiveWidget"/> to <c>false</c>.
		/// <param name="includeNested"><c>true</c> if nested widgets should be included. <c>false</c> otherwise.</param>
		/// </summary>
		void DisableWidgets(bool includeNested);

		/// <summary>
		/// Enables or disables all widgets added to this component.
		/// Sets the <see cref="IInteractiveWidget.IsEnabled"/> property of all <see cref="IInteractiveWidget"/>.
		/// </summary>
		/// <param name="enabled"><c>true</c> if the widgets should be set enabled. <c>false</c> otherwise.</param>
		/// <param name="includeNested"><c>true</c> if nested widgets should be included. <c>false</c> otherwise.</param>
		void SetWidgetsEnabled(bool enabled, bool includeNested);

		/// <summary>
		/// Shows all widgets added to this component.
		/// Sets the <see cref="IWidget.IsVisible"/> property of all <see cref="IWidget"/> to <c>true</c>.
		/// </summary>
		/// <remarks>Also changes nested widgets.</remarks>
		void ShowWidgets();

		/// <summary>
		/// Shows widgets added to this component.
		/// Sets the <see cref="IWidget.IsVisible"/> property of all <see cref="IWidget"/> to <c>true</c>.
		/// </summary>
		/// <param name="includeNested"><c>true</c> if nested widgets should be included. <c>false</c> otherwise.</param>
		void ShowWidgets(bool includeNested);

		/// <summary>
		/// Hides all widgets added to this component.
		/// Sets the <see cref="IWidget.IsVisible"/> property of all <see cref="IWidget"/> to <c>false</c>.
		/// </summary>
		/// <remarks>Also changes nested widgets.</remarks>
		void HideWidgets();

		/// <summary>
		/// Hides widgets added to this component.
		/// Sets the <see cref="IWidget.IsVisible"/> property of all <see cref="IWidget"/> to <c>false</c>.
		/// </summary>
		/// <param name="includeNested"><c>true</c> if nested widgets should be included. <c>false</c> otherwise.</param>
		void HideWidgets(bool includeNested);

		/// <summary>
		/// Shows or hides all widgets added to this component.
		/// Sets the <see cref="IWidget.IsVisible"/> property of all <see cref="IWidget"/>.
		/// </summary>
		/// <param name="visible"><c>true</c> if the widgets should be set visible. <c>false</c> otherwise.</param>
		/// <param name="includeNested"><c>true</c> if nested widgets should be included. <c>false</c> otherwise.</param>
		void SetWidgetsVisible(bool visible, bool includeNested);

		/// <summary>
		/// Removes all widgets and sections from the component.
		/// </summary>
		void Clear();
	}
}