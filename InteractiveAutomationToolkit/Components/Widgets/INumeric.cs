namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;

	/// <summary>
	///     Represents a spinner or numeric up-down control.
	///     Has a slider when the range is limited.
	/// </summary>
	public interface INumeric : IInteractiveWidget, IValidate
	{
		/// <summary>
		///     Triggered when the value of the numeric changes.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		event EventHandler<Numeric.ChangedEventArgs> Changed;

		/// <summary>
		///     Gets or sets the number of decimals to show.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 0.</exception>
		/// <remarks>The default value of this property is 0.</remarks>
		int Decimals { get; set; }

		/// <summary>
		/// 	Gets or sets a value indicating whether it is mandatory to provide a value.
		/// 	If the <see cref="INumeric"/> is empty, the it will have a red border and will display that the field cannot be empty.
		/// 	This is only a visual indicator to the user, so take care to always validate user input.
		/// </summary>
		/// <remarks>This property only works for web compliant scripts or when launched from a DataMiner web app.</remarks>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
		bool IsRequired { get; set; }

		/// <summary>
		///     Gets or sets the maximum value of the range.
		/// </summary>
		/// <remarks>While this restricts the slider and up-down controls, the user is still able to manually input a larger value, so take care to always validate the user input.</remarks>
		/// <remarks>The default value of this property is <c>Double.MaxValue</c>.</remarks>
		/// <exception cref="ArgumentException">When the value is <c>Double.NaN</c> or infinity.</exception>
		double Maximum { get; set; }

		/// <summary>
		///     Gets or sets the minimum value of the range.
		/// </summary>
		/// <remarks>While this restricts the slider and up-down controls, the user is still able to manually input a smaller value, so take care to always validate the user input.</remarks>
		/// <remarks>The default value of this property is <c>Double.MinValue</c>.</remarks>
		/// <exception cref="ArgumentException">When the value is <c>Double.NaN</c> or infinity.</exception>
		double Minimum { get; set; }

		/// <summary>
		///     Gets or sets the step size.
		/// </summary>
		/// <remarks>While this restricts the slider and up-down controls, the user is still able to manually input a value with a different the step size, so take care to always validate the user input.</remarks>
		/// <remarks>The default value of this property is 1.</remarks>
		/// <exception cref="ArgumentException">When the value is <c>Double.NaN</c> or infinity.</exception>
		double StepSize { get; set; }

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		/// <remarks>This property only works for web compliant scripts or when launched from a DataMiner web app.</remarks>
		/// <remarks>Available from DataMiner Feature Release 10.0.8 and Main Release 10.1.0 onwards.</remarks>
		string Tooltip { get; set; }

		/// <summary>
		///     Gets or sets the value of the numeric.
		/// </summary>
		/// <remarks>The value can still lay outside the specified maximum, minimum or step size, take care to always validate the user input.</remarks>
		double Value { get; set; }
	}
}