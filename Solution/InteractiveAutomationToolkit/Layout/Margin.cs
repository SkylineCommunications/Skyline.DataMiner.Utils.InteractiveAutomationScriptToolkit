namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;

	/// <summary>
	/// Defines the whitespace that is displayed around a Widget.
	/// </summary>
	public class Margin
	{
		private int bottom;
		private int left;
		private int right;
		private int top;

		/// <summary>
		/// Initializes a new instance of the Margin class.
		/// </summary>
		/// <param name="left">Amount of margin on the left side of the Widget in pixels.</param>
		/// <param name="top">Amount of margin on the top of the Widget in pixels.</param>
		/// <param name="right">Amount of margin on the right side of the Widget in pixels.</param>
		/// <param name="bottom">Amount of margin on the bottom of the Widget in pixels.</param>
		public Margin(int left, int top, int right, int bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		/// <summary>
		/// Initializes a new instance of the Margin class.
		/// All of the margins get the default values of 3 pixels.
		/// </summary>
		public Margin() : this(3, 3, 3, 3)
		{
		}

		/// <summary>
		/// Initializes a new instance of the Margin class based on a string.
		/// This string should have the following syntax: left;top;right;bottom
		/// </summary>
		/// <exception cref="ArgumentException">If the string does not match the predefined syntax. Or if any of the margins is not a number.</exception>
		/// <param name="margin">Margin in string format.</param>
		public Margin(string margin)
		{
			if(String.IsNullOrWhiteSpace(margin))
			{
				left = 0;
				top = 0;
				right = 0;
				bottom = 0;
				return;
			}

			string[] splitMargin = margin.Split(';');
			if (splitMargin.Length != 4) throw new ArgumentException("Margin should have the following format: left;top;right;bottom");

			if (!Int32.TryParse(splitMargin[0], out left)) throw new ArgumentException("Left margin is not a number");
			if (!Int32.TryParse(splitMargin[1], out top)) throw new ArgumentException("Top margin is not a number");
			if (!Int32.TryParse(splitMargin[2], out right)) throw new ArgumentException("Right margin is not a number");
			if (!Int32.TryParse(splitMargin[3], out bottom)) throw new ArgumentException("Bottom margin is not a number");
		}

		/// <summary>
		/// Amount of margin in pixels at the bottom of the Widget.
		/// </summary>
		public int Bottom
		{
			get
			{
				return bottom;
			}

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				bottom = value;
			}
		}

		/// <summary>
		/// Amount of margin in pixels at the left hand side of the Widget.
		/// </summary>
		public int Left
		{
			get
			{
				return left;
			}

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				left = value;
			}
		}

		/// <summary>
		/// Amount of margin in pixels at the right hand side of the Widget.
		/// </summary>
		public int Right
		{
			get
			{
				return right;
			}

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				right = value;
			}
		}

		/// <summary>
		/// Amount of margin in pixels at the top of the Widget.
		/// </summary>
		public int Top
		{
			get
			{
				return top;
			}

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				top = value;
			}
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return String.Join(";", new object[] { left, top, right, bottom });
		}
	}
}
