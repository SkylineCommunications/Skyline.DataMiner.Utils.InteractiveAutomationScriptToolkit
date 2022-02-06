namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.ComponentModel;

	using Automation;

	/// <summary>
	///     Base class for widgets.
	/// </summary>
	public class Widget : IWidget
	{
		private readonly UIBlockDefinition blockDefinition = new UIBlockDefinition();

		/// <summary>
		/// Initializes a new instance of the Widget class.
		/// </summary>
		protected Widget()
		{
			Type = UIBlockType.Undefined;
			IsVisible = true;
			SetHeightAuto();
			SetWidthAuto();
		}

		/// <inheritdoc />
		public int Height
		{
			get
			{
				return BlockDefinition.Height;
			}

			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException(nameof(value));
				}

				BlockDefinition.Height = value;
			}
		}

		/// <inheritdoc />
		public bool IsVisible { get; set; }

		/// <inheritdoc />
		public int MaxHeight
		{
			get
			{
				return BlockDefinition.MaxHeight;
			}

			set
			{
				if (value <= -2)
				{
					throw new ArgumentOutOfRangeException(nameof(value));
				}

				BlockDefinition.MaxHeight = value;
			}
		}

		/// <inheritdoc />
		public int MaxWidth
		{
			get
			{
				return BlockDefinition.MaxWidth;
			}

			set
			{
				if (value <= -2)
				{
					throw new ArgumentOutOfRangeException(nameof(value));
				}

				BlockDefinition.MaxWidth = value;
			}
		}

		/// <inheritdoc />
		public int MinHeight
		{
			get
			{
				return BlockDefinition.MinHeight;
			}

			set
			{
				if (value <= -2)
				{
					throw new ArgumentOutOfRangeException(nameof(value));
				}

				BlockDefinition.MinHeight = value;
			}
		}

		/// <inheritdoc />
		public int MinWidth
		{
			get
			{
				return BlockDefinition.MinWidth;
			}

			set
			{
				if (value <= -2)
				{
					throw new ArgumentOutOfRangeException(nameof(value));
				}

				BlockDefinition.MinWidth = value;
			}
		}

		/// <inheritdoc />
		public UIBlockType Type
		{
			get
			{
				return BlockDefinition.Type;
			}

			protected set
			{
				BlockDefinition.Type = value;
			}
		}

		/// <inheritdoc />
		public int Width
		{
			get
			{
				return BlockDefinition.Width;
			}

			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException(nameof(value));
				}

				BlockDefinition.Width = value;
			}
		}

		/// <inheritdoc />
		public Margin Margin
		{
			get
			{
				return new Margin(BlockDefinition.Margin);
			}

			set
			{
				BlockDefinition.Margin = value.ToString();
			}
		}

		/// <inheritdoc />
		[EditorBrowsable(EditorBrowsableState.Never)]
		public UIBlockDefinition BlockDefinition
		{
			get
			{
				return blockDefinition;
			}
		}

		/// <inheritdoc />
		public void SetHeightAuto()
		{
			BlockDefinition.Height = -1;
			BlockDefinition.MaxHeight = -1;
			BlockDefinition.MinHeight = -1;
		}

		/// <inheritdoc />
		public void SetWidthAuto()
		{
			BlockDefinition.Width = -1;
			BlockDefinition.MaxWidth = -1;
			BlockDefinition.MinWidth = -1;
		}
	}
}
