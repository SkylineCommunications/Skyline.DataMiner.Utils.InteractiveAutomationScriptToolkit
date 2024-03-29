﻿namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;

	using Skyline.DataMiner.Automation;

	/// <summary>
	/// 	Dialog used to display an exception.
	/// </summary>
	public class ExceptionDialog : Dialog
	{
		private readonly Label exceptionLabel = new Label();
		private Exception exception;

		/// <summary>
		/// Initializes a new instance of the <see cref="ExceptionDialog"/> class.
		/// </summary>
		/// <param name="engine">Link with DataMiner.</param>
		public ExceptionDialog(IEngine engine) : base(engine)
		{
			Title = "Exception Occurred";
			OkButton = new Button("OK") { Style = ButtonStyle.CallToAction };

			AddWidget(exceptionLabel, 0, 0);
			AddWidget(OkButton, 1, 0);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExceptionDialog"/> class with a specific exception to be displayed.
		/// </summary>
		/// <param name="engine">Link with DataMiner.</param>
		/// <param name="exception">Exception to be displayed by the exception dialog.</param>
		public ExceptionDialog(IEngine engine, Exception exception) : this(engine)
		{
			Exception = exception;
		}

		/// <summary>
		/// Gets or sets the exception to be displayed by the exception dialog.
		/// </summary>
		public Exception Exception
		{
			get
			{
				return exception;
			}

			set
			{
				exception = value;
				exceptionLabel.Text = exception.ToString();
			}
		}

		/// <summary>
		/// Gets the button that is displayed below the exception.
		/// </summary>
		public Button OkButton { get; private set; }
	}
}
