namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System;
	using System.ComponentModel;

	using Skyline.DataMiner.Automation;

	/// <summary>
	/// Default implementation of the <see cref="IValidate"/> interface.
	/// Use this class to add the default implementation to your custom widget.
	/// </summary>
	public class Validation : IValidate
	{
		private readonly IWidget widget;

		/// <summary>
		/// 	Initializes a new instance of the <see cref="Validation" /> class.
		/// </summary>
		/// <param name="widget">Widget that will implement this behavior.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="widget"/> is <c>null</c>.</exception>
		public Validation(IWidget widget)
		{
			this.widget = widget ?? throw new ArgumentNullException(nameof(widget));
			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
		}

		/// <inheritdoc />
		public UIValidationState ValidationState
		{
			get => widget.BlockDefinition.ValidationState;

			set
			{
				if (!Enum.IsDefined(typeof(UIValidationState), value))
				{
					throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(UIValidationState));
				}

				widget.BlockDefinition.ValidationState = value;
			}
		}

		/// <inheritdoc />
		public string ValidationText
		{
			get => widget.BlockDefinition.ValidationText;
			set => widget.BlockDefinition.ValidationText = value ?? String.Empty;
		}
	}
}