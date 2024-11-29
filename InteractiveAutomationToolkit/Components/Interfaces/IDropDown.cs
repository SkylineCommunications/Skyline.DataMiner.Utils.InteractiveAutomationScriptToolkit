namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	public interface IDropDown : IOptionWidget
	{
		string Selected { get; set; }
	}

	public interface IDropDown<T> : IOptionWidget<T>
	{
		Option<T> SelectedOption { get; set; }

		T Selected { get; set; }
	}
}