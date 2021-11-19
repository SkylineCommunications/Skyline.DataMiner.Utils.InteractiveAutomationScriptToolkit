namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Skyline.DataMiner.Automation;

	/// <summary>
	/// A section is a special component that can be used to group Widgets together.
	/// </summary>
	public class Section
	{
		private readonly Dictionary<Widget, IWidgetLayout> widgetLayouts = new Dictionary<Widget, IWidgetLayout>();

		private bool isEnabled = true;
		private bool isVisible = true;

		public int ColumnCount { get; private set; }

		public int RowCount { get; private set; }

		/// <summary>
		///		Gets or sets a value indicating whether the Widgets within the section are visible or not.
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
				foreach (Widget widget in Widgets)
				{
					widget.IsVisible = isVisible;
				}
			}
		}

		/// <summary>
		///		Gets or sets a value indicating whether the Interactive Widgets within the section are enabled or not.
		/// </summary>
		public bool IsEnabled
		{
			get
			{
				return isEnabled;
			}

			set
			{
				isEnabled = value;
				foreach (Widget widget in Widgets)
				{
					InteractiveWidget interactiveWidget = widget as InteractiveWidget;
					if (interactiveWidget != null)
					{
						interactiveWidget.IsEnabled = isEnabled;
					}
				}
			}
		}

		/// <summary>
		///     Gets widgets that are added to the section.
		/// </summary>
		public IEnumerable<Widget> Widgets
		{
			get
			{
				return widgetLayouts.Keys;
			}
		}

		/// <summary>
		///     Adds a widget to the dialog.
		/// </summary>
		/// <param name="widget">Widget to add to the dialog.</param>
		/// <param name="widgetLayout">Location of the widget on the grid layout.</param>
		/// <returns>The dialog.</returns>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is already added to the dialog.</exception>
		/// <exception cref="ArgumentException">When the widget overlaps with another widget.</exception>
		public Section AddWidget(Widget widget, IWidgetLayout widgetLayout)
		{
			if (widget == null)
			{
				throw new ArgumentNullException("widget");
			}

			if (widgetLayouts.ContainsKey(widget))
			{
				throw new ArgumentException("Widget is already added to the dialog");
			}

			if (Overlaps(widgetLayout))
			{
				throw new ArgumentException(String.Format("The widget overlaps with another widget in the Dialog on Row {0}, Column {1}, RowSpan {2}, ColumnSpan {3}", widgetLayout.Row, widgetLayout.Column, widgetLayout.RowSpan, widgetLayout.ColumnSpan));
			}

			widgetLayouts.Add(widget, widgetLayout);
			UpdateRowAndColumnCount();

			return this;
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
			return widgetLayouts[widget];
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

			widgetLayouts.Remove(widget);
			UpdateRowAndColumnCount();
		}

		/// <summary>
		///     Sets the layout of the widget in the dialog.
		/// </summary>
		/// <param name="widget">A widget that is part of the dialog.</param>
		/// <param name="widgetLayout">The layout to apply on the widget.</param>
		/// <exception cref="NullReferenceException">When widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of the dialog.</exception>
		/// <exception cref="NullReferenceException">When widgetLayout is null.</exception>
		/// <exception cref="ArgumentException">When widgetLayout is out of bounds of the dialog grid.</exception>
		public void SetWidgetLayout(Widget widget, IWidgetLayout widgetLayout)
		{
			CheckWidgetExits(widget);
			widgetLayouts[widget] = widgetLayout;
		}

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
		/// <exception cref="ArgumentException">When the location is out of bounds of the grid.</exception>
		/// <exception cref="ArgumentException">When the widget is already added to the dialog.</exception>
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
		/// <exception cref="ArgumentException">When the location is out of bounds of the grid.</exception>
		/// <exception cref="ArgumentException">When the widget is already added to the dialog.</exception>
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
		/// Adds the widgets from the section to the Dialog.
		/// </summary>
		/// <param name="section">Section to be added to the Dialog.</param>
		/// <param name="layout">Left top position of the Section within the Dialog.</param>
		public Section AddSection(Section section, ILayout layout)
		{
			foreach (Widget widget in section.Widgets)
			{
				IWidgetLayout widgetLayout = section.GetWidgetLayout(widget);
				AddWidget(
					widget,
					new WidgetLayout(
						widgetLayout.Row + layout.Row,
						widgetLayout.Column + layout.Column,
						widgetLayout.RowSpan,
						widgetLayout.ColumnSpan,
						widgetLayout.HorizontalAlignment,
						widgetLayout.VerticalAlignment));
			}

			return this;
		}

		/// <summary>
		/// Removes all Widgets from the Section.
		/// </summary>
		public void Clear()
		{
			widgetLayouts.Clear();
			RowCount = 0;
			ColumnCount = 0;
		}

		/// <summary>
		///		Checks if the widget layout overlaps with another widget in the Dialog.
		/// </summary>
		/// <param name="widgetLayout">Layout to be checked.</param>
		/// <returns>True if the layout overlaps with another layout, else false.</returns>
		private bool Overlaps(IWidgetLayout widgetLayout)
		{
			for (int column = widgetLayout.Column; column < widgetLayout.Column + widgetLayout.ColumnSpan; column++)
			{
				for (int row = widgetLayout.Row; row < widgetLayout.Row + widgetLayout.RowSpan; row++)
				{
					foreach (IWidgetLayout existingWidgetLayout in widgetLayouts.Values)
					{
						if (column >= existingWidgetLayout.Column && column < existingWidgetLayout.Column + existingWidgetLayout.ColumnSpan && row >= existingWidgetLayout.Row && row < existingWidgetLayout.Row + existingWidgetLayout.RowSpan)
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		private void CheckWidgetExits(Widget widget)
		{
			if (widget == null)
			{
				throw new ArgumentNullException("widget");
			}

			if (!widgetLayouts.ContainsKey(widget))
			{
				throw new ArgumentException("Widget is not part of this dialog");
			}
		}

		/// <summary>
		///		Used to update the RowCount and ColumnCount properties based on the Widgets added to the section.
		/// </summary>
		private void UpdateRowAndColumnCount()
		{
			if(widgetLayouts.Any())
			{
				RowCount = widgetLayouts.Values.Max(w => w.Row + w.RowSpan);
				ColumnCount = widgetLayouts.Values.Max(w => w.Column + w.ColumnSpan);
			}
			else
			{
				RowCount = 0;
				ColumnCount = 0;
			}
		}
	}
}
