namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	///     Represents a group of radio buttons.
	/// </summary>
	public interface IRadioButtonList : IInteractiveWidget
	{
		/// <summary>
		///     Triggered when a different option is selected.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		event EventHandler<RadioButtonList.ChangedEventArgs> Changed;

		/// <summary>
		///     Gets all options.
		/// </summary>
		ICollection<string> Options { get; }

		/// <summary>
		///     Gets or sets a value indicating whether the options are sorted naturally.
		/// </summary>
		/// <remarks>Available from DataMiner 9.5.6 onwards.</remarks>
		bool IsSorted { get; set; }

		/// <summary>
		///     Gets or sets the selected option.
		///     Will do nothing if the option does not exist.
		/// </summary>
		/// <remarks><c>null</c> is allowed to show nothing is selected.</remarks>
		string Selected { get; set; }

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		string Tooltip { get; set; }

		/// <summary>
		///     Adds a radio button to the group.
		/// </summary>
		/// <param name="option">Option to add.</param>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		void AddOption(string option);

		/// <summary>
		///     Removes an option from the radio button list.
		/// </summary>
		/// <remarks>
		///     If the currently selected option is removed,
		///     <see cref="RadioButtonList.Selected" /> will be set to <c>null</c>.
		/// </remarks>
		/// <param name="option">Option to remove.</param>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		void RemoveOption(string option);

		/// <summary>
		///     Sets the displayed options.
		///     Replaces existing options.
		/// </summary>
		/// <param name="optionsToSet">Options to set.</param>
		/// <exception cref="ArgumentNullException">When optionsToSet is null.</exception>
		void SetOptions(IEnumerable<string> optionsToSet);
	}
}