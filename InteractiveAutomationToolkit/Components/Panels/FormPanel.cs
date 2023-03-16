namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;

	/// <inheritdoc cref="IFormPanel" />
	public class FormPanel : Panel, IFormPanel
	{
		private readonly List<IComponent> components = new List<IComponent>();
		private readonly List<ILabel> labels = new List<ILabel>();
		private readonly Dictionary<IPanel, PanelLocation> panelLocations = new Dictionary<IPanel, PanelLocation>();
		private readonly List<int> spans = new List<int>();

		private readonly Dictionary<IWidget, WidgetLocation>
			widgetLocations = new Dictionary<IWidget, WidgetLocation>();

		/// <inheritdoc/>
		public void Add(string label, IComponent component)
		{
			Add(new Label(label), component);
		}

		/// <inheritdoc/>
		public void Add(ILabel label, IComponent component)
		{
			if (label == null)
			{
				throw new ArgumentNullException(nameof(label));
			}

			if (component == null)
			{
				throw new ArgumentNullException(nameof(component));
			}

			AddParentTo(component);
			components.Add(component);
			AddParentTo(label);
			labels.Add(label);
			spans.Add(1);
		}

		/// <inheritdoc/>
		public void Add(string label, IWidget widget, int span)
		{
			Add(new Label(label), widget, span);
		}

		/// <inheritdoc/>
		public void Add(ILabel label, IWidget widget, int span)
		{
			if (label == null)
			{
				throw new ArgumentNullException(nameof(label));
			}

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
			AddParentTo(label);
			labels.Add(label);
			spans.Add(span);
		}

		/// <inheritdoc />
		public override void Clear()
		{
			foreach (IComponent component in components)
			{
				RemoveParentFrom(component);
			}

			components.Clear();

			foreach (ILabel label in labels)
			{
				RemoveParentFrom(label);
			}

			labels.Clear();
			spans.Clear();
		}

		/// <inheritdoc />
		public override IEnumerable<IPanel> GetPanels(bool includeNested)
		{
			return includeNested
				? components.OfType<IPanel>().Concat(components.OfType<IPanel>().SelectMany(panel => panel.GetPanels()))
				: components.OfType<IPanel>();
		}

		/// <inheritdoc />
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

		/// <inheritdoc/>
		public override IEnumerable<IWidget> GetWidgets(bool includeNested)
		{
			return includeNested
				? GetWidgets(false).Concat(GetPanels(false).SelectMany(panel => panel.GetWidgets()))
				: components.OfType<IWidget>().Concat(labels);
		}

		/// <inheritdoc/>
		public void Remove(IComponent component)
		{
			int i = components.IndexOf(component);
			if (i == -1)
			{
				return;
			}

			RemoveParentFrom(components[i]);
			components.RemoveAt(i);
			RemoveParentFrom(labels[i]);
			labels.RemoveAt(i);
			spans.RemoveAt(i);
		}

		private void AssignLocations()
		{
			widgetLocations.Clear();
			panelLocations.Clear();

			var row = 0;
			for (var i = 0; i < components.Count; i++)
			{
				object component = components[i];
				ILabel label = labels[i];
				switch (component)
				{
					case IWidget widget when !widget.IsVisible:
						continue;

					case IWidget widget:
						widgetLocations.Add(label, new WidgetLocation(row, 0));
						widgetLocations.Add(widget, new WidgetLocation(row, 1, 1, spans[i]));
						row++;
						continue;

					case IPanel panel:
					{
						int rowCount = panel.GetRowCount();
						if (rowCount == 0)
						{
							continue;
						}

						widgetLocations.Add(label, new WidgetLocation(row, 0));
						panelLocations.Add(panel, new PanelLocation(row, 1));
						row += rowCount;
						continue;
					}

					default:
						Debug.Fail("Unexpected component type.");
						return;
				}
			}
		}
	}
}