namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System.Collections.Generic;
	using System.ComponentModel;

	/// <summary>
	///     Represents a special component that can group widgets together.
	/// </summary>
	public interface IPanel : IComponent
	{
		/// <summary>
		///     Gets the current number of columns allocated in the panel.
		/// </summary>
		/// <returns>The current number of columns allocated in the panel.</returns>
		int GetColumnCount();

		/// <summary>
		///     Gets the current number of rows allocated in the panel.
		/// </summary>
		/// <returns>The current number of rows allocated in the panel.</returns>
		int GetRowCount();

		/// <summary>
		/// 	Gets all widgets and their locations as if they are part of one big grid.
		/// 	This is used by the toolkit to build the UI.
		/// </summary>
		/// <returns>All widgets and their locations as if they are part of one big grid.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		IEnumerable<WidgetLocationPair> GetWidgetLocationPairs();

		/// <summary>
		///     Removes all widgets and panels from the panel.
		/// </summary>
		void Clear();

		/// <summary>
		///     Disables all widgets added to this panel.
		///     Sets the <see cref="IInteractiveWidget.IsEnabled" /> property of all <see cref="IInteractiveWidget" /> to
		///     <c>false</c>.
		/// </summary>
		/// <remarks>Also changes nested widgets.</remarks>
		void DisableWidgets();

		/// <summary>
		///     Disables widgets added to this panel.
		///     Sets the <see cref="IInteractiveWidget.IsEnabled" /> property of all <see cref="IInteractiveWidget" /> to
		///     <c>false</c>.
		/// </summary>
		/// <param name="includeNested"><c>true</c> if nested widgets should be included. <c>false</c> otherwise.</param>
		void DisableWidgets(bool includeNested);

		/// <summary>
		///     Enables all widgets added to this panel.
		///     Sets the <see cref="IInteractiveWidget.IsEnabled" /> property of all <see cref="IInteractiveWidget" /> to
		///     <c>true</c>.
		/// </summary>
		/// <remarks>Also changes nested widgets.</remarks>
		void EnableWidgets();

		/// <summary>
		///     Enables widgets added to this panel.
		///     Sets the <see cref="IInteractiveWidget.IsEnabled" /> property of all <see cref="IInteractiveWidget" /> to
		///     <c>true</c>.
		/// </summary>
		/// <param name="includeNested"><c>true</c> if nested widgets should be included. <c>false</c> otherwise.</param>
		void EnableWidgets(bool includeNested);

		/// <summary>
		///     Gets all panels added to this panel.
		/// </summary>
		/// <returns>Panels added to this panel.</returns>
		/// <remarks>Also returns nested panels.</remarks>
		IEnumerable<IPanel> GetPanels();

		/// <summary>
		///     Gets panels added to this panel.
		/// </summary>
		/// <param name="includeNested">Include nested panels.</param>
		/// <returns>Panels added to this panel.</returns>
		IEnumerable<IPanel> GetPanels(bool includeNested);

		/// <summary>
		///     Gets all widgets added to this panel.
		/// </summary>
		/// <returns>Widgets added to this panel.</returns>
		/// <remarks>Also returns widgets from nested panels.</remarks>
		IEnumerable<IWidget> GetWidgets();

		/// <summary>
		///     Gets the widgets added to this panel.
		/// </summary>
		/// <param name="includeNested">Include widgets from nested panels.</param>
		/// <returns>Widgets added to this panel.</returns>
		IEnumerable<IWidget> GetWidgets(bool includeNested);

		/// <summary>
		///     Hides all widgets added to this panel.
		///     Sets the <see cref="IWidget.IsVisible" /> property of all <see cref="IWidget" /> to <c>false</c>.
		/// </summary>
		/// <remarks>Also changes nested widgets.</remarks>
		void HideWidgets();

		/// <summary>
		///     Hides widgets added to this panel.
		///     Sets the <see cref="IWidget.IsVisible" /> property of all <see cref="IWidget" /> to <c>false</c>.
		/// </summary>
		/// <param name="includeNested"><c>true</c> if nested widgets should be included. <c>false</c> otherwise.</param>
		void HideWidgets(bool includeNested);

		/// <summary>
		///     Enables or disables all widgets added to this panel.
		///     Sets the <see cref="IInteractiveWidget.IsEnabled" /> property of all <see cref="IInteractiveWidget" />.
		/// </summary>
		/// <param name="enabled"><c>true</c> if the widgets should be set enabled. <c>false</c> otherwise.</param>
		/// <param name="includeNested"><c>true</c> if nested widgets should be included. <c>false</c> otherwise.</param>
		void SetWidgetsEnabled(bool enabled, bool includeNested);

		/// <summary>
		///     Shows or hides all widgets added to this panel.
		///     Sets the <see cref="IWidget.IsVisible" /> property of all <see cref="IWidget" />.
		/// </summary>
		/// <param name="visible"><c>true</c> if the widgets should be set visible. <c>false</c> otherwise.</param>
		/// <param name="includeNested"><c>true</c> if nested widgets should be included. <c>false</c> otherwise.</param>
		void SetWidgetsVisible(bool visible, bool includeNested);

		/// <summary>
		///     Shows all widgets added to this panel.
		///     Sets the <see cref="IWidget.IsVisible" /> property of all <see cref="IWidget" /> to <c>true</c>.
		/// </summary>
		/// <remarks>Also changes nested widgets.</remarks>
		void ShowWidgets();

		/// <summary>
		///     Shows widgets added to this panel.
		///     Sets the <see cref="IWidget.IsVisible" /> property of all <see cref="IWidget" /> to <c>true</c>.
		/// </summary>
		/// <param name="includeNested"><c>true</c> if nested widgets should be included. <c>false</c> otherwise.</param>
		void ShowWidgets(bool includeNested);
	}
}