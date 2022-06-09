namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	/// <summary>
	///     Represents a widget that can display the value of a protocol parameter.
	/// </summary>
	public interface IParameter : IWidget
	{
		/// <summary>
		///     Gets or sets the ID of the DataMiner Agent that has the parameter.
		/// </summary>
		int DmaId { get; set; }

		/// <summary>
		///     Gets or sets the ID of the element that has the parameter.
		/// </summary>
		int ElementId { get; set; }

		/// <summary>
		///     Gets or sets the primary key of the table entry.
		/// </summary>
		/// <remarks>Should be <c>null</c> for standalone parameters.</remarks>
		string Index { get; set; }

		/// <summary>
		///     Gets or sets the ID of the parameter.
		/// </summary>
		int ParameterId { get; set; }
	}
}