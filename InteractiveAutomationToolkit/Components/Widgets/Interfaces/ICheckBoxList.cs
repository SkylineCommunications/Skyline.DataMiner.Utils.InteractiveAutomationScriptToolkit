namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System.Collections.Generic;

	/// <summary>
	/// Defines a checkbox list widget with basic operations for managing selected and unselected options.
	/// </summary>
	public interface ICheckBoxList : ICheckBoxListBase, IOptionWidget
	{
		/// <summary>
		/// Gets a collection of strings representing the currently checked options.
		/// </summary>
		IEnumerable<string> Checked { get; }

		/// <summary>
		/// Gets a collection of strings representing the currently unchecked options.
		/// </summary>
		IEnumerable<string> Unchecked { get; }

		/// <summary>
		/// Marks the specified option as checked.
		/// </summary>
		/// <param name="option">The option to check.</param>
		void Check(string option);

		/// <summary>
		/// Marks the specified option as unchecked.
		/// </summary>
		/// <param name="option">The option to uncheck.</param>
		void Uncheck(string option);
	}

	/// <summary>
	/// Defines a generic checkbox list widget with support for typed options and advanced management of selected and unselected options.
	/// </summary>
	/// <typeparam name="T">The type of the value associated with each option.</typeparam>
	public interface ICheckBoxList<T> : ICheckBoxListBase, IOptionWidget<T>
	{
		/// <summary>
		/// Gets a collection of options of type <typeparamref name="T"/> that are currently checked.
		/// </summary>
		IEnumerable<Option<T>> CheckedOptions { get; }

		/// <summary>
		/// Gets a collection of values of type <typeparamref name="T"/> that are currently checked.
		/// </summary>
		IEnumerable<T> Checked { get; }

		/// <summary>
		/// Gets a collection of options of type <typeparamref name="T"/> that are currently unchecked.
		/// </summary>
		IEnumerable<Option<T>> UncheckedOptions { get; }

		/// <summary>
		/// Gets a collection of values of type <typeparamref name="T"/> that are currently unchecked.
		/// </summary>
		IEnumerable<T> Unchecked { get; }

		/// <summary>
		/// Marks the specified option as checked.
		/// </summary>
		/// <param name="option">The option to check.</param>
		void Check(Option<T> option);

		/// <summary>
		/// Marks the option with the specified value as checked.
		/// </summary>
		/// <param name="value">The value of the option to check.</param>
		void Check(T value);

		/// <summary>
		/// Marks the specified option as unchecked.
		/// </summary>
		/// <param name="option">The option to uncheck.</param>
		void Uncheck(Option<T> option);

		/// <summary>
		/// Marks the option with the specified value as unchecked.
		/// </summary>
		/// <param name="value">The value of the option to uncheck.</param>
		void Uncheck(T value);
	}

}