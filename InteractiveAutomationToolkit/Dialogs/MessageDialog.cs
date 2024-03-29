﻿namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;

	using Skyline.DataMiner.Automation;

	/// <summary>
	/// 	Dialog used to display a message.
	/// </summary>
	public class MessageDialog : Dialog
	{
		private readonly Label messageLabel = new Label();

		/// <summary>
		/// Initializes a new instance of the <see cref="MessageDialog" /> class without a message.
		/// </summary>
		/// <param name="engine">Link with DataMiner.</param>
		public MessageDialog(IEngine engine) : base(engine)
		{
			OkButton = new Button("OK") { Width = 150, Style = ButtonStyle.CallToAction };

			AddWidget(messageLabel, 0, 0);
			AddWidget(OkButton, 1, 0);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MessageDialog" /> class with a specific message.
		/// </summary>
		/// <param name="engine">Link with DataMiner.</param>
		/// <param name="message">Message to be displayed in the dialog.</param>
		public MessageDialog(IEngine engine, String message) : this(engine)
		{
			Message = message;
		}

		/// <summary>
		/// Gets or sets the message to be displayed in the dialog.
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
		/// Gets the button that is displayed below the message.
		/// </summary>
		public Button OkButton { get; private set; }
	}
}
