namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	///     Represents a list of checkboxes.
	/// </summary>
	public interface ICheckBoxList : IInteractiveWidget
	{
		/// <summary>
		///     Triggered when the state of a checkbox changes.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		event EventHandler<CheckBoxList.ChangedEventArgs> Changed;

		/// <summary>
		///     Gets all selected options.
		/// </summary>
		[Obsolete("Use CheckedOptions instead.")]
		ICollection<string> Checked { get; }

		/// <summary>
		/// 	Gets all selected options.
		/// </summary>
		ICollection<string> CheckedOptions { get; }

		/// <summary>
		///     Gets all options.
		/// </summary>
		IList<string> Options { get; }

		/// <summary>
		///     Gets or sets a value indicating whether the options are sorted naturally.
		/// </summary>
		/// <remarks>Available from DataMiner 9.5.6 onwards.</remarks>
		bool IsSorted { get; set; }

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		/// <remarks>This property only works for web compliant scripts or when launched from a DataMiner web app.</remarks>
		/// <remarks>Available from DataMiner Feature Release 10.0.8 and Main Release 10.1.0 onwards.</remarks>
		string Tooltip { get; set; }

		/// <summary>
		///     Adds an option to the checkbox list.
		/// </summary>
		/// <param name="option">Option to add.</param>
		[Obsolete("Call Options.Add instead.")]
		void AddOption(string option);

		/// <summary>
		///     Selects an option.
		/// </summary>
		/// <param name="option">Option to be selected.</param>
		/// <exception cref="ArgumentException">When the option does not exist.</exception>
		void Check(string option);

		/// <summary>
		///     Selects all options.
		/// </summary>
		void CheckAll();

		/// <summary>
		///     Removes an option from the checkbox list.
		/// </summary>
		/// <param name="option">Option to remove.</param>
		[Obsolete("Call Options.Remove instead.")]
		void RemoveOption(string option);

		/// <summary>
		///     Sets the displayed options.
		///     Replaces existing options.
		/// </summary>
		/// <param name="options">Options to set.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		void SetOptions(IEnumerable<string> options);

		/// <summary>
		///     Clears an option.
		/// </summary>
		/// <param name="option">Option to be cleared.</param>
		/// <exception cref="ArgumentException">When the option does not exist.</exception>
		void Uncheck(string option);

		/// <summary>
		///     Clears all options.
		/// </summary>
		void UncheckAll();
	}
}