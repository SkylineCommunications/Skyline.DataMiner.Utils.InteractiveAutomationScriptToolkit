namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	public interface IFormPanel : IPanel
	{
		void Add(string label, IComponent component);

		void Add(ILabel label, IComponent component);

		void Add(string label, IWidget widget, int span);

		void Add(ILabel label, IWidget widget, int span);

		void Remove(IComponent component);
	}
}