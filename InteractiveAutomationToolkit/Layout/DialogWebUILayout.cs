namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	/// <summary>
	/// Specifies how a dialog is displayed in DataMiner Web.
	/// </summary>
	public enum DialogWebUILayout
	{
		/// <summary>
		/// Uses the default layout, which defaults to <see cref="Table"/>.
		/// A non-zero tab order automatically selects <see cref="Grid"/> unless <see cref="Table"/> is explicitly selected.
		/// </summary>
		Default,

		/// <summary>
		/// Uses the traditional table layout.
		/// </summary>
		Table,

		/// <summary>
		/// Uses the Grid layout, where block append order determines keyboard tab order.
		/// </summary>
		Grid
	}
}
