namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;

	/// <summary>
	/// A WidgetLocationPair holds a <see cref="IWidget"/> and a <see cref="WidgetLocation"/>.
	/// </summary>
	public readonly struct WidgetLocationPair : IEquatable<WidgetLocationPair>
	{
		/// <summary>
		/// 	Initializes a new instance of the <see cref="WidgetLocationPair"/> structure with the specified widget
		/// 	and location.
		/// </summary>
		/// <param name="widget">The widget in the widget/location pair.</param>
		/// <param name="location">The location in the widget/location pair.</param>
		public WidgetLocationPair(IWidget widget, WidgetLocation location)
		{
			Widget = widget;
			Location = location;
		}

		/// <summary>
		/// 	Gets the widget in the widget/location pair.
		/// </summary>
		public IWidget Widget { get; }

		/// <summary>
		/// 	Gets the location in the widget/location pair.
		/// </summary>
		public WidgetLocation Location { get; }

		public static bool operator ==(WidgetLocationPair left, WidgetLocationPair right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(WidgetLocationPair left, WidgetLocationPair right)
		{
			return !left.Equals(right);
		}

		/// <inheritdoc/>
		public bool Equals(WidgetLocationPair other)
		{
			return Equals(Widget, other.Widget) && Location.Equals(other.Location);
		}

		/// <inheritdoc/>
		public override bool Equals(object obj)
		{
			return obj is WidgetLocationPair other && Equals(other);
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			unchecked
			{
				return (Widget != null ? Widget.GetHashCode() : 0) * 397 ^ Location.GetHashCode();
			}
		}
	}
}