namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System.Linq;
	using System.Text;
	using Skyline.DataMiner.Automation;

	/// <summary>
	/// When progress is displayed, this dialog has to be shown without requiring user interaction.
	/// When you are done displaying progress, call the Finish method and show the dialog with user interaction required.
	/// </summary>
	public class ProgressDialog : Dialog
	{
		private readonly StringBuilder progress = new StringBuilder();
		private readonly Label progressLabel = new Label();

		/// <summary>
		/// Used to instantiate a new instance of the <see cref="ProgressDialog" /> class.
		/// </summary>
		/// <param name="engine">Link with DataMiner.</param>
		public ProgressDialog(IEngine engine) : base(engine)
		{
			OkButton = new Button("OK") { IsEnabled = true, Width = 150 };
		}

		/// <summary>
		/// Button that is displayed after the Finish method is called.
		/// </summary>
		public Button OkButton { get; private set; }

		/// <summary>
		/// Clears the current progress and displays the provided text.
		/// </summary>
		/// <param name="text">Indication of the progress made.</param>
		public void SetProgress(string text)
		{
			progress.Clear();
			progress.AppendLine(text);
			Engine.ShowProgress(progress.ToString());
		}

		/// <summary>
		/// Adds the provided text to the current progress.
		/// </summary>
		/// <param name="text">Text to add to the current line of progress.</param>
		public void AddProgress(string text)
		{
			progress.Append(text);
			Engine.ShowProgress(progress.ToString());
		}

		/// <summary>
		/// Adds the provided text on a new line to the current progress.
		/// </summary>
		/// <param name="text">Indication of the progress made. This will be placed on a separate line.</param>
		public void AddProgressLine(string text)
		{
			progress.AppendLine(text);
			Engine.ShowProgress(progress.ToString());
		}

		/// <summary>
		/// Clears the progress.
		/// </summary>
		public void ClearProgress()
		{
			progress.Clear();
			Engine.ShowProgress(progress.ToString());
		}

		/// <summary>
		/// Call this method when you are done updating the progress through this dialog.
		/// This will cause the OK button to appear.
		/// Display this form with user interactivity required after this method is called.
		/// </summary>
		public void Finish() // TODO: ShowConfirmation
		{
			progressLabel.Text = progress.ToString();

			if (!Widgets.Contains(progressLabel)) AddWidget(progressLabel, 0, 0);
			if (!Widgets.Contains(OkButton)) AddWidget(OkButton, 1, 0);
		}
	}
}
