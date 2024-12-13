namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using Skyline.DataMiner.Automation;

	/// <summary>
	///		Defines a widget supporting visual validation.
	/// </summary>
	public interface IValidationWidget
	{
		/// <summary>
		/// 	Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
		/// </summary>
		UIValidationState ValidationState { get; set; }

		/// <summary>
		/// 	Gets or sets the text that is shown if the validation state is invalid.
		/// </summary>
		string ValidationText { get; set; }
	}
}
