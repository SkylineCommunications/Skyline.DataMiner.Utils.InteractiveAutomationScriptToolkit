namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Event loop of the interactive Automation script.
	/// </summary>
	public class InteractiveController : IInteractiveController
	{
		private bool isManualModeRequested;
		private Action manualAction;
		private IDialog nextDialog;

		/// <summary>
		///     Initializes a new instance of the <see cref="InteractiveController" /> class.
		///     This object will manage the event loop of the interactive Automation script.
		/// </summary>
		/// <param name="engine">Link with the SLAutomation process.</param>
		/// <exception cref="ArgumentNullException">When engine is null.</exception>
		public InteractiveController(IEngine engine)
		{
			Engine = engine ?? throw new ArgumentNullException(nameof(engine));
		}

		/// <inheritdoc />
		public IEngine Engine { get; }

		/// <inheritdoc />
		public IDialog CurrentDialog { get; private set; }

		/// <inheritdoc />
		public bool IsManualMode { get; private set; }

		/// <inheritdoc />
		public bool IsRunning { get; private set; }

		/// <inheritdoc />
		public bool InteractionPreventsScriptTimeout { get; set; } = true;

		/// <inheritdoc />
		public void RequestManualMode(Action action)
		{
			isManualModeRequested = true;
			manualAction = action;
		}

		/// <inheritdoc />
		public void Run(IDialog startDialog)
		{
			nextDialog = startDialog ?? throw new ArgumentNullException(nameof(startDialog));

			if (IsRunning)
			{
				throw new InvalidOperationException("Already running");
			}

			IsRunning = true;
			while (IsRunning)
			{
				DateTime start = DateTime.UtcNow;
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

				if (InteractionPreventsScriptTimeout)
				{
					TimeSpan elapsedTime = (DateTime.UtcNow - start).Duration();
					Engine.Timeout += elapsedTime;
				}
			}
		}

		/// <inheritdoc />
		public void ShowDialog(IDialog dialog)
		{
			nextDialog = dialog ?? throw new ArgumentNullException(nameof(dialog));
		}

		/// <inheritdoc />
		public void Stop()
		{
			IsRunning = false;
		}

		/// <inheritdoc />
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