namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;

	internal static class ButtonStyleConverter
	{
		internal static string StyleToUiString(ButtonStyle buttonStyle)
		{
			switch (buttonStyle)
			{
				case ButtonStyle.None:
					return Automation.Style.Button.None;
				case ButtonStyle.CallToAction:
					return Automation.Style.Button.CallToAction;
				default:
					throw new ArgumentOutOfRangeException("buttonStyle", buttonStyle, null);
			}
		}
	}
}
