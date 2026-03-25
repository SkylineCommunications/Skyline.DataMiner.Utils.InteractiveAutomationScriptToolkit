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
		/// Attempts to select the specified option. If the option does not exist in the collection, the selection remains unchanged.
		/// </summary>
		bool TrySelectOption(T value);

		/// <summary>
		/// Attempts to select the specified option. If the option does not exist in the collection, the selection remains unchanged.
		/// </summary>
		bool TrySelectOption(Option<T> option);
	}

}