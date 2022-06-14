namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.ComponentModel;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Represents a widget.
	/// </summary>
	public interface IWidget
	{
		/// <summary>
		///     Gets the internal DataMiner representation of the widget.
		///     This object should not be used!
		///     This library exists so you don't need to use this object.
		/// </summary>
		/// <remarks>A widget should implement everything, so you don't need to use this object.</remarks>
		[EditorBrowsable(EditorBrowsableState.Never)]
		UIBlockDefinition BlockDefinition { get; }

		/// <summary>
		///     Gets the UIBlockType of the widget.
		/// </summary>
		UIBlockType Type { get; }

		/// <summary>
		///     Gets or sets the fixed height (in pixels) of the widget.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		int Height { get; set; }

		/// <summary>
		///     Gets or sets the horizontal alignment of the widget.
		/// </summary>
		HorizontalAlignment HorizontalAlignment { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether the widget is visible in the dialog.
		/// </summary>
		bool IsVisible { get; set; }

		/// <summary>
		///     Gets or sets the margin of the widget.
		/// </summary>
		Margin Margin { get; set; }

		/// <summary>
		///     Gets or sets the maximum height (in pixels) of the widget.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		int MaxHeight { get; set; }

		/// <summary>
		///     Gets or sets the maximum width (in pixels) of the widget.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		int MaxWidth { get; set; }

		/// <summary>
		///     Gets or sets the minimum height (in pixels) of the widget.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		int MinHeight { get; set; }

		/// <summary>
		///     Gets or sets the minimum width (in pixels) of the widget.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		int MinWidth { get; set; }

		/// <summary>
		///     Gets or sets the vertical alignment of the widget.
		/// </summary>
		VerticalAlignment VerticalAlignment { get; set; }

		/// <summary>
		///     Gets or sets the fixed width (in pixels) of the widget.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		int Width { get; set; }

		/// <summary>
		///     Set the height of the widget based on its content.
		/// </summary>
		void SetHeightAuto();

		/// <summary>
		///     Set the width of the widget based on its content.
		/// </summary>
		void SetWidthAuto();
	}
}