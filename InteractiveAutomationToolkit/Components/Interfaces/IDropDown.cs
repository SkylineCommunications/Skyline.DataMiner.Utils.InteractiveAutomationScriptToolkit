namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	/// <summary>
	/// Defines a dropdown widget with a single selected option.
	/// </summary>
	public interface IDropDown : IOptionWidget
	{
		/// <summary>
		/// Gets or sets the currently selected option as a string.
		/// </summary>
		string Selected { get; set; }

		/// <summary>
		/// Attempts to select the specified option. If the option does not exist in the collection, the selection remains unchanged.
		/// </summary>
		/// <param name="option">The option to select.</param>
		/// <returns><c>true</c> if the option was found and selected; otherwise, <c>false</c>.</returns>
		bool TrySelectOption(string option);
	}

	/// <summary>
	/// Defines a generic dropdown widget with a single selected option of a specified type.
	/// </summary>
	/// <typeparam name="T">The type of the value associated with each option in the dropdown.</typeparam>
	public interface IDropDown<T> : IOptionWidget<T>
	{
		/// <summary>
		/// Gets or sets the currently selected option as an <see cref="Option{T}"/>.
		/// </summary>
		Option<T> SelectedOption { get; set; }

		/// <summary>
		/// Gets or sets the value of the currently selected option.
		/// </summary>
		T Selected { get; set; }

		/// <summary>
		/// Attempts to select the option with the specified value. If no matching option exists, the selection remains unchanged.
		/// </summary>
		/// <param name="value">The value of the option to select.</param>
		/// <returns><c>true</c> if a matching option was found and selected; otherwise, <c>false</c>.</returns>
		bool TrySelectOption(T value);

		/// <summary>
		/// Attempts to select the specified option. If the option does not exist in the collection, the selection remains unchanged.
		/// </summary>
		/// <param name="option">The option to select.</param>
		/// <returns><c>true</c> if the option was found and selected; otherwise, <c>false</c>.</returns>
		bool TrySelectOption(Option<T> option);
	}

}