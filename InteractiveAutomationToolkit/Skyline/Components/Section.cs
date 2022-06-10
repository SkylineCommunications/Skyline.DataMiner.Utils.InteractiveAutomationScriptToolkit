namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	///     A section is a special component that can be used to group widgets together.
	/// </summary>
	public class Section : ISection
	{
		private readonly Dictionary<ISection, SectionLocation> sectionLocations =
			new Dictionary<ISection, SectionLocation>();

		private readonly HashSet<ISection> sections = new HashSet<ISection>();

		private readonly Dictionary<IWidget, WidgetLocation>
			widgetLocations = new Dictionary<IWidget, WidgetLocation>();

		private readonly HashSet<IWidget> widgets = new HashSet<IWidget>();

		/// <inheritdoc />
		public int ColumnCount
		{
			get
			{
				try
				{
					return widgetLocations
						.Where(pair => pair.Key.IsVisible)
						.Select(pair => pair.Value.Column + (pair.Value.ColumnSpan - 1) + 1)
						.Concat(sectionLocations.Select(pair => pair.Key.ColumnCount + pair.Value.Column))
						.Max();
				}
				catch (InvalidOperationException)
				{
					// thrown by Enumerable.Max()
					// no widgets
					return 0;
				}
			}
		}

		/// <inheritdoc />
		public int RowCount
		{
			get
			{
				try
				{
					return widgetLocations
						.Where(pair => pair.Key.IsVisible)
						.Select(pair => pair.Value.Row + (pair.Value.RowSpan - 1) + 1)
						.Concat(sectionLocations.Select(pair => pair.Key.RowCount + pair.Value.Row))
						.Max();
				}
				catch (InvalidOperationException)
				{
					// thrown by Enumerable.Max()
					// no widgets
					return 0;
				}
			}
		}

		/// <inheritdoc />
		public ISection AddSection(ISection section, SectionLocation location)
		{
			if (section == null)
			{
				throw new ArgumentNullException(nameof(section));
			}

			if (section == this)
			{
				throw new ArgumentException("Cannot add a section to itself.");
			}

			if (GetSections().Contains(section) || section.GetSections().Contains(this))
			{
				throw new ArgumentException("Section is already added to the component or nested components.");
			}

			if (GetWidgets().Intersect(section.GetWidgets()).Any())
			{
				throw new ArgumentException(
					"Section contains widgets that are already part of this component or nested components.");
			}

			sections.Add(section);
			sectionLocations.Add(section, location);
			return this;
		}

		/// <inheritdoc />
		public ISection AddSection(ISection section, int fromRow, int fromColumn)
		{
			return AddSection(section, new SectionLocation(fromRow, fromColumn));
		}

		/// <inheritdoc />
		public ISection AddWidget(IWidget widget, WidgetLocation location)
		{
			if (widget == null)
			{
				throw new ArgumentNullException(nameof(widget));
			}

			if (GetWidgets().Contains(widget))
			{
				throw new ArgumentException("Widget is already added.");
			}

			widgets.Add(widget);
			widgetLocations.Add(widget, location);
			return this;
		}

		/// <inheritdoc />
		public ISection AddWidget(IWidget widget, int row, int column)
		{
			AddWidget(widget, new WidgetLocation(row, column));
			return this;
		}

		/// <inheritdoc />
		public ISection AddWidget(IWidget widget, int fromRow, int fromColumn, int rowSpan, int colSpan)
		{
			AddWidget(widget, new WidgetLocation(fromRow, fromColumn, rowSpan, colSpan));
			return this;
		}

		/// <inheritdoc />
		public void Clear()
		{
			widgetLocations.Clear();
			widgets.Clear();
			sectionLocations.Clear();
			sections.Clear();
		}

		/// <inheritdoc />
		public void DisableWidgets()
		{
			DisableWidgets(true);
		}

		/// <inheritdoc />
		public void DisableWidgets(bool includeNested)
		{
			SetWidgetsEnabled(false, true);
		}

		/// <inheritdoc />
		public void EnableWidgets()
		{
			EnableWidgets(true);
		}

		/// <inheritdoc />
		public void EnableWidgets(bool includeNested)
		{
			SetWidgetsEnabled(true, includeNested);
		}

		/// <inheritdoc />
		public SectionLocation GetSectionLocation(ISection section)
		{
			if (section == null)
			{
				throw new ArgumentNullException(nameof(section));
			}

			if (!sectionLocations.TryGetValue(section, out SectionLocation location))
			{
				throw new ArgumentException("Widget is not part of this component.");
			}

			return location;
		}

		/// <inheritdoc />
		public IEnumerable<ISection> GetSections()
		{
			return GetSections(true);
		}

		/// <inheritdoc />
		public IEnumerable<ISection> GetSections(bool includeNested)
		{
			return includeNested
				? sections.Concat(sections.SelectMany(section => section.GetSections()))
				: sections;
		}

		/// <inheritdoc />
		public WidgetLocation GetWidgetLocation(IWidget widget)
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

		/// <inheritdoc />
		public IEnumerable<IWidget> GetWidgets()
		{
			return GetWidgets(true);
		}

		/// <inheritdoc />
		public IEnumerable<IWidget> GetWidgets(bool includeNested)
		{
			return includeNested
				? widgets.Concat(sections.SelectMany(section => section.GetWidgets()))
				: widgets;
		}

		/// <inheritdoc />
		public void HideWidgets()
		{
			HideWidgets(true);
		}

		/// <inheritdoc />
		public void HideWidgets(bool includeNested)
		{
			SetWidgetsVisible(false, true);
		}

		/// <inheritdoc />
		public void MoveSection(ISection section, SectionLocation location)
		{
			if (section == null)
			{
				throw new ArgumentNullException(nameof(section));
			}

			if (!sections.Contains(section))
			{
				throw new ArgumentException("Widget is not part of this component.");
			}

			sectionLocations[section] = location;
		}

		/// <inheritdoc />
		public void MoveSection(ISection section, int fromRow, int fromColumn)
		{
			MoveSection(section, new SectionLocation(fromRow, fromColumn));
		}

		/// <inheritdoc />
		public void MoveWidget(IWidget widget, WidgetLocation widgetLocation)
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
		public void MoveWidget(IWidget widget, int row, int column)
		{
			MoveWidget(widget, row, column, 1, 1);
		}

		/// <inheritdoc />
		public void MoveWidget(IWidget widget, int fromRow, int fromColumn, int rowSpan, int colSpan)
		{
			MoveWidget(widget, new WidgetLocation(fromRow, fromColumn, rowSpan, colSpan));
		}

		/// <inheritdoc />
		public void RemoveSection(ISection section)
		{
			if (section == null)
			{
				throw new ArgumentNullException(nameof(section));
			}

			sections.Remove(section);
			sectionLocations.Remove(section);
		}

		/// <inheritdoc />
		public void RemoveWidget(IWidget widget)
		{
			if (widget == null)
			{
				throw new ArgumentNullException(nameof(widget));
			}

			widgets.Remove(widget);
			widgetLocations.Remove(widget);
		}

		/// <inheritdoc />
		public void SetWidgetsEnabled(bool enabled, bool includeNested)
		{
			foreach (IInteractiveWidget widget in GetWidgets(includeNested).OfType<IInteractiveWidget>())
			{
				widget.IsEnabled = enabled;
			}
		}

		/// <inheritdoc />
		public void SetWidgetsVisible(bool visible, bool includeNested)
		{
			foreach (IWidget widget in GetWidgets(includeNested))
			{
				widget.IsVisible = visible;
			}
		}

		/// <inheritdoc />
		public void ShowWidgets()
		{
			ShowWidgets(true);
		}

		/// <inheritdoc />
		public void ShowWidgets(bool includeNested)
		{
			SetWidgetsVisible(true, true);
		}
	}
}