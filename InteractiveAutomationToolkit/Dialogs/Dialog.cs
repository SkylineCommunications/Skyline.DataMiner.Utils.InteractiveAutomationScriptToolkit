namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Net.Exceptions;

	/// <summary>
	///     A dialog represents a single window that can be shown.
	///     You can show widgets in the window by adding them to the dialog.
	///     The dialog uses a grid to determine the layout of its widgets.
	/// </summary>
	[Obsolete("Use Dialog<TPanel> instead", false)]
	public class Dialog : Dialog<GridPanel>
	{
		private bool isEnabled = true;

		/// <summary>
		/// Initializes a new instance of the <see cref="Dialog" /> class.
		/// </summary>
		/// <param name="engine">Allows interaction with the DataMiner System.</param>
		public Dialog(IEngine engine) : base(engine)
		{
			if (engine == null)
			{
				throw new ArgumentNullException("engine");
			}
		}

		/// <summary>
		///     Gets the number of columns of the grid layout.
		/// </summary>
		public int ColumnCount
		{
			get
			{
				var widgets = Panel.GetWidgetLocationPairs();
				if (!widgets.Any())
				{
					return 0;
				}

				var columns = new bool[widgets.Max(p => p.Location.Column + p.Location.ColumnSpan)];
				foreach (var widget in widgets)
				{
					for (int i = widget.Location.Column; i < widget.Location.Column + widget.Location.ColumnSpan; i++)
					{
						columns[i] = true;
					}
				}

				return columns.Count(c => c);
			}
		}

		/// <summary>
		///     Gets the number of rows in the grid layout.
		/// </summary>
		public int RowCount
		{
			get
			{
				var widgets = Panel.GetWidgetLocationPairs();
				if (!widgets.Any())
				{
					return 0;
				}

				var rows = new bool[widgets.Max(p => p.Location.Row + p.Location.RowSpan)];
				foreach (var widget in widgets)
				{
					for (int i = widget.Location.Row; i < widget.Location.Row + widget.Location.RowSpan; i++)
					{
						rows[i] = true;
					}
				}

				return rows.Count(c => c);
			}
		}

		/// <summary>
		/// 	Gets or sets a value indicating whether the interactive widgets within the dialog are enabled or not.
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
					if (interactiveWidget != null && !(interactiveWidget is CollapseButton))
					{
						interactiveWidget.IsEnabled = isEnabled;
					}
				}
			}
		}

		/// <summary>
		///     Gets widgets that are added to the dialog.
		/// </summary>
		public IEnumerable<Widget> Widgets
		{
			get
			{
				return Panel.GetAllWidgetLocationPairs().Select(w => (Widget)w.Widget);
			}
		}

		/// <summary>
		///     Adds a widget to the dialog.
		/// </summary>
		/// <param name="widget">Widget to add to the dialog.</param>
		/// <param name="widgetLayout">Location of the widget on the grid layout.</param>
		/// <returns>The dialog.</returns>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to the dialog.</exception>
		public Dialog AddWidget(Widget widget, IWidgetLayout widgetLayout)
		{
			if (widget == null)
			{
				throw new ArgumentNullException("widget");
			}

			var existingWidget = Widgets.FirstOrDefault(w => w == widget);
			if (existingWidget != null)
			{
				throw new ArgumentException("Widget is already added to the dialog");
			}

			widget.HorizontalAlignment = widgetLayout.HorizontalAlignment;
			widget.VerticalAlignment = widgetLayout.VerticalAlignment;
			widget.Margin = widgetLayout.Margin;
			Panel.Add(widget, new WidgetLocation(widgetLayout.Row, widgetLayout.Column, widgetLayout.RowSpan, widgetLayout.ColumnSpan));
			return this;
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
		/// <exception cref="ArgumentException">When the widget has already been added to the dialog.</exception>
		public Dialog AddWidget(
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
		/// <exception cref="ArgumentException">When the widget has already been added to the dialog.</exception>
		public Dialog AddWidget(
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
		///     Gets the layout of the widget in the dialog.
		/// </summary>
		/// <param name="widget">A widget that is part of the dialog.</param>
		/// <returns>The widget layout in the dialog.</returns>
		/// <exception cref="NullReferenceException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of the dialog.</exception>
		public IWidgetLayout GetWidgetLayout(Widget widget)
		{
			if (widget == null)
			{
				throw new ArgumentNullException("widget");
			}

			var existingWidget = Panel.GetWidgetLocationPairs().FirstOrDefault(w => w.Widget == widget);
			if (existingWidget.Widget is null)
			{
				throw new ArgumentException("Widget is not part of this dialog");
			}

			return new WidgetLayout(
				existingWidget.Location.Row,
				existingWidget.Location.Column,
				existingWidget.Location.RowSpan,
				existingWidget.Location.ColumnSpan,
				existingWidget.Widget.HorizontalAlignment,
				existingWidget.Widget.VerticalAlignment);
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

			Panel.Remove(widget);
			foreach (var panel in Panel.GetPanels())
			{
				switch (panel)
				{
					case GridPanel gridPanel:
						gridPanel.Remove(widget);
						break;

					case FormPanel formPanel:
						formPanel.Remove(widget);
						break;

					case StackPanel stackPanel:
						stackPanel.Remove(widget);
						break;

					default:
						throw new ArgumentException("Widget is not part of this dialog");
				}
			}
		}

		/// <summary>
		/// Adds the widgets from the section to the dialog.
		/// </summary>
		/// <param name="section">Section to be added to the dialog.</param>
		/// <param name="layout">Left top position of the section within the dialog.</param>
		/// <returns>Updated dialog.</returns>
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
		public Dialog AddSection(Section section, int fromRow, int fromColumn)
		{
			return AddSection(section, new SectionLayout(fromRow, fromColumn));
		}

		/// <summary>
		///     Sets the layout of the widget in the dialog.
		/// </summary>
		/// <param name="widget">A widget that is part of the dialog.</param>
		/// <param name="widgetLayout">The layout to apply to the widget.</param>
		/// <exception cref="NullReferenceException">When widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of the dialog.</exception>
		public void SetWidgetLayout(Widget widget, IWidgetLayout widgetLayout)
		{
			if (widget == null)
			{
				throw new ArgumentNullException("widget");
			}

			var existingWidget = Widgets.FirstOrDefault(w => w == widget);
			if (existingWidget is null)
			{
				throw new ArgumentException("Widget is not part of this dialog");
			}

			Panel.Move(widget, widgetLayout.Row, widgetLayout.Column, widgetLayout.RowSpan, widgetLayout.ColumnSpan);
			existingWidget.HorizontalAlignment = widgetLayout.HorizontalAlignment;
			existingWidget.VerticalAlignment = widgetLayout.VerticalAlignment;
			existingWidget.Margin = widgetLayout.Margin;
		}

		/// <summary>
		///     Shows the dialog window.
		///     Also loads changes and triggers events when <paramref name="requireResponse" /> is <c>true</c>.
		/// </summary>
		/// <param name="requireResponse">If the dialog expects user interaction.</param>
		/// <remarks>Should only be used when you create your own event loop.</remarks>
		public void Show(bool requireResponse = true)
		{
			UIBuilder uiBuilder = Build();
			uiBuilder.RequireResponse = requireResponse;

			IUIResults uiResults;

			try
			{
				uiResults = new WrappedUIResults(Engine.ShowUI(uiBuilder));
			}
			catch (InteractiveUserDetachedException)
			{
				throw;
			}
			catch (DataMinerException e)
			{
				throw new InvalidOperationException($"{nameof(IEngine)}.{nameof(Engine.ShowUI)} failed with {nameof(UIBuilder)} argument {uiBuilder}", e);
			}

			if (requireResponse)
			{
				LoadChanges(uiResults);
				RaiseResultEvents(uiResults);
			}
		}

		/// <summary>
		/// Hides the dialog. This does not block any background logic from running.
		/// Use <see cref="Show"/> if you want to show the dialog again.
		/// </summary>
		public void Hide()
		{
			Engine.HideUI();
		}

		/// <summary>
		/// Removes all widgets from the dialog.
		/// </summary>
		public void Clear()
		{
			Panel.Clear();
		}
	}
}
