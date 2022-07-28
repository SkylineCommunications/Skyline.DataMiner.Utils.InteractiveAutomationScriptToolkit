namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Linq;

	public class StackPanel : Panel, IStackPanel
	{
		private readonly List<object> components = new List<object>();
		private readonly List<int> spans = new List<int>();
		private readonly Dictionary<IPanel, PanelLocation> panelLocations = new Dictionary<IPanel, PanelLocation>();

		private readonly Dictionary<IWidget, WidgetLocation>
			widgetLocations = new Dictionary<IWidget, WidgetLocation>();

		private Direction direction = Direction.Horizontal;

		public StackPanel()
		{
		}

		public StackPanel(Direction direction) => Direction = direction;

		public Direction Direction
		{
			get => direction;

			set
			{
				if (!Enum.IsDefined(typeof(Direction), direction))
				{
					throw new InvalidEnumArgumentException(nameof(direction), (int)direction, typeof(Direction));
				}

				direction = value;
			}
		}

		public override IEnumerable<WidgetLocationPair> GetWidgetLocationPairs()
		{
			AssignLocations();
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

		public override void Clear()
		{
			components.Clear();
			spans.Clear();
		}

		public override IEnumerable<IPanel> GetPanels(bool includeNested)
		{
			return includeNested
				? components.OfType<IPanel>().Concat(components.OfType<IPanel>().SelectMany(panel => panel.GetPanels()))
				: components.OfType<IPanel>();
		}

		public override IEnumerable<IWidget> GetWidgets(bool includeNested)
		{
			return includeNested
				? components.OfType<IWidget>().Concat(GetPanels(false).SelectMany(panel => panel.GetWidgets()))
				: components.OfType<IWidget>();
		}

		public void Remove(IPanel panel)
		{
			int i = components.IndexOf(panel);
			if (i == -1)
			{
				return;
			}

			components.RemoveAt(i);
			spans.RemoveAt(i);
		}

		public void Remove(IWidget widget)
		{
			int i = components.IndexOf(widget);
			if (i == -1)
			{
				return;
			}

			components.RemoveAt(i);
			spans.RemoveAt(i);
		}

		public void Add(IPanel panel)
		{
			if (panel == null)
			{
				throw new ArgumentNullException(nameof(panel));
			}

			ValidatePanel(panel);
			components.Add(panel);
			spans.Add(-1); // panels cannot span
		}

		public void Add(IWidget widget)
		{
			Add(widget, 1);
		}

		public void Add(IWidget widget, int span)
		{
			if (widget == null)
			{
				throw new ArgumentNullException(nameof(widget));
			}

			if (span <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(span));
			}

			ValidateWidget(widget);
			components.Add(widget);
			spans.Add(span);
		}

		public void Insert(int index, IPanel panel)
		{
			if (panel == null)
			{
				throw new ArgumentNullException(nameof(panel));
			}

			ValidatePanel(panel);
			components.Insert(index, panel);
			spans.Insert(index, -1); // panels cannot span
		}

		public void Insert(int index, IWidget widget)
		{
			Insert(index, widget, 1);
		}

		public void Insert(int index, IWidget widget, int span)
		{
			if (widget == null)
			{
				throw new ArgumentNullException(nameof(widget));
			}

			if (span <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(span));
			}

			ValidateWidget(widget);
			components.Insert(index, widget);
			spans.Insert(index, span);
		}

		public void RemoveAt(int index)
		{
			components.RemoveAt(index);
			spans.RemoveAt(index);
		}

		public IPanel PanelAt(int index)
		{
			object component = components[index];
			if (component is IPanel panel)
			{
				return panel;
			}

			throw new ComponentNotFoundException(
				FormattableString.Invariant($"No panel could be found at index {index}."));
		}

		public IWidget WidgetAt(int index)
		{
			object component = components[index];
			if (component is IWidget widget)
			{
				return widget;
			}

			throw new ComponentNotFoundException(
				FormattableString.Invariant($"No widget could be found at index {index}."));
		}

		public object ComponentAt(int index)
		{
			return components[index];
		}

		public int IndexOf(IPanel panel)
		{
			return components.IndexOf(panel);
		}

		public int IndexOf(IWidget widget)
		{
			return components.IndexOf(widget);
		}

		private void AssignLocations()
		{
			widgetLocations.Clear();
			panelLocations.Clear();

			var location = 0;
			for (var i = 0; i < components.Count; i++)
			{
				object component = components[i];
				switch (component)
				{
					case IWidget widget when !widget.IsVisible:
						continue;

					case IWidget widget when Direction == Direction.Vertical:
						widgetLocations.Add(widget, new WidgetLocation(location, 0, 1, spans[i]));
						location++;
						continue;

					case IWidget widget when Direction == Direction.Horizontal:
						widgetLocations.Add(widget, new WidgetLocation(0, location, spans[i], 1));
						location++;
						continue;

					case IPanel panel when Direction == Direction.Vertical:
					{
						int rowCount = panel.GetRowCount();
						if (rowCount == 0)
						{
							continue;
						}

						panelLocations.Add(panel, new PanelLocation(location, 0));
						location += rowCount;
						continue;
					}

					case IPanel panel when Direction == Direction.Horizontal:
					{
						int columnCount = panel.GetColumnCount();
						if (columnCount == 0)
						{
							continue;
						}

						panelLocations.Add(panel, new PanelLocation(0, location));
						location += columnCount;
						continue;
					}

					default:
						Debug.Fail("Unexpected component type or direction.");
						return;
				}
			}
		}
	}
}