namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Skyline.DataMiner.Automation;

	/// <summary>
	///		Dialog used to display an Message.
	/// </summary>
	public class MessageDialog : Dialog
	{
		private readonly Label messageLabel = new Label();

		public MessageDialog(Engine engine) : base(engine)
		{
			OkButton = new Button("OK") { Width = 150 };

			AddWidget(messageLabel, 0, 0);
			AddWidget(OkButton, 1, 0);
		}

		public MessageDialog(Engine engine, String message) : this(engine)
		{
			Message = message;
		}

		public string Message
		{
			get
			{
				return messageLabel.Text;
			}

			set
			{
				messageLabel.Text = value;
			}
		}

		public Button OkButton { get; private set; }
	}
}
