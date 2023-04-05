namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;

	/// <summary>
	/// Defines the whitespace that is displayed around a widget.
	/// </summary>
	public class Margin
	{
		private int bottom;
		private int left;
		private int right;
		private int top;

		/// <summary>
		/// Initializes a new instance of the <see cref="Margin"/> class.
		/// </summary>
		/// <param name="left">Amount of margin on the left-hand side of the widget in pixels.</param>
		/// <param name="top">Amount of margin at the top of the widget in pixels.</param>
		/// <param name="right">Amount of margin on the right-hand side of the widget in pixels.</param>
		/// <param name="bottom">Amount of margin at the bottom of the widget in pixels.</param>
		public Margin(int left, int top, int right, int bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Margin"/> class.
		/// A margin is by default 3 pixels wide.
		/// </summary>
		public Margin() : this(3, 3, 3, 3)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Margin"/> class based on a string.
		/// This string should have the following syntax: left;top;right;bottom.
		/// </summary>
		/// <exception cref="ArgumentException">If the string does not match the predefined syntax, or if any of the margins is not a number.</exception>
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
			if (splitMargin.Length != 4)
			{
				throw new ArgumentException("Margin should have the following format: left;top;right;bottom");
			}

			if (!Int32.TryParse(splitMargin[0], out left))
			{
				throw new ArgumentException("Left margin is not a number");
			}

			if (!Int32.TryParse(splitMargin[1], out top))
			{
				throw new ArgumentException("Top margin is not a number");
			}

			if (!Int32.TryParse(splitMargin[2], out right))
			{
				throw new ArgumentException("Right margin is not a number");
			}

			if (!Int32.TryParse(splitMargin[3], out bottom))
			{
				throw new ArgumentException("Bottom margin is not a number");
			}
		}

		/// <summary>
		/// Gets or sets the amount of margin in pixels at the bottom of the widget.
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
		/// Gets or sets the amount of margin in pixels at the left-hand side of the widget.
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
		/// Gets or sets the amount of margin in pixels at the right-hand side of the widget.
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
		/// Gets or sets the amount of margin in pixels at the top of the widget.
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


		/// <summary>Returns a string that represents the current object.</summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			return String.Join(";", new object[] { left, top, right, bottom });
		}
	}
}
