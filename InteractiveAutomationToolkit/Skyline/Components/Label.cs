namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;

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
				style = value;
				BlockDefinition.Style = StyleToUiString(value);
			}
		}

		/// <inheritdoc />
		public string Text
		{
			get => BlockDefinition.Text;
			set => BlockDefinition.Text = value;
		}

		/// <inheritdoc />
		public string Tooltip
		{
			get => BlockDefinition.TooltipText;

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value));
				}

				BlockDefinition.TooltipText = value;
			}
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