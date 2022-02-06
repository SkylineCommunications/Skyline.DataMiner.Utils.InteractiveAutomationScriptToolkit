namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;

	public interface ICollapseButton : IInteractiveWidget
	{
		/// <summary>
		/// This method is used to collapse the collapse button.
		/// </summary>
		void Collapse();

		/// <summary>
		/// This method is used to expand the collapse button.
		/// </summary>
		void Expand();

		/// <summary>
		/// Gets or sets the text to be displayed in the collapse button when the button is expanded.
		/// </summary>
		string CollapseText { get; set; }

		/// <summary>
		/// Gets or sets the text to be displayed in the collapse button when the button is collapsed.
		/// </summary>
		string ExpandText { get; set; }

		/// <summary>
		/// Indicates if the collapse button is collapsed or not.
		/// If the collapse button is collapsed, the IsVisible property of all linked widgets is set to false.
		/// If the collapse button is not collapsed, the IsVisible property of all linked widgets is set to true.
		/// </summary>
		bool IsCollapsed { get; set; }

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		/// <exception cref="ArgumentNullException">When the value is <c>null</c>.</exception>
		string Tooltip { get; set; }

		/// <summary>
		/// Collection of widgets that are affected by this collapse button.
		/// </summary>
		List<IWidget> LinkedWidgets { get; }

		/// <summary>
		///     Triggered when the button is pressed.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		event EventHandler<EventArgs> Pressed;
	}
}