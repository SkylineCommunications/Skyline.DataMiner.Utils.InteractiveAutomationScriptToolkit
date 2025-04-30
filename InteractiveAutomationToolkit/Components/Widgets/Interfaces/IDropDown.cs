namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using Skyline.DataMiner.Utils.InteractiveAutomationScript.Components.Widgets.Generics;

	/// <summary>
	/// Defines a dropdown widget with a single selected option.
	/// </summary>
	public interface IDropDown : IOptionWidget
	{
		/// <summary>
		/// Gets or sets the currently selected option as a string.
		/// </summary>
		string Selected { get; set; }
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
	}

}