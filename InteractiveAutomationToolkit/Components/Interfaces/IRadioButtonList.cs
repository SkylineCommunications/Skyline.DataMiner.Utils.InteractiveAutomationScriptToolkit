namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	/// <summary>
	///		Represents a radio button list where every option is a string.
	/// </summary>
	public interface IRadioButtonList : IOptionWidget
	{
		/// <summary>
		///		Currently selected option.
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
	///		Represents a radio button list where every option is an generic instance.
	/// </summary>
	/// <typeparam name="T">Type of generic instance.</typeparam>
	public interface IRadioButtonList<T> : IOptionWidget<T>
	{
		/// <summary>
		///		Currently selected option.
		/// </summary>
		Option<T> SelectedOption { get; set; }

		/// <summary>
		///		Value of the currently selected option.
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
