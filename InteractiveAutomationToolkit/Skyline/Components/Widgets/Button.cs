namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
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
			WantsOnChange = true;
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

				// The WantsOnChange flag should not be cleared.
				// If a dialog has no "interactive" UIBlock, it will automatically add an awkward "send interaction" button.
				// This button always appears on the first row and column, potentially overlapping with other widgets.
				// This might lead to confusion if the dialog does have a button (one without a event handler).
				// So we think it is better to always make a button "interactive" to prevent this.
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