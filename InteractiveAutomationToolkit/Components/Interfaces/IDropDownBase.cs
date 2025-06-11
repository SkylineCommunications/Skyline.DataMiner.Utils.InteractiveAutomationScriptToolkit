namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using Skyline.DataMiner.Automation;

	public interface IDropDownBase : IValidationWidget, IIsReadonlyWidget
	{
		/// <summary>
		///     Gets or sets a value indicating whether a filter box is available for the drop-down list.
		/// </summary>
		bool IsDisplayFilterShown { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether the options are sorted naturally.
		/// </summary>
		bool IsSorted { get; set; }

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		/// <exception cref="ArgumentNullException">When the value is <c>null</c>.</exception>
		string Tooltip { get; set; }
	}
}