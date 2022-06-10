namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.ComponentModel;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Base class for widgets.
	/// </summary>
	public class Widget : IWidget
	{
		private HorizontalAlignment horizontalAlignment;
		private VerticalAlignment verticalAlignment;

		/// <summary>
		///     Initializes a new instance of the <see cref="Widget" /> class.
		/// </summary>
		protected Widget()
		{
			Type = UIBlockType.Undefined;
			IsVisible = true;
			Margin = new Margin(4);
			HorizontalAlignment = HorizontalAlignment.Left;
			VerticalAlignment = VerticalAlignment.Center;
			SetHeightAuto();
			SetWidthAuto();
		}

		/// <inheritdoc />
		[EditorBrowsable(EditorBrowsableState.Never)]
		public UIBlockDefinition BlockDefinition { get; } = new UIBlockDefinition();

		/// <inheritdoc />
		public int Height
		{
			get => BlockDefinition.Height;

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
		public HorizontalAlignment HorizontalAlignment
		{
			get => horizontalAlignment;

			set
			{
				horizontalAlignment = value;
				BlockDefinition.HorizontalAlignment = AlignmentToUiString(value);
			}
		}

		/// <inheritdoc />
		public bool IsVisible { get; set; }

		/// <inheritdoc />
		public Margin Margin
		{
			get => new Margin(BlockDefinition.Margin);
			set => BlockDefinition.Margin = value.ToString();
		}

		/// <inheritdoc />
		public int MaxHeight
		{
			get => BlockDefinition.MaxHeight;

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
			get => BlockDefinition.MaxWidth;

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
			get => BlockDefinition.MinHeight;

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
			get => BlockDefinition.MinWidth;

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
			get => BlockDefinition.Type;
			protected set => BlockDefinition.Type = value;
		}

		/// <inheritdoc />
		public VerticalAlignment VerticalAlignment
		{
			get => verticalAlignment;

			set
			{
				verticalAlignment = value;
				BlockDefinition.VerticalAlignment = AlignmentToUiString(value);
			}
		}

		/// <inheritdoc />
		public int Width
		{
			get => BlockDefinition.Width;

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

		private static string AlignmentToUiString(HorizontalAlignment horizontalAlignment)
		{
			switch (horizontalAlignment)
			{
				case HorizontalAlignment.Center:
					return "Center";

				case HorizontalAlignment.Left:
					return "Left";

				case HorizontalAlignment.Right:
					return "Right";

				case HorizontalAlignment.Stretch:
					return "Stretch";

				default:
					throw new InvalidEnumArgumentException(
						nameof(horizontalAlignment),
						(int)horizontalAlignment,
						typeof(HorizontalAlignment));
			}
		}

		private static string AlignmentToUiString(VerticalAlignment verticalAlignment)
		{
			switch (verticalAlignment)
			{
				case VerticalAlignment.Center:
					return "Center";

				case VerticalAlignment.Top:
					return "Top";

				case VerticalAlignment.Bottom:
					return "Bottom";

				case VerticalAlignment.Stretch:
					return "Stretch";

				default:
					throw new InvalidEnumArgumentException(
						nameof(verticalAlignment),
						(int)verticalAlignment,
						typeof(VerticalAlignment));
			}
		}
	}
}