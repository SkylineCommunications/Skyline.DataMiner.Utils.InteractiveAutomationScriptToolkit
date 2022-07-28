namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Runtime.Serialization;

	public interface IStackPanel : IPanel
	{
		Direction Direction { get; set; }

		void Add(IPanel panel);

		void Add(IWidget widget);

		void Add(IWidget widget, int span);

		void Insert(int index, IPanel panel);

		void Insert(int index, IWidget widget);

		void Insert(int index, IWidget widget, int span);

		void RemoveAt(int index);

		IPanel PanelAt(int index);

		IWidget WidgetAt(int index);

		object ComponentAt(int index);

		int IndexOf(IPanel panel);

		int IndexOf(IWidget widget);

		/// <summary>
		///     Removes a panel from the dialog.
		/// </summary>
		/// <param name="panel">Panel to remove.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="panel" /> is <c>null</c>.</exception>
		void Remove(IPanel panel);

		/// <summary>
		///     Removes a widget from this component.
		/// </summary>
		/// <param name="widget">Widget to remove.</param>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		void Remove(IWidget widget);
	}

	[Serializable]
	public class ComponentNotFoundException : Exception
	{
		public ComponentNotFoundException()
		{
		}

		public ComponentNotFoundException(string message) : base(message)
		{
		}

		public ComponentNotFoundException(string message, Exception inner) : base(message, inner)
		{
		}

		protected ComponentNotFoundException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}