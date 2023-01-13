namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	///     Represents a group of radio buttons.
	/// </summary>
	/// <typeparam name="T">The type of the value of the options.</typeparam>
	public interface IRadioButtonList<T> : IInteractiveWidget, IValidate
	{
		/// <summary>
		///     Triggered when a different option is selected.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		event EventHandler<RadioButtonList<T>.ChangedEventArgs> Changed;

		/// <summary>
		///     Gets all options.
		/// </summary>
		IList<Option<T>> Options { get; }

		/// <summary>
		///     Gets or sets a value indicating whether the options are sorted naturally.
		/// </summary>
		/// <remarks>Available from DataMiner 9.5.6 onwards.</remarks>
		bool IsSorted { get; set; }

		/// <summary>
		///     Gets or sets the selected option.
		///     Will do nothing if the option does not exist.
		/// </summary>
		/// <remarks><c>default</c> is allowed to show nothing is selected.</remarks>
		Option<T> Selected { get; set; }

		/// <summary>
		///     Gets or sets the selected option via the name.
		///     Will do nothing if the option does not exist.
		/// </summary>
		/// <remarks><c>null</c> is allowed to show nothing is selected.</remarks>
		string SelectedName { get; set; }

		/// <summary>
		///     Gets or sets the selected option via the value.
		///     Will do nothing if the option does not exist.
		/// </summary>
		/// <remarks><c>default</c> is allowed to show nothing is selected.</remarks>
		T SelectedValue { get; set; }

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		string Tooltip { get; set; }
	}
}