namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using Skyline.DataMiner.Automation;

	/// <summary>
	///		A whitespace.
	/// </summary>
	public class WhiteSpace : Widget
	{
		/// <summary>
		/// Initializes a new instance of the Whitespace class.
		/// </summary>
		public WhiteSpace()
		{
			Type = UIBlockType.StaticText;
			BlockDefinition.Style = null;
			BlockDefinition.Text = String.Empty;
		}
	}
}
