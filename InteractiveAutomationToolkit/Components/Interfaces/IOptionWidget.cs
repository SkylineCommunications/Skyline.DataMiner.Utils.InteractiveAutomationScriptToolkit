namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System.Collections.Generic;

	public interface IOptionWidget
	{
		IEnumerable<string> Options { get; }

		void SetOptions(IEnumerable<string> options);

		void AddOption(string option);

		void RemoveOption(string option);
	}

	public interface IOptionWidget<T>
	{
		IEnumerable<Option<T>> Options { get; set; }

		IEnumerable<T> Values { get; set; }

		void SetOptions(IEnumerable<Option<T>> options);

		void SetOptions(IEnumerable<T> options);

		void AddOption(Option<T> option);

		void AddOption(T value);

		void RemoveOption(Option<T> option);

		void RemoveOption(T value);
	}
}