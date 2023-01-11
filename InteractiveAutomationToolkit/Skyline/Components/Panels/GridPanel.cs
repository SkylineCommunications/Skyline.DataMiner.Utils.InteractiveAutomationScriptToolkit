namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <inheritdoc cref="IGridPanel" />
	public class GridPanel : Panel, IGridPanel
	{
		private readonly HashSet<IPanel> panels = new HashSet<IPanel>();

		private readonly Dictionary<IPanel, PanelLocation> panelLocations =
			new Dictionary<IPanel, PanelLocation>();

		private readonly HashSet<IWidget> widgets = new HashSet<IWidget>();

		private readonly Dictionary<IWidget, WidgetLocation>
			widgetLocations = new Dictionary<IWidget, WidgetLocation>();

		/// <inheritdoc/>
		public override IEnumerable<WidgetLocationPair> GetWidgetLocationPairs()
		{
			foreach (KeyValuePair<IWidget, WidgetLocation> pair in widgetLocations.Where(pair => pair.Key.IsVisible))
			{
				yield return new WidgetLocationPair(pair.Key, pair.Value);
			}

			foreach (KeyValuePair<IPanel, PanelLocation> pair in panelLocations)
			{
				IPanel panel = pair.Key;
				PanelLocation panelLocation = pair.Value;
				foreach (WidgetLocationPair widgetLocationPair in panel.GetWidgetLocationPairs())
				{
					IWidget widget = widgetLocationPair.Widget;
					WidgetLocation widgetLocation = widgetLocationPair.Location;
					yield return new WidgetLocationPair(widget, widgetLocation.AddOffset(panelLocation));
				}
			}
		}

		/// <inheritdoc />
		public override void Clear()
		{
			widgetLocations.Clear();
			foreach (IWidget widget in widgets)
			{
				RemoveParentFrom(widget);
			}

			widgets.Clear();
			panelLocations.Clear();

			foreach (IPanel panel in panels)
			{
				RemoveParentFrom(panel);
			}

			panels.Clear();
		}

		/// <inheritdoc />
		public void Add(IPanel panel, PanelLocation location)
		{
			if (panel == null)
			{
				throw new ArgumentNullException(nameof(panel));
			}

			AddParentTo(panel);
			panels.Add(panel);
			panelLocations.Add(panel, location);
		}

		/// <inheritdoc />
		public void Add(IPanel panel, int fromRow, int fromColumn)
		{
			Add(panel, new PanelLocation(fromRow, fromColumn));
		}

		/// <inheritdoc />
		public void Add(IWidget widget, WidgetLocation location)
		{
			if (widget == null)
			{
				throw new ArgumentNullException(nameof(widget));
			}

			AddParentTo(widget);
			widgets.Add(widget);
			widgetLocations.Add(widget, location);
		}

		/// <inheritdoc />
		public void Add(IWidget widget, int row, int column)
		{
			Add(widget, new WidgetLocation(row, column));
		}

		/// <inheritdoc />
		public void Add(IWidget widget, int fromRow, int fromColumn, int rowSpan, int colSpan)
		{
			Add(widget, new WidgetLocation(fromRow, fromColumn, rowSpan, colSpan));
		}

		/// <inheritdoc />
		public override IEnumerable<IPanel> GetPanels(bool includeNested)
		{
			return includeNested
				? panels.Concat(panels.SelectMany(panel => panel.GetPanels()))
				: panels;
		}

		/// <inheritdoc />
		public override IEnumerable<IWidget> GetWidgets(bool includeNested)
		{
			return includeNested
				? widgets.Concat(panels.SelectMany(panel => panel.GetWidgets()))
				: widgets;
		}

		/// <inheritdoc />
		public void Move(IPanel panel, PanelLocation location)
		{
			if (panel == null)
			{
				throw new ArgumentNullException(nameof(panel));
			}

			if (!panels.Contains(panel))
			{
				throw new ArgumentException("Widget is not part of this component.");
			}

			panelLocations[panel] = location;
		}

		/// <inheritdoc />
		public void Move(IPanel panel, int fromRow, int fromColumn)
		{
			Move(panel, new PanelLocation(fromRow, fromColumn));
		}

		/// <inheritdoc />
		public void Move(IWidget widget, WidgetLocation widgetLocation)
		{
			if (widget == null)
			{
				throw new ArgumentNullException(nameof(widget));
			}

			if (!widgets.Contains(widget))
			{
				throw new ArgumentException("Widget is not part of this component.");
			}

			widgetLocations[widget] = widgetLocation;
		}

		/// <inheritdoc />
		public void Move(IWidget widget, int row, int column)
		{
			Move(widget, row, column, 1, 1);
		}

		/// <inheritdoc />
		public void Move(IWidget widget, int fromRow, int fromColumn, int rowSpan, int colSpan)
		{
			Move(widget, new WidgetLocation(fromRow, fromColumn, rowSpan, colSpan));
		}

		/// <inheritdoc />
		public void Remove(IPanel panel)
		{
			if (panel == null)
			{
				throw new ArgumentNullException(nameof(panel));
			}

			RemoveParentFrom(panel);
			panels.Remove(panel);
			panelLocations.Remove(panel);
		}

		/// <inheritdoc />
		public void Remove(IWidget widget)
		{
			if (widget == null)
			{
				throw new ArgumentNullException(nameof(widget));
			}

			RemoveParentFrom(widget);
			widgets.Remove(widget);
			widgetLocations.Remove(widget);
		}

		/// <inheritdoc />
		public PanelLocation GetLocation(IPanel panel)
		{
			if (panel == null)
			{
				throw new ArgumentNullException(nameof(panel));
			}

			if (!panelLocations.TryGetValue(panel, out PanelLocation location))
			{
				throw new ArgumentException("Widget is not part of this component.");
			}

			return location;
		}

		/// <inheritdoc />
		public WidgetLocation GetLocation(IWidget widget)
		{
			if (widget == null)
			{
				throw new ArgumentNullException(nameof(widget));
			}

			if (!widgetLocations.TryGetValue(widget, out WidgetLocation widgetLocation))
			{
				throw new ArgumentException("Widget is not part of this component.");
			}

			return widgetLocation;
		}
	}
}