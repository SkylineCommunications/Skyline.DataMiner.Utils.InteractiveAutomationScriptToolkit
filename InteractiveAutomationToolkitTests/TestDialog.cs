namespace InteractiveAutomationToolkitTests
{
    using Skyline.DataMiner.Automation;
    using Skyline.DataMiner.Utils.InteractiveAutomationScript;

    public class TestDialog : Dialog
	{
		public TestDialog(Engine engine) : base(engine)
		{
		}

        public TestDialog(IEngine engine) : base(engine)
        {
        }
    }
}
