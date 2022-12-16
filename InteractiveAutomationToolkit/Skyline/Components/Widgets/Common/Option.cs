namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Defines a text/value pair that can be used to easily link displayed text in a widget with the associated value.
	/// </summary>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	public readonly struct Option<TValue> : IEquatable<Option<TValue>>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Option{TValue}"/> struct with the specified text and value.
		/// </summary>
		/// <param name="text">The text associated with <paramref name="value"/>.</param>
		/// <param name="value">The definition associated with <paramref name="text"/>.</param>
		/// <remarks><see cref="Option.Create{TValue}"/> can also be used to create new instances but with the advantage that the generic type can be inferred.</remarks>
		public Option(string text, TValue value)
		{
			Text = text ?? String.Empty;
			Value = value;
		}

		/// <summary>
		/// Gets the text in the text/value pair.
		/// </summary>
		public string Text { get; }

		/// <summary>
		/// Gets the value in the text/value pair.
		/// </summary>
		public TValue Value { get; }

		public static implicit operator Option<TValue>(KeyValuePair<string, TValue> pair)
		{
			return new Option<TValue>(pair.Key, pair.Value);
		}

		public static implicit operator KeyValuePair<string, TValue>(Option<TValue> option)
		{
			return new KeyValuePair<string, TValue>(option.Text, option.Value);
		}

		public static bool operator ==(Option<TValue> left, Option<TValue> right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Option<TValue> left, Option<TValue> right)
		{
			return !left.Equals(right);
		}

		/// <inheritdoc/>
		public bool Equals(Option<TValue> other)
		{
			return Text == other.Text && EqualityComparer<TValue>.Default.Equals(Value, other.Value);
		}

		/// <inheritdoc/>
		public override bool Equals(object obj)
		{
			return obj is Option<TValue> other && Equals(other);
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			unchecked
			{
				return (Text != null ? Text.GetHashCode() : 0) * 397 ^
					EqualityComparer<TValue>.Default.GetHashCode(Value);
			}
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"[{Text},{Value}]";
		}
	}

	/// <summary>
	/// Creates instances of the <see cref="Option{TValue}"/> struct.
	/// </summary>
	public static class Option
	{
		/// <summary>
		/// Creates a new text/value pair instance using the provided values.
		/// </summary>
		/// <param name="text">The text of the new <see cref="Option{TValue}"/> to be created.</param>
		/// <param name="value">the value of the new <see cref="Option{TValue}"/> to be created.</param>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <returns>A text/value pair containing the provided arguments as values.</returns>
		public static Option<TValue> Create<TValue>(string text, TValue value)
		{
			return new Option<TValue>(text, value);
		}
	}
}