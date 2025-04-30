namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using Skyline.DataMiner.Utils.InteractiveAutomationScript.Components.Widgets.Generics;

	/// <summary>
	///		Represents a radio button list where every option is a string.
	/// </summary>
	public interface IRadioButtonList : IOptionWidget
	{
		/// <summary>
		///		Currently selected option.
		/// </summary>
		string Selected { get; set; }
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
	}
}
