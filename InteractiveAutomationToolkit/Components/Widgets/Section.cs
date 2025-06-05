namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// A section is a special component that can be used to group widgets together.
	/// </summary>
	[Obsolete("Use panels instead", false)]
	public class Section : GridPanel
	{
		private bool isEnabled = true;
		private bool isVisible = true;
		private bool isReadOnly = false;

		/// <summary>
		/// Gets the number of columns that are currently defined by the widgets that have been added to this section.
		/// </summary>
		public int ColumnCount { get => GetColumnCount(); }

		/// <summary>
		/// Gets the number of rows that are currently defined by the widgets that have been added to this section.
		/// </summary>
		public int RowCount { get => GetRowCount(); }

		/// <summary>
		/// 	Gets or sets a value indicating whether the widgets within the section are visible or not.
		/// </summary>
		public virtual bool IsVisible
		{
			get
			{
				return isVisible;
			}

			set
			{
				isVisible = value;
				if (isVisible)
				{
					ShowWidgets(true);
				}
				else
				{
					HideWidgets(true);
				}
			}
		}

		/// <summary>
		/// 	Gets or sets a value indicating whether the interactive widgets within the section are enabled or not.
		/// </summary>
		public virtual bool IsEnabled
		{
			get
			{
				return isEnabled;
			}

			set
			{
				isEnabled = value;
				if (isEnabled)
				{
					EnableWidgets(true);
				}
				else
				{
					EnableWidgets(true);
				}
			}
		}

		/// <summary>
		/// 		Gets or sets a value indicating whether the control is displayed in read-only mode.
		/// 		Read-only mode causes all widgets in section to appear read-write but the user won't be able to change their value.
		/// 		This only affects interactive scripts running in a web environment.
		/// </summary>
		/// <remarks>Available from DataMiner 10.4.1 onwards.</remarks>
		public virtual bool IsReadOnly
		{
			get
			{
				return isReadOnly;
			}

			set
			{
				isReadOnly = value;
				foreach (Widget widget in Widgets)
				{
					widget.BlockDefinition.IsReadOnly = isReadOnly;
				}
			}
		}

		/// <summary>
		///     Gets widgets that have been added to the section.
		/// </summary>
		public IEnumerable<Widget> Widgets
		{
			get
			{
				return GetWidgetLocationPairs().Select(x => (Widget)x.Widget);
			}
		}

		/// <summary>
		///     Adds a widget to the section.
		/// </summary>
		/// <param name="widget">Widget to add to the <see cref="Section" />.</param>
		/// <param name="widgetLayout">Location of the widget in the grid layout.</param>
		/// <returns>The dialog.</returns>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to the <see cref="Section" />.</exception>
		public Section AddWidget(Widget widget, IWidgetLayout widgetLayout)
		{
			if (widget == null)
			{
				throw new ArgumentNullException("widget");
			}

			var existingWidget = Widgets.FirstOrDefault(w => w == widget);
			if (existingWidget != null)
			{
				throw new ArgumentException("Widget is already added to the section");
			}

			widget.HorizontalAlignment = widgetLayout.HorizontalAlignment;
			widget.VerticalAlignment = widgetLayout.VerticalAlignment;
			widget.Margin = widgetLayout.Margin;
			Add(widget, new WidgetLocation(widgetLayout.Row, widgetLayout.Column, widgetLayout.RowSpan, widgetLayout.ColumnSpan));

			return this;
		}

		/// <summary>
		///     Adds a widget to the section.
		/// </summary>
		/// <param name="widget">Widget to add to the section.</param>
		/// <param name="row">Row location of the widget on the grid.</param>
		/// <param name="column">Column location of the widget on the grid.</param>
		/// <param name="horizontalAlignment">Horizontal alignment of the widget.</param>
		/// <param name="verticalAlignment">Vertical alignment of the widget.</param>
		/// <returns>The updated section.</returns>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the location is out of bounds of the grid.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to the dialog.</exception>
		public Section AddWidget(
			Widget widget,
			int row,
			int column,
			HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment verticalAlignment = VerticalAlignment.Center)
		{
			AddWidget(widget, new WidgetLayout(row, column, horizontalAlignment, verticalAlignment));
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
		/// <param name="horizontalAlignment">Horizontal alignment of the widget.</param>
		/// <param name="verticalAlignment">Vertical alignment of the widget.</param>
		/// <returns>The updated section.</returns>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the location is out of bounds of the grid.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to the dialog.</exception>
		public Section AddWidget(
			Widget widget,
			int fromRow,
			int fromColumn,
			int rowSpan,
			int colSpan,
			HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment verticalAlignment = VerticalAlignment.Center)
		{
			AddWidget(
				widget,
				new WidgetLayout(fromRow, fromColumn, rowSpan, colSpan, horizontalAlignment, verticalAlignment));
			return this;
		}

		/// <summary>
		/// Adds the widgets from the section to the section.
		/// </summary>
		/// <param name="section">Section to be added to the section.</param>
		/// <param name="layout">Left-top position of the section within the parent section.</param>
		/// <returns>The updated section.</returns>
		public Section AddSection(Section section, ILayout layout)
		{
			Add(section, new PanelLocation(layout.Row, layout.Column));

			return this;
		}

		/// <summary>
		/// Adds the widgets from the section to the section.
		/// </summary>
		/// <param name="section">Section to be added to the section.</param>
		/// <param name="row">Row of the section within the parent section.</param>
		/// <param name="column">Column of the section within the parent section.</param>
		/// <returns>The updated section.</returns>
		public Section AddSection(Section section, int row, int column)
		{
			return AddSection(section, new SectionLayout(row, column));
		}

		/// <summary>
		///     Gets the layout of the widget in the dialog.
		/// </summary>
		/// <param name="widget">A widget that is part of the dialog.</param>
		/// <returns>The widget layout in the dialog.</returns>
		/// <exception cref="NullReferenceException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of the dialog.</exception>
		public IWidgetLayout GetWidgetLayout(Widget widget)
		{
			CheckWidgetExits(widget);

			var pair = GetWidgetLocationPairs().FirstOrDefault(w => w.Widget == widget);
			return GetWidgetLayout(pair.Widget, pair.Location);
		}

		/// <summary>
		///     Removes a widget from the dialog.
		/// </summary>
		/// <param name="widget">Widget to remove.</param>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		public void RemoveWidget(Widget widget)
		{
			if (widget == null)
			{
				throw new ArgumentNullException("widget");
			}

			Remove(widget);
			foreach (var panel in GetPanels())
			{
				panel.Remove(widget);
			}
		}

		/// <summary>
		///     Sets the layout of a widget in the section.
		/// </summary>
		/// <param name="widget">A widget that is part of the section.</param>
		/// <param name="widgetLayout">The layout to apply to the widget.</param>
		/// <exception cref="NullReferenceException">When widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of the section.</exception>
		/// <exception cref="NullReferenceException">When widgetLayout is null.</exception>
		public void SetWidgetLayout(Widget widget, IWidgetLayout widgetLayout)
		{
			if (widgetLayout == null)
			{
				throw new ArgumentNullException(nameof(widgetLayout));
			}

			CheckWidgetExits(widget);
			Move(widget, widgetLayout.Row, widgetLayout.Column, widgetLayout.RowSpan, widgetLayout.ColumnSpan);
			widget.HorizontalAlignment = widgetLayout.HorizontalAlignment;
			widget.VerticalAlignment = widgetLayout.VerticalAlignment;
			widget.Margin = widgetLayout.Margin;
		}

		private void CheckWidgetExits(Widget widget)
		{
			if (widget is null)
			{
				throw new ArgumentNullException("widget");
			}

			var existingWidget = Widgets.FirstOrDefault(w => w == widget);
			if (existingWidget is null)
			{
				throw new ArgumentException("Widget is not part of this dialog");
			}
		}

		private IWidgetLayout GetWidgetLayout(IWidget widget, WidgetLocation location)
		{
			return new WidgetLayout(
				location.Row,
				location.Column,
				location.RowSpan,
				location.ColumnSpan,
				widget.HorizontalAlignment,
				widget.VerticalAlignment);
		}
	}
}
