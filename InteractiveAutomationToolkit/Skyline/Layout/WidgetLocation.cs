namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;

	/// <summary>
	///     Used to define the location of a widget in a grid layout.
	/// </summary>
	public readonly struct WidgetLocation : IEquatable<WidgetLocation>
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="WidgetLocation" /> struct.
		/// </summary>
		/// <param name="fromRow">Row index of top-left cell.</param>
		/// <param name="fromColumn">Column index of the top-left cell.</param>
		/// <param name="rowSpan">Number of vertical cells the widget spans across.</param>
		/// <param name="columnSpan">Number of horizontal cells the widget spans across.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="fromRow" /> is less than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="fromColumn" /> is less than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="rowSpan" /> is less than 1.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="columnSpan" /> is less than 1.</exception>
		public WidgetLocation(int fromRow, int fromColumn, int rowSpan, int columnSpan)
		{
			if (fromRow < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(fromRow));
			}

			if (fromColumn < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(fromColumn));
			}

			if (rowSpan <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(rowSpan));
			}

			if (columnSpan <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(columnSpan));
			}

			Row = fromRow;
			Column = fromColumn;
			RowSpan = rowSpan;
			ColumnSpan = columnSpan;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="WidgetLocation" /> struct.
		/// </summary>
		/// <param name="row">Row index of the cell where the widget is placed.</param>
		/// <param name="column">Column index of the cell where the widget is placed.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="row" /> is less than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="column" /> is less than 0.</exception>
		public WidgetLocation(int row, int column)
			: this(row, column, 1, 1)
		{
		}

		/// <summary>
		///     Gets the column location of the widget on the grid.
		/// </summary>
		public int Column { get; }

		/// <summary>
		///     Gets how many columns the widget spans on the grid.
		/// </summary>
		public int ColumnSpan { get; }

		/// <summary>
		///     Gets the row location of the widget on the grid.
		/// </summary>
		public int Row { get; }

		/// <summary>
		///     Gets how many rows the widget spans on the grid.
		/// </summary>
		public int RowSpan { get; }

		/// <summary>
		///     Determines whether two specified instances of <see cref="WidgetLocation" /> are equal.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>
		///     <c>true</c> if <paramref name="left" /> and <paramref name="right" /> represent the same margin; otherwise,
		///     <c>false</c>.
		/// </returns>
		public static bool operator ==(WidgetLocation left, WidgetLocation right)
		{
			return left.Equals(right);
		}

		/// <summary>
		///     Determines whether two specified instances of <see cref="WidgetLocation" /> are not equal.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>
		///     <c>true</c> if <paramref name="left" /> and <paramref name="right" /> do not represent the same margin;
		///     otherwise, <c>false</c>.
		/// </returns>
		public static bool operator !=(WidgetLocation left, WidgetLocation right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		///     Determines if the current location overlaps with the specified other location.
		/// </summary>
		/// <param name="other">The other location to compare the current location to.</param>
		/// <returns>Whether the location overlaps with <paramref name="other" />.</returns>
		public bool Overlaps(WidgetLocation other)
		{
			// https://stackoverflow.com/a/20925869
			bool rowsOverlap = Row + RowSpan > other.Row && other.Row + other.RowSpan > Row;
			bool columnsOverlap = Column + ColumnSpan > other.Column && other.Column + other.ColumnSpan > Column;

			return rowsOverlap && columnsOverlap;
		}

		/// <summary>
		///     Returns a value indicating whether the value of this instance is equal to the value of the specified
		///     <see cref="WidgetLocation" /> instance.
		/// </summary>
		/// <param name="other">The object to compare to this instance.</param>
		/// <returns><c>true</c> if the value parameter equals the value of this instance; otherwise, <c>false</c>.</returns>
		public bool Equals(WidgetLocation other)
		{
			return Column == other.Column &&
				ColumnSpan == other.ColumnSpan &&
				Row == other.Row &&
				RowSpan == other.RowSpan;
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			return obj is WidgetLocation location && Equals(location);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = Column;
				hashCode = hashCode * 397 ^ ColumnSpan;
				hashCode = hashCode * 397 ^ Row;
				hashCode = hashCode * 397 ^ RowSpan;
				return hashCode;
			}
		}

		internal WidgetLocation AddOffset(SectionLocation offset)
		{
			return new WidgetLocation(Row + offset.Row, Column + offset.Row, RowSpan, ColumnSpan);
		}
	}
}