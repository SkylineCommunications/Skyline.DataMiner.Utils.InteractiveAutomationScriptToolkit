namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     A dialog represents a single window that can be shown.
	///     You can show widgets in the window by adding them to the dialog.
	///     The dialog uses a grid to determine the layout of its widgets.
	/// </summary>
	[Obsolete("Use Dialog<TPanel> instead.", false)]
	public class Dialog : Dialog<GridPanel>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Dialog"/> class.
		/// </summary>
		/// <param name="engine">Allows interaction with the DataMiner System.</param>
		public Dialog(IEngine engine) : base(engine)
		{
		}

		/// <summary>
		///     Gets widgets that are added to the dialog.
		/// </summary>
		[Obsolete("Call GetWidgets instead.", false)]
		public IEnumerable<Widget> Widgets => Panel.GetWidgets(true).Cast<Widget>();

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
		[Obsolete("Call Panel.Add instead.", false)]
		public Dialog AddWidget(
			Widget widget,
			int row,
			int column,
			HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment verticalAlignment = VerticalAlignment.Center)
		{
			Panel.Add(widget, row, column);
			widget.HorizontalAlignment = horizontalAlignment;
			widget.VerticalAlignment = verticalAlignment;
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
		/// <exception cref="ArgumentException">When the widget has already been added to the dialog.</exception>
		[Obsolete("Call Panel.Add instead.", false)]
		public Dialog AddWidget(
			Widget widget,
			int fromRow,
			int fromColumn,
			int rowSpan,
			int colSpan,
			HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment verticalAlignment = VerticalAlignment.Center)
		{
			Panel.Add(widget, fromRow, fromColumn, rowSpan, colSpan);
			widget.HorizontalAlignment = horizontalAlignment;
			widget.VerticalAlignment = verticalAlignment;
			return this;
		}

		/// <summary>
		///     Adds a widget to the dialog.
		/// </summary>
		/// <param name="widget">Widget to add to the dialog.</param>
		/// <param name="widgetLayout">Location of the widget on the grid layout.</param>
		/// <returns>The dialog.</returns>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to the dialog.</exception>
		[Obsolete("Call Panel.Add instead.", false)]
		public Dialog AddWidget(Widget widget, IWidgetLayout widgetLayout)
		{
			Panel.Add(widget, widgetLayout.Row, widgetLayout.Column, widgetLayout.RowSpan, widgetLayout.ColumnSpan);
			widget.HorizontalAlignment = widgetLayout.HorizontalAlignment;
			widget.VerticalAlignment = widgetLayout.VerticalAlignment;
			return this;
		}

		/// <summary>
		/// Adds the widgets from the section to the dialog.
		/// </summary>
		/// <param name="section">Section to be added to the dialog.</param>
		/// <param name="layout">Left top position of the section within the dialog.</param>
		/// <returns>Updated dialog.</returns>
		[Obsolete("Call Panel.Add instead.", false)]
		public Dialog AddSection(Section section, SectionLayout layout)
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
		/// Adds the widgets from the section to the dialog.
		/// </summary>
		/// <param name="section">Section to be added to the dialog.</param>
		/// <param name="fromRow">Row in the dialog where the section should be added.</param>
		/// <param name="fromColumn">Column in the dialog where the section should be added.</param>
		/// <returns>Updated dialog.</returns>
		[Obsolete("Call Panel.Add instead.", false)]
		public Dialog AddSection(Section section, int fromRow, int fromColumn)
		{
			return AddSection(section, new SectionLayout(fromRow, fromColumn));
		}

		/// <summary>
		/// Removes all widgets from the dialog.
		/// </summary>
		[Obsolete("Call Panel.Clear instead.", false)]
		public void Clear()
		{
			Panel.Clear();
		}

		/// <summary>
		///     Sets the layout of the widget in the dialog.
		/// </summary>
		/// <param name="widget">A widget that is part of the dialog.</param>
		/// <param name="widgetLayout">The layout to apply to the widget.</param>
		/// <exception cref="NullReferenceException">When widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of the dialog.</exception>
		[Obsolete("Call Panel.Move instead.", false)]
		public void SetWidgetLayout(Widget widget, IWidgetLayout widgetLayout)
		{
			var widgetLocation = new WidgetLocation(
				widgetLayout.Row,
				widgetLayout.Column,
				widgetLayout.RowSpan,
				widgetLayout.ColumnSpan);

			Panel.Move(widget, widgetLocation);
			widget.HorizontalAlignment = widgetLayout.HorizontalAlignment;
			widget.VerticalAlignment = widgetLayout.VerticalAlignment;
		}

		/// <summary>
		///     Gets the layout of the widget in the dialog.
		/// </summary>
		/// <param name="widget">A widget that is part of the dialog.</param>
		/// <returns>The widget layout in the dialog.</returns>
		/// <exception cref="NullReferenceException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of the dialog.</exception>
		[Obsolete("Call Panel.GetLocation instead.", false)]
		public IWidgetLayout GetWidgetLayout(Widget widget)
		{
			WidgetLocation location = Panel.GetLocation(widget);
			return new WidgetLayout(
				location.Row,
				location.Column,
				location.RowSpan,
				location.RowSpan,
				widget.HorizontalAlignment,
				widget.VerticalAlignment);
		}

		/// <summary>
		///     Removes a widget from the dialog.
		/// </summary>
		/// <param name="widget">Widget to remove.</param>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		[Obsolete("Call Panel.Remove instead.", false)]
		public void RemoveWidget(Widget widget)
		{
			if (widget == null)
			{
				throw new ArgumentNullException(nameof(widget));
			}

			Panel.Remove(widget);
		}

		/// <summary>
		///     Shows the dialog window.
		///     Also loads changes and triggers events when <paramref name="requireResponse" /> is <c>true</c>.
		/// </summary>
		/// <param name="requireResponse">If the dialog expects user interaction.</param>
		/// <remarks>Should only be used when you create your own event loop.</remarks>
		[Obsolete("Call ShowInteractive or ShowStatic instead.", false)]
		public void Show(bool requireResponse = true)
		{
			UIBuilder uib = Build();
			uib.RequireResponse = requireResponse;

			UIResults uir = Engine.ShowUI(uib);

			if (requireResponse)
			{
				LoadChanges(uir);
				RaiseResultEvents(uir);
			}
		}
	}
}