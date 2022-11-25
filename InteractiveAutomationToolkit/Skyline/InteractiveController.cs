namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Event loop of the interactive Automation script.
	/// </summary>
	public class InteractiveController
	{
		private bool isManualModeRequested;
		private Action manualAction;
		private Dialog nextDialog;

		/// <summary>
		///     Initializes a new instance of the <see cref="InteractiveController" /> class.
		///     This object will manage the event loop of the interactive Automation script.
		/// </summary>
		/// <param name="engine">Link with the SLAutomation process.</param>
		/// <exception cref="ArgumentNullException">When engine is null.</exception>
		public InteractiveController(IEngine engine)
		{
			if (engine == null)
			{
				throw new ArgumentNullException("engine");
			}

			Engine = engine;
		}

		/// <summary>
		///     Gets the dialog that is shown to the user.
		/// </summary>
		public Dialog CurrentDialog { get; private set; }

		/// <summary>
		///     Gets the link to the SLManagedAutomation process.
		/// </summary>
		public IEngine Engine { get; private set; }

		/// <summary>
		///     Gets a value indicating whether the event loop is updated manually or automatically.
		/// </summary>
		public bool IsManualMode { get; private set; }

		/// <summary>
		///     Gets a value indicating whether the event loop has been started.
		/// </summary>
		public bool IsRunning { get; private set; }

		/// <summary>
		///     Switches the event loop to manual control.
		///     This mode allows the dialog to be updated without user interaction using <see cref="Update" />.
		///     The passed action method will be called when all events have been processed.
		///     The app returns to automatic user interaction mode when the method is exited.
		/// </summary>
		/// <param name="action">Method that will control the event loop manually.</param>
		public void RequestManualMode(Action action)
		{
			isManualModeRequested = true;
			manualAction = action;
		}

		/// <summary>
		///     Starts the application event loop.
		///     Updates the displayed dialog after each user interaction.
		///     Only user interaction on widgets with the WantsOnChange property set to true will cause updates.
		///     Use <see cref="RequestManualMode" /> if you want to manually control when the dialog is updated.
		/// </summary>
		/// <param name="startDialog">Dialog to be shown first.</param>
		public void Run(Dialog startDialog)
		{
			if (startDialog == null)
			{
				throw new ArgumentNullException("startDialog");
			}

			nextDialog = startDialog;

			if (IsRunning)
			{
				throw new InvalidOperationException("Already running");
			}

			IsRunning = true;
			while (true)
			{
				try
				{
					if (isManualModeRequested)
					{
						RunManualAction();
					}
					else
					{
						CurrentDialog = nextDialog;
						CurrentDialog.Show();
					}
				}
				catch (Exception)
				{
					IsRunning = false;
					IsManualMode = false;
					throw;
				}
			}
		}

		/// <summary>
		///     Sets the dialog that will be shown after user interaction events are processed,
		///     or when <see cref="Update" /> is called in manual mode.
		/// </summary>
		/// <param name="dialog">The next dialog to be shown.</param>
		/// <exception cref="ArgumentNullException">When dialog is null.</exception>
		public void ShowDialog(Dialog dialog)
		{
			if (dialog == null)
			{
				throw new ArgumentNullException("dialog");
			}

			nextDialog = dialog;
		}

		/// <summary>
		///     Manually updates the dialog.
		///     Use this method when you want to update the dialog without user interaction.
		///     Note that no events will be raised.
		/// </summary>
		/// <exception cref="InvalidOperationException">When not in manual mode.</exception>
		/// <exception cref="InvalidOperationException">When no dialog has been set.</exception>
		public void Update()
		{
			if (!IsManualMode)
			{
				throw new InvalidOperationException("Not allowed in automatic mode");
			}

			if (CurrentDialog == null)
			{
				throw new InvalidOperationException("No dialog has been set");
			}

			CurrentDialog = nextDialog;
			CurrentDialog.Show(false);
		}

		private void RunManualAction()
		{
			isManualModeRequested = false;
			IsManualMode = true;
			manualAction();
			IsManualMode = false;
		}
	}
}
