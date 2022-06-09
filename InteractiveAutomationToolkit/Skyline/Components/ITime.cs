namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;

	using Automation;

	/// <summary>
	///     Represents a widget to show/edit a time duration.
	/// </summary>
	public interface ITime : IInteractiveWidget
	{
		/// <summary>
		///     Triggered when the timespan changes.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		event EventHandler<Time.ChangedEventArgs> Changed;

		/// <summary>
		///     Gets or sets the timespan displayed in the time widget.
		/// </summary>
		TimeSpan TimeSpan { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether the value is clipped to the range.
		///     Default: <c>false</c>
		/// </summary>
		bool ClipValueToRange { get; set; }

		/// <summary>
		///     Gets or sets the number of digits to be used in order to represent the fractions of seconds.
		///     Default: <c>0</c>
		/// </summary>
		int Decimals { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether seconds are displayed in the time widget.
		///     Default: <c>true</c>
		/// </summary>
		bool HasSeconds { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether a spinner button is shown.
		///     Default: <c>true</c>
		/// </summary>
		bool HasSpinnerButton { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether the spinner button is enabled.
		///     Default: <c>true</c>
		/// </summary>
		bool IsSpinnerButtonEnabled { get; set; }

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		/// <exception cref="ArgumentNullException">When the value is <c>null</c>.</exception>
		string Tooltip { get; set; }

		/// <summary>
		///     Gets or sets the maximum timespan.
		///     Default: <c>TimeSpan.MaxValue</c>
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the maximum is smaller than the minimum.</exception>
		TimeSpan Maximum { get; set; }

		/// <summary>
		///     Gets or sets the minimum timespan.
		///     Default: <c>TimeSpan.MinValue</c>
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the minimum is larger than the maximum.</exception>
		TimeSpan Minimum { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether the widget will only trigger an event when the enter key is pressed.
		///     Default: <c>false</c>
		/// </summary>
		bool UpdateOnEnter { get; set; }

		/// <summary>
		///		Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
		///		This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
		UIValidationState ValidationState { get; set; }

		/// <summary>
		///		Gets or sets the text that is shown if the validation state is invalid.
		///		This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
		string ValidationText { get; set; }
	}
}