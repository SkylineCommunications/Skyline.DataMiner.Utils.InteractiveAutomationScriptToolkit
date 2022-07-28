namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	///     A panel is a special component that can be used to group widgets together.
	/// </summary>
	public abstract class Panel : IPanel
	{
		/// <inheritdoc />
		public virtual int GetColumnCount()
		{
			try
			{
				return GetWidgetLocationPairs()
					.Max(pair => pair.Location.Column + (pair.Location.ColumnSpan - 1) + 1);
			}
			catch (InvalidOperationException)
			{
				return 0;
			}
		}

		/// <inheritdoc />
		public virtual int GetRowCount()
		{
			try
			{
				return GetWidgetLocationPairs()
					.Max(pair => pair.Location.Row + (pair.Location.RowSpan - 1) + 1);
			}
			catch (InvalidOperationException)
			{
				return 0;
			}
		}

		public abstract IEnumerable<WidgetLocationPair> GetWidgetLocationPairs();

		/// <inheritdoc />
		public abstract void Clear();

		/// <inheritdoc />
		public void DisableWidgets()
		{
			DisableWidgets(true);
		}

		/// <inheritdoc />
		public void DisableWidgets(bool includeNested)
		{
			SetWidgetsEnabled(false, includeNested);
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
		public IEnumerable<IPanel> GetPanels()
		{
			return GetPanels(true);
		}

		/// <inheritdoc />
		public abstract IEnumerable<IPanel> GetPanels(bool includeNested);

		/// <inheritdoc />
		public IEnumerable<IWidget> GetWidgets()
		{
			return GetWidgets(true);
		}

		/// <inheritdoc />
		public abstract IEnumerable<IWidget> GetWidgets(bool includeNested);

		/// <inheritdoc />
		public void HideWidgets()
		{
			HideWidgets(true);
		}

		/// <inheritdoc />
		public void HideWidgets(bool includeNested)
		{
			SetWidgetsVisible(false, includeNested);
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
			SetWidgetsVisible(true, includeNested);
		}

		protected void ValidatePanel(IPanel panel)
		{
			if (panel == this)
			{
				throw new ArgumentException("Cannot add a panel to itself.");
			}

			if (GetPanels().Contains(panel) || panel.GetPanels().Contains(this))
			{
				throw new ArgumentException("Panel is already added to the component or nested components.");
			}

			if (GetWidgets().Intersect(panel.GetWidgets()).Any())
			{
				throw new ArgumentException(
					"Panel contains widgets that are already part of this component or nested components.");
			}
		}

		protected void ValidateWidget(IWidget widget)
		{
			if (GetWidgets().Contains(widget))
			{
				throw new ArgumentException("Widget is already added.");
			}
		}
	}
}