namespace Skyline.DataMiner.Utils.InteractiveAutomationScript.Dialogs
{
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	/// <summary>
	///  Dialog used to ask user confirmation.
	/// </summary>
	public class OkCancelDialog : Dialog<GridPanel>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OkCancelDialog"/> class.
		/// </summary>
		/// <param name="engine">Link with DataMiner.</param>
		/// <param name="message">Message to be displayed to the user.</param>
		/// <param name="callToAction">Specifies what button should get the <see cref="ButtonStyle.CallToAction"/> style.</param>
		public OkCancelDialog(IEngine engine, string message, CallToAction callToAction = CallToAction.OK) : base(engine)
		{
			OkButton.Style = callToAction == CallToAction.OK ? ButtonStyle.CallToAction : ButtonStyle.None;
			CancelButton.Style = callToAction == CallToAction.Cancel ? ButtonStyle.CallToAction : ButtonStyle.None;

			Panel.Add(new Label(message), 0, 0, 1, 2);

			Panel.Add(new WhiteSpace(), 1, 0);

			if (callToAction == CallToAction.OK)
			{
				Panel.Add(CancelButton, 2, 0);
				Panel.Add(OkButton, 2, 1);
			}
			else
			{
				Panel.Add(OkButton, 2, 0);
				Panel.Add(CancelButton, 2, 1);
			}

			SetColumnWidth(0, 140);
			SetColumnWidth(1, 140);
		}

		/// <summary>
		///	Gets the button with "OK" that is displayed below the message.
		/// </summary>
		public Button OkButton { get; } = new Button("OK")
		{
			Width = 130,
			HorizontalAlignment = HorizontalAlignment.Right,
		};

		/// <summary>
		///	Gets the button with "Cancel" that is displayed below the message.
		/// </summary>
		public Button CancelButton { get; } = new Button("Cancel")
		{
			Width = 130,
			HorizontalAlignment = HorizontalAlignment.Right,
		};

		/// <summary>
		/// Shows the <see cref="OkCancelDialog"/> without passing through the <see cref="InteractiveController"/>
		/// This will block any further script execution until the user interacts with the dialog.
		/// </summary>
		/// <param name="engine">Link with DataMiner.</param>
		/// <param name="message">Message to be displayed to the user.</param>
		/// <param name="title">Title of the dialog.</param>
		/// <param name="callToAction">Specifies what button should get the <see cref="ButtonStyle.CallToAction"/> style.</param>
		/// <param name="showScriptAbortPopup">Specifies whether a popup is shown when the user aborts the script from this dialog.</param>
		/// <returns></returns>
		public static bool Show(IEngine engine, string message, string title = "Are you sure?", CallToAction callToAction = CallToAction.OK, bool showScriptAbortPopup = true)
		{
			var dialog = new OkCancelDialog(engine, message, callToAction)
			{
				Title = title,
				ShowScriptAbortPopup = showScriptAbortPopup,
			};

			bool okButtonPressed = false;
			dialog.OkButton.Pressed += (s, e) => okButtonPressed = true;
			dialog.CancelButton.Pressed += (s, e) => okButtonPressed = false;

			dialog.ShowInteractive();

			return okButtonPressed;
		}

		/// <summary>
		/// Specifies which button should get the <see cref="ButtonStyle.CallToAction"/> style.
		/// </summary>
		public enum CallToAction
		{
			OK,
			Cancel,
		}
	}
}
