namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Creates instances of the <see cref="Option{TValue}"/> struct.
	/// </summary>
	public static class Option
	{
		/// <summary>
		/// Creates a new name/value pair instance using the provided parameters.
		/// </summary>
		/// <param name="name">The name of the new <see cref="Option{TValue}"/> to be created.</param>
		/// <param name="value">the value of the new <see cref="Option{TValue}"/> to be created.</param>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <returns>A name/value pair containing the provided arguments as values.</returns>
		public static Option<TValue> Create<TValue>(string name, TValue value)
		{
			return new Option<TValue>(name, value);
		}
	}

	/// <summary>
	/// Defines a name/value pair that can be used to easily link displayed name in a widget with the associated value.
	/// </summary>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	public class Option<TValue> : IEquatable<Option<TValue>>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Option{TValue}"/> class.
		/// </summary>
		/// <param name="name">The name associated with <paramref name="value"/>.</param>
		/// <param name="value">The definition associated with <paramref name="name"/>.</param>
		/// <remarks><see cref="Option.Create{TValue}"/> can also be used to create new instances but with the advantage that the generic type can be inferred.</remarks>
		public Option(string name, TValue value)
		{
			Name = name;
			Value = value;
		}

		/// <summary>
		/// Gets the name in the name/value pair.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Gets the value in the name/value pair.
		/// </summary>
		public TValue Value { get; }

		public static implicit operator Option<TValue>(KeyValuePair<string, TValue> pair)
		{
			return new Option<TValue>(pair.Key, pair.Value);
		}

		public static implicit operator KeyValuePair<string, TValue>(Option<TValue> option)
		{
			return new KeyValuePair<string, TValue>(option.Name, option.Value);
		}

		public static bool operator ==(Option<TValue> left, Option<TValue> right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Option<TValue> left, Option<TValue> right)
		{
			return !Equals(left, right);
		}

		/// <inheritdoc/>
		public bool Equals(Option<TValue> other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return Name == other.Name && EqualityComparer<TValue>.Default.Equals(Value, other.Value);
		}

		/// <inheritdoc/>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (obj.GetType() != GetType())
			{
				return false;
			}

			return Equals((Option<TValue>)obj);
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			unchecked
			{
				return (Name != null ? Name.GetHashCode() : 0) * 397 ^
					EqualityComparer<TValue>.Default.GetHashCode(Value);
			}
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"[{Name},{Value}]";
		}
	}
}