namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// A section is a special component that can be used to group widgets together.
	/// </summary>
	public class Section
	{
		private readonly Dictionary<IWidget, WidgetLocation> widgetLocations = new Dictionary<IWidget, WidgetLocation>();

		private bool isVisible = true;

		/// <summary>
		/// Number of columns that are currently defined by the widgets that have been added to this section.
		/// </summary>
		public int ColumnCount { get; private set; }

		/// <summary>
		/// Number of rows that are currently defined by the widgets that have been added to this section.
		/// </summary>
		public int RowCount { get; private set; }

		/// <summary>
		///		Gets or sets a value indicating whether the widgets within the section are visible or not.
		/// </summary>
		public bool IsVisible
		{
			get
			{
				return isVisible;
			}

			set
			{
				isVisible = value;
				foreach (IWidget widget in Widgets)
				{
					widget.IsVisible = isVisible;
				}
			}
		}

		/// <summary>
		///     Gets widgets that have been added to the section.
		/// </summary>
		public IReadOnlyCollection<IWidget> Widgets
		{
			get
			{
				return widgetLocations.Keys;
			}
		}

		/// <summary>
		///     Adds a widget to the section.
		/// </summary>
		/// <param name="widget">Widget to add to the <see cref="Section" />.</param>
		/// <param name="widgetLocation">Location of the widget in the grid.</param>
		/// <returns>The dialog.</returns>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to the <see cref="Section" />.</exception>
		public Section AddWidget(IWidget widget, WidgetLocation widgetLocation)
		{
			if (widget == null)
			{
				throw new ArgumentNullException(nameof(widget));
			}

			if (widgetLocations.ContainsKey(widget))
			{
				throw new ArgumentException("Widget is already added to the section");
			}

			widgetLocations.Add(widget, widgetLocation);
			UpdateRowAndColumnCount();

			return this;
		}

		/// <summary>
		///     Adds a widget to the section.
		/// </summary>
		/// <param name="widget">Widget to add to the section.</param>
		/// <param name="row">Row location of the widget on the grid.</param>
		/// <param name="column">Column location of the widget on the grid.</param>
		/// <returns>The updated section.</returns>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the location is out of bounds of the grid.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to the dialog.</exception>
		public Section AddWidget(
			IWidget widget,
			int row,
			int column)
		{
			AddWidget(widget, new WidgetLocation(row, column));
			return this;
		}

		/// <summary>
		///     Adds a widget to the section.
		/// </summary>
		/// <param name="widget">Widget to add to the section.</param>
		/// <param name="fromRow">Row location of the widget on the grid.</param>
		/// <param name="fromColumn">Column location of the widget on the grid.</param>
		/// <param name="rowSpan">Number of rows the widget will use.</param>
		/// <param name="colSpan">Number of columns the widget will use.</param>
		/// <returns>The updated section.</returns>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the location is out of bounds of the grid.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to the dialog.</exception>
		public Section AddWidget(
			IWidget widget,
			int fromRow,
			int fromColumn,
			int rowSpan,
			int colSpan)
		{
			AddWidget(widget, new WidgetLocation(fromRow, fromColumn, rowSpan, colSpan));
			return this;
		}

		/// <summary>
		/// Adds the widgets from the section to the section.
		/// </summary>
		/// <param name="section">Section to be added to this section.</param>
		/// <param name="location">Starting location of the section within the parent section.</param>
		/// <returns>The updated section.</returns>
		public Section AddSection(Section section, SectionLocation location)
		{
			foreach (IWidget widget in section.Widgets)
			{
				WidgetLocation widgetLocation = section.GetWidgetLocation(widget);
				AddWidget(
					widget,
					new WidgetLocation(
						widgetLocation.Row + location.Row,
						widgetLocation.Column + location.Column,
						widgetLocation.RowSpan,
						widgetLocation.ColumnSpan));
			}

			return this;
		}

		/// <summary>
		/// Removes the widgets from the section off this section.
		/// </summary>
		/// <param name="section">Section to be removed from this section.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="section"/> is <c>null</c>.</exception>
		public void RemoveSection(Section section)
		{
			if (section == null) throw new ArgumentNullException(nameof(section));

			foreach (IWidget widget in section.Widgets)
			{
				RemoveWidget(widget);
			}
		}

		/// <summary>
		///     Gets the location of the widget in the dialog.
		/// </summary>
		/// <param name="widget">A widget that is part of the dialog.</param>
		/// <returns>The location of the widget in the dialog.</returns>
		/// <exception cref="NullReferenceException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of the dialog.</exception>
		public WidgetLocation GetWidgetLocation(IWidget widget)
		{
			CheckWidgetExits(widget);
			return widgetLocations[widget];
		}

		/// <summary>
		///     Removes a widget from the dialog.
		/// </summary>
		/// <param name="widget">Widget to remove.</param>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		public void RemoveWidget(IWidget widget)
		{
			if (widget == null)
			{
				throw new ArgumentNullException(nameof(widget));
			}

			widgetLocations.Remove(widget);
			UpdateRowAndColumnCount();
		}

		/// <summary>
		///     Moves the widget within the section.
		/// </summary>
		/// <param name="widget">A widget that is part of the section.</param>
		/// <param name="widgetLocation">The new location of the widget.</param>
		/// <exception cref="NullReferenceException">When widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of the section.</exception>
		/// <exception cref="NullReferenceException">When widgetLocation is null.</exception>
		public void MoveWidget(IWidget widget, WidgetLocation widgetLocation)
		{
			CheckWidgetExits(widget);
			widgetLocations[widget] = widgetLocation;
		}

		/// <summary>
		/// Removes all widgets from the section.
		/// </summary>
		public void Clear()
		{
			widgetLocations.Clear();
			RowCount = 0;
			ColumnCount = 0;
		}

		/// <summary>
		/// Enables all widgets added to this section.
		/// Sets the <see cref="IInteractiveWidget.IsEnabled"/> property of all <see cref="IInteractiveWidget"/> to <c>true</c>.
		/// </summary>
		public void EnableAllWidgets()
		{
			SetWidgetsEnabled(true);
		}

		/// <summary>
		/// Disables all widgets added to this section.
		/// Sets the <see cref="IInteractiveWidget.IsEnabled"/> property of all <see cref="IInteractiveWidget"/> to <c>false</c>.
		/// </summary>
		public void DisableAllWidgets()
		{
			SetWidgetsEnabled(false);
		}

		private void SetWidgetsEnabled(bool enabled)
		{
			foreach (IInteractiveWidget widget in Widgets.OfType<IInteractiveWidget>())
			{
				widget.IsEnabled = enabled;
			}
		}

		private void CheckWidgetExits(IWidget widget)
		{
			if (widget == null)
			{
				throw new ArgumentNullException(nameof(widget));
			}

			if (!widgetLocations.ContainsKey(widget))
			{
				throw new ArgumentException("Widget is not part of this dialog");
			}
		}

		/// <summary>
		///		Used to update the RowCount and ColumnCount properties based on the Widgets added to the section.
		/// </summary>
		private void UpdateRowAndColumnCount()
		{
			if (widgetLocations.Any())
			{
				RowCount = widgetLocations.Values.Max(w => w.Row + w.RowSpan);
				ColumnCount = widgetLocations.Values.Max(w => w.Column + w.ColumnSpan);
			}
			else
			{
				RowCount = 0;
				ColumnCount = 0;
			}
		}
	}
}
