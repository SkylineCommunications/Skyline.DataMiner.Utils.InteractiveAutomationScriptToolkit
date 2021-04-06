namespace ReportingProgressExample
{
	using System;
	using System.Threading;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit;

	public class Script
	{
		private InteractiveController app;
		private MainDialog mainDialog;
		private ProgressDialog progressDialog;

		/// <summary>
		/// The Script entry point.
		/// Engine.ShowUI();
		/// </summary>
		/// <param name="engine">Link with SLScripting process.</param>
		public void Run(Engine engine)
		{
			try
			{
				engine.SetFlag(RunTimeFlags.NoKeyCaching);
				engine.Timeout = TimeSpan.FromHours(10);
				RunSafe(engine);
			}
			catch (ScriptAbortException)
			{
				throw;
			}
			catch (Exception e)
			{
				engine.Log("Run|Something went wrong: " + e);
				ShowExceptionDialog(engine, e);
			}
		}

		private void RunSafe(Engine engine)
		{
			app = new InteractiveController(engine);

			mainDialog = new MainDialog(engine);
			progressDialog = new ProgressDialog(engine);

			mainDialog.ContinueButton.Pressed += (sender, args) =>
			{
				app.ShowDialog(progressDialog);

				LongRunningMethod();

				progressDialog.Finish();
				app.ShowDialog(progressDialog);
			};

			progressDialog.OkButton.Pressed += (sender, args) => engine.ExitSuccess("Script ran successfully");

			app.Run(mainDialog);
		}

		private void ShowExceptionDialog(Engine engine, Exception exception)
		{
			ExceptionDialog dialog = new ExceptionDialog(engine, exception);
			dialog.OkButton.Pressed += (sender, args) => engine.ExitFail("Something went wrong during the creation of the new event.");
			if (app.IsRunning) app.ShowDialog(dialog); else app.Run(dialog);
		}

		private void LongRunningMethod()
		{
			string[] progressTexts = new string[] { "removing weeds", "tending gnomes", "cleaning litter", "mowing lawn", "upsetting neighbors", "feeding birds" };
			foreach (string text in progressTexts)
			{
				progressDialog.AddProgressLine(text + "...");
				Thread.Sleep(2000);
				progressDialog.AddProgressLine("finished " + text);
			}
		}
	}

	public class MainDialog : Dialog
	{
		public MainDialog(Engine engine) : base(engine)
		{
			Title = "Progress Reporter Demo";

			AddWidget(new Label("Welcome to the progress reporter demo."), 0, 0);
			AddWidget(new WhiteSpace(), 1, 0);
			AddWidget(ContinueButton, 2, 0);
		}

		public Button ContinueButton { get; private set; } = new Button("Continue");
	}
}
