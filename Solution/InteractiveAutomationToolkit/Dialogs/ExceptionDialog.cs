namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using Skyline.DataMiner.Automation;

	/// <summary>
	///		Dialog used to display an Exception.
	/// </summary>
	public class ExceptionDialog : Dialog
	{
		private readonly Label exceptionLabel = new Label();
		private Exception exception;

		/// <summary>
		/// Initializes a new instance of the ExceptionDialog class.
		/// </summary>
		/// <param name="engine">Link with DataMiner.</param>
		public ExceptionDialog(Engine engine) : base(engine)
		{
			Title = "Exception Occurred";
			OkButton = new Button("OK");

			AddWidget(exceptionLabel, 0, 0);
			AddWidget(OkButton, 1, 0);
		}

		/// <summary>
		/// Initializes a new instance of the ExceptionDialog with a specific Exception to be displayed.
		/// </summary>
		/// <param name="engine">Link with DataMiner.</param>
		/// <param name="exception">Exception to be displayed by the ExceptionDialog.</param>
		public ExceptionDialog(Engine engine, Exception exception) : this(engine)
		{
			Exception = exception;
		}

		/// <summary>
		/// Exception to be displayed by the ExceptionDialog.
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
		/// Button that is displayed below the Exception.
		/// </summary>
		public Button OkButton { get; private set; }
	}
}
