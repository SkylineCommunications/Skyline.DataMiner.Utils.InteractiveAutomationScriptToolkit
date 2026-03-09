namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using Skyline.DataMiner.Automation;

	/// <summary>
	///  Dialog used to ask the user a yes/no question.
	/// </summary>
	public class YesNoDialog : Dialog
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="YesNoDialog"/> class.
		/// </summary>
		/// <param name="engine">Link with DataMiner.</param>
		/// <param name="message">Message to be displayed to the user.</param>
		/// <param name="callToAction">Specifies what button should get the <see cref="ButtonStyle.CallToAction"/> style.</param>
		public YesNoDialog(IEngine engine, string message, CallToAction callToAction = CallToAction.Yes) : base(engine)
		{
			YesButton.Style = callToAction == CallToAction.Yes ? ButtonStyle.CallToAction : ButtonStyle.None;
			NoButton.Style = callToAction == CallToAction.No ? ButtonStyle.CallToAction : ButtonStyle.None;

			AddWidget(new Label(message) { IsMultiline = true }, 0, 0, 1, 2);

			AddWidget(new WhiteSpace(), 1, 0);

			if (callToAction == CallToAction.Yes)
			{
				AddWidget(NoButton, 2, 0);
				AddWidget(YesButton, 2, 1, HorizontalAlignment.Right);
			}
			else
			{
				AddWidget(YesButton, 2, 0);
				AddWidget(NoButton, 2, 1, HorizontalAlignment.Right);
			}

			SetColumnWidth(0, 140);
			SetColumnWidth(1, 140);
		}

		/// <summary>
		///	Gets the button with "Yes" that is displayed below the message.
		/// </summary>
		public Button YesButton { get; } = new Button("Yes") { Width = 130 };

		/// <summary>
		///	Gets the button with "No" that is displayed below the message.
		/// </summary>
		public Button NoButton { get; } = new Button("No") { Width = 130 };

		/// <summary>
		/// Shows the <see cref="YesNoDialog"/> without passing through the <see cref="InteractiveController"/>
		/// This will block any further script execution until the user interacts with the dialog.
		/// </summary>
		/// <param name="engine">Link with DataMiner.</param>
		/// <param name="message">Message to be displayed to the user.</param>
		/// <param name="title">Title of the dialog.</param>
		/// <param name="callToAction">Specifies what button should get the <see cref="ButtonStyle.CallToAction"/> style.</param>
		/// <param name="showScriptAbortPopup">Specifies whether a popup is shown when the user aborts the script from this dialog..</param>
		/// <returns></returns>
		public static bool Show(IEngine engine, string message, string title = "Are you sure?", CallToAction callToAction = CallToAction.Yes, bool showScriptAbortPopup = true)
		{
			var dialog = new YesNoDialog(engine, message, callToAction)
			{
				Title = title,
				ShowScriptAbortPopup = showScriptAbortPopup,
			};

			bool yesButtonPressed = false;
			dialog.YesButton.Pressed += (s, e) => yesButtonPressed = true;
			dialog.NoButton.Pressed += (s, e) => yesButtonPressed = false;

			dialog.Show(true);

			return yesButtonPressed;
		}

		/// <summary>
		/// Specifies which button should get the <see cref="ButtonStyle.CallToAction"/> style.
		/// </summary>
		public enum CallToAction
		{
			Yes,
			No,
		}
	}
}
