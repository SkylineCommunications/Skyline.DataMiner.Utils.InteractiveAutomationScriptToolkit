namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	public interface IRadioButtonListBase
	{
		bool IsReadOnly { get; set; }

		bool IsSorted { get; set; }

		string Tooltip { get; set; }
	}
}