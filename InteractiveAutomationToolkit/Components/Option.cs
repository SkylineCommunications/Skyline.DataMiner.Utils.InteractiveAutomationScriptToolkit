namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;

	public class Option<T> : IOption<T>
	{
		public Option(string displayedValue, T value)
		{
			if (String.IsNullOrWhiteSpace(displayedValue)) throw new ArgumentException($"Displayed value cannot be null or whitespace");

			DisplayValue = displayedValue;
			Value = value;
		}

		public string DisplayValue { get; }

		public T Value { get; }

		public override bool Equals(object obj)
		{
			if (!(obj is Option<T> other)) return false;
			if (!string.Equals(DisplayValue, other.DisplayValue)) return false;
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
