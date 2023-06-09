﻿namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;

	/// <summary>
	/// Used to define the position of an item in a grid layout.
	/// </summary>
	[Obsolete("Replaced by WidgetLocation and PanelLocation.", false)]
	public interface ILayout
	{
		/// <summary>
		///     Gets the column location of the widget on the grid.
		/// </summary>
		/// <remarks>The top-left position is (0, 0) by default.</remarks>
		int Column { get; }

		/// <summary>
		///     Gets the row location of the widget on the grid.
		/// </summary>
		/// <remarks>The top-left position is (0, 0) by default.</remarks>
		int Row { get; }
	}
}