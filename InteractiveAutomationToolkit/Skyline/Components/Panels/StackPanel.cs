namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Linq;

	/// <inheritdoc cref="Skyline.DataMiner.InteractiveAutomationToolkit.IStackPanel" />
	public class StackPanel : Panel, IStackPanel
	{
		private readonly List<IComponent> components = new List<IComponent>();
		private readonly Dictionary<IPanel, PanelLocation> panelLocations = new Dictionary<IPanel, PanelLocation>();
		private readonly List<int> spans = new List<int>();

		private readonly Dictionary<IWidget, WidgetLocation>
			widgetLocations = new Dictionary<IWidget, WidgetLocation>();

		private Direction direction = Direction.Vertical;

		/// <summary>
		/// Initializes a new instance of the <see cref="StackPanel"/> class.
		/// </summary>
		public StackPanel()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StackPanel"/> class with the specified direction.
		/// </summary>
		/// <param name="direction">The orientation by which the child components are stacked.</param>
		public StackPanel(Direction direction) => Direction = direction;

		/// <inheritdoc/>
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

		/// <summary>
		/// Gets the number of components in the panel.
		/// </summary>
		public int Count => components.Count;

		/// <inheritdoc/>
		public bool IsReadOnly => false;

		/// <summary>
		/// Gets or sets the component at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the component to get or set.</param>
		/// <returns>The component at the specified index.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is not a valid index in the stack.</exception>
		public IComponent this[int index]
		{
			get => components[index];

			set
			{
				IComponent component = components[index];
				RemoveParentFrom(component);
				AddParentTo(value);
				components[index] = value;
				spans[index] = 1;
			}
		}

		/// <summary>
		/// Adds a component to the stack.
		/// </summary>
		/// <param name="component">Component to be added to the stack.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="component" /> is <c>null</c>.</exception>
		/// <exception cref="InvalidOperationException">When trying to add a panel to itself.</exception>
		/// <exception cref="InvalidOperationException">When the component is already added.</exception>
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

		/// <inheritdoc/>
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

		/// <inheritdoc cref="IPanel.Clear"/>/>
		public override void Clear()
		{
			foreach (IComponent component in components)
			{
				RemoveParentFrom(component);
			}

			components.Clear();
			spans.Clear();
		}

		/// <summary>
		/// 	Determines whether the stack has the specified component.
		/// </summary>
		/// <param name="item">Component to locate in the stack.</param>
		/// <returns><c>true</c> if the component is found, <c>false</c> otherwise.</returns>
		public bool Contains(IComponent item)
		{
			return components.Contains(item);
		}

		/// <inheritdoc/>
		public void CopyTo(IComponent[] array, int arrayIndex)
		{
			components.CopyTo(array, arrayIndex);
		}

		/// <inheritdoc/>
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

		/// <inheritdoc/>
		public override IEnumerable<IPanel> GetPanels(bool includeNested)
		{
			return includeNested
				? components.OfType<IPanel>().Concat(components.OfType<IPanel>().SelectMany(panel => panel.GetPanels()))
				: components.OfType<IPanel>();
		}

		/// <inheritdoc/>
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
				? components.OfType<IWidget>().Concat(GetPanels(false).SelectMany(panel => panel.GetWidgets()))
				: components.OfType<IWidget>();
		}

		/// <summary>
		/// Returns the zero-based index of component in the stack.
		/// </summary>
		/// <param name="component">The component to locate in the stack.</param>
		/// <returns>The zero-based index of the component in the stack, if found; otherwise, -1.</returns>
		public int IndexOf(IComponent component)
		{
			return components.IndexOf(component);
		}

		/// <summary>
		/// Inserts a component into the stack at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which the component needs to be inserted.</param>
		/// <param name="component">The component to insert.</param>
		/// <exception cref="IndexOutOfRangeException">When <paramref name="index"/> is less than 0.</exception>
		/// <exception cref="IndexOutOfRangeException">When <paramref name="index"/> is is greater than <see cref="StackPanel.Count"/>.</exception>
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

		/// <inheritdoc/>
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

		/// <summary>
		///     Removes a component from the panel.
		/// </summary>
		/// <param name="component">Component to remove.</param>
		/// <returns><c>true</c> if the component was removed, <c>false</c> otherwise.</returns>
		public bool Remove(IComponent component)
		{
			int i = components.IndexOf(component);
			if (i == -1)
			{
				return false;
			}

			RemoveParentFrom(component);
			components.RemoveAt(i);
			spans.RemoveAt(i);
			return true;
		}

		/// <summary>
		/// Removes the component at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the component to remove.</param>
		/// <exception cref="IndexOutOfRangeException">When <paramref name="index"/> is less than 0.</exception>
		/// <exception cref="IndexOutOfRangeException">When <paramref name="index"/> is is greater than <see cref="StackPanel.Count"/>.</exception>
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