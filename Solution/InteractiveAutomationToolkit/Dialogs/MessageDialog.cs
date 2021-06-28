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

		/// <summary>
		/// Used to instantiate a new MessageDialog without a message.
		/// </summary>
		/// <param name="engine">Link with DataMiner.</param>
		public MessageDialog(Engine engine) : base(engine)
		{
			OkButton = new Button("OK") { Width = 150 };

			AddWidget(messageLabel, 0, 0);
			AddWidget(OkButton, 1, 0);
		}

		/// <summary>
		/// Used to instantiate a new MessageDialog with a specific message.
		/// </summary>
		/// <param name="engine">Link with DataMiner.</param>
		/// <param name="message">Message to be displayed in the dialog.</param>
		public MessageDialog(Engine engine, String message) : this(engine)
		{
			Message = message;
		}

		/// <summary>
		/// Message to be displayed in the dialog.
		/// </summary>
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

		/// <summary>
		/// Button that is displayed below the message.
		/// </summary>
		public Button OkButton { get; private set; }
	}
}
