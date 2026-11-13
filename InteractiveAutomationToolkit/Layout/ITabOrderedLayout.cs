namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	/// <summary>
	/// Exposes an explicit tab order for a widget or section layout. A value of zero means no explicit order.
	/// </summary>
	public interface ITabOrderedLayout
	{
		/// <summary>
		///     Gets or sets the tab order used by dialogs in Grid layout.
		/// </summary>
		int TabOrder { get; set; }
	}
}
