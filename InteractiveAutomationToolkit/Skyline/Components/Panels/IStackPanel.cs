namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Arranges child components into a single line that can be oriented horizontally or vertically.
	/// </summary>
	public interface IStackPanel : IPanel, IList<IComponent>, IReadOnlyList<IComponent>
	{
		/// <summary>
		/// Gets or sets a value indication the orientation by which the child components are stacked.
		/// </summary>
		Direction Direction { get; set; }

		/// <inheritdoc cref="ICollection{T}.Count"/>
		new int Count { get; }

		/// <inheritdoc cref="IList{T}.this"/>
		new IComponent this[int index] { get; }

		/// <summary>
		/// Adds a widget to the stack and specified how many rows or columns the widget must span.
		/// </summary>
		/// <param name="widget">Widget to be added to the stack.</param>
		/// <param name="span">Number of rows for a horizontal orientation or number of columns for a vertical orientation.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="widget" /> is <c>null</c>.</exception>
		/// <exception cref="InvalidOperationException">When trying to add a panel to itself.</exception>
		/// <exception cref="InvalidOperationException">When the component is already added.</exception>
		/// <exception cref="ArgumentOutOfRangeException">When <paramref name="span"/> is smaller than 1.</exception>
		void Add(IWidget widget, int span);

		/// <summary>
		/// Gets the component at the specified index of the stack.
		/// </summary>
		/// <param name="index">The zero-based index of the component.</param>
		/// <returns>The component at the specified index of the stack.</returns>
		/// <exception cref="IndexOutOfRangeException">When <paramref name="index"/> is outside the range of valid indexes.</exception>
		IComponent ComponentAt(int index);

		/// <summary>
		/// Inserts a widget into the stack at the specified index and specified how many rows or columns the widget must span.
		/// </summary>
		/// <param name="index">The zero-based index at which the widget needs to be inserted.</param>
		/// <param name="widget">The widget to insert.</param>
		/// <param name="span">Number of rows for a horizontal orientation or number of columns for a vertical orientation.</param>
		/// <exception cref="IndexOutOfRangeException">When <paramref name="index"/> is less than 0.</exception>
		/// <exception cref="IndexOutOfRangeException">When <paramref name="index"/> is is greater than <see cref="IList{T}.Count"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">When <paramref name="span"/> is smaller than 1.</exception>
		void Insert(int index, IWidget widget, int span);

		/// <inheritdoc cref="IPanel.Clear"/>
		new void Clear();
	}
}