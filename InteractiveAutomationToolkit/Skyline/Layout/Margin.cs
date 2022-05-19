namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;

	/// <summary>
	/// Defines the whitespace that is displayed around a widget.
	/// </summary>
	public struct Margin : IEquatable<Margin>
	{
		/// <summary>
		/// Initializes a new instance of the Margin class.
		/// </summary>
		/// <param name="left">Amount of margin on the left-hand side of the widget in pixels.</param>
		/// <param name="top">Amount of margin at the top of the widget in pixels.</param>
		/// <param name="right">Amount of margin on the right-hand side of the widget in pixels.</param>
		/// <param name="bottom">Amount of margin at the bottom of the widget in pixels.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="left"/> is less than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="top"/> is less than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="right"/> is less than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="bottom"/> is less than 0.</exception>
		/// <remarks>All widgets have a default margin of 4 pixels on all sides.</remarks>
		public Margin(int left, int top, int right, int bottom)
		{
			if (left < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(left));
			}

			if (top < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(top));
			}

			if (right < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(right));
			}

			if (bottom < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(bottom));
			}

			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		/// <summary>
		/// Initializes a new instance of the Margin class.
		/// </summary>
		/// <param name="leftRight">Amount of margin on the left and right sides of the widget in pixels.</param>
		/// <param name="top">Amount of margin at the top of the widget in pixels.</param>
		/// <param name="bottom">Amount of margin at the top of the widget in pixels.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="leftRight"/> is less than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="top"/> is less than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="bottom"/> is less than 0.</exception>
		/// <remarks>All widgets have a default margin of 4 pixels on all sides.</remarks>
		public Margin(int leftRight, int top, int bottom) : this(leftRight, top, leftRight, bottom)
		{
		}

		/// <summary>
		/// Initializes a new instance of the Margin class.
		/// </summary>
		/// <param name="leftRight">Amount of margin on the left and right sides of the widget in pixels.</param>
		/// <param name="topBottom">Amount of margin on the top and bottom sides of the widget in pixels.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="leftRight"/> is less than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="topBottom"/> is less than 0.</exception>
		/// <remarks>All widgets have a default margin of 4 pixels on all sides.</remarks>
		public Margin(int leftRight, int topBottom) : this(leftRight, topBottom, leftRight, topBottom)
		{
		}

		/// <summary>
		/// Initializes a new instance of the Margin class.
		/// </summary>
		/// <param name="all">Amount of margin on all sides of the widget in pixels.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="all"/> is less than 0.</exception>
		/// <remarks>All widgets have a default margin of 4 pixels on all sides.</remarks>
		public Margin(int all) : this(all, all, all, all)
		{
		}

		/// <summary>
		/// Initializes a new instance of the Margin class based on a string.
		/// This string should have the following syntax: left;top;right;bottom
		/// </summary>
		/// <exception cref="FormatException">If the string does not match the predefined syntax, or if any of the margins is not a number.</exception>
		/// <exception cref="ArgumentException">If one of the sides is less than 0.</exception>
		/// <param name="margin">Margin in string format.</param>
		/// <remarks>All widgets have a default margin of 4 pixels on all sides.</remarks>
		public Margin(string margin)
		{
			if (String.IsNullOrWhiteSpace(margin))
			{
				Left = 0;
				Top = 0;
				Right = 0;
				Bottom = 0;
				return;
			}

			string[] splitMargin = margin.Split(';');
			if (splitMargin.Length != 4) throw new FormatException("Margin should have the following format: left;top;right;bottom");

			int left;
			if (!Int32.TryParse(splitMargin[0], out left)) throw new FormatException("Left margin is not a number");

			int top;
			if (!Int32.TryParse(splitMargin[1], out top)) throw new FormatException("Top margin is not a number");

			int right;
			if (!Int32.TryParse(splitMargin[2], out right)) throw new FormatException("Right margin is not a number");

			int bottom;
			if (!Int32.TryParse(splitMargin[3], out bottom)) throw new FormatException("Bottom margin is not a number");

			if (left < 0)
			{
				throw new ArgumentException("Left margin is less than 0.");
			}

			if (top < 0)
			{
				throw new ArgumentException("Top margin is less than 0.");
			}

			if (right < 0)
			{
				throw new ArgumentException("Right margin is less than 0.");
			}

			if (bottom < 0)
			{
				throw new ArgumentException("Bottom margin is less than 0.");
			}

			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		/// <summary>
		/// Amount of margin in pixels at the bottom of the widget.
		/// </summary>
		public int Bottom { get; }

		/// <summary>
		/// Amount of margin in pixels at the left-hand side of the widget.
		/// </summary>
		public int Left { get; }

		/// <summary>
		/// Amount of margin in pixels at the right-hand side of the widget.
		/// </summary>
		public int Right { get; }

		/// <summary>
		/// Amount of margin in pixels at the top of the widget.
		/// </summary>
		public int Top { get; }

		/// <summary>
		/// Determines whether two specified instances of <see cref="Margin"/> are equal.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> represent the same margin; otherwise, <c>false</c>.</returns>
		public static bool operator ==(Margin left, Margin right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Determines whether two specified instances of <see cref="Margin"/> are not equal.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> do not represent the same margin; otherwise, <c>false</c>.</returns>
		public static bool operator !=(Margin left, Margin right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		/// Returns a value indicating whether the value of this instance is equal to the value of the specified <see cref="Margin"/> instance.
		/// </summary>
		/// <param name="other">The object to compare to this instance.</param>
		/// <returns><c>true</c> if the value parameter equals the value of this instance; otherwise, <c>false</c>.</returns>
		public bool Equals(Margin other)
		{
			return Bottom == other.Bottom && Left == other.Left && Right == other.Right && Top == other.Top;
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			return obj is Margin && Equals((Margin)obj);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = Bottom;
				hashCode = (hashCode * 397) ^ Left;
				hashCode = (hashCode * 397) ^ Right;
				hashCode = (hashCode * 397) ^ Top;
				return hashCode;
			}
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return String.Join(";", Left, Top, Right, Bottom);
		}
	}
}
