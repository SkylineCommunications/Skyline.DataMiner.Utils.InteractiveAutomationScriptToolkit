namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Skyline.DataMiner.Automation;

	/// <summary>
	/// When displaying progress, this dialog has to be shown without requiring user interaction.
	/// When you are done displaying progress, call the Finish method and Show the dialog with user interaction required.
	/// </summary>
	public class ProgressDialog : Dialog
	{
		private readonly StringBuilder progress = new StringBuilder();
		private readonly Label progressLabel = new Label();

		public ProgressDialog(Engine engine) : base(engine)
		{
			OkButton = new Button("OK") { IsEnabled = true, Width = 150 };
		}

		public Button OkButton { get; private set; }

		/// <summary>
		/// This will clear the current progress and display the provided text.
		/// </summary>
		/// <param name="text">Indication of the progress made.</param>
		public void SetProgress(string text)
		{
			progress.Clear();
			progress.AppendLine(text);
			Engine.ShowProgress(progress.ToString());
		}

		/// <summary>
		/// This will add the provided text to the current progress.
		/// </summary>
		/// <param name="text">Text to add to current line of progress.</param>
		public void AddProgress(string text)
		{
			progress.Append(text);
			Engine.ShowProgress(progress.ToString());
		}

		/// <summary>
		/// This will add the provided text on a new line to the current progress.
		/// </summary>
		/// <param name="text">Indication of the progress made. This will be placed on a separate line.</param>
		public void AddProgressLine(string text)
		{
			progress.AppendLine(text);
			Engine.ShowProgress(progress.ToString());
		}

		/// <summary>
		/// This will clear the progress.
		/// </summary>
		public void ClearProgress()
		{
			progress.Clear();
			Engine.ShowProgress(progress.ToString());
		}

		/// <summary>
		/// Call this method when you are done updating the progress through this dialog.
		/// This will cause the OK button to appear.
		/// Display this form with User Interactivity required after this method is called.
		/// </summary>
		public void Finish() // TODO: ShowConfirmation
		{
			progressLabel.Text = progress.ToString();

			if (!Widgets.Contains(progressLabel)) AddWidget(progressLabel, 0, 0);
			if (!Widgets.Contains(OkButton)) AddWidget(OkButton, 1, 0);
		}
	}
}
