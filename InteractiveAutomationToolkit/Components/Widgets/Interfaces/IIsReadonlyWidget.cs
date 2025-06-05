namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	public interface IIsReadonlyWidget
	{
		/// <summary>
		///        Gets or sets a value indicating whether the control is displayed in read-only mode.
		///        Read-only mode causes the widgets to appear read-write but the user won't be able to change their value.
		///        This only affects interactive scripts running in a web environment.
		/// </summary>
		/// <remarks>Available from DataMiner 10.4.1 onwards.</remarks>
		bool IsReadOnly { get; set; }
	}
}