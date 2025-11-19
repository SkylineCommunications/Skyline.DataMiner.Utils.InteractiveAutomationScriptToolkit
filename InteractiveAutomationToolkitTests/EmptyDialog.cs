namespace InteractiveAutomationToolkitTests
{
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	public class EmptyDialog : Dialog
	{
		public EmptyDialog(Engine engine) : base(engine)
		{
		}

		public EmptyDialog(IEngine engine) : base(engine)
		{
		}
	}
}
