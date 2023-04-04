namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     A button that can be pressed.
	/// </summary>
	public class Button : InteractiveWidget
	{
		private bool pressed;
		private ButtonStyle style;

		/// <summary>
		///     Initializes a new instance of the <see cref="Button" /> class.
		/// </summary>
		/// <param name="text">Text displayed in the button.</param>
		public Button(string text)
		{
			Type = UIBlockType.Button;
			Text = text;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Button" /> class.
		/// </summary>
		public Button() : this(String.Empty)
		{
		}

		/// <summary>
		///     Triggered when the button is pressed.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<EventArgs> Pressed
		{
			add
			{
				OnPressed += value;
				BlockDefinition.WantsOnChange = true;
			}

			remove
			{
				OnPressed -= value;
				if (OnPressed == null || !OnPressed.GetInvocationList().Any())
				{
					BlockDefinition.WantsOnChange = false;
				}
			}
		}

		private event EventHandler<EventArgs> OnPressed;

		/// <summary>
		///     Gets or sets the text style of the label.
		/// </summary>
		public ButtonStyle Style
		{
			get
			{
				return style;
			}

			set
			{
				style = value;
				BlockDefinition.Style = StyleToUiString(value);
			}
		}

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		/// <exception cref="ArgumentNullException">When the value is <c>null</c>.</exception>
		public string Tooltip
		{
			get
			{
				return BlockDefinition.TooltipText;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				BlockDefinition.TooltipText = value;
			}
		}

		/// <summary>
		///     Gets or sets the text displayed in the button.
		/// </summary>
		public string Text
		{
			get
			{
				return BlockDefinition.Text;
			}

			set
			{
				BlockDefinition.Text = value;
			}
		}

		/// <summary>
		///     Load any changes made through user interaction.
		/// </summary>
		/// <param name="uiResults">
		///     Represents the information a user has entered or selected in a dialog box of an interactive
		///     Automation script.
		/// </param>
		/// <remarks><see cref="InteractiveWidget.DestVar" /> should be used as key to get the changes for this widget.</remarks>
		internal override void LoadResult(UIResults uiResults)
		{
			pressed = uiResults.WasButtonPressed(this);
		}

		/// <summary>
		///     Raises zero or more events of the widget.
		///     This method is called after <see cref="InteractiveWidget.LoadResult" /> was called on all widgets.
		/// </summary>
		/// <remarks>It is up to the implementer to determine if an event must be raised.</remarks>
		internal override void RaiseResultEvents()
		{
			if ((OnPressed != null) && pressed)
			{
				OnPressed(this, EventArgs.Empty);
			}

			pressed = false;
		}

		private static string StyleToUiString(ButtonStyle buttonStyle)
		{
			switch (buttonStyle)
			{
				case ButtonStyle.None:
					return Automation.Style.Button.None;
				case ButtonStyle.CallToAction:
					return Automation.Style.Button.CallToAction;
				default:
					throw new ArgumentOutOfRangeException("buttonStyle", buttonStyle, null);
			}
		}
	}
}
