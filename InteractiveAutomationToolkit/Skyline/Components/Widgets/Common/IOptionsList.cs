namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System.Collections.Generic;

	/// <summary>
	/// Represents a unique collection of option objects.
	/// </summary>
	/// <typeparam name="T">The type of the values of the options.</typeparam>
	public interface IOptionsList<T> : IList<Option<T>>
	{
		/// <summary>
		/// Adds an option to the <see cref="IOptionsList{T}"/>with the specified text and value.
		/// </summary>
		/// <param name="text">The text in the text/value pair.</param>
		/// <param name="value">The value in the text/value pair.</param>
		void Add(string text, T value);

		/// <summary>
		/// Adds the options of the specified collection to the end of the <see cref="IOptionsList{T}"/>.
		/// </summary>
		/// <param name="options">The collection whose options of the specified collection to the end of the <see cref="IOptionsList{T}"/>.</param>
		void AddRange(IEnumerable<Option<T>> options);

		/// <summary>
		/// Determines whether an option with the specified text is in the <see cref="IOptionsList{T}"/>.
		/// </summary>
		/// <param name="text">The text to locate int the <see cref="IOptionsList{T}"/>.</param>
		/// <returns><c>true</c> if <paramref name="text"/> is found in the <see cref="IOptionsList{T}"/>; otherwise, <c>false</c>.</returns>
		bool ContainsText(string text);

		/// <summary>
		/// Determines whether an option with the specified value is in the <see cref="IOptionsList{T}"/>.
		/// </summary>
		/// <param name="value">The value to locate int the <see cref="IOptionsList{T}"/>.</param>
		/// <returns><c>true</c> if <paramref name="value"/> is found in the <see cref="IOptionsList{T}"/>; otherwise, <c>false</c>.</returns>
		bool ContainsValue(T value);

		/// <summary>
		/// Searches or the specified text and returns the zero-based index of the first occurrence within the entire <see cref="IOptionsList{T}"/>.
		/// </summary>
		/// <param name="text">The text to locate in the <see cref="IOptionsList{T}"/>.</param>
		/// <returns>the zero-based index of the first occurrence within the entire <see cref="IOptionsList{T}"/>, if found; otherwise -1.</returns>
		int IndexOfText(string text);

		/// <summary>
		/// Searches or the specified value and returns the zero-based index of the first occurrence within the entire <see cref="IOptionsList{T}"/>.
		/// </summary>
		/// <param name="value">The value to locate in the <see cref="IOptionsList{T}"/>.</param>
		/// <returns>the zero-based index of the first occurrence within the entire <see cref="IOptionsList{T}"/>, if found; otherwise -1.</returns>
		int IndexOfValue(T value);

		/// <summary>
		/// Inserts an option with the specified text and value to the <see cref="IOptionsList{T}"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which the option should be inserted.</param>
		/// <param name="text">The text in the text/value pair.</param>
		/// <param name="value">The value in the text/value pair.</param>
		void Insert(int index, string text, T value);

		/// <summary>
		/// Removes the first occurrence of an option with specified text from the <see cref="IOptionsList{T}"/>.
		/// </summary>
		/// <param name="text">The option with the specified text to remove from the <see cref="IOptionsList{T}"/>.</param>
		/// <returns><c>true</c> if the option is successfully removed; otherwise, <c>false</c>. This method also returns <c>false</c> if the option was not found.</returns>
		bool RemoveText(string text);

		/// <summary>
		/// Removes the first occurrence of an option with specified value from the <see cref="IOptionsList{T}"/>.
		/// </summary>
		/// <param name="value">The option with the specified value to remove from the <see cref="IOptionsList{T}"/>.</param>
		/// <returns><c>true</c> if the option is successfully removed; otherwise, <c>false</c>. This method also returns <c>false</c> if the option was not found.</returns>
		bool RemoveValue(T value);
	}
}