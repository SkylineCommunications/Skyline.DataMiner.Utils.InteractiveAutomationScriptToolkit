namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     A button that can be pressed.
	/// </summary>
	public class Button : InteractiveWidget, IButton
	{
		private bool pressed;

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
		public Button()
			: this(String.Empty)
		{
		}

		/// <inheritdoc />
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
				if (OnPressed == null || !OnPressed.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<EventArgs> OnPressed;

		/// <inheritdoc />
		public string Text
		{
			get => BlockDefinition.Text;
			set => BlockDefinition.Text = value ?? String.Empty;
		}

		/// <inheritdoc />
		public string Tooltip
		{
			get => BlockDefinition.TooltipText;
			set => BlockDefinition.TooltipText = value ?? String.Empty;
		}

		/// <inheritdoc />
		protected internal override void LoadResult(UIResults results)
		{
			pressed = results.WasButtonPressed(this);
		}

		/// <inheritdoc />
		protected internal override void RaiseResultEvents()
		{
			if (OnPressed != null && pressed)
			{
				OnPressed(this, EventArgs.Empty);
			}

			pressed = false;
		}
	}
}