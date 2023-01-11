namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <inheritdoc cref="IPanel" />
	public abstract class Panel : Component, IPanel
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

		/// <inheritdoc/>
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
			foreach (InteractiveWidget widget in GetWidgets(includeNested).OfType<InteractiveWidget>())
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

		/// <summary>
		/// Sets the <see cref="IComponent.Parent"/> property of <paramref name="component"/> to <c>null</c>.
		/// </summary>
		/// <param name="component">A component that is going to be removed from this panel.</param>
		protected static void RemoveParentFrom(IComponent component)
		{
			component.Parent = null;
		}

		/// <summary>
		/// Sets the <see cref="IComponent.Parent"/> property of <paramref name="component"/> to <c>this</c>.
		/// </summary>
		/// <param name="component">A component that is going to be added to this panel.</param>
		/// <exception cref="InvalidOperationException">When trying to add a component more than once.</exception>
		/// <exception cref="InvalidOperationException">When trying to add a component recursively.</exception>
		protected void AddParentTo(IComponent component)
		{
			if (component.Parent != null)
			{
				throw new InvalidOperationException("Component cannot be added to more than once.");
			}

			if (component == this)
			{
				throw new InvalidOperationException("Panel cannot be added to itself.");
			}

			if (WalkParents(this).Any(c => c == component))
			{
				throw new InvalidOperationException("Panel cannot recurse.");
			}

			component.Parent = this;
		}

		private static IEnumerable<IComponent> WalkParents(IComponent component)
		{
			if (component.Parent == null)
			{
				yield break;
			}

			yield return component.Parent;

			foreach (IComponent parent in WalkParents(component.Parent))
			{
				yield return parent;
			}
		}
	}
}