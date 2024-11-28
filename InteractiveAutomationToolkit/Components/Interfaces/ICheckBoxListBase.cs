namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using Skyline.DataMiner.Automation;

	public interface ICheckBoxListBase
	{
		bool IsReadOnly { get; set; }

		bool IsSorted { get; set; }

		string Tooltip { get; set; }

		UIValidationState ValidationState { get; set; }

		string ValidationText { get; set; }

		void CheckAll();

		void UncheckAll();
	}
}