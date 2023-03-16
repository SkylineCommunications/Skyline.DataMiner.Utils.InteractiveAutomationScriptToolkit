namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     A label is used to display text.
	///     Text can have different styles.
	/// </summary>
	public class Label : Widget
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
		public Label() : this("Label")
		{
		}

		/// <summary>
		///     Gets or sets the text style of the label.
		/// </summary>
		public TextStyle Style
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
		///     Gets or sets the displayed text.
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
					throw new ArgumentOutOfRangeException("textStyle", textStyle, null);
			}
		}
	}
}
