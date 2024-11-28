namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	public interface IRadioButtonList : IOptionWidget
	{
		string Selected { get; set; }
	}

	public interface IRadioButtonList<T> : IOptionWidget<T>
	{
		Option<T> SelectedOption { get; set; }

		T Selected { get; set; }
	}
}
