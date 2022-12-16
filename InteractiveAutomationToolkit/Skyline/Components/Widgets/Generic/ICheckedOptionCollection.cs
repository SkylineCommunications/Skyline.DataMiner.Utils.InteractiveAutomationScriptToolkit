namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System.Collections.Generic;

	/// <summary>
	/// Collection of checked options in this <see cref="ICheckBoxList{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the value of the options.</typeparam>
	public interface ICheckedOptionCollection<T> : ICollection<Option<T>>
	{
		/// <summary>
		/// Checks the option with specified text.
		/// </summary>
		/// <param name="text">The option with specified text to be checked.</param>
		void AddText(string text);

		/// <summary>
		/// Checks the option with specified value.
		/// </summary>
		/// <param name="value">The options with specified value to be checked.</param>
		void AddValue(T value);

		/// <summary>
		/// Checks all options of the specified collection.
		/// </summary>
		/// <param name="options">The collections of options to be checked.</param>
		void AddRange(IEnumerable<Option<T>> options);

		/// <summary>
		/// Checks all options with the specified texts.
		/// </summary>
		/// <param name="texts">The collections of texts of options to be checked.</param>
		void AddTextRange(IEnumerable<string> texts);

		/// <summary>
		/// Checks all options with the specified values.
		/// </summary>
		/// <param name="values">The collections of values of options to be checked.</param>
		void AddValueRange(IEnumerable<T> values);

		/// <summary>
		/// Determines whether an option with specified text is checked.
		/// </summary>
		/// <param name="text">The option with the specified text to determine if checked.</param>
		/// <returns><c>true</c> if the option is checked; otherwise <c>false</c>.</returns>
		bool ContainsText(string text);

		/// <summary>
		/// Determines whether an option with specified value is checked.
		/// </summary>
		/// <param name="value">The option with the specified value to determine if checked.</param>
		/// <returns><c>true</c> if the option is checked; otherwise <c>false</c>.</returns>
		bool ContainsValue(T value);

		/// <summary>
		/// Unchecks the option with specified text.
		/// </summary>
		/// <param name="text">The options with the specified text to uncheck.</param>
		void RemoveText(string text);

		/// <summary>
		/// Unchecks the option with specified value.
		/// </summary>
		/// <param name="value">The option with the specified value to uncheck.</param>
		void RemoveValue(T value);
	}
}