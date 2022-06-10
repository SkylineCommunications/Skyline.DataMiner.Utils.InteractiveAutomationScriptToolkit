namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;

	/// <summary>
	///     Used to define the location of a section in another section or dialog.
	/// </summary>
	public readonly struct SectionLocation : IEquatable<SectionLocation>
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="SectionLocation" /> struct.
		/// </summary>
		/// <param name="row">Row index of the cell that the top-left cell of the section will be mapped to.</param>
		/// <param name="column">Column index of the cell that the top-left cell of the section will be mapped to.</param>
		public SectionLocation(int row, int column)
		{
			if (row < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(row));
			}

			if (column < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(column));
			}

			Row = row;
			Column = column;
		}

		/// <summary>
		///     Gets the column location of the section on the dialog grid.
		/// </summary>
		/// <remarks>The top-left location is (0, 0) by default.</remarks>
		public int Column { get; }

		/// <summary>
		///     Gets the row location of the section on the dialog grid.
		/// </summary>
		/// <remarks>The top-left location is (0, 0) by default.</remarks>
		public int Row { get; }

		/// <summary>
		///     Determines whether two specified instances of <see cref="SectionLocation" /> are equal.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>
		///     <c>true</c> if <paramref name="left" /> and <paramref name="right" /> represent the same margin; otherwise,
		///     <c>false</c>.
		/// </returns>
		public static bool operator ==(SectionLocation left, SectionLocation right)
		{
			return left.Equals(right);
		}

		/// <summary>
		///     Determines whether two specified instances of <see cref="SectionLocation" /> are not equal.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>
		///     <c>true</c> if <paramref name="left" /> and <paramref name="right" /> do not represent the same margin;
		///     otherwise, <c>false</c>.
		/// </returns>
		public static bool operator !=(SectionLocation left, SectionLocation right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		///     Returns a value indicating whether the value of this instance is equal to the value of the specified
		///     <see cref="SectionLocation" /> instance.
		/// </summary>
		/// <param name="other">The object to compare to this instance.</param>
		/// <returns><c>true</c> if the value parameter equals the value of this instance; otherwise, <c>false</c>.</returns>
		public bool Equals(SectionLocation other)
		{
			return Column == other.Column && Row == other.Row;
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			return obj is SectionLocation location && Equals(location);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			unchecked
			{
				return Column * 397 ^ Row;
			}
		}

		internal SectionLocation AddOffset(SectionLocation offset)
		{
			return new SectionLocation(Row + offset.Row, Column + offset.Column);
		}
	}
}