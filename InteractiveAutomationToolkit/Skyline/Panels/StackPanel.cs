namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Linq;

	public class StackPanel : Panel, IStackPanel
	{
		private readonly List<IComponent> components = new List<IComponent>();
		private readonly Dictionary<IPanel, PanelLocation> panelLocations = new Dictionary<IPanel, PanelLocation>();
		private readonly List<int> spans = new List<int>();

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

		public void Add(IComponent component)
		{
			if (component == null)
			{
				throw new ArgumentNullException(nameof(component));
			}

			AddParentTo(component);
			components.Add(component);
			spans.Add(1);
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

			AddParentTo(widget);
			components.Add(widget);
			spans.Add(span);
		}

		public override void Clear()
		{
			foreach (IComponent component in components)
			{
				RemoveParentFrom(component);
			}

			components.Clear();
			spans.Clear();
		}

		public IComponent ComponentAt(int index)
		{
			return components[index];
		}

		/// <inheritdoc />
		public IEnumerator<IComponent> GetEnumerator()
		{
			return components.GetEnumerator();
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public override IEnumerable<IPanel> GetPanels(bool includeNested)
		{
			return includeNested
				? components.OfType<IPanel>().Concat(components.OfType<IPanel>().SelectMany(panel => panel.GetPanels()))
				: components.OfType<IPanel>();
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

		public override IEnumerable<IWidget> GetWidgets(bool includeNested)
		{
			return includeNested
				? components.OfType<IWidget>().Concat(GetPanels(false).SelectMany(panel => panel.GetWidgets()))
				: components.OfType<IWidget>();
		}

		public int IndexOf(IComponent component)
		{
			return components.IndexOf(component);
		}

		public void Insert(int index, IComponent component)
		{
			if (component == null)
			{
				throw new ArgumentNullException(nameof(component));
			}

			AddParentTo(component);
			components.Insert(index, component);
			spans.Insert(index, 1);
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

			AddParentTo(widget);
			components.Insert(index, widget);
			spans.Insert(index, span);
		}

		public void Remove(IComponent component)
		{
			int i = components.IndexOf(component);
			if (i == -1)
			{
				return;
			}

			RemoveParentFrom(component);
			components.RemoveAt(i);
			spans.RemoveAt(i);
		}

		public void RemoveAt(int index)
		{
			RemoveParentFrom(components[index]);
			components.RemoveAt(index);
			spans.RemoveAt(index);
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