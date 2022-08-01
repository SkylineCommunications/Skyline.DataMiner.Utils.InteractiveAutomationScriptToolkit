namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	public interface IComponent
	{
		/// <summary>
		/// Gets or sets the parent panel of the component.
		/// The value will be <c>null</c> if the component has not been added to a <see cref="Panel"/>.
		/// </summary>
		/// <remarks>This property should only be set by a <see cref="Panel"/> via <see cref="Panel.AddParentTo"/>.</remarks>
		IPanel Parent { get; set; }
	}
}