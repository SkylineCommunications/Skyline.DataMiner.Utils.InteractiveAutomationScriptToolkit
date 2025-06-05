namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	/// <summary>
	/// Defines the base functionality for a radio button list widget, including properties for state and appearance management.
	/// </summary>
	public interface IRadioButtonListBase : IIsReadonlyWidget
	{
		/// <summary>
		/// Gets or sets a value indicating whether the options in the radio button list are sorted.
		/// </summary>
		bool IsSorted { get; set; }

		/// <summary>
		/// Gets or sets the tooltip text associated with the radio button list.
		/// </summary>
		string Tooltip { get; set; }
	}
}