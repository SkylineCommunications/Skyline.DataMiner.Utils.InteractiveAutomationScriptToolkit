namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;

	public class Option<T> : IEquatable<Option<T>>
	{
		public static readonly Option<T> Empty = default;

		public Option(T value) : this(Convert.ToString(value), value)
		{
		}

		public Option(string displayedValue, T value)
		{
			DisplayValue = displayedValue;
			Value = value;
		}

		public string DisplayValue { get; }

		public T Value { get; }

		public bool IsEmpty => Equals(Empty);

		public static bool operator ==(Option<T> left, Option<T> right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Option<T> left, Option<T> right)
		{
			return !Equals(left, right);
		}

		public override bool Equals(object obj)
		{
			return obj is Option<T> option && Equals(option);
		}

		public bool Equals(Option<T> other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (ReferenceEquals(null, other)) return false;

			var thisDisplay = String.IsNullOrEmpty(DisplayValue) ? String.Empty : DisplayValue;
			var otherDisplay = String.IsNullOrEmpty(other.DisplayValue) ? String.Empty : other.DisplayValue;

			return String.Equals(thisDisplay, otherDisplay) &&
				EqualityComparer<T>.Default.Equals(Value, other.Value);
		}

		public override int GetHashCode()
		{
			var normalizedDisplay = String.IsNullOrEmpty(DisplayValue) ? String.Empty : DisplayValue;

			int hashCode = 11;
			hashCode ^= 13 * normalizedDisplay.GetHashCode();
			hashCode ^= 13 * (Value != null ? Value.GetHashCode() : 0);
			return hashCode;
		}

		public override string ToString()
		{
			return $"{DisplayValue} => {Value}";
		}
	}

	public static class Option
	{
		public static Option<T> Create<T>(string displayValue, T value)
		{
			return new Option<T>(displayValue, value);
		}
	}
}
