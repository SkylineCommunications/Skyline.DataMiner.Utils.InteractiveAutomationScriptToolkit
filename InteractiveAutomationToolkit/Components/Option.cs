namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;

	public sealed class Option<T> : IEquatable<Option<T>>
	{
		public static readonly Option<T> Empty = new Option<T>(String.Empty, default);

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
			if (other is null) return false;

			var thisDisplay = NormalizeDisplayValue(DisplayValue);
			var otherDisplay = NormalizeDisplayValue(other.DisplayValue);

			return String.Equals(thisDisplay, otherDisplay) &&
				EqualityComparer<T>.Default.Equals(Value, other.Value);
		}

		public override int GetHashCode()
		{
			var normalizedDisplay = NormalizeDisplayValue(DisplayValue);

			int hashCode = 11;
			hashCode ^= 13 * normalizedDisplay.GetHashCode();
			hashCode ^= 13 * (Value != null ? Value.GetHashCode() : 0);
			return hashCode;
		}

		public override string ToString()
		{
			return $"{DisplayValue} => {Value}";
		}

		private static string NormalizeDisplayValue(string displayValue)
		{
			return String.IsNullOrEmpty(displayValue) ? String.Empty : displayValue;
		}
	}

	public static class Option
	{
		public static Option<T> Create<T>(string displayValue, T value)
		{
			return new Option<T>(displayValue, value);
		}

		public static Option<T> Empty<T>()
		{
			return Option<T>.Empty;
		}
	}
}
