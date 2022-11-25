namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
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
		///     Triggered when the button is pressed.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<EventArgs> Pressed
		{
			add
			{
				OnPressed += value;
				WantsOnChange = true;
			}

			remove
			{
				OnPressed -= value;
				if(OnPressed == null || !OnPressed.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<EventArgs> OnPressed;

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

		internal override void LoadResult(UIResults uiResults)
		{
			pressed = uiResults.WasButtonPressed(this);
		}

		/// <inheritdoc />
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
