namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;

	using Automation;

	public interface ICheckBoxList : IInteractiveWidget
	{
		/// <summary>
		///     Triggered when the state of a checkbox changes.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		event EventHandler<CheckBoxList.CheckBoxListChangedEventArgs> Changed;

		/// <summary>
		///     Gets all selected options.
		/// </summary>
		ICollection<string> Checked { get; }

		/// <summary>
		///     Gets or sets a value indicating whether the options are sorted naturally.
		/// </summary>
		/// <remarks>Available from DataMiner 9.5.6 onwards.</remarks>
		bool IsSorted { get; set; }

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		/// <exception cref="ArgumentNullException">When the value is <c>null</c>.</exception>
		string Tooltip { get; set; }

		/// <summary>
		///     Gets all options.
		/// </summary>
		ICollection<string> Options { get; }

		/// <summary>
		///     Gets all options that are not selected.
		/// </summary>
		ICollection<string> Unchecked { get; }

		/// <summary>
		///		Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
		///		This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner 10.0.5 onwards.</remarks>
		UIValidationState ValidationState { get; set; }

		/// <summary>
		///		Gets or sets the text that is shown if the validation state is invalid.
		///		This should be used by the client to add a visual marker on the input field.
		///		The validation text is not displayed for a checkbox list, but if this value is not explicitly set, the validation state will have no influence on the way the component is displayed.
		/// </summary>
		/// <remarks>Available from DataMiner 10.0.5 onwards.</remarks>
		string ValidationText { get; set; }

		/// <summary>
		///     Adds an option to the checkbox list.
		/// </summary>
		/// <param name="option">Option to add.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		void AddOption(string option);

		/// <summary>
		///     Selects an option.
		/// </summary>
		/// <param name="option">Option to be selected.</param>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		/// <exception cref="ArgumentException">When the option does not exist.</exception>
		void Check(string option);

		/// <summary>
		///     Selects all options.
		/// </summary>
		void CheckAll();

		/// <summary>
		///     Sets the displayed options.
		///     Replaces existing options.
		/// </summary>
		/// <param name="options">Options to set.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		void SetOptions(IEnumerable<string> options);

		/// <summary>
		/// 	Removes an option from the checkbox list.
		/// </summary>
		/// <param name="option">Option to remove.</param>
		/// <exception cref="NullReferenceException">When option is null.</exception>
		void RemoveOption(string option);

		/// <summary>
		///     Clears an option.
		/// </summary>
		/// <param name="option">Option to be cleared.</param>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		/// <exception cref="ArgumentException">When the option does not exist.</exception>
		void Uncheck(string option);

		/// <summary>
		///     Clears all options.
		/// </summary>
		void UncheckAll();
	}
}