namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System.Collections.Generic;

	public interface IStackPanel : IPanel, IEnumerable<IComponent>
	{
		Direction Direction { get; set; }

		void Add(IComponent component);

		void Add(IWidget widget, int span);

		IComponent ComponentAt(int index);

		int IndexOf(IComponent component);

		void Insert(int index, IComponent component);

		void Insert(int index, IWidget widget, int span);

		/// <summary>
		///     Removes a component from the panel.
		/// </summary>
		/// <param name="component">Component to remove.</param>
		void Remove(IComponent component);

		void RemoveAt(int index);
	}
}