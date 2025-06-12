namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Linq;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;

	/// <summary>
	///     Event loop of the interactive Automation script.
	/// </summary>
	public class InteractiveController
	{
		private bool isManualModeRequested;
		private Action manualAction;
		private Dialog nextDialog;
		private bool isRunning;

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
		///		Gets or sets the behavior of the popup shown whenever the script is aborted.
		///		This popup is shown when the window in which the Dialogs are shown is closed by the user and asks for confirmation whether the script can be stopped or not.
		///		<see cref="ScriptAbortPopupBehavior.OnDialogLevel"/> causes the <see cref="Dialog.ShowScriptAbortPopup"/> value of the currently displayed Dialog to determine whether the popup should be shown or not.
		///		<see cref="ScriptAbortPopupBehavior.HideAlways"/> causes the popup to never be displayed, regardless of the <see cref="Dialog.ShowScriptAbortPopup"/> value of the displayed Dialog.
		///		<see cref="ScriptAbortPopupBehavior.ShowAlways"/> causes the popup to always be displayed, regardless of the <see cref="Dialog.ShowScriptAbortPopup"/> value of the displayed Dialog.
		/// </summary>
		public ScriptAbortPopupBehavior ScriptAbortPopupBehavior { get; set; } = ScriptAbortPopupBehavior.OnDialogLevel;

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
		///		Stops the application event loop.
		///		Use the Run method in order to start it again after stopping.
		/// </summary>
		public void Stop()
		{
			isRunning = false;
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

			if (isRunning)
			{
				nextDialog = dialog;
			}
			else
			{
				Run(dialog);
			}
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

			SetScriptAbortPopupBehavior(CurrentDialog);
			CurrentDialog.Show(false);
		}

		/// <summary>
		/// Hides the UI. This does not block any background logic from running.
		/// Use <see cref="ShowDialog" /> if you want to show the UI again.
		/// </summary>
		public void Hide()
		{
			Engine.HideUI();
			nextDialog = null;
		}


		/// <summary>
		///     Starts the application event loop.
		///     Updates the displayed dialog after each user interaction.
		///     Only user interaction on widgets with the WantsOnChange property set to true will cause updates.
		///     Use <see cref="RequestManualMode" /> if you want to manually control when the dialog is updated.
		/// </summary>
		/// <param name="startDialog">Dialog to be shown first.</param>
		private void Run(Dialog startDialog)
		{
			if (startDialog == null)
			{
				throw new ArgumentNullException("startDialog");
			}

			nextDialog = startDialog;

			if (isRunning)
			{
				throw new InvalidOperationException("Already running");
			}

			isRunning = true;
			while (isRunning)
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
						if (CurrentDialog == null)
						{
							isRunning = false;
							IsManualMode = false;
						}
						else
						{
							SetScriptAbortPopupBehavior(CurrentDialog);

							if (RequiresResponse(CurrentDialog))
							{
								CurrentDialog.Show();
							}
							else
							{
								CurrentDialog.Show(false);
								System.Threading.Thread.Sleep(10000); // Wait for 10 seconds before checking for new dialogs
							}
						}
					}
				}
				catch (Exception)
				{
					isRunning = false;
					IsManualMode = false;
					throw;
				}
			}
		}

		private void RunManualAction()
		{
			isManualModeRequested = false;
			IsManualMode = true;
			manualAction();
			IsManualMode = false;
		}

		private void SetScriptAbortPopupBehavior(Dialog dialog)
		{
			switch (ScriptAbortPopupBehavior)
			{
				case ScriptAbortPopupBehavior.HideAlways:
					dialog.ShowScriptAbortPopup = false;
					return;
				case ScriptAbortPopupBehavior.ShowAlways:
					dialog.ShowScriptAbortPopup = true;
					return;
				default:
					// Behavior is defined on Dialog level
					return;
			}
		}

		private bool RequiresResponse(Dialog dialog)
		{
			foreach (var visibleWidget in dialog.Widgets.Where(w => w.IsVisible))
			{
				if (!visibleWidget.BlockDefinition.WantsOnChange
					&& !visibleWidget.BlockDefinition.WantsOnFocusLost)
				{
					continue;
				}

				if (!visibleWidget.BlockDefinition.IsReadOnly && visibleWidget.BlockDefinition.IsEnabled)
				{
					return true;
				}
			}

			return false;
		}
	}
}
