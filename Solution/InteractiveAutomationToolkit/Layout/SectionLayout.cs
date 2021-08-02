namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;

	/// <summary>
	/// Used to define the position of a section in another section or dialog.
	/// </summary>
	public class SectionLayout : ILayout
	{
		private int column;
		private int row;

		/// <summary>
		/// Initializes a new instance of the <see cref="SectionLayout"/> class.
		/// </summary>
		/// <param name="row">Row index of the cell that the top-left cell of the section will be mapped to.</param>
		/// <param name="column">Column index of the cell that the top-left cell of the section will be mapped to.</param>
		public SectionLayout(int row, int column)
		{
			this.row = row;
			this.column = column;
		}

		/// <summary>
		///     Gets or sets the column location of the section on the dialog grid.
		/// </summary>
		/// <remarks>The top-left position is (0, 0) by default.</remarks>
		public int Column
		{
			get
			{
				return column;
			}

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				column = value;
			}
		}

		/// <summary>
		///     Gets or sets the row location of the section on the dialog grid.
		/// </summary>
		/// <remarks>The top-left position is (0, 0) by default.</remarks>
		public int Row
		{
			get
			{
				return row;
			}

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				row = value;
			}
		}
	}
}
