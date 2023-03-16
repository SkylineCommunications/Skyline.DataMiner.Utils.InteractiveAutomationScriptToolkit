namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;

	/// <summary>
	/// Arranges child components into a form of widgets and their associated labels.
	/// </summary>
	public interface IFormPanel : IPanel
	{
		/// <summary>
		/// Adds a component to the form with the given label.
		/// </summary>
		/// <param name="label">The label shown next to the component.</param>
		/// <param name="component">The component to be added to the form.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="component"/> is null.</exception>
		void Add(string label, IComponent component);

		/// <summary>
		/// Adds a component to the form with the given label.
		/// </summary>
		/// <param name="label">The label shown next to the component.</param>
		/// <param name="component">The component to be added to the form.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="label"/> is null.</exception>
		/// <exception cref="ArgumentNullException">When <paramref name="component"/> is null.</exception>
		void Add(ILabel label, IComponent component);

		/// <summary>
		/// Adds a widget with specified span and with the given label to the form.
		/// </summary>
		/// <param name="label">The label shown next to the component.</param>
		/// <param name="widget">The widget to be added to the form.</param>
		/// <param name="span">The number of columns the widget spans.</param>
		/// <exception cref="ArgumentOutOfRangeException">When <paramref name="span"/> is smaller than 1.</exception>
		/// <exception cref="ArgumentNullException">When <paramref name="widget"/> is null.</exception>
		void Add(string label, IWidget widget, int span);

		/// <summary>
		/// Adds a widget with specified span and with the given label to the form.
		/// </summary>
		/// <param name="label">The label shown next to the component.</param>
		/// <param name="widget">The widget to be added to the form.</param>
		/// <param name="span">The number of columns the widget spans.</param>
		/// <exception cref="ArgumentOutOfRangeException">When <paramref name="span"/> is smaller than 1.</exception>
		/// <exception cref="ArgumentNullException">When <paramref name="label"/> is null.</exception>
		/// <exception cref="ArgumentNullException">When <paramref name="widget"/> is null.</exception>
		void Add(ILabel label, IWidget widget, int span);

		/// <summary>
		/// Removes a component from the form.
		/// </summary>
		/// <param name="component">The component to remove.</param>
		void Remove(IComponent component);
	}
}