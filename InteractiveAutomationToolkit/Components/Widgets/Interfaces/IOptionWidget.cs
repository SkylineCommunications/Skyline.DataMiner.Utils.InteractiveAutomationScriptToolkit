namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System.Collections.Generic;

	using Skyline.DataMiner.Utils.InteractiveAutomationScript.Components.Widgets.Generics;

	/// <summary>
	/// Defines the base functionality for a widget that manages a list of options represented as strings.
	/// </summary>
	public interface IOptionWidget
	{
		/// <summary>
		/// Gets the collection of available options as strings.
		/// </summary>
		IEnumerable<string> Options { get; }

		/// <summary>
		/// Replaces the current list of options with the specified collection of strings.
		/// </summary>
		/// <param name="options">The collection of options to set.</param>
		void SetOptions(IEnumerable<string> options);

		/// <summary>
		/// Adds a new option to the list of available options.
		/// </summary>
		/// <param name="option">The option to add.</param>
		void AddOption(string option);

		/// <summary>
		/// Removes the specified option from the list of available options.
		/// </summary>
		/// <param name="option">The option to remove.</param>
		void RemoveOption(string option);
	}

	/// <summary>
	/// Defines a generic widget that manages a list of typed options.
	/// </summary>
	/// <typeparam name="T">The type of the value associated with each option.</typeparam>
	public interface IOptionWidget<T>
	{
		/// <summary>
		/// Gets or sets the collection of available options as <see cref="Option{T}"/> objects.
		/// </summary>
		IEnumerable<Option<T>> Options { get; set; }

		/// <summary>
		/// Gets or sets the collection of available values of type <typeparamref name="T"/>.
		/// </summary>
		IEnumerable<T> Values { get; set; }

		/// <summary>
		/// Replaces the current list of options with the specified collection of <see cref="Option{T}"/> objects.
		/// </summary>
		/// <param name="options">The collection of options to set.</param>
		void SetOptions(IEnumerable<Option<T>> options);

		/// <summary>
		/// Replaces the current list of options with the specified collection of values of type <typeparamref name="T"/>.
		/// </summary>
		/// <param name="options">The collection of values to set.</param>
		void SetOptions(IEnumerable<T> options);

		/// <summary>
		/// Adds a new option to the list of available options.
		/// </summary>
		/// <param name="option">The option to add as an <see cref="Option{T}"/>.</param>
		void AddOption(Option<T> option);

		/// <summary>
		/// Adds a new value to the list of available options.
		/// </summary>
		/// <param name="value">The value to add.</param>
		void AddOption(T value);

		/// <summary>
		/// Removes the specified option from the list of available options.
		/// </summary>
		/// <param name="option">The option to remove as an <see cref="Option{T}"/>.</param>
		void RemoveOption(Option<T> option);

		/// <summary>
		/// Removes all options representing the specified value from the list of available options.
		/// </summary>
		/// <param name="value">The value to remove.</param>
		void RemoveOption(T value);
	}

}