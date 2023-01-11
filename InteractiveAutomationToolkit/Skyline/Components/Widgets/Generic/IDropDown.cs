namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System;

	/// <summary>
	///     Represents a drop-down list.
	/// </summary>
	/// <typeparam name="T">The type of the value of the options.</typeparam>
	public interface IDropDown<T> : IInteractiveWidget, IValidate
	{
		/// <summary>
		///     Triggered when a different option is selected.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		event EventHandler<DropDown<T>.ChangedEventArgs> Changed;

		/// <summary>
		///     Gets  the possible options.
		/// </summary>
		IOptionsList<T> Options { get; }

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
		/// Gets or sets the selected option.
		/// Will do nothing if the option does not exist.
		/// </summary>
		/// <remarks>Can return <c>default</c>, but only when <see cref="Options" /> is empty.</remarks>
		Option<T> Selected { get; set; }

		/// <summary>
		///     Gets or sets the selected option via name.
		///     Will do nothing if the option does not exist.
		/// </summary>
		/// <remarks>Can return <c>null</c>, but only when <see cref="Options" /> is empty.</remarks>
		string SelectedName { get; set; }

		/// <summary>
		///     Gets or sets the selected option via the value.
		///     Will do nothing if the option does not exist.
		/// </summary>
		/// <remarks>Can return <c>default</c>, but only when <see cref="Options" /> is empty.</remarks>
		T SelectedValue { get; set; }

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		string Tooltip { get; set; }

		/// <summary>
		///     Allows setting the selected value to something else than what is available in the options list.
		///     Useful for setting something like "Please select a value".
		/// </summary>
		/// <remarks>This only works in HTML5 (Dashboards, etc.).</remarks>
		/// <param name="selected">String that will appear as selected value even if not available the in options list.</param>
		void ForceSelected(string selected);
	}
}