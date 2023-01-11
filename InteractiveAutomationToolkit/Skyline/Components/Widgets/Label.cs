namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System;
	using System.ComponentModel;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     A label is used to display text.
	///     Text can have different styles.
	/// </summary>
	public class Label : Widget, ILabel
	{
		private TextStyle style;

		/// <summary>
		///     Initializes a new instance of the <see cref="Label" /> class.
		/// </summary>
		/// <param name="text">The text that is displayed by the label.</param>
		public Label(string text)
		{
			Type = UIBlockType.StaticText;
			Style = TextStyle.None;
			Text = text;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Label" /> class.
		/// </summary>
		public Label()
			: this("Label")
		{
		}

		/// <inheritdoc />
		public TextStyle Style
		{
			get => style;

			set
			{
				if (!Enum.IsDefined(typeof(TextStyle), value))
				{
					throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(TextStyle));
				}

				style = value;
				BlockDefinition.Style = StyleToUiString(value);
			}
		}

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

		private static string StyleToUiString(TextStyle textStyle)
		{
			switch (textStyle)
			{
				case TextStyle.None:
					return null;

				case TextStyle.Title:
					return "Title1";

				case TextStyle.Bold:
					return "Title2";

				case TextStyle.Heading:
					return "Title3";

				default:
					throw new ArgumentOutOfRangeException(nameof(textStyle), textStyle, null);
			}
		}
	}
}