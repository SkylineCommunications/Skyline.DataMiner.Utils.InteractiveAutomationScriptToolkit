namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	/// <summary>
	/// Specifies how a dialog is laid out in DataMiner Web.
	/// </summary>
	public enum DialogWebUILayout
	{
		/// <summary>
		/// Uses the default layout, which is table-compatible.
		/// A non-zero tab order automatically selects Grid unless Table is explicitly selected.
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
