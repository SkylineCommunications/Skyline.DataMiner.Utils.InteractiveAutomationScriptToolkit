namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Represents the event loop of the interactive Automation script.
	/// </summary>
	public interface IInteractiveController
	{
		/// <summary>
		///     Gets the dialog that is shown to the user.
		/// </summary>
		IDialog CurrentDialog { get; }

		/// <summary>
		///     Gets the link to the SLManagedAutomation process.
		/// </summary>
		IEngine Engine { get; }

		/// <summary>
		///     Gets a value indicating whether the event loop is updated manually or automatically.
		/// </summary>
		bool IsManualMode { get; }

		/// <summary>
		///     Gets a value indicating whether the event loop has been started.
		/// </summary>
		bool IsRunning { get; }

		/// <summary>
		///     Switches the event loop to manual control.
		///     This mode allows the dialog to be updated without user interaction using
		///     <see cref="InteractiveController.Update" />.
		///     The passed action method will be called when all events have been processed.
		///     The app returns to automatic user interaction mode when the method is exited.
		/// </summary>
		/// <param name="action">Method that will control the event loop manually.</param>
		void RequestManualMode(Action action);

		/// <summary>
		///     Starts the application event loop.
		///     Updates the displayed dialog after each user interaction.
		///     Only user interaction on widgets with the WantsOnChange property set to true will cause updates.
		///     Use <see cref="InteractiveController.RequestManualMode" /> if you want to manually control when the dialog is
		///     updated.
		/// </summary>
		/// <param name="startDialog">Dialog to be shown first.</param>
		void Run(IDialog startDialog);

		/// <summary>
		///     Sets the dialog that will be shown after user interaction events are processed,
		///     or when <see cref="InteractiveController.Update" /> is called in manual mode.
		/// </summary>
		/// <param name="dialog">The next dialog to be shown.</param>
		/// <exception cref="ArgumentNullException">When dialog is null.</exception>
		void ShowDialog(IDialog dialog);

		/// <summary>
		///     Stops the event loop.
		///     Code will continue from the <see cref="InteractiveController.Run" /> method after all event handlers have finished.
		/// </summary>
		void Stop();

		/// <summary>
		///     Manually updates the dialog.
		///     Use this method when you want to update the dialog without user interaction.
		///     Note that no events will be raised.
		/// </summary>
		/// <exception cref="InvalidOperationException">When not in manual mode.</exception>
		/// <exception cref="InvalidOperationException">When no dialog has been set.</exception>
		void Update();
	}
}