namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;

	public sealed class Option<T> : IEquatable<Option<T>>
	{
		public Option(T value) : this(value.ToString(), value)
		{
		}

		public Option(string displayedValue, T value)
		{
			if (String.IsNullOrWhiteSpace(displayedValue)) throw new ArgumentException($"Displayed value cannot be null or whitespace");

			DisplayValue = displayedValue;
			Value = value;
		}

		public string DisplayValue { get; }

		public T Value { get; }

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
			if (!(obj is Option<T> other)) return false;
			if (!String.Equals(DisplayValue, other.DisplayValue)) return false;
			if (!Equals(Value, other.Value)) return false;
			return true;
		}

		public bool Equals(Option<T> other)
		{
			if (!String.Equals(DisplayValue, other.DisplayValue)) return false;
			if (!Equals(Value, other.Value)) return false;
			return true;
		}

		public override int GetHashCode()
		{
			int hashCode = 11;
			hashCode ^= 13 * DisplayValue.GetHashCode();
			hashCode ^= 17 * Value.GetHashCode();
			return hashCode;
		}

		public override string ToString()
		{
			return $"{DisplayValue} => {Value}";
		}
	}
}
