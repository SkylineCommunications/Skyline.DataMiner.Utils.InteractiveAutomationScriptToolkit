namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.ComponentModel;
	using System.Reflection;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Base class for widgets.
	/// </summary>
	public class Widget : Component, IWidget
	{
		private UIBlockDefinition blockDefinition = new UIBlockDefinition();
		private HorizontalAlignment horizontalAlignment;
		private VerticalAlignment verticalAlignment;

		/// <summary>
		/// Initializes a new instance of the <see cref="Widget"/> class.
		/// </summary>
		protected Widget()
		{
			Type = UIBlockType.Undefined;
			IsVisible = true;
			SetHeightAuto();
			SetWidthAuto();
		}

		/// <inheritdoc />
		public HorizontalAlignment HorizontalAlignment
		{
			get
			{
				return horizontalAlignment;
			}

			set
			{
				horizontalAlignment = value;
				BlockDefinition.HorizontalAlignment = AlignmentToUiString(value);
			}
		}

		/// <inheritdoc />
		public VerticalAlignment VerticalAlignment
		{
			get
			{
				return verticalAlignment;
			}

			set
			{
				verticalAlignment = value;
				BlockDefinition.VerticalAlignment = AlignmentToUiString(value);
			}
		}

		/// <summary>
		///     Gets or sets the fixed height (in pixels) of the widget.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
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
					throw new ArgumentOutOfRangeException("value");
				}

				BlockDefinition.Height = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the widget is visible in the dialog.
		/// </summary>
		public virtual bool IsVisible { get; set; }

		/// <summary>
		///     Gets or sets the maximum height (in pixels) of the widget.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
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
					throw new ArgumentOutOfRangeException("value");
				}

				BlockDefinition.MaxHeight = value;
			}
		}

		/// <summary>
		///     Gets or sets the maximum width (in pixels) of the widget.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
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
					throw new ArgumentOutOfRangeException("value");
				}

				BlockDefinition.MaxWidth = value;
			}
		}

		/// <summary>
		///     Gets or sets the minimum height (in pixels) of the widget.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
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
					throw new ArgumentOutOfRangeException("value");
				}

				BlockDefinition.MinHeight = value;
			}
		}

		/// <summary>
		///     Gets or sets the minimum width (in pixels) of the widget.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
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
					throw new ArgumentOutOfRangeException("value");
				}

				BlockDefinition.MinWidth = value;
			}
		}

		/// <summary>
		///     Gets or sets the fixed width (in pixels) of the widget.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
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
					throw new ArgumentOutOfRangeException("value");
				}

				BlockDefinition.Width = value;
			}
		}

		/// <summary>
		/// Gets or sets the margin of the widget.
		/// </summary>
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

		/// <summary>
		/// Gets or sets the value of the 'data-cy' attribute of the dma-automation-grid-component element used to display the widget in HTML.
		/// Note: it is only be possible to use this DebugTag in web, this will not be added when running IAS in cube.
		/// </summary>
		public string DebugTag
		{
			get
			{
				return BlockDefinition.DebugTag;
			}

			set
			{
				BlockDefinition.DebugTag = value;
			}
		}

		/// <summary>
		///     Gets or sets the UIBlockType of the widget.
		/// </summary>
		public UIBlockType Type
		{
			get
			{
				return BlockDefinition.Type;
			}

			set
			{
				BlockDefinition.Type = value;
			}
		}

		/// <summary>
		///     Gets the internal DataMiner representation of the widget.
		///     This object should not be used!
		///     This library exists so you don't need to use this object.
		/// </summary>
		/// <remarks>A widget should implement everything, so you don't need to use this object.</remarks>
		public UIBlockDefinition BlockDefinition
		{
			get
			{
				return blockDefinition;
			}
		}

		/// <summary>
		///     Set the height of the widget based on its content.
		/// </summary>
		public void SetHeightAuto()
		{
			BlockDefinition.Height = -1;
			BlockDefinition.MaxHeight = -1;
			BlockDefinition.MinHeight = -1;
		}

		/// <summary>
		///     Set the width of the widget based on its content.
		/// </summary>
		public void SetWidthAuto()
		{
			BlockDefinition.Width = -1;
			BlockDefinition.MaxWidth = -1;
			BlockDefinition.MinWidth = -1;
		}

		/// <summary>
		/// Ugly method to clear the internal list of DropDown items that can't be accessed.
		/// </summary>
		protected void RecreateUiBlock()
		{
			UIBlockDefinition newUiBlockDefinition = new UIBlockDefinition();
			PropertyInfo[] propertyInfo = typeof(UIBlockDefinition).GetProperties();

			foreach (PropertyInfo property in propertyInfo)
			{
				if (property.CanWrite)
				{
					property.SetValue(newUiBlockDefinition, property.GetValue(blockDefinition));
				}
			}

			blockDefinition = newUiBlockDefinition;
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
