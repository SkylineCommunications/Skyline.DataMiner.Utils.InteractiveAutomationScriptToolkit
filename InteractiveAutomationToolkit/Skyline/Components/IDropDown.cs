namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Represents a drop-down list.
	/// </summary>
	public interface IDropDown : IInteractiveWidget
	{
		/// <summary>
		///     Triggered when a different option is selected.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		event EventHandler<DropDown.ChangedEventArgs> Changed;

		/// <summary>
		///     Gets  the possible options.
		/// </summary>
		ICollection<string> Options { get; }

		/// <summary>
		///     Gets or sets a value indicating whether a filter box is available for the drop-down list.
		/// </summary>
		/// <remarks>Available from DataMiner 9.5.6 onwards.</remarks>
		bool IsDisplayFilterShown { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether the options are sorted naturally.
		/// </summary>
		/// <remarks>Available from DataMiner 9.5.6 onwards.</remarks>
		bool IsSorted { get; set; }

		/// <summary>
		///     Gets or sets the selected option.
		///     Will do nothing if the option does not exist.
		/// </summary>
		/// <remarks>Can be <c>null</c>, but only when <see cref="Options" /> is empty.</remarks>
		string Selected { get; set; }

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		/// <exception cref="ArgumentNullException">When the value is <c>null</c>.</exception>
		string Tooltip { get; set; }

		/// <summary>
		///     Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
		///     This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and 10.0.1.0 Main Release.</remarks>
		UIValidationState ValidationState { get; set; }

		/// <summary>
		///     Gets or sets the text that is shown if the validation state is invalid.
		///     This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
		string ValidationText { get; set; }

		/// <summary>
		///     Adds an option to the drop-down list.
		/// </summary>
		/// <param name="option">Option to add.</param>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		void AddOption(string option);

		/// <summary>
		///     Allows setting the selected value to something else than what is available in the options list.
		///     Useful for setting something like "Please select a value".
		/// </summary>
		/// <remarks>This only works in HTML5 (Dashboards, etc.).</remarks>
		/// <param name="selected">String that will appear as selected value even if not available the in options list.</param>
		/// <exception cref="ArgumentNullException">When selected is null.</exception>
		void ForceSelected(string selected);

		/// <summary>
		///     Removes an option from the drop-down list.
		/// </summary>
		/// <param name="option">Option to remove.</param>
		/// <remarks>
		///     If the currently selected option is removed,
		///     <see cref="DropDown.Selected" /> will be set to the first available option.
		///     In case this was the last option, <see cref="DropDown.Selected" /> will be set to <c>null</c>.
		/// </remarks>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		void RemoveOption(string option);

		/// <summary>
		///     Sets the displayed options.
		///     Replaces existing options.
		/// </summary>
		/// <remarks>Will keep the selected option if it is still part of the new set.</remarks>
		/// <param name="optionsToSet">Options to set.</param>
		/// <exception cref="ArgumentNullException">When optionsToSet is null.</exception>
		void SetOptions(IEnumerable<string> optionsToSet);
	}
}