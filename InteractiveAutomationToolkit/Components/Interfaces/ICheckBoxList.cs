namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System.Collections.Generic;

	public interface ICheckBoxList : ICheckBoxListBase, IOptionWidget
	{
		IEnumerable<string> Checked { get; }

		IEnumerable<string> Unchecked { get; }

		void Check(string option);

		void Uncheck(string option);
	}

	public interface ICheckBoxList<T> : ICheckBoxListBase, IOptionWidget<T>
	{
		IEnumerable<Option<T>> CheckedOptions { get; }

		IEnumerable<T> Checked { get; }

		IEnumerable<Option<T>> UncheckedOptions { get; }

		IEnumerable<T> Unchecked { get; }

		void CheckOption(Option<T> option);

		void Check(T value);

		void UncheckOption(Option<T> option);

		void Uncheck(T value);
	}
}