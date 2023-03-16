namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
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

		public ExceptionDialog(Engine engine) : base(engine)
		{
			Title = "Exception Occurred";
			OkButton = new Button("OK");

			AddWidget(exceptionLabel, 0, 0);
			AddWidget(OkButton, 1, 0);
		}

		public ExceptionDialog(Engine engine, Exception exception) : this(engine)
		{
			Exception = exception;
		}

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

		public Button OkButton { get; private set; }
	}
}
