namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;

	public readonly struct WidgetLocationPair : IEquatable<WidgetLocationPair>
	{
		public WidgetLocationPair(IWidget widget, WidgetLocation location)
		{
			Widget = widget;
			Location = location;
		}

		public IWidget Widget { get; }

		public WidgetLocation Location { get; }

		public static bool operator ==(WidgetLocationPair left, WidgetLocationPair right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(WidgetLocationPair left, WidgetLocationPair right)
		{
			return !left.Equals(right);
		}

		public bool Equals(WidgetLocationPair other)
		{
			return Equals(Widget, other.Widget) && Location.Equals(other.Location);
		}

		public override bool Equals(object obj)
		{
			return obj is WidgetLocationPair other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Widget != null ? Widget.GetHashCode() : 0) * 397 ^ Location.GetHashCode();
			}
		}
	}
}