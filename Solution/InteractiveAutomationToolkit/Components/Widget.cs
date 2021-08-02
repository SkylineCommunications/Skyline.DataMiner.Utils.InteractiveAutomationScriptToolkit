namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Reflection;
	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Base class for widgets.
	/// </summary>
	public class Widget
	{
		private UIBlockDefinition blockDefinition = new UIBlockDefinition();

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
		public bool IsVisible { get; set; }

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
		///     Gets or sets the UIBlockType of the widget.
		/// </summary>
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
		/// Margin of the widget.
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

		internal UIBlockDefinition BlockDefinition
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
	}
}
