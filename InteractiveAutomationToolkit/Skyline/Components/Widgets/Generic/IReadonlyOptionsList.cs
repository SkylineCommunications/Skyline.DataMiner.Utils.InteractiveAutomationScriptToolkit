namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System.Collections.Generic;

	/// <summary>
	/// Represents a unique read-only collection of option objects.
	/// </summary>
	/// <typeparam name="T">The type of the values of the options.</typeparam>
	public interface IReadonlyOptionsList<T> : IReadOnlyList<Option<T>>
	{
		/// <summary>
		/// Determines whether an option with the specified text is in the <see cref="IReadonlyOptionsList{T}"/>.
		/// </summary>
		/// <param name="name">The text to locate int the <see cref="IReadonlyOptionsList{T}"/>.</param>
		/// <returns><c>true</c> if <paramref name="name"/> is found in the <see cref="IReadonlyOptionsList{T}"/>; otherwise, <c>false</c>.</returns>
		bool ContainsName(string name);

		/// <summary>
		/// Determines whether an option with the specified value is in the <see cref="IReadonlyOptionsList{T}"/>.
		/// </summary>
		/// <param name="value">The value to locate int the <see cref="IReadonlyOptionsList{T}"/>.</param>
		/// <returns><c>true</c> if <paramref name="value"/> is found in the <see cref="IReadonlyOptionsList{T}"/>; otherwise, <c>false</c>.</returns>
		bool ContainsValue(T value);

		/// <summary>
		/// Searches or the specified text and returns the zero-based index of the first occurrence within the entire <see cref="IReadonlyOptionsList{T}"/>.
		/// </summary>
		/// <param name="name">The text to locate in the <see cref="IReadonlyOptionsList{T}"/>.</param>
		/// <returns>the zero-based index of the first occurrence within the entire <see cref="IReadonlyOptionsList{T}"/>, if found; otherwise -1.</returns>
		int IndexOfName(string name);

		/// <summary>
		/// Searches or the specified value and returns the zero-based index of the first occurrence within the entire <see cref="IReadonlyOptionsList{T}"/>.
		/// </summary>
		/// <param name="value">The value to locate in the <see cref="IReadonlyOptionsList{T}"/>.</param>
		/// <returns>the zero-based index of the first occurrence within the entire <see cref="IReadonlyOptionsList{T}"/>, if found; otherwise -1.</returns>
		int IndexOfValue(T value);
	}
}