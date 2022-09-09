namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Represents a drop-down list.
	/// </summary>
	/// <typeparam name="T">The type of the value of the options.</typeparam>
	public interface IDropDown<T> : IInteractiveWidget
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
		///     Gets or sets the selected option.
		///     Will do nothing if the option does not exist.
		/// </summary>
		/// <remarks>Can be <c>null</c>, but only when <see cref="Options" /> is empty.</remarks>
		string SelectedText { get; set; }

		/// <summary>
		///     Gets or sets the selected option.
		///     Will do nothing if the option does not exist.
		/// </summary>
		/// <remarks>Can be <c>default</c>, but only when <see cref="Options" /> is empty.</remarks>
		T SelectedValue { get; set; }

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		string Tooltip { get; set; }

		/// <summary>
		///     Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
		///     This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and 10.0.1.0 Main Release.</remarks>
		/// <exception cref="InvalidEnumArgumentException">When <paramref name="value"/> does not specify a valid member of <see cref="UIValidationState"/>.</exception>
		UIValidationState ValidationState { get; set; }

		/// <summary>
		///     Gets or sets the text that is shown if the validation state is invalid.
		///     This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
		string ValidationText { get; set; }

		/// <summary>
		///     Allows setting the selected value to something else than what is available in the options list.
		///     Useful for setting something like "Please select a value".
		/// </summary>
		/// <remarks>This only works in HTML5 (Dashboards, etc.).</remarks>
		/// <param name="selected">String that will appear as selected value even if not available the in options list.</param>
		void ForceSelected(string selected);
	}

	public interface IOptionsList<T> : IList<KeyValuePair<string, T>>, IReadOnlyList<KeyValuePair<string, T>>
	{
		void Add(string text, T value);

		void AddRange(IEnumerable<KeyValuePair<string, T>> options);

		bool ContainsText(string text);

		bool ContainsValue(T value);

		int IndexOfText(string text);

		int IndexOfValue(T value);

		void Insert(int index, string text, T value);

		bool RemoveText(string text);

		bool RemoveValue(T value);
	}
}