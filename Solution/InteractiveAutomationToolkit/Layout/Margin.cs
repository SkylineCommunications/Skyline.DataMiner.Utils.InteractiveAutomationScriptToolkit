namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;

	public class Margin
	{
		private int bottom;
		private int left;
		private int right;
		private int top;

		public Margin(int left, int top, int right, int bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		public Margin() : this(3, 3, 3, 3)
		{
		}

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
