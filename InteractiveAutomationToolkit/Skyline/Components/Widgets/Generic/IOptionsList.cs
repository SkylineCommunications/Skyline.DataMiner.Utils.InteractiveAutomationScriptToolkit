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
		/// Adds an option to the <see cref="IOptionsList{T}"/>with the specified name and value.
		/// </summary>
		/// <param name="name">The name associated with the value.</param>
		/// <param name="value">The value in the name/value pair.</param>
		void Add(string name, T value);

		/// <summary>
		/// Adds the options of the specified collection to the end of the <see cref="IOptionsList{T}"/>.
		/// </summary>
		/// <param name="options">The collection whose options of the specified collection to the end of the <see cref="IOptionsList{T}"/>.</param>
		void AddRange(IEnumerable<Option<T>> options);

		/// <summary>
		/// Determines whether an option with the specified name is in the <see cref="IOptionsList{T}"/>.
		/// </summary>
		/// <param name="name">The name to locate int the <see cref="IOptionsList{T}"/>.</param>
		/// <returns><c>true</c> if <paramref name="name"/> is found in the <see cref="IOptionsList{T}"/>; otherwise, <c>false</c>.</returns>
		bool ContainsName(string name);

		/// <summary>
		/// Determines whether an option with the specified value is in the <see cref="IOptionsList{T}"/>.
		/// </summary>
		/// <param name="value">The value to locate int the <see cref="IOptionsList{T}"/>.</param>
		/// <returns><c>true</c> if <paramref name="value"/> is found in the <see cref="IOptionsList{T}"/>; otherwise, <c>false</c>.</returns>
		bool ContainsValue(T value);

		/// <summary>
		/// Searches for the specified name and returns the zero-based index of the first occurrence within the entire <see cref="IOptionsList{T}"/>.
		/// </summary>
		/// <param name="name">The name to locate in the <see cref="IOptionsList{T}"/>.</param>
		/// <returns>the zero-based index of the first occurrence within the entire <see cref="IOptionsList{T}"/>, if found; otherwise -1.</returns>
		int IndexOfName(string name);

		/// <summary>
		/// Searches for the specified value and returns the zero-based index of the first occurrence within the entire <see cref="IOptionsList{T}"/>.
		/// </summary>
		/// <param name="value">The value to locate in the <see cref="IOptionsList{T}"/>.</param>
		/// <returns>the zero-based index of the first occurrence within the entire <see cref="IOptionsList{T}"/>, if found; otherwise -1.</returns>
		int IndexOfValue(T value);

		/// <summary>
		/// Inserts an option with the specified name and value to the <see cref="IOptionsList{T}"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which the option should be inserted.</param>
		/// <param name="name">The name associated with the value.</param>
		/// <param name="value">The value in the name/value pair.</param>
		void Insert(int index, string name, T value);

		/// <summary>
		/// Removes the first occurrence of an option with specified name from the <see cref="IOptionsList{T}"/>.
		/// </summary>
		/// <param name="name">The option with the specified name to remove from the <see cref="IOptionsList{T}"/>.</param>
		/// <returns><c>true</c> if the option is successfully removed; otherwise, <c>false</c>. This method also returns <c>false</c> if the option was not found.</returns>
		bool RemoveName(string name);

		/// <summary>
		/// Removes the first occurrence of an option with specified value from the <see cref="IOptionsList{T}"/>.
		/// </summary>
		/// <param name="value">The option with the specified value to remove from the <see cref="IOptionsList{T}"/>.</param>
		/// <returns><c>true</c> if the option is successfully removed; otherwise, <c>false</c>. This method also returns <c>false</c> if the option was not found.</returns>
		bool RemoveValue(T value);
	}
}